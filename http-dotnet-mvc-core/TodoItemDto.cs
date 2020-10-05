using System;

namespace http_dotnet_mvc_core {
  public class TodoItemDto {
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsComplete { get; set; }
  }
}