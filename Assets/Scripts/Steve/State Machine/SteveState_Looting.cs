using UnityEngine;

public class SteveState_Looting : BaseState
{
    public override string Name => "Looting";

    Steve steve;

    public SteveState_Looting(SteveStateMachine sm)
    {
        steve = sm.steve;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{steve.gameObject.name} State: {Name}");

        steve.move.target=steve.closestLoot.transform;
        steve.move.evade=false;
        steve.combat.range=steve.huggyRange;
    }

    protected override void OnUpdate(float deltaTime)
    {

    }

    protected override void OnExit()
    {
    }
}
