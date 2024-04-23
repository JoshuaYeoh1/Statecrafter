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

    GameObject bed;

    protected override void OnUpdate(float deltaTime)
    {
        bed = steve.GetClosestAvailableBed();

        if(bed)
        {
            steve.move.target=bed.transform;

            StationManager.Current.OccupyStation(bed, steve.gameObject);
        }

        steve.move.evade=false;
        steve.combat.range=steve.huggyRange;

        if(steve.combat.InRange())
        {
            steve.hp.regenInterval=steve.sleepRegenInterval;
        }
        else 
        {
            steve.hp.regenInterval=steve.hp.defaultRegenInterval;
        }
    }

    protected override void OnExit()
    {
        steve.hp.regenInterval=steve.hp.defaultRegenInterval;

        if(bed)
        StationManager.Current.UnoccupyStation(bed, steve.gameObject);
    }
}
