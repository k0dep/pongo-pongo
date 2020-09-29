using System;
using System.Collections.Generic;
using SimpleJSON;
using Telepathy;

namespace pongo_pongo
{
    public class PongoServerService
    {
        private readonly Server _server;

        // таблички соответстсия игрока к комнате или другому игроку
        private readonly Dictionary<int, int> _connectionToRoom;
        private readonly Dictionary<int, (int, int)> _roomToConnections;
        private readonly Dictionary<int, int> _partners;

        private int _lastRoomId = 0;

        private int _queuedClient = int.MinValue;

        public PongoServerService(Server server)
        {
            _server = server;
            _connectionToRoom = new Dictionary<int, int>();
            _roomToConnections = new Dictionary<int, (int, int)>();
            _partners = new Dictionary<int, int>();
        }

        public void ClientConnected(int connectionId)
        {
            Console.WriteLine($"+ Client connected {connectionId}");
            if (_queuedClient == connectionId)
            {
                _queuedClient = int.MinValue;
            }
        }

        public void ClientDisconnected(int connectionId)
        {
            Console.WriteLine($"- Client disconnected {connectionId}");
            if (_queuedClient == connectionId)
            {
                _queuedClient = int.MinValue;
            }
        }

        // Метод обработки сообщения
        public void OnDataRecv(int connectionId, JSONNode data)
        {

            switch(data["type"].ToString()) // обрабатываем сообщение по полю с типом сообщения
            {
                case "queue":
                    QueueClient(connectionId);
                break;

                case "broadcast":
                    HandleState(connectionId, data);
                break;

                default:
                    throw new Exception($"Not known event {data[0]} from connection {connectionId}");
            }
        }

        // обработка сообщения по ретнарсляции сообщения от клиента к клиенту
        private void HandleState(int connectionId, JSONNode data)
        {
            var partner = _partners[connectionId];
            _server.Send(partner, data.GetBytes());
        }

        // обработка сообщения установки в очередь
        private void QueueClient(int connectionId)
        {
            // если в очереди никого нету или в очереди тот же игрок
            if (_queuedClient == int.MinValue || _queuedClient == connectionId)
            {
                // то игнорируем этот сценарий
                _queuedClient = connectionId;
                Console.WriteLine($"Client with connection id {connectionId} is queued for room");
                return;
            }

            // отправляем игрокам в одной комнате сообщенние о присоединении к комнате
            _server.Send(connectionId, new {
                type = "player_found",
                isAuthority = false // у первого нет авторитета
            }.JsonBytes());

            _server.Send(_queuedClient, new {
                type = "player_found",
                isAuthority = true // у второго авторитет есть
            }.JsonBytes());

            ++_lastRoomId;

            // устанавливаем все связи и таблички для быстрого поиска игроков в комнатах
            _roomToConnections[_lastRoomId] = (connectionId, _queuedClient);
            _connectionToRoom[_queuedClient] = _lastRoomId;
            _connectionToRoom[connectionId] = _lastRoomId;
            _partners[_queuedClient] = connectionId;
            _partners[connectionId] = _queuedClient;

            Console.WriteLine($"Create room {_lastRoomId} with clients {_queuedClient} and {connectionId}");

            // очищаем очередь
            _queuedClient = int.MinValue;
        }
    }
}