using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace http_dotnet_mvc_core.Controllers.api {
    [ApiController]
    // [Route("api/[controller]")]
    [Route ("api/todo")]
    public class ToDoController : ControllerBase {
        private static readonly List<TodoItem> items = new [] {
            new TodoItem {
                Id = "1",
                Name = "Test 1",
                IsComplete = false,
                Secret = "some secret description 1"
            },
            new TodoItem {
                Id = "2",
                Name = "Test 2",
                IsComplete = true,
                Secret = "some secret description 2"
            }
        }.ToList ();

        private readonly ILogger<ToDoController> _logger;

        public ToDoController (ILogger<ToDoController> logger) {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<TodoItemDto>> GetAll () {
            return items.Select(item => new TodoItemDto() {
                Id = item.Id,
                Name = item.Name,
                IsComplete = item.IsComplete,
            });
        }

        [HttpGet ("{id}")]
        public async Task<ActionResult<TodoItemDto>> GetItem (string id) {
            var item = items.Find (item => item.Id == id);
            if (item == null) {
                return NotFound ();
            } else {
                return new TodoItemDto() {
                    Id = item.Id,
                    Name = item.Name,
                    IsComplete = item.IsComplete
                };
            }
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDto>> Create (TodoItemDto dto) {
            dto.Id = Guid.NewGuid ().ToString ();
            var item = new TodoItem() {
                    Id = dto.Id,
                    Name = dto.Name,
                    IsComplete = dto.IsComplete
            };
            items.Add (item);
            return dto;
        }

        [HttpPut ("{id}")]
        public async Task<IActionResult> PutTodoItem (string id, TodoItemDto dto) {
            var index = items.FindIndex (item => item.Id == id);
            if (index == -1) {
                return NotFound ();
            }

            var item = items[index];
            item.Name = dto.Name;
            item.IsComplete = dto.IsComplete;
            return NoContent ();
        }

        [HttpDelete ("{id}")]
        public async Task<ActionResult<TodoItemDto>> RemoveTodoItem (string id) {
            var index = items.FindIndex (item => item.Id == id);
            if (index == -1) {
                return NotFound ();
            }

            var item = items[index];
            items.RemoveAt (index);
            return new TodoItemDto() {
                Id = item.Id,
                Name = item.Name,
                IsComplete = item.IsComplete
            };
        }

    }
}