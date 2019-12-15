using UnityEngine;

public class AutoAddForce : MonoBehaviour
{
    public Rigidbody2D Rigidbody;
    public Vector2 ForceCoefficient = Vector2.one;

    void Start()
    {
        var corner = Vector2.one;

        if(Random.Range(0, 2) == 1)
        {
            corner.x *= -1;
        }

        if(Random.Range(0, 2) == 1)
        {
            corner.y *= -1; 
        }

        var rotate = Random.Range(-1f, 1f) * 20f;

        var rotation = Quaternion.Euler(0, 0, rotate) * corner;

        Rigidbody.velocity = new Vector2(rotation.x, rotation.y) * ForceCoefficient;
    }
}
