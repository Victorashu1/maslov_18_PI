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
                // получаем адреса для запуска сокета
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

                // создаем сокет
                Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    // связываем сокет с локальной точкой, по которой будем принимать данные
                    listenSocket.Bind(ipPoint);

                    // начинаем прослушивание
                    listenSocket.Listen(10);

                    Console.WriteLine("Сервер запущен. Ожидание подключений...");

                    while (true)
                    {
                        Socket handler = listenSocket.Accept();
                        // получаем сообщение
                        StringBuilder builder = new StringBuilder();
                        int bytes = 0; // количество полученных байтов
                        byte[] data = new byte[256]; // буфер для получаемых данных

                        do
                        {
                            bytes = handler.Receive(data);
                            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        }
                        while (handler.Available > 0);

                        Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                        // отправляем ответ
                        string message = ""+ Encoding.Unicode.GetString(data, 0, bytes).Length;
                        data = Encoding.Unicode.GetBytes(message);
                        handler.Send(data);
                        // закрываем сокет
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        
    }
    }
