using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHurt : MonoBehaviour
{
    Hurt2D hurt;

    public List<string> requiredTools = new();

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

        string toolName = hurtInfo.coll.attachedRigidbody.gameObject.name;

        if(!requiredTools.Contains(toolName)) return;

        hurt.Hit(attacker, hurtInfo);
    }
}
