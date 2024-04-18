using UnityEngine;

public class SteveState_Smelting : BaseState
{
    public override string Name => "Smelting";

    Steve steve;

    public SteveState_Smelting(SteveStateMachine sm)
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
