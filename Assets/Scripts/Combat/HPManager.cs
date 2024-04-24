using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    public float hp=100;
    public float hpMax=100;

    [Header("Regeneration")]
    public bool regen;
    public bool regenWhenEmpty;
    public float regenHp=.2f, regenInterval=.1f;
    [HideInInspector] public float defaultRegenHp;
    [HideInInspector] public float defaultRegenInterval;
    float prevRegenInterval;

    void Awake()
    {
        defaultRegenHp=regenHp;
        defaultRegenInterval=regenInterval;
    }
    
    void OnEnable()
    {
        StartHpRegen();
    }

    void Start()
    {
        EventManager.Current.OnUIBarUpdate(gameObject, hp, hpMax);
    }

    void Update()
    {
        hp = Mathf.Clamp(hp, 0, hpMax);     

        if(prevRegenInterval != regenInterval)
        {
            prevRegenInterval = regenInterval;

            StartHpRegen();
        }
    }

    public void Hurt(float dmg)
    {
        if(dmg>0)
        {
            if(hp>dmg) hp-=dmg;
            else hp=0;
        }
        
        EventManager.Current.OnUIBarUpdate(gameObject, hp, hpMax);
    }    

    void StartHpRegen()
    {
        if(hpRegeneratingRt!=null) StopCoroutine(hpRegeneratingRt);
        hpRegeneratingRt = StartCoroutine(HpRegenerating());
    }

    Coroutine hpRegeneratingRt;

    IEnumerator HpRegenerating()
    {
        while(true)
        {
            yield return new WaitForSeconds(regenInterval);

            if(hp<hpMax && (hp>0 || regenWhenEmpty) )
            {
                if(regen) Add(regenHp);

                EventManager.Current.OnUIBarUpdate(gameObject, hp, hpMax);
            }
        }
    }

    public void Add(float amount)
    {
        hp+=amount;
        if(hp>hpMax) hp=hpMax;

        EventManager.Current.OnUIBarUpdate(gameObject, hp, hpMax);
    }

    public float GetHPPercent()
    {
        return hp/hpMax*100;
    }

    public void SetHPPercent(float percent)
    {
        percent = Mathf.Clamp(percent, 1, 100);

        hp = hpMax * percent/100;

        EventManager.Current.OnUIBarUpdate(gameObject, hp, hpMax);
    }
}
