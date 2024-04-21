using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericHurt : MonoBehaviour
{
    Hurt2D hurt;

    void Awake()
    {
        hurt=GetComponent<Hurt2D>();
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
}
