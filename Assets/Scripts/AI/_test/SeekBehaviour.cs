using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ForceVehicle2D))]

public class SeekBehaviour : MonoBehaviour
{
    ForceVehicle2D vehicle;
    
    void Awake()
    {
        vehicle=GetComponent<ForceVehicle2D>();
    }
    void FixedUpdate()
    {
        vehicle.Steer(GetVector());
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public Transform target;

    [Header("Seek")]
    public bool arrival=true;
    public float slowingRange=1;
    public float stoppingRange=.05f;
    
    [Header("Flee")]
    public bool flee;

    Vector3 GetVector()
    {
        if(!target) return Vector3.zero;

        Vector3 dirToTarget = flee ?
            GetDir(transform.position, target.position):
            GetDir(target.position, transform.position);

        Vector3 desiredVelocity;

        if(!flee && arrival)
        {
            float distance = Vector3.Distance(target.position, transform.position);

            if(distance<=stoppingRange)
            {
                desiredVelocity=Vector3.zero;
            }
            else
            {
                float rampedSpeed = vehicle.maxSpeed * distance/slowingRange;

                float clippedSpeed = Mathf.Min(rampedSpeed, vehicle.maxSpeed);

                desiredVelocity = clippedSpeed * dirToTarget;
            }
        }
        else desiredVelocity = vehicle.maxSpeed * dirToTarget;

        return desiredVelocity;
    }
    
    Vector3 GetDir(Vector3 targetPos, Vector3 selfPos)
    {
        return (targetPos-selfPos).normalized;
    }
}
