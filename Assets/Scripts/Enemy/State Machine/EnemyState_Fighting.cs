using UnityEngine;

public class EnemyState_Fighting : BaseState
{
    public override string Name => "Fighting";

    Enemy enemy;

    public EnemyState_Fighting(EnemyStateMachine sm)
    {
        enemy = sm.enemy;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{enemy.gameObject.name} State: {Name}");
    }

    GameObject target;

    protected override void OnUpdate(float deltaTime)
    {
        target = enemy.closestEnemy;

        if(target)
        {
            enemy.move.target=target.transform;
        }

        enemy.combat.range = enemy.range;

        if(enemy.combat.InRange())
        {
            enemy.move.evade=true;
            enemy.move.departure=true;

            enemy.combat.Attack(enemy.currentWeapon.prefab);
        }
        else enemy.move.evade=false;
    }

    protected override void OnExit()
    {
    }
}
