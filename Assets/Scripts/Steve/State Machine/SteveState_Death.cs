using UnityEngine;

public class SteveState_Death : BaseState
{
    public override string Name => "Death";

    Steve steve;

    public SteveState_Death(SteveStateMachine sm)
    {
        steve = sm.steve;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{steve.gameObject.name} State: {Name}");

        steve.rb.velocity=Vector3.zero;

        steve.coll.enabled=false;
        steve.rb.isKinematic=true;
        
        steve.vehicle.maxSpeed=0;
        steve.sr.gameObject.SetActive(false);

        steve.inv.DropAll();
        steve.Respawning();
    }

    protected override void OnUpdate(float deltaTime)
    {
        steve.move.target = null;
        steve.move.evade=false;
        steve.move.arrival=true;
        steve.move.departure=true;
        steve.combat.range=steve.huggyRange;
    }

    protected override void OnExit()
    {
        steve.coll.enabled=true;
        steve.rb.isKinematic=false;
        
        steve.vehicle.maxSpeed=steve.vehicle.defMaxSpeed;
        steve.sr.gameObject.SetActive(true);
    }
}
    