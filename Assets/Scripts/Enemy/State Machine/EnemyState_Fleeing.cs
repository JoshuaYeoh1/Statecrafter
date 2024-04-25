using UnityEngine;

public class EnemyState_Fleeing: BaseState
{
    public override string Name => "Fleeing";

    Enemy enemy;

    public EnemyState_Fleeing(EnemyStateMachine sm)
    {
        enemy = sm.enemy;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{enemy.gameObject.name} State: {Name}");
    }

    GameObject hazard;

    protected override void OnUpdate(float deltaTime)
    {
        hazard = enemy.closestHazard;

        if(hazard)
        {
            enemy.move.target=hazard.transform;
        }

        enemy.move.evade=true;
        enemy.move.departure=false;
    }

    protected override void OnExit()
    {
        enemy.move.evade=false;
        enemy.move.departure=true;
    }
}
