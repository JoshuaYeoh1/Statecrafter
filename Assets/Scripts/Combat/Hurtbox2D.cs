using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtInfo
{
    public Collider2D coll;
    public GameObject owner;
    public string attackerName;
    public string victimName;

    public float dmg;
    public float kbForce;
    public Vector3 contactPoint;

    public bool hasSweepingEdge;
}

public class Hurtbox2D : MonoBehaviour
{
    Collider2D coll;
    public GameObject owner;
    public string ownerName;

    public bool enabledOnAwake=true;
    public float dmg=1;
    public float kbForce=1;

    public bool hasSweepingEdge=true;

    void Awake()
    {
        coll=GetComponent<Collider2D>();

        ToggleColl(enabledOnAwake);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.isTrigger) return;

        Rigidbody2D otherRb = other.attachedRigidbody;
        if(otherRb) Hit(other, otherRb);
    }

    public Transform hitboxOrigin;
    [HideInInspector] public Vector3 contactPoint;

    void Hit(Collider2D other, Rigidbody2D otherRb)
    {
        if(hitboxOrigin) contactPoint = other.ClosestPoint(hitboxOrigin.position);
        else contactPoint = other.ClosestPoint(transform.position);

        ToggleColl(hasSweepingEdge); // if can swipe through multiple
        
        EventManager.Current.OnHit(owner, otherRb.gameObject, GetHurtInfoCopy());
    }

    HurtInfo GetHurtInfoCopy()
    {
        HurtInfo info = new();

        info.coll = coll;
        info.owner = owner;
        info.attackerName = ownerName;
        info.dmg = dmg;
        info.kbForce = kbForce;
        info.contactPoint = contactPoint;
        info.hasSweepingEdge = hasSweepingEdge;

        return info;
    }

    public void BlinkHitbox(float time=.1f)
    {
        if(time>0)
        {
            if(blinkingHitboxRt!=null) StopCoroutine(blinkingHitboxRt);
            blinkingHitboxRt = StartCoroutine(BlinkingHitbox(time)); 
        }
    }

    Coroutine blinkingHitboxRt;
    IEnumerator BlinkingHitbox(float t)
    {
        ToggleColl(true);
        yield return new WaitForSeconds(t);
        ToggleColl(false);
    }

    public void ToggleColl(bool toggle)
    {
        coll.enabled=toggle;
    }
}
