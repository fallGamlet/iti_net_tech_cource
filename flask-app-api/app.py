from flask import Flask, jsonify, send_from_directory, request

'''
python3 -m venv env

pip install -r requarenments.txt
pip freeze > requarenments.txt
'''

app = Flask(__name__)
app.secret_key = b'_5#y2L"F4Q8z\n\xec]/'

def create_generator_id(id=0):
    while True:
        yield id
        id += 1

generator_id = create_generator_id(3)
    

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

@app.route('/')
def index():
    return send_from_directory('client/public', 'index.html')


@app.route('/<path:path>')
def home(path):
    return send_from_directory('client/public', path)


@app.route('/todo/', methods=['GET'])
def get_todo_list():
    return jsonify(todoList)


@app.route('/todo/', methods=['POST'])
def create_todo_item():
    data = request.get_json()
    task = {
        'task': data['task'],
        'done': False,
        'id': next(generator_id)
    }
    todoList.append(task)
    return jsonify(task)


if __name__ == '__main__':
    app.run(host='127.0.0.1', port=8000, debug=True)
