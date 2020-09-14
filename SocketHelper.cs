using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace net_tech_cource {
  class SocketHelper {
    public static String ReadMessage (Socket socket) {
      var stringBuilder = new StringBuilder ();
      var buffer = new byte[1024];
      var count = 0;
      while ((count = socket.Receive (buffer)) > 0) {
        var chunk = Encoding.UTF8.GetString (buffer, 0, count);
        bool hasEnd = chunk.EndsWith (EOM);
        if (hasEnd) chunk = chunk.Replace (EOM, "");
        stringBuilder.Append (chunk);
        if (hasEnd) break;
      }
      return stringBuilder.ToString ();
    }

    public static void SendMessage (Socket socket, string text) {
      Write(socket, text);
      WriteEOM(socket);
    }
    private static void Write (Socket socket, string text) {
      var bytes = Encoding.UTF8.GetBytes (text);
      socket.Send (bytes);
    }

    private static void WriteEOM (Socket socket) {
      socket.Send (Encoding.UTF8.GetBytes (EOM));
    }

    private static string EOM = "<EOM>";

  }
}