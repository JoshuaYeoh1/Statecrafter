using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAI : MonoBehaviour
{
    PursuitAI move;

    void Awake()
    {
        move=GetComponent<PursuitAI>();
    }

    void Update()
    {
        UpdateRange();
        UpdateAimer();
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    [Header("Range")]
    public float range=.05f;

    void UpdateRange()
    {
        move.stoppingRange = move.evadeRange = range;
    }

    public bool InRange()
    {
        if(!move.target) return false;

        float distance = Vector3.Distance(transform.position, move.target.position);

        return distance <= range;
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    [Header("Aim")]
    public Transform aimer;
    public float aimSpeed=10;
    public bool linearTurn;
    public float angleOffset=-90;

    void UpdateAimer()
    {
        Aim(GetAim());
    }

    Vector3 GetAim()
    {
        if(!move.target) return Vector3.zero;

        return move.GetDir(move.target.position, transform.position);
    }

    void Aim(Vector3 dir)
    {
        if(dir==Vector3.zero) return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion lookRotation = Quaternion.Euler(0, 0, angle + angleOffset);

        transform.rotation = linearTurn ?
            Quaternion.Lerp(aimer.rotation, lookRotation, aimSpeed * Time.deltaTime): // linearly aim the direction
            Quaternion.Slerp(aimer.rotation, lookRotation, aimSpeed * Time.deltaTime); // smoothly aim the direction
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    [Header("Attack")]
    public float attackCooldown=.6f;
    bool canAttack=true;

    public void Attack(GameObject prefab)
    {
        if(!canAttack) return;
        canAttack=false;
        Invoke(nameof(EnableAttack), attackCooldown);
        
        GameObject spawned = Instantiate(prefab, aimer.position, aimer.rotation);
        spawned.transform.parent = aimer;

        if(spawned.TryGetComponent(out Hurtbox2D hurtbox))
        {
            hurtbox.owner = gameObject;
        }
        
        if(spawned.TryGetComponent(out BowAnim bow))
        {
            bow.owner = gameObject;
        }
    }

    void EnableAttack()
    {
        canAttack=true;
    }    
}
