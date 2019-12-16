using System;
using Telepathy;

namespace pongo_pongo
{
    class Program
    {
        static void Main(string[] args)
        {
            var port = int.Parse(Environment.GetEnvironmentVariable("PP_PORT") ?? "9876");

            var server = new Server();
            server.Start(port);

            var isExiting = false;

            AppDomain.CurrentDomain.ProcessExit += (_, __) =>
            {
                isExiting = true;
            };

            var service = new PongoServerService(server);

            while (!isExiting)
            {
                if (!server.GetNextMessage(out var msg))
                {
                    continue;
                }

                try
                {
                    switch (msg.eventType)
                    {
                        case EventType.Connected:
                            service.ClientConnected(msg.connectionId);
                            break;
                        case EventType.Data:
                            service.OnDataRecv(msg.connectionId, msg.data);
                            break;
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

            server.Stop();
        }
    }
}
