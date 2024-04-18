using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steve : MonoBehaviour
{
    [HideInInspector] public ForceVehicle2D vehicle;
    [HideInInspector] public PursuitBehaviour move;
    [HideInInspector] public HPManager hp;
    [HideInInspector] public Hurt2D hurt;
    [HideInInspector] public Radar2D radar;
    [HideInInspector] public Inventory inv;

    void Awake()
    {
        vehicle=GetComponent<ForceVehicle2D>();
        move=GetComponent<PursuitBehaviour>();
        hp=GetComponent<HPManager>();
        hurt=GetComponent<Hurt2D>();
        radar=GetComponent<Radar2D>();
        inv=GetComponent<Inventory>();
    }

    void OnEnable()
    {
        EventManager.Current.HitEvent += OnHit;
    }
    void OnDisable()
    {
        EventManager.Current.HitEvent -= OnHit;
    }

    public void OnHit(GameObject attacker, GameObject victim, HurtInfo hurtInfo)
    {
        if(victim!=gameObject) return;

        hurt.Hit(attacker, hurtInfo);
    }

    public float lowHPPercent=30;

    public bool IsLowHP()
    {
        return hp.GetHPPercent()<=lowHPPercent;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public GameObject closestEnemy;
    public GameObject closestLoot;

    void FixedUpdate()
    {
        closestEnemy = GetClosestEnemy();
        closestLoot = GetClosestLoot();
    }

    public GameObject GetClosestEnemy()
    {
        return radar.GetClosest(radar.GetTargetsWithTag("Enemy"));
    }

    public GameObject GetClosestLoot()
    {
        return radar.GetClosest(radar.GetTargetsWithTag("Loot"));
    }
    
}
