﻿using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D Rigidbody;

    public float MaxVelocity = 1;

    void Update()
    {
        if(Input.GetKey(KeyCode.D))
        {
            AddForce(1);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            AddForce(-1);
        }
    }

    public void AddForce(float force)
    {
        Rigidbody.AddForce(Vector3.right * force * MaxVelocity * Time.deltaTime);
    }
}