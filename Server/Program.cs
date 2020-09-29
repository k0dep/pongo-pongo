using System;
using System.Text;
using System.Threading;
using Telepathy;

namespace pongo_pongo
{
    class Program
    {
        static void Main(string[] args)
        {
            // получаем порт или устанавливаем порт по умолчанию
            var port = int.Parse(Environment.GetEnvironmentVariable("PP_PORT") ?? "9876");

            // создаем и запускаем сервер
            var server = new Server();
            server.Start(port);

            var isExiting = false;

            // при выходе проставляем фалжек что нужно оканчивать обработку соединий
            AppDomain.CurrentDomain.ProcessExit += (_, __) =>
            {
                isExiting = true;
            };

            // создаем сервис для обработки бизнес логики сервера
            var service = new PongoServerService(server);

            // в бесконечном цикле обрабатывем поток сообщений от сервера пока не поднимется флажек завершения работы сервера
            while (!isExiting)
            {
                // пвтаемся взять следующее сообщение
                if (!server.GetNextMessage(out var msg))
                {
                    // если сообщений нету, то ждем несколько миллисекунд
                    Thread.Sleep(1);
                    continue; // и переходи на новую итерацию
                }

                try
                {
                    // по типу сообщения обрабатывем соответственной логикой
                    switch (msg.eventType)
                    {
                        case EventType.Connected:
                            service.ClientConnected(msg.connectionId); // обработка подключения нового соеднинения
                            break;
                        
                        // обработка сообщения с данными от клиента
                        case EventType.Data:
                            // из массива байт получаем строку
                            var data = Encoding.UTF8.GetString(msg.data);
                            // в этой строке json, парсим его
                            var json = SimpleJSON.JSON.Parse(data);
                            // оброабатываем сообщение бизнес-логикой
                            service.OnDataRecv(msg.connectionId, json);
                            break;
                        
                        // обработка отключения клиента
                        case EventType.Disconnected:
                            service.ClientDisconnected(msg.connectionId);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unhandled exception: {e.ToString()}");
                }
            }

            Console.WriteLine("Process: Exiting");
            
            // остановка сервера
            server.Stop();
        }
    }
}
