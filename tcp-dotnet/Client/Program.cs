using System;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        private const int port = 8888;
        private const string host = "127.0.0.1";
 
        static void Main(string[] args)
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(host, port);
                 
                byte[] buffer = new byte[256];
                StringBuilder response = new StringBuilder();
                NetworkStream stream = client.GetStream();
                 
                do
                {
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    response.Append(Encoding.UTF8.GetString(buffer, 0, bytes));
                }
                while (stream.DataAvailable); // пока данные есть в потоке
             
                Console.WriteLine(response.ToString());
 
                // Закрываем потоки
                stream.Close();
                client.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
 
            Console.WriteLine("Запрос завершен...");
            Console.Read();
        }
    }
}
