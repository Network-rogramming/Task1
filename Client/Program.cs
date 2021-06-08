using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // Настройка парметров подключения к серверу:
            int port = 9001;
            IPAddress addr = IPAddress.Parse("127.0.0.1");
            IPEndPoint ep = new IPEndPoint(addr, port);

            // Цикл работы клиента (с перезапуском):
            try
            {
                string choice = "y";
                while (choice != "n")
                {
                    // 1 - Формируем и зашифровываем сообщение для сервера:
                    Console.WriteLine("1. Группы по названию факультета");
                    Console.WriteLine("2. Студенты по названию группы");
                    Console.WriteLine("3. Перевод студента: ('Имья студента'); (В группу в какую перевести)");
                    Console.Write("\n> Выберите нужный вариант и введите название: ");
                    string mess = Console.ReadLine();
                    byte[] buff1 = Encoding.UTF8.GetBytes(mess);

                    // 2 - Создаем сеансовый сокет:
                    Socket sessionSocket = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.IP);

                    // 3 - Отправляем запрос на соединение с сервером и стартовое сообщение:
                    sessionSocket.Connect(ep);
                    sessionSocket.Send(buff1);

                    // 4 - Принимаем ответное сообщение сервера:
                    byte[] buff2 = new byte[1024];
                    int count = sessionSocket.Receive(buff2);

                    // 5 - Расшифровываем сообщение сервера:
                    string responseMessage = Encoding.UTF8.GetString(buff2, 0, count);
                    Console.WriteLine($"  Ответ сервера: {responseMessage}");

                    // 6 - Разрываем соединение с сервером:
                    sessionSocket.Shutdown(SocketShutdown.Both);
                    sessionSocket.Close();

                    // 7 - Проверка условия выхода из программы:
                    Console.Write("\n> Продолжать (y/n)? - ");
                    choice = Console.ReadLine();
                }
            }
            catch (Exception err)
            {
                Console.WriteLine($"\n> Server Error: {err.Message}");
            }
            finally
            {
                Console.WriteLine("\n> Клиент остановлен\n");
            }
        }
    }
}
