using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D Rigidbody;

    public float MaxVelocity = 1;

    void Update()
    {
        if(Input.GetKey(KeyCode.D))
        {
            Rigidbody.AddForce(Vector3.right * MaxVelocity * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            Rigidbody.AddForce(Vector3.left * MaxVelocity * Time.deltaTime);
        }
    }
}
