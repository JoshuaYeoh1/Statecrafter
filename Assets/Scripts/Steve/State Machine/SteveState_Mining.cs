using UnityEngine;

public class SteveState_Mining : BaseState
{
    public override string Name => "Mining";

    Steve steve;

    public SteveState_Mining(SteveStateMachine sm)
    {
        steve = sm.steve;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{steve.gameObject.name} State: {Name}");
    }

    GameObject targetResource;

    protected override void OnUpdate(float deltaTime)
    {
        targetResource = steve.GetGoalResource();

        if(targetResource)
        {
            steve.move.target=targetResource.transform;
        }
        
        steve.move.evade=false;
        steve.combat.range=steve.meleeRange;

        if(steve.combat.InRange())
        {
            steve.combat.Attack(steve.currentMeleePrefab);
        }
    }

    protected override void OnExit()
    {
    }
}
