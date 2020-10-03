from flask import Flask, jsonify, send_from_directory, request


app = Flask(__name__)
last_id = 3
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
  last_id = last_id + 1
  return last_id

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
      'id': get_id()
    }
    todoList.append(task)
    return jsonify(task)


if __name__ == '__main__':
    app.run(host='127.0.0.1', port=8000, debug=True)
