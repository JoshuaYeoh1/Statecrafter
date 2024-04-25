using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Buff
{
    Speed,
}

public class BuffManager : MonoBehaviour
{
    public static BuffManager Current;

    void Awake()
    {
        Current=this;
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void OnEnable()
    {
        EventManager.Current.AddBuffEvent += OnAddBuff;
        EventManager.Current.RemoveBuffEvent += OnRemoveBuff;
    }
    void OnDisable()
    {
        EventManager.Current.AddBuffEvent -= OnAddBuff;
        EventManager.Current.RemoveBuffEvent -= OnRemoveBuff;
    }
            
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public List<BuffedObj> buffedObjs = new();

    BuffedObj GetBuffedObj(GameObject target)
    {
        return buffedObjs.Find(obj => obj.obj == target);
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void OnAddBuff(GameObject target, Buff newBuff, float duration)
    {
        BuffedObj buffedObj = GetBuffedObj(target);

        if(buffedObj==null) // if not in the list, construct and add new
        {
            buffedObj = new BuffedObj(target);
            buffedObjs.Add(buffedObj);
        }

        buffedObj.AddBuff(newBuff, duration);
    }

    void OnRemoveBuff(GameObject target, Buff buff)
    {
        BuffedObj buffedObj = GetBuffedObj(target);

        if(buffedObj==null) return; // dont care if not in list

        buffedObj.RemoveBuff(buff);
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Update()
    {
        foreach(BuffedObj buffedObj in buffedObjs)
        {
            buffedObj.UpdateDuration();
        }
    }
}
