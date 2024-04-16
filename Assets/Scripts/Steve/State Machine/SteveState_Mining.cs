using UnityEngine;

public class SteveState_Mining : BaseState
{
    public override string Name => "Idle";

    Steve steve;

    public SteveState_Mining(SteveStateMachine sm)
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
