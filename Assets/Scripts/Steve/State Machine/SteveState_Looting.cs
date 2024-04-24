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

    GameObject loot;
    
    protected override void OnUpdate(float deltaTime)
    {
        loot=steve.closestLoot;

        if(loot)
        {
            steve.move.target=loot.transform;

            StationManager.Current.OccupyTarget(loot, steve.gameObject);
        }

        steve.move.evade=false;
        steve.combat.range=steve.huggyRange;
    }

    protected override void OnExit()
    {
        if(loot)
        {
            StationManager.Current.UnoccupyTarget(loot, steve.gameObject);
        }
    }
}
