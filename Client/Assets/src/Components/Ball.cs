using Messages;
using Poster;
using UnityEngine;

namespace Components
{
    public class Ball : MonoBehaviour
    {
        public IMessageSender Bus = MessageBusStatic.Bus;

        public float MinRadius = 1;
        public float MaxRadius = 2;

        public string TopBorderTag = "TopBorder";
        public string BottomBorderTag = "BottomBorder";

        private void Start()
        {
            var transform = GetComponent<Transform>();
            var multiplier = ((Random.value * (MaxRadius - MinRadius)) + MinRadius);
            transform.localScale = transform.localScale * multiplier;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag(TopBorderTag))
            {
                Bus.Send(new CollideTopBorderMessage());
            }
            else if (col.gameObject.CompareTag(BottomBorderTag))
            {
                Bus.Send(new CollideBottomBorderMessage());
            }
        }
    }
}