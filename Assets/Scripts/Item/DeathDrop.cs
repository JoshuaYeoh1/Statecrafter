using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDrop : MonoBehaviour
{
    DropTable drop;

    void Awake()
    {
        drop=GetComponent<DropTable>();
    }
    
    public bool destroyOnDeath=true;

    void OnEnable()
    {
        EventManager.Current.DeathEvent += OnDeath;
    }
    void OnDisable()
    {
        EventManager.Current.DeathEvent -= OnDeath;
    }
    
    void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtInfo)
    {
        if(victim!=gameObject) return;

        drop.Drop(killer);

        if(destroyOnDeath) Destroy(gameObject);
    }
}
