using UnityEngine;

namespace Messages
{
    public class CollideBottomBorderMessage { }
    public class CollideTopBorderMessage { }

    public class BallSpawnedMessage
    {
        public GameObject BallObject;
    }

    public class DisconnectServerMessage { }

    public class ConnectedServerMessage { }

    public class StartRoomSessionMessage
    {
        public readonly bool IsAuthority;

        public StartRoomSessionMessage(bool isAuthority)
        {
            this.IsAuthority = isAuthority;
        }
    }

    public class StartSearchRoomMessage { }

    public class WorldStateMessage
    {
        public float PartnerPosition { get; set; }
        public float PartnetVelocity { get; set; }
        public Vector2 BallPosition { get; set; }
        public Vector2 BallVelocity { get; set; }
    }

    public class ReplicateWorldStateMessage : WorldStateMessage
    {
    }
}