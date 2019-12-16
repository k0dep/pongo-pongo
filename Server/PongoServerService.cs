using System;
using System.Collections.Generic;
using Telepathy;

namespace pongo_pongo
{
    public class PongoServerService
    {
        enum Messages : byte
        {
            Queue = 0x1,
            Broadcast = 0x2
        }

        enum ClientMessages : byte
        {
            PlayerFound = 0xF1
        }

        private readonly Server _server;

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

        public void OnDataRecv(int connectionId, byte[] data)
        {

            switch((Messages)data[0])
            {
                case Messages.Queue:
                    QueueClient(connectionId);
                break;

                case Messages.Broadcast:
                    HandleState(connectionId, data[1..]);
                break;

                default:
                    throw new Exception($"Not known event {data[0]} from connection {connectionId}");
            }
        }

        private void HandleState(int connectionId, byte[] v)
        {
            var partner = _partners[connectionId];
            _server.Send(partner, v);
        }

        private void QueueClient(int connectionId)
        {
            if (_queuedClient == int.MinValue || _queuedClient == connectionId)
            {
                _queuedClient = connectionId;
                Console.WriteLine($"Client with connection id {connectionId} is queued for room");
                return;
            }

            _server.Send(connectionId, new byte[] { (byte)ClientMessages.PlayerFound, 0 });
            _server.Send(_queuedClient, new byte[] { (byte)ClientMessages.PlayerFound, 0xFF });

            ++_lastRoomId;

            _roomToConnections[_lastRoomId] = (connectionId, _queuedClient);
            _connectionToRoom[_queuedClient] = _lastRoomId;
            _connectionToRoom[connectionId] = _lastRoomId;
            _partners[_queuedClient] = connectionId;
            _partners[connectionId] = _queuedClient;

            Console.WriteLine($"Create room {_lastRoomId} with clients {_queuedClient} and {connectionId}");

            _queuedClient = int.MinValue;
        }
    }
}