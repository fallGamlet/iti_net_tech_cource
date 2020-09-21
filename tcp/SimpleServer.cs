using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace net_tech_cource {
    class SimpleServer {
        private TcpListener server;
        private Action<String> logger;
        private ICollection<Socket> sockets;
        private bool isRunning = false;

        public SimpleServer (string ipAdress, int port, Action<String> logger) {
            this.logger = logger;
            IPAddress address = IPAddress.Parse (ipAdress);
            server = new TcpListener (address, port);
            sockets = new LinkedList<Socket>();
        }
        public async Task Start () {
            server.Start (10);
            logger ("Server start working");
            isRunning = true;
            while (isRunning) {
                logger("Server wait client connection");
                await server.AcceptSocketAsync ()
                    .ContinueWith (HandleConnection);
            }
            logger ("Server end working");
        }

        private async Task<bool> HandleConnection (Task<Socket> task) {
            try {
                return HandeClient (task.Result);
            } catch (Exception e) {
                logger ($"Server exception: {e}");
                return false;
            }
        }

        private bool HandeClient (Socket socket) {
            logger ("Server: socket connected");
            sockets.Add(socket);
            startClientMesagings(socket);
            return true;
        }

        private async Task startClientMesagings(Socket socket) {
            while(true) {
                var message = SocketHelper.ReadMessage (socket);
                logger ($"Server recieved: {message}");
                SocketHelper.SendMessage (socket, "echo-" + message);
                if (message == ".exit") {
                    isRunning = false;
                    stopAllClient();
                    break;
                }
            }
        }

        private void stopAllClient() {
            sockets.Select(socket => {
                try {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                } catch (Exception e) {
                    logger($"server exceptino: {e}");
                }
                return true;
            });
        }

        public void Stop () {
            server.Stop ();
        }
    }
}