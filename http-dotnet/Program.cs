using System;
using System.Net;
using System.IO;
using System.Text;

namespace http
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Simple Http Server");
            var server = new SimpleHttpServer(Log);
            server.Start();

            Console.WriteLine("Press any key for exit");
            Console.ReadLine();
            Console.WriteLine("Program completed");
        }

        private static void Log(string text) {
            Console.WriteLine(text);
        }
    }
}
