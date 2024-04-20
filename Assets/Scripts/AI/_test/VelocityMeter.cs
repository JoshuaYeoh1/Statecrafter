using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityMeter : MonoBehaviour
{
    public Vector3 velocity;
    Vector3 prevPos;

    void FixedUpdate()
    {
        velocity = transform.position - prevPos / Time.deltaTime;
        prevPos = transform.position;
    }
}
