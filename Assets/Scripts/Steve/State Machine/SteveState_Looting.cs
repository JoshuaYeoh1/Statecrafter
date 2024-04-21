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

    GameObject closestLoot;
    protected override void OnUpdate(float deltaTime)
    {
        closestLoot=steve.closestLoot;

        if(closestLoot)
        {
            steve.move.target=closestLoot.transform;
        }

        steve.move.evade=false;
        steve.combat.range=steve.huggyRange;
    }

    protected override void OnExit()
    {
    }
}
