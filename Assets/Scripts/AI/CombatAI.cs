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
        UpdateAIRange();
        UpdateAimer();
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    [Header("Range")]
    public float range=.05f;

    void UpdateAIRange()
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
    public Transform firepoint;

    void UpdateAimer()
    {
        firepoint.up = move.target ? GetAim() : transform.up;
    }

    Vector3 GetAim()
    {
        if(!move.target) return Vector3.zero;

        return move.GetDir(move.target.position, transform.position);
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    [Header("Attack")]
    public float attackCooldown=.4f;
    bool canAttack=true;

    public void Attack(GameObject prefab)
    {
        if(!canAttack) return;
        canAttack=false;
        Invoke(nameof(EnableAttack), attackCooldown);
        
        Instantiate(prefab, firepoint.position, firepoint.rotation);
        prefab.transform.parent = firepoint;
    }

    void EnableAttack()
    {
        canAttack=true;
    }    
}
