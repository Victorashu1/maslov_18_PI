using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApp5
{
    class Program
    {
        private const int port = 8888;
        private const string host = "127.0.0.1";


        static void Main(string[] args)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(host), port);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                socket.Connect(ipPoint);
                Console.Write("Введите сообщение:");
                string message = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);

                // получаем ответ
                data = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт
                int reuslt_count;
                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Convert.ToInt32(Encoding.Unicode.GetString(data, 0, bytes)));
                }
                while (socket.Available > 0);

                reuslt_count = Convert.ToInt32(builder.ToString());
                Console.WriteLine("ответ сервера: " + reuslt_count);

                // закрываем сокет
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}

