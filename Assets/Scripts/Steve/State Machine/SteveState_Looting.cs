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


    }

    protected override void OnUpdate(float deltaTime)
    {

    }

    protected override void OnExit()
    {
    }
}
