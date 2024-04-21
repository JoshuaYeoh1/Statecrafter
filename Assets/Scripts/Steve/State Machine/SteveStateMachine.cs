using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteveStateMachine : MonoBehaviour
{
    StateMachine sm;

    [HideInInspector] public Steve steve;

    void Start()
    {
        sm = new StateMachine();

        steve=GetComponent<Steve>();

        // STATES
        /////////////////////////////////////////////////////////////////////////////////////////

        SteveState_Idle idle = new(this);
        SteveState_Mining mining = new(this);
        SteveState_Looting looting = new(this);
        SteveState_Crafting crafting = new(this);
        SteveState_Fighting fighting = new(this);
        SteveState_Fleeing fleeing = new(this);
        SteveState_Sleeping sleeping = new(this);

        // TRANSITIONS
        /////////////////////////////////////////////////////////////////////////////////////////

        idle.AddTransition(mining, (timeInState) =>
        {
            if(
                !steve.CanCraftGoalItem() &&
                !steve.closestLoot &&
                !steve.closestEnemy &&
                !steve.IsLowHP() &&
                steve.GetGoalResource()
            )
            {
                return true;
            }
            return false;
        });

        idle.AddTransition(looting, (timeInState) =>
        {
            if(
                steve.closestLoot &&
                !steve.closestEnemy &&
                !steve.IsLowHP()
            )
            {
                return true;
            }
            return false;
        });

        idle.AddTransition(crafting, (timeInState) =>
        {
            if(
                steve.CanCraftGoalItem() &&
                steve.GetRequiredCraftingStation() &&
                !steve.closestLoot &&
                !steve.closestEnemy &&
                !steve.IsLowHP()
            )
            {
                return true;
            }
            return false;
        });

        idle.AddTransition(fighting, (timeInState) =>
        {
            if(
                steve.closestEnemy &&
                !steve.IsLowHP()
            )
            {
                return true;
            }
            return false;
        });

        idle.AddTransition(fleeing, (timeInState) =>
        {
            if(
                steve.closestEnemy &&
                steve.IsLowHP()
            )
            {
                return true;
            }
            return false;
        });

        idle.AddTransition(sleeping, (timeInState) =>
        {
            if(
                !steve.closestEnemy &&
                steve.IsLowHP()
            )
            {
                return true;
            }
            return false;
        });

        /////////////////////////////////////////////////////////////////////////////////////////

        mining.AddTransition(idle, (timeInState) =>
        {
            if(
                steve.CanCraftGoalItem() ||
                steve.closestLoot ||
                steve.closestEnemy ||
                steve.IsLowHP() ||
                !steve.GetGoalResource()
            )
            {
                return true;
            }
            return false;
        });

        /////////////////////////////////////////////////////////////////////////////////////////
        
        looting.AddTransition(idle, (timeInState) =>
        {
            if(
                !steve.closestLoot ||
                steve.closestEnemy ||
                steve.IsLowHP()
            )
            {
                return true;
            }
            return false;
        });

        /////////////////////////////////////////////////////////////////////////////////////////
        
        crafting.AddTransition(idle, (timeInState) =>
        {
            if(
                !steve.CanCraftGoalItem() ||
                !steve.GetRequiredCraftingStation() ||
                steve.closestLoot ||
                steve.closestEnemy ||
                steve.IsLowHP() ||
                steve.HasCrafted()
            )
            {
                return true;
            }
            return false;
        });

        /////////////////////////////////////////////////////////////////////////////////////////
        
        fighting.AddTransition(idle, (timeInState) =>
        {
            if(
                !steve.closestEnemy ||
                steve.IsLowHP()
            )
            {
                return true;
            }
            return false;
        });

        /////////////////////////////////////////////////////////////////////////////////////////
        
        fleeing.AddTransition(idle, (timeInState) =>
        {
            if(
                !steve.closestEnemy ||
                !steve.IsLowHP()
            )
            {
                return true;
            }
            return false;
        });

        /////////////////////////////////////////////////////////////////////////////////////////
        
        sleeping.AddTransition(idle, (timeInState) =>
        {
            if(
                steve.closestEnemy ||
                steve.IsFullHP()
            )
            {
                return true;
            }
            return false;
        });

        // DEFAULT
        /////////////////////////////////////////////////////////////////////////////////////////
        
        sm.SetInitialState(idle);
    }

    void Update()
    {
        sm.Tick(Time.deltaTime);
    }
}
