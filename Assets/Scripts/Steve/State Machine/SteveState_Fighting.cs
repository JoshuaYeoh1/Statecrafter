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


    }

    protected override void OnUpdate(float deltaTime)
    {

    }

    protected override void OnExit()
    {
    }
}
