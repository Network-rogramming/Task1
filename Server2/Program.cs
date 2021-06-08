using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Server2.Models;

namespace Server2
{
    class Program
    {
        static DataManager db = new DataManager();
        static void Main(string[] args)
        {
            // Настройка парметров подключения к серверу:
            int port = 9001;
            IPAddress addr = IPAddress.Parse("127.0.0.1");
            IPEndPoint ep = new IPEndPoint(addr, port);

            // Создание слушающего сокета:
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.IP);

            try
            {
                // Связываем сокет с точкой подключения и запускаем его на прослушивание:
                listenerSocket.Bind(ep);
                listenerSocket.Listen(10);
                Console.WriteLine("\n> Ожидание запросов на соединение...");

                // Цикл прослушивания запросов на соединение от клиентов:
                while (true)
                {
                    // 1 - Установка соединения и создание принимающего сокета:
                    Socket acceptorSocket = listenerSocket.Accept();

                    // 2-  Принимаем сообщение от клиента, соединение с которым было установлено
                    byte[] buff1 = new byte[1024];
                    int count = acceptorSocket.Receive(buff1);

                    // 3 - Расшифровываем сообщение от клиента:
                    string requestMessage = Encoding.UTF8.GetString(buff1, 0, count);
                    string[] listWord = requestMessage.Split('.');
                    string name = listWord[1].Trim();
                    string displayString = "";
                    if(Char.Parse(listWord[0]) == '1')
                    {
                        List<Group> groups = db.Groups.Where(g => g.Faculty.Name == name).ToList();
                        foreach (var group in groups)
                        {
                            displayString += $" {group.Name}";
                        }
                    }
                    else if(Char.Parse(listWord[0]) == '2')
                    {
                        List<Student> students = db.Students.Where(g => g.Group.Name == name).ToList();
                        foreach (var student in students)
                        {
                            displayString += $" {student.Name}";
                        }
                    }
                    else if(Char.Parse(listWord[0]) == '3')
                    {
                        string[] param = listWord[1].Split(';');
                        string sName = param[0].Trim();
                        string gName = param[1].Trim();
                        Student student = db.Students.Where(s => s.Name == sName).FirstOrDefault();
                        Group group = db.Groups.Where(g => g.Name == gName).FirstOrDefault();
                        if(student != null && group != null)
                        {
                            student.GroupId = group.Id;
                            db.SaveChanges();
                            displayString += "Студент успешно переведен";
                        }
                        else
                        {
                            displayString += "Студент не переведен";
                        }
                    }
                    // 5 - Зашифровываем и отправляем ответное сообщение:
                    byte[] buff2 = Encoding.UTF8.GetBytes(displayString);
                    acceptorSocket.Send(buff2);

                    // 6 - Разрываем соединение с клиентом:
                    acceptorSocket.Shutdown(SocketShutdown.Both);
                    acceptorSocket.Close();

                    // 7 - Проверка ключевого сообщения - команды на остановку сервера:
                    if (requestMessage == "STOP-SERVER-123")
                        break;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine($"\n> Sercer Error: {err.Message}");
            }
            finally
            {
                Console.WriteLine("\n> Сервер остановлен\n");
            }
        }
    }
}
