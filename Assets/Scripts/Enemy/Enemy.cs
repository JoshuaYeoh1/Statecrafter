using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public PursuitAI move;
    [HideInInspector] public Radar2D radar;
    [HideInInspector] public CombatAI combat;
    [HideInInspector] public WanderAI wander;

    void Awake()
    {
        move=GetComponent<PursuitAI>();
        radar=GetComponent<Radar2D>();
        combat=GetComponent<CombatAI>();
        wander=GetComponent<WanderAI>();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Update()
    {
        UpdateRadar();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Radar")]
    public GameObject closestEnemy;

    void UpdateRadar()
    {
        closestEnemy = radar.GetClosest(radar.targets);
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Combat")]
    public float huggyRange=.05f;
    public float range=1.5f;
    public Tool currentWeapon;
}
