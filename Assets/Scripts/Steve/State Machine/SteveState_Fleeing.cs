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

    GameObject enemy;

    protected override void OnUpdate(float deltaTime)
    {
        enemy = steve.closestHazard ? steve.closestHazard : steve.closestEnemy;

        if(enemy)
        {
            steve.move.target=enemy.transform;
        }

        steve.move.evade=true;
        steve.move.departure=false;
    }

    protected override void OnExit()
    {
        steve.move.evade=false;
        steve.move.departure=true;
    }
}
