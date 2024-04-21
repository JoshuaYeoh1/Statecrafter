using UnityEngine;

public class SteveState_Idle : BaseState
{
    public override string Name => "Idle";

    Steve steve;

    public SteveState_Idle(SteveStateMachine sm)
    {
        steve = sm.steve;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{steve.gameObject.name} State: {Name}");
    }

    protected override void OnUpdate(float deltaTime)
    {
        steve.move.target=null;
        steve.move.evade=false;
        steve.move.arrival=true;
        steve.move.departure=true;
        steve.combat.range=steve.meleeRange;
    }

    protected override void OnExit()
    {
    }
}
