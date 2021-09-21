using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Server
{
    class Program
    {
        static Random random = new Random();
        const int port = 8888; // порт для прослушивания подключений
        static void Main(string[] args) 
        {
            
        }
        static void Main1(string[] args)
        {
            TcpListener server = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, port);
                server.Start();

                while (true)
                {
                    Console.WriteLine("Ожидание подключений... ");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Подключен клиент. Выполнение запроса...");

                    HandleClient(client);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (server != null)
                    server.Stop();
            }
        }

        static async Task<String> HandleClient(TcpClient client)
        {
            string msg = "ʕ•́ᴥ•̀ʔっ ";
            NetworkStream stream = client.GetStream();
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(random.Next(100));
            foreach (var num in Enumerable.Range(1, 1))
            {
                byte[] data = Encoding.UTF8.GetBytes($"{num}{msg}");
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Отправлено сообщение: {0}", msg);
            }

            stream.Close();
            client.Close();
            return "ok";
        }
    }
}
