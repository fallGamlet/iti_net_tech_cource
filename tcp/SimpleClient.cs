using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace net_tech_cource {
    public class SimpleClient {
        public readonly string uuid;
        public readonly string Name;

        private string ipAdress;
        private int port;
        private Socket socket;
        private Action<String> logger;
        private Func<string> input;
        public SimpleClient (string uuid, string ipAdress, int port, Action<String> logger, Func<string> input) {
            this.uuid = uuid;
            this.Name = $"Client<{uuid}>";
            this.ipAdress = ipAdress;
            this.port = port;
            this.logger = logger;
            this.input = input;
            IPAddress address = IPAddress.Parse (ipAdress);
            this.socket = new Socket (
                address.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );
        }

        public void Connect () {
            this.socket.ConnectAsync (ipAdress, port)
                .ContinueWith (handleConnection);
        }
        private bool handleConnection (Task task) {
            try {
                logger ($"{Name} connected...");
                while (Messaging());
                return true;
            } catch (Exception e) {
                logger ($"{Name} exception: {e}");
                return false;
            } finally {
                logger($"{Name} Shutdown");
                socket.Shutdown (SocketShutdown.Both);
                socket.Close ();
            }
        }

        private bool Messaging() {
            var message = input();
            SocketHelper.SendMessage (socket, message);
            if (message == ".exit") {
                return false;
            } else {
                var text = SocketHelper.ReadMessage (socket);
                logger ($"{Name} recieved: {text}");
                return true;
            }
        }

    }
}