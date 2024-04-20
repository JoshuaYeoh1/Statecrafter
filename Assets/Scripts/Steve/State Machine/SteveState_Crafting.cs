using UnityEngine;

public class SteveState_Crafting : BaseState
{
    public override string Name => "Crafting";

    Steve steve;

    public SteveState_Crafting(SteveStateMachine sm)
    {
        steve = sm.steve;
    }

    GameObject station;

    protected override void OnEnter()
    {
        Debug.Log($"{steve.gameObject.name} State: {Name}");

        station = steve.GetRequiredCraftingStation();

        steve.move.target=station.transform;
        steve.move.evade=false;
        steve.combat.range=steve.meleeRange;
    }

    protected override void OnUpdate(float deltaTime)
    {
        if(steve.combat.InRange())
        {
            EventManager.Current.OnUpdateCraft(steve.gameObject, station, steve.GetTargetRecipe());
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
