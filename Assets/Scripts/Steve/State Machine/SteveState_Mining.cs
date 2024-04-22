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
        targetResource = steve.goalResource;

        if(targetResource)
        {
            steve.move.target=targetResource.transform;
        }
        
        steve.combat.range=steve.meleeRange;

        if(steve.combat.InRange())
        {
            steve.move.evade=true;
            steve.move.departure=true;

            steve.combat.Attack(steve.currentTool.prefab);
        }
        else steve.move.evade=false;
    }

    protected override void OnExit()
    {
    }
}
