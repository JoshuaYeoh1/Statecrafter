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

    GameObject resource;

    protected override void OnUpdate(float deltaTime)
    {
        resource = steve.goalResource;

        if(resource)
        {
            steve.move.target=resource.transform;

            StationManager.Current.OccupyTarget(resource, steve.gameObject);
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
        if(resource)
        {
            StationManager.Current.UnoccupyTarget(resource, steve.gameObject);
        }
    }
}
