using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float MinRadius = 1;
    public float MaxRadius = 3;

    void Start()
    {
        var transform = GetComponent<Transform>();
        transform.localScale = Vector3.one * ((Random.value * (MaxRadius - MinRadius)) + MinRadius);

        var trail = GetComponent<TrailRenderer>();
        trail.widthMultiplier *= transform.localScale.x;
    }

}
