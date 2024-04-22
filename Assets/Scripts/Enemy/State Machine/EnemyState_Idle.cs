using UnityEngine;

public class EnemyState_Idle : BaseState
{
    public override string Name => "Idle";

    Enemy enemy;

    public EnemyState_Idle(EnemyStateMachine sm)
    {
        enemy = sm.enemy;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{enemy.gameObject.name} State: {Name}");
    }

    protected override void OnUpdate(float deltaTime)
    {
        enemy.move.target = enemy.wander.wanderTr;
        enemy.move.evade=false;
        enemy.move.arrival=true;
        enemy.move.departure=true;
        enemy.combat.range=enemy.huggyRange;
    }

    protected override void OnExit()
    {
    }
}
