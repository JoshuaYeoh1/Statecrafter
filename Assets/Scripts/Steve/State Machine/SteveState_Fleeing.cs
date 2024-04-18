using UnityEngine;

public class SteveState_Fleeing: BaseState
{
    public override string Name => "Fleeing";

    Steve steve;

    public SteveState_Fleeing(SteveStateMachine sm)
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
