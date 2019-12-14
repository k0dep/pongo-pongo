using Messages;
using Poster;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public IMessageSender Bus = MessageBusStatic.Bus;

    public float MinRadius = 1;
    public float MaxRadius = 3;

    public string TopBorderTag = "TopBorder";
    public string BottomBorderTag = "BottomBorder";

    private void Start()
    {
        var transform = GetComponent<Transform>();
        transform.localScale = Vector3.one * ((Random.value * (MaxRadius - MinRadius)) + MinRadius);

        var trail = GetComponent<TrailRenderer>();
        trail.widthMultiplier *= transform.localScale.x;
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
