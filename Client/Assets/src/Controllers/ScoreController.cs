using Poster;
using Messages;
using UnityEngine;
using System;

namespace Controllers
{
    public class ScoreController : MonoBehaviour
    {
        public IMessageBinder Bus = MessageBusStatic.Bus;
        public GameState State;

        public void Start()
        {
            State.CurrentScore = 0;
        }
        
        public void OnEnable()
        {
            Bus.Bind<CollideTopBorderMessage>(HandleBallTopCollided);
        }

        public void OnDisable()
        {
            Bus.UnBind<CollideTopBorderMessage>(HandleBallTopCollided);
        }

        private void HandleBallTopCollided(CollideTopBorderMessage obj)
        {
            State.CurrentScore += 1;
            State.BestScore = Math.Max(State.CurrentScore, State.BestScore);
        }
    }
}