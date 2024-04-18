using UnityEngine;

public class SteveState_Crafting : BaseState
{
    public override string Name => "Crafting";

    Steve steve;

    public SteveState_Crafting(SteveStateMachine sm)
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
