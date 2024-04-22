using UnityEngine;

public class SteveState_Fighting : BaseState
{
    public override string Name => "Fighting";

    Steve steve;

    public SteveState_Fighting(SteveStateMachine sm)
    {
        steve = sm.steve;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{steve.gameObject.name} State: {Name}");
    }

    GameObject closestEnemy;

    protected override void OnUpdate(float deltaTime)
    {
        closestEnemy = steve.closestEnemy;

        if(closestEnemy)
        {
            steve.move.target=closestEnemy.transform;
        }

        bool canShoot = steve.inv.HasItem(Item.Arrow) && steve.currentBow!=null;

        steve.combat.range = canShoot ? steve.longRange : steve.meleeRange;

        if(steve.combat.InRange())
        {
            steve.move.evade=true;
            steve.move.departure=true;

            steve.combat.Attack(canShoot ? steve.currentBow.prefab : steve.currentSword.prefab);
        }
        else steve.move.evade=false;
    }

    protected override void OnExit()
    {
    }
}
