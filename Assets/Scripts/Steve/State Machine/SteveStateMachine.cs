using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteveStateMachine : MonoBehaviour
{
    StateMachine sm;

    [HideInInspector] public Steve steve;

    void Awake()
    {
        sm = new StateMachine();

        steve=GetComponent<Steve>();

        // 1. Create all possible states        
        SteveState_Idle idle = new(this);
        SteveState_Mining mining = new(this);

        // 2. Set all transitions
        /////////////////////////////////////////////////////////////////////////////////////////

        idle.AddTransition(mining, (timeInState) =>
        {
            // if(condition)
            // {
            //     return true;
            // }
            return false;
        });

        /////////////////////////////////////////////////////////////////////////////////////////


        /////////////////////////////////////////////////////////////////////////////////////////
        
        sm.SetInitialState(idle);
    }

    void Update()
    {
        sm.Tick(Time.deltaTime);
    }
}
