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

    }

    protected override void OnExit()
    {
    }
}
