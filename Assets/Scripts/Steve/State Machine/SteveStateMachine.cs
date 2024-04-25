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
        SteveState_Death death = new(this);

        // TRANSITIONS
        /////////////////////////////////////////////////////////////////////////////////////////

        idle.AddTransition(mining, (timeInState) =>
        {
            if(
                !steve.CanCraftGoalItem() &&
                !steve.closestLoot &&
                !steve.closestEnemy &&
                !steve.closestHazard &&
                steve.IsOkHP() &&
                steve.goalResource
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
                !steve.IsDead()
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
                steve.goalCraftingStation &&
                !steve.closestLoot &&
                !steve.closestEnemy &&
                !steve.closestHazard &&
                steve.IsOkHP()
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
                !steve.closestLoot &&
                !steve.closestHazard &&
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
                (
                    steve.closestEnemy &&
                    !steve.closestLoot &&
                    steve.IsLowHP() &&
                    !steve.IsDead()
                )
                ||
                (
                    steve.closestHazard &&
                    !steve.IsDead()
                )
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
                !steve.closestHazard &&
                !steve.closestLoot &&
                !steve.IsOkHP() &&
                !steve.IsDead()
            )
            {
                return true;
            }
            return false;
        });

        idle.AddTransition(death, (timeInState) =>
        {
            if(
                steve.IsDead()
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
                steve.closestHazard ||
                !steve.IsOkHP() ||
                !steve.goalResource
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
                steve.IsDead()
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
                !steve.goalCraftingStation ||
                steve.closestLoot ||
                steve.closestEnemy ||
                steve.closestHazard ||
                !steve.IsOkHP() ||
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
                steve.closestLoot ||
                steve.closestHazard ||
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
                (
                    !steve.closestEnemy ||
                    steve.closestLoot ||
                    !steve.IsLowHP() ||
                    steve.IsDead()
                )
                &&
                (
                    !steve.closestHazard ||
                    steve.IsDead()
                )
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
                steve.closestHazard ||
                steve.closestLoot ||
                steve.IsFullHP() ||
                steve.IsDead()
            )
            {
                return true;
            }
            return false;
        });

        /////////////////////////////////////////////////////////////////////////////////////////
        
        death.AddTransition(idle, (timeInState) =>
        {
            if(
                !steve.IsDead()
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
