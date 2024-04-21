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

    GameObject station;

    protected override void OnUpdate(float deltaTime)
    {
        station = steve.GetRequiredCraftingStation();

        if(station)
        {
            steve.move.target=station.transform;
        }
        steve.move.evade=false;
        steve.combat.range=steve.meleeRange;

        if(steve.combat.InRange())
        {
            EventManager.Current.OnUpdateCraft(steve.gameObject, station, steve.GetGoalRecipe());
        }
        else
        {
            EventManager.Current.OnUpdateNotCraft(station);
        }
    }

    protected override void OnExit()
    {
        EventManager.Current.OnUpdateNotCraft(station);
    }
}
