using UnityEngine;

public class AutoAddForce : MonoBehaviour
{
    public Rigidbody2D Rigidbody;
    public Vector2 ForceCoefficient = Vector2.one;

    void Start()
    {
        Rigidbody.velocity = new Vector2(Random.value * 2 - 1, Random.value * 2 - 1) * ForceCoefficient;
    }
}
