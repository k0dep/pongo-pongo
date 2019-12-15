using System;
using Poster;
using UnityEngine;

namespace Controllers
{
    public class BallLifecycleService : MonoBehaviour
    {
        public IMessageSender Bus = MessageBusStatic.Bus;
        public GameObject BallPrefab;

        private GameObject _currentBall;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Respawn();
            }
        }

        public void Start ()
        {
            Spawn();
        }

        public void Spawn()
        {
            var newBall = Instantiate(BallPrefab);
            Bus.Send(new Messages.BallSpawnedMessage
            {
                BallObject = newBall
            });
            _currentBall = newBall;
        }
        
        public void Respawn()
        {
            Destroy(_currentBall);
            Spawn();
        }
    }
}