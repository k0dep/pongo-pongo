using Components;
using UnityEngine;

namespace Controllers
{
    public class PlayerMovingController : MonoBehaviour
    {
        public Player Player;

        private bool _movingLeft;
        private bool _movingRight;

        public void BeginLeftMove()
        {
            _movingLeft = true;
        }

        public void EndLeftMove()
        {
            _movingLeft = false;
        }

        public void BeginRightMove()
        {
            _movingRight = true;
        }

        public void EndRightMove()
        {
            _movingRight = false;
        }

        public void Update()
        {
            if(_movingLeft)
            {
                Player.AddForce(-1);
            }
            else if(_movingRight)
            {
                Player.AddForce(1);
            }
        }
    }
}