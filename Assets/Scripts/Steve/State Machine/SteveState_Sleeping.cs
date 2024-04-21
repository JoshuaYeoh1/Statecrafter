using UnityEngine;

public class SteveState_Sleeping : BaseState
{
    public override string Name => "Sleeping";

    Steve steve;

    public SteveState_Sleeping(SteveStateMachine sm)
    {
        steve = sm.steve;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{steve.gameObject.name} State: {Name}");        
    }

    GameObject closestBed;

    protected override void OnUpdate(float deltaTime)
    {
        closestBed = steve.GetClosestBed();

        if(closestBed)
        {
            steve.move.target=closestBed.transform;
        }

        steve.move.evade=false;
        steve.combat.range=steve.huggyRange;

        if(steve.combat.InRange())
        {
            steve.hp.regen=true;
        }
        else steve.hp.regen=false;
    }

    protected override void OnExit()
    {
        steve.hp.regen=false;
    }
}
