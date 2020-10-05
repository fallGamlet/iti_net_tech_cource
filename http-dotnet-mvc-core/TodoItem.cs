using System;

namespace http_dotnet_mvc_core {
  public class TodoItem {
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsComplete { get; set; }
    public string Secret { get; set; }
  }
}