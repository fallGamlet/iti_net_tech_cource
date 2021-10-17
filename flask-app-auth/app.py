from flask import Flask, jsonify, send_from_directory, request
from flask import session, redirect, url_for, make_response
from datetime import datetime

'''
python3 -m venv env
. env/bin/activate
pip install -r requarenments.txt
pip freeze > requarenments.txt

pip install flask
'''


class User:
    def __init__(self, login, passwd, name) -> None:
        self.login = login
        self.passwd = passwd
        self.name = name
        self.session = None

    def check_auth(self, login, passwd):
        return self.login == login and self.passwd == passwd


class Session:
    def __init__(self, user=None) -> None:
        self.user = user
        self.time = datetime.now

    def parse_cookies(self, cookies) -> None:
        self.user = cookies.get("user", None)
        self.time = cookies.get("session_start_at", None)

    def write_to_cookies(self, resp) -> None:
        resp.set_cookie('user', self.user)
        resp.set_cookie('session_start_at', str(self.time))


def checkAccess(request):
    session = Session()
    session.parse_cookies(request.cookies)
    if (session.user is None):
        raise PermissionError


app = Flask(__name__)
app.secret_key = b'_5#y2L"F4Q8z\n\xec]/'

users = {
    'stepan': {
        'login': 'stepan',
        'passwd': '123456',
        'name': "Stepan",
        'session': None
    }
}
_last_id = {'last_id' : 3}

todoList = [
    {
        "id": 1,
        "task": "Make a what todo",
        "done": False
    },
    {
        "id": 2,
        "task": "Read todo list",
        "done": True
    }
]


def get_id():
    _last_id['last_id'] = _last_id['last_id'] + 1
    return _last_id['last_id']


@app.route('/')
def index():
    return send_from_directory('client/public', 'index.html')


@app.route('/<path:path>')
def home(path):
    return send_from_directory('client/public', path)


@app.route('/auth/', methods=['POST'])
def auth():
    data = request.get_json()
    login = data['login']
    passwd = data['passwd']

    user = users.get(login, None)
    if (user is None or user['passwd'] != passwd):
        resp = make_response("user name or password not valid")
        resp.status_code = 403
        return resp

    user['session'] = True
    user['session_start_at'] = datetime.utcnow()
    resp = make_response("auth successful")
    Session(login).write_to_cookies(resp)
    return resp


@app.route('/todo/', methods=['GET'])
def get_todo_list():
    try:
        checkAccess(request)
        session = Session()
        session.parse_cookies(request.cookies)
        print(session)
        return jsonify(todoList)
    except PermissionError as err:
        resp = make_response("auth not successful")
        resp.status_code = 401
        return resp


@app.route('/todo/', methods=['POST'])
def create_todo_item():
    data = request.get_json()
    task = {
        'task': data['task'],
        'done': False,
        'id': get_id()
    }
    todoList.append(task)
    return jsonify(task)


if __name__ == '__main__':
    app.run(host='127.0.0.1', port=8000, debug=True)
