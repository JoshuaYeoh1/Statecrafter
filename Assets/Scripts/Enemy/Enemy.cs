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

    void OnEnable()
    {
        EventManager.Current.HurtEvent += OnHurt;

        ActorManager.Current.enemies.Add(gameObject);
    }
    void OnDisable()
    {
        EventManager.Current.HurtEvent -= OnHurt;

        ActorManager.Current.enemies.Add(gameObject);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Update()
    {
        UpdateRadar();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public EnemyName enemyName;

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

    public void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        if(victim!=gameObject) return;

        if(attacker==SpectatorCam.Current.spectatedNPC)
        {
            CameraManager.Current.Shake();
        }
    }
}
