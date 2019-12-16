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
        }

        public void OnDisable()
        {
            Binder.UnBind<BallSpawnedMessage>(BallSpawned);
            Binder.UnBind<WorldStateMessage>(HandleWorldState);
        }

        public void Update()
        {
            if (!State.IsPlayerHasAuthority)
            {
                return;
            }

            if (Time.unscaledTime - _lastStateSend < State.SendRate)
            {
                return;
            }

            _lastStateSend = Time.unscaledTime;

            var world = new ReplicateWorldStateMessage
            {
                PartnerPosition = Player.transform.position.x,
                PartnetVelocity = Player.GetComponent<Rigidbody2D>().velocity.x,
                BallPosition = _ball.transform.position,
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
            Enemy.AiEnaled = false;
            Enemy.transform.position = new Vector3(obj.PartnerPosition, Enemy.transform.position.y, Enemy.transform.position.z);
            Enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(obj.PartnetVelocity, 0);
            _ball.transform.position = new Vector3(obj.BallPosition.x, obj.BallPosition.y, 0);
            _ball.GetComponent<Rigidbody2D>().velocity = obj.BallVelocity;

            Debug.Log("World replicated");
        }

    }
}