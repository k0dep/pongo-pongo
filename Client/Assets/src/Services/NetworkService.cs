using System.IO;
using Messages;
using Poster;
using Telepathy;
using UnityEngine;
using EventType = Telepathy.EventType;

namespace Services
{
    public class NetworkService : MonoBehaviour
    {
        enum ClientMessages : byte
        {
            PlayerFound = 0xF1,
            WorldState = 0xF2,
            Platform = 0xF3
        }

        enum ServerMessages : byte
        {
            Queue = 0x1,
            Broadcast = 0x2
        }

        public string Host = "127.0.0.1";
        public int Port = 9876;

        public IMessageSender Sender = MessageBusStatic.Bus;
        public IMessageBinder Binder = MessageBusStatic.Bus;

        private Client _client;

        void Awake()
        {
            DontDestroyOnLoad(this);

            _client = new Client();
            _client.Connect(Host, Port);

            while(_client.Connecting);

            Application.runInBackground = true;

            Telepathy.Logger.Log = Debug.Log;
            Telepathy.Logger.LogWarning = Debug.LogWarning;
            Telepathy.Logger.LogError = Debug.LogError;

            Binder.Bind<StartSearchRoomMessage>(StartSearchRoom);
            Binder.Bind<ReplicateWorldStateMessage>(ReplicateWorld);
            Binder.Bind<ReplicatePartnetPlatformMessage>(ReplicatePartnerPlatform);
        }

        void OnDestroy()
        {
            _client.Disconnect();
        }

        void Update()
        {
            if (!_client.Connected)
            {
                return;
            }

            while (_client.GetNextMessage(out var msg))
            {
                switch (msg.eventType)
                {
                    case EventType.Connected:
                        Sender.Send(new ConnectedServerMessage());
                        break;

                    case EventType.Data:
                        ParseMessage(msg.data);
                        break;

                    case EventType.Disconnected:
                        Sender.Send(new DisconnectServerMessage());
                        break;
                }
            }
        }

        private void ParseMessage(byte[] data)
        {

            switch ((ClientMessages)data[0])
            {
                case ClientMessages.PlayerFound:
                    var isAuthority = data[1] == 0xFF;
                    Sender.Send(new StartRoomSessionMessage(isAuthority));
                    break;

                case ClientMessages.WorldState:
                    using (var stream = new MemoryStream(data))
                    using (var reader = new BinaryReader(stream))
                    {
                        reader.ReadByte();
                        var stateMessage = new WorldStateMessage
                        {
                            PartnerPosition = reader.ReadSingle(),
                            PartnetVelocity = reader.ReadSingle(),
                            BallPosition = new Vector2(reader.ReadSingle(), reader.ReadSingle()),
                            BallVelocity = new Vector2(reader.ReadSingle(), reader.ReadSingle())
                        };
                        Sender.Send(stateMessage);
                    }
                    break;

                case ClientMessages.Platform:
                    using (var stream = new MemoryStream(data))
                    using (var reader = new BinaryReader(stream))
                    {
                        reader.ReadByte();
                        var stateMessage = new PartnetPlatformMessage
                        {
                            PartnerPosition = reader.ReadSingle(),
                            PartnerVelocity = reader.ReadSingle()
                        };
                        Sender.Send(stateMessage);
                    }
                    break;

                default:
                    Debug.LogError($"Not known event {data[0]:X}");
                    break;
            }
        }


        private void StartSearchRoom(StartSearchRoomMessage obj)
        {
            Debug.Log("Start search room");
            _client.Send(new[] { (byte)ServerMessages.Queue });
        }

        private void ReplicateWorld(ReplicateWorldStateMessage obj)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write((byte)ServerMessages.Broadcast);
                writer.Write((byte)ClientMessages.WorldState);
                writer.Write(obj.PartnerPosition);
                writer.Write(obj.PartnetVelocity);
                writer.Write(obj.BallPosition.x); writer.Write(obj.BallPosition.y);
                writer.Write(obj.BallVelocity.x); writer.Write(obj.BallVelocity.y);

                _client.Send(stream.ToArray());
            }
        }

        private void ReplicatePartnerPlatform(ReplicatePartnetPlatformMessage obj)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write((byte)ServerMessages.Broadcast);
                writer.Write((byte)ClientMessages.Platform);
                writer.Write(obj.PartnerPosition);
                writer.Write(obj.PartnerVelocity);

                _client.Send(stream.ToArray());
            }
        }
    }
}