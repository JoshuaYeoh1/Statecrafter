using UnityEngine;

public class SteveState_Mining : BaseState
{
    public override string Name => "Mining";

    Steve steve;

    public SteveState_Mining(SteveStateMachine sm)
    {
        steve = sm.steve;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{steve.gameObject.name} State: {Name}");

        steve.move.target=steve.GetTargetResource().transform;
        steve.move.evade=false;
        steve.combat.range=steve.meleeRange;
    }

    protected override void OnUpdate(float deltaTime)
    {
        if(steve.combat.InRange())
        {
            steve.combat.Attack(steve.currentMeleePrefab);
        }
    }

    protected override void OnExit()
    {
    }
}
