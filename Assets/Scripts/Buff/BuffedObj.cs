using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffedObj
{
    public GameObject obj;

    public BuffedObj(GameObject newObj)
    {
        obj=newObj;
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public Dictionary<Buff, float> buffDurations = new();

    public void UpdateDuration()
    {
        List<Buff> buffsToRemove = new();

        List<Buff> keys = new(buffDurations.Keys);

        foreach(Buff buff in keys)
        {
            float duration = buffDurations[buff];

            duration -= Time.deltaTime;

            if(duration<=0)
            {
                buffsToRemove.Add(buff);
            }
            else
            {
                buffDurations[buff] = duration; // update dict value
            }
        }

        foreach(Buff buff in buffsToRemove)
        {
            EventManager.Current.OnRemoveBuff(obj, buff);
        }
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public void AddBuff(Buff newBuff, float duration)
    {
        if(duration<=0) return;

        if(!buffDurations.ContainsKey(newBuff))
        {
            buffDurations[newBuff] = duration;
        }
        else buffDurations[newBuff] += duration; // stack duration

        UpdateEffects();
    }

    public void RemoveBuff(Buff buff)
    {
        if(buffDurations.ContainsKey(buff))
        {
            buffDurations.Remove(buff);

            UpdateEffects();
        }
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public void UpdateEffects()
    {
        Speed();
    }

    public void Speed()
    {
        if(obj.TryGetComponent(out ForceVehicle2D vehicle))
        {
            if(buffDurations.ContainsKey(Buff.Speed))
            {
                vehicle.maxSpeed = vehicle.defMaxSpeed*2;
                vehicle.turnSpeed = vehicle.defTurnSpeed*2;
            }
            else
            {
                vehicle.maxSpeed = vehicle.defMaxSpeed;
                vehicle.turnSpeed = vehicle.defTurnSpeed;
            }
        }
    }

    
}
