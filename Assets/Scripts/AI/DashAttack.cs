using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour
{
    ForceVehicle2D vehicle;
    CombatAI combat;

    void Awake()
    {
        vehicle=GetComponent<ForceVehicle2D>();
        combat=GetComponent<CombatAI>();
    }

    void OnEnable()
    {
        EventManager.Current.AttackEvent += OnAttack;
    }
    void OnDisable()
    {
        EventManager.Current.AttackEvent -= OnAttack;
    }

    public float dashForce=10;

    void OnAttack(GameObject attacker)
    {
        if(attacker!=gameObject) return;

        vehicle.Push(dashForce, combat.aimer.up);
    }
}
