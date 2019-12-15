using Poster;
using Messages;
using UnityEngine;

namespace Controllers
{
    public class GameCycleController : MonoBehaviour
    {
        public IMessageBinder Bus = MessageBusStatic.Bus;
        
        public BallLifecycleService BallService;
        
        public void OnEnable()
        {
            Bus.Bind<CollideTopBorderMessage>(HandleBallTopCollided);
            Bus.Bind<CollideBottomBorderMessage>(HandleBallBottomCollided);
        }

        public void OnDisable()
        {
            Bus.UnBind<CollideTopBorderMessage>(HandleBallTopCollided);
            Bus.UnBind<CollideBottomBorderMessage>(HandleBallBottomCollided);
        }

        private void HandleBallBottomCollided(CollideBottomBorderMessage obj)
        {
            BallService.Respawn();
        }

        private void HandleBallTopCollided(CollideTopBorderMessage obj)
        {
            BallService.Respawn();
        }
    }
}