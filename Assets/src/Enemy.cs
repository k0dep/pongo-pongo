using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D Rigidbody;
    public Transform Ball;

    public float MaxVelocity = 1;

    void Update()
    {
        if(transform.position.x < Ball.position.x)
        {
            Rigidbody.AddForce(Vector3.right * MaxVelocity * Time.deltaTime);
        }
        else
        {
            Rigidbody.AddForce(Vector3.left * MaxVelocity * Time.deltaTime);
        }
    }
}
