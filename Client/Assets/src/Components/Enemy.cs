using Messages;
using Poster;
using UnityEngine;

namespace Components
{
    public class Enemy : MonoBehaviour
    {
        public IMessageBinder Bus = MessageBusStatic.Bus;

        public Rigidbody2D Rigidbody;
        public bool AiEnaled = true;

        private Transform _ball;

        public float MaxVelocity = 1;

        public void OnEnable()
        {
            Bus.Bind<BallSpawnedMessage>(BallSpawned);
        }

        public void OnDisable()
        {
            Bus.UnBind<BallSpawnedMessage>(BallSpawned);
        }

        private void BallSpawned(BallSpawnedMessage obj)
        {
            _ball = obj.BallObject.transform;
        }

        void Update()
        {
            if (_ball == null || !AiEnaled)
            {
                return;
            }

            if (transform.position.x < _ball.position.x)
            {
                Rigidbody.AddForce(Vector3.right * MaxVelocity * Time.deltaTime);
            }
            else
            {
                Rigidbody.AddForce(Vector3.left * MaxVelocity * Time.deltaTime);
            }
        }
    }
}