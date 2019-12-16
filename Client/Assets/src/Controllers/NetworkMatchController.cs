using System;
using Messages;
using Poster;
using UnityEngine;

namespace Controllers
{
    public class NetworkMatchController : MonoBehaviour
    {
        public IMessageBinder Binder = MessageBusStatic.Bus;
        public IMessageSender Sender = MessageBusStatic.Bus;

        public GameState State;

        public Enemy Enemy;
        public Player Player;

        private Transform _ball;
        private float _lastStateSend = 0;

        public void OnEnable()
        {
            Binder.Bind<BallSpawnedMessage>(BallSpawned);
            Binder.Bind<WorldStateMessage>(HandleWorldState);
            Binder.Bind<PartnetPlatformMessage>(HandlePartnerState);
        }

        public void OnDisable()
        {
            Binder.UnBind<BallSpawnedMessage>(BallSpawned);
            Binder.UnBind<WorldStateMessage>(HandleWorldState);
            Binder.UnBind<PartnetPlatformMessage>(HandlePartnerState);
        }

        public void Start()
        {
            if (State.IsNetworkMatch)
            {
                Enemy.AiEnaled = false;
            }
            else
            {
                enabled = false;
            }
        }

        public void Update()
        {
            if (Time.unscaledTime - _lastStateSend < State.SendRate)
            {
                return;
            }
            _lastStateSend = Time.unscaledTime;

            if (State.IsPlayerHasAuthority)
            {
                ReplicateWorldState();
            }
            else
            {
                ReplicateSelfPlatform();
            }
        }

        private void ReplicateSelfPlatform()
        {
            var world = new ReplicatePartnetPlatformMessage
            {
                PartnerPosition = Player.transform.localPosition.x,
                PartnerVelocity = Player.GetComponent<Rigidbody2D>().velocity.x
            };

            Sender.Send(world);
        }

        private void ReplicateWorldState()
        {
            var world = new ReplicateWorldStateMessage
            {
                PartnerPosition = Player.transform.localPosition.x,
                PartnetVelocity = Player.GetComponent<Rigidbody2D>().velocity.x,
                BallPosition = _ball.transform.localPosition,
                BallVelocity = _ball.GetComponent<Rigidbody2D>().velocity
            };

            Sender.Send(world);
        }

        private void BallSpawned(BallSpawnedMessage obj)
        {
            _ball = obj.BallObject.transform;
        }

        private void HandleWorldState(WorldStateMessage obj)
        {
            Enemy.transform.localPosition = new Vector3(-obj.PartnerPosition, Enemy.transform.localPosition.y, Enemy.transform.localPosition.z);
            Enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(-obj.PartnetVelocity, 0);

            _ball.transform.localPosition = new Vector3(-obj.BallPosition.x, -obj.BallPosition.y, 0);
            _ball.GetComponent<Rigidbody2D>().velocity = -obj.BallVelocity;
        }

        private void HandlePartnerState(PartnetPlatformMessage obj)
        {
            Enemy.transform.localPosition = new Vector3(-obj.PartnerPosition, Enemy.transform.localPosition.y, Enemy.transform.localPosition.z);
            Enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(-obj.PartnerVelocity, 0);
        }
    }
}