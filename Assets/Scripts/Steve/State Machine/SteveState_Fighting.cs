using UnityEngine;

public class SteveState_Fighting : BaseState
{
    public override string Name => "Fighting";

    Steve steve;

    public SteveState_Fighting(SteveStateMachine sm)
    {
        steve = sm.steve;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{steve.gameObject.name} State: {Name}");

        steve.move.target=steve.closestEnemy.transform;
    }

    protected override void OnUpdate(float deltaTime)
    {
        if(steve.inv.HasItem(Item.Arrow))
        {
            steve.combat.range=steve.longRange;

            if(steve.combat.InRange())
            {
                steve.move.evade=true;
                steve.move.departure=true;

                steve.combat.Attack(steve.bowPrefab);
            }
            else steve.move.evade=false;
        }
        else
        {
            steve.combat.range=steve.meleeRange;

            steve.move.evade=false;

            if(steve.combat.InRange())
            {
                steve.combat.Attack(steve.currentMeleePrefab);
            }
        }
    }

    protected override void OnExit()
    {
    }
}
