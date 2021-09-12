using System;

namespace net_tech_cource
{
  class Program {
        static void Main (string[] args) {
            Console.WriteLine ("Hello World!");
            var port = 13000;
            var address = "127.0.0.1";

            var server = new SimpleServer (address, port, logger);
            var serverTask = server.Start ();

            var client = GenerateClient(address, port);
            client.Connect();

            serverTask.Wait();
            Console.WriteLine ("End of program");
        }

        private static SimpleClient GenerateClient(string address, int port) {
            return new SimpleClient(
                Guid.NewGuid().ToString(),
                address,
                port,
                logger,
                input
            );
        }
        private static void logger(string text) {
            Console.WriteLine(text);
        }

        private static string input() {
            return Console.ReadLine();
        }
    }
}