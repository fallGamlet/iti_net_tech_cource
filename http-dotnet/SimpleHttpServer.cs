using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace http {
    public class SimpleHttpServer {
        private HttpListener server;
        private bool isRunning = false;
        private Action<String> logger;
        public SimpleHttpServer (Action<String> logger) {
            this.logger = logger;
            server = new HttpListener ();
            server.Prefixes.Add ("http://127.0.0.1:8000/");
            server.Prefixes.Add ("http://localhost:8000/");
        }

        public void Stop () {
            isRunning = false;
            server.Stop ();
        }

        public void Start () {
            isRunning = true;
            server.Start ();
            new Task (() => {
                while (isRunning) {
                    try {
                        handleConnection (server.GetContext ());
                    } catch (Exception e) {
                        logger ($"HttpServer connection handle error: {e}");
                    }
                }
            }).Start ();
        }

        private void handleConnection (HttpListenerContext context) {
            var request = context.Request;
            logger (">>> Connected");
            logger ($">>> {request.HttpMethod} {request.Url?.AbsoluteUri}");

            if (request.HasEntityBody) {
                var body = readFromStream (request.InputStream);
                logger ($">>> {body}");
            }

            HttpListenerResponse response = context.Response;
            string msg = "{\"status\": \"success\"}";
            byte[] buffer = Encoding.UTF8.GetBytes (msg);
            response.ContentLength64 = buffer.Length;
            Stream st = response.OutputStream;
            st.Write (buffer, 0, buffer.Length);

            response.Close ();
            logger (">>> end handle connection");
        }

        private String readFromStream (Stream stream) {
            var input = new StreamReader (stream, Encoding.UTF8);
            return input.ReadToEnd ();
        }
    }
}