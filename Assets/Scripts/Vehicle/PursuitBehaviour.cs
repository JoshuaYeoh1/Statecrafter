using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ForceVehicle2D))]

public class PursuitBehaviour : MonoBehaviour
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
    
    [Header("Pursuit")]
    public bool arrival=true;
    public float slowingRange=1;
    public float stoppingRange=.05f;
    
    [Header("Evasion")]
    public bool evade;

    Vector3 GetVector()
    {
        if(!target) return Vector3.zero;

        Vector3 predictedDir = GetPredictedDir();

        Vector3 desiredVelocity;

        if(!evade && arrival)
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

                desiredVelocity = clippedSpeed * predictedDir;
            }
        }
        else desiredVelocity = vehicle.maxSpeed * predictedDir;

        return desiredVelocity;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Vector3 GetPredictedDir()
    {
        Vector3 dirToTarget = evade ?
            GetDir(transform.position, target.position):
            GetDir(target.position, transform.position);

        Vector3 targetsVelocity = GetTargetVelocity();

        float dot = Vector3.Dot(dirToTarget, targetsVelocity.normalized);
        
        Vector3 predictedTargetPos;

        if((!evade && dot >= -0.9239f) || (evade && dot <= -0.9239f))
        {
            predictedTargetPos = GetFuturePos(vehicle.maxSpeed, targetsVelocity);
        }
        else predictedTargetPos = target.position;

        Vector3 predictedDir = evade ?
            GetDir(transform.position, predictedTargetPos):
            GetDir(predictedTargetPos, transform.position);
        
        return predictedDir;
    }

    Vector3 prevTargetPos;

    Vector3 GetTargetVelocity()
    {
        Vector3 velocity = (target.position - prevTargetPos) / Time.deltaTime;
        prevTargetPos = target.position;
        return velocity;
    }
        
    Vector3 GetFuturePos(float speed, Vector3 targetsVelocity)
    {
        float distance = Vector3.Distance(target.position, transform.position);

        float timeToReach = distance / speed;

        return target.position + (targetsVelocity * timeToReach);
    }

    Vector3 GetDir(Vector3 targetPos, Vector3 selfPos)
    {
        return (targetPos-selfPos).normalized;
    }
}
