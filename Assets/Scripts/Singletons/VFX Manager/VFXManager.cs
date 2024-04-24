using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void OnEnable()
    {
        EventManager.Current.HurtEvent += OnHurt;
        EventManager.Current.DeathEvent += OnDeath;
        EventManager.Current.LootEvent += OnLoot;
    }
    void OnDisable()
    {
        EventManager.Current.HurtEvent -= OnHurt;
        EventManager.Current.DeathEvent -= OnDeath;
        EventManager.Current.LootEvent -= OnLoot;
    }
    
    void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        HurtActor(victim, attacker, hurtInfo);

        HurtResource(victim, attacker, hurtInfo);
    }

    void HurtActor(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        if(victim.TryGetComponent(out Resource resource)) return; // not resource

        Color color = victim.tag=="NPC" ? Color.red : Color.white;

        SpawnPopUpText(SpriteManager.Current.GetColliderTop(victim), $"{hurtInfo.dmg}", color);

        SpawnHitmarker(hurtInfo.contactPoint, color);

        if(victim.TryGetComponent(out Enemy enemy))
        {
            SpawnImpact(hurtInfo.contactPoint);

            SpawnMcCrit(hurtInfo.contactPoint);

            switch(enemy.enemyName)
            {
                case EnemyName.Zombie: SpawnGreenBlood(hurtInfo.contactPoint); break;
                case EnemyName.Spider: SpawnPurpleBlood(hurtInfo.contactPoint); break;
            }
        }
        else
        {
            SpawnBlood(hurtInfo.contactPoint);

            SpawnRedImpact(hurtInfo.contactPoint);
        }
    }
    
    [Header("Resource")]
    public Color woodColor;
    public Color stoneColor;
    public Color coalColor;
    public Color ironColor;
    public Color diamondColor;

    void HurtResource(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        if(victim.TryGetComponent(out Resource resource))
        {
            Color color = Color.white;

            switch(resource.type)
            {
                case Item.WoodLog: color = woodColor; break;
                case Item.Stone: color = stoneColor; break;
                case Item.CoalOre: color = coalColor; break;
                case Item.IronOre: color = ironColor; break;
                case Item.Diamond: color = diamondColor; break;
            }

            if(resource.type!=Item.WoodLog) SpawnSparks(hurtInfo.contactPoint);

            SpawnFlash(hurtInfo.contactPoint, color, true);

            SpawnMcDig(hurtInfo.contactPoint, color);
        }
    }

    void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtInfo)
    {
        if(victim.TryGetComponent(out Resource resource)) return; // not resource

        SpawnPopUpText(SpriteManager.Current.GetColliderTop(victim), "DEAD!", Color.red);

        SpawnMcSmoke(SpriteManager.Current.GetColliderCenter(victim));
    }    

    public void OnLoot(GameObject looter, LootInfo lootInfo)
    {
        string suffix = lootInfo.quantity>1 ? $"({lootInfo.quantity})" : "";

        SpawnPopUpText(lootInfo.contactPoint, $"+{lootInfo.item}{suffix}", Color.white);

        SpriteManager.Current.FlashColor(looter, 1, 1, 1);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Spawn")]
    public bool hideInHierarchy=true;
    public GameObject popupPrefab;

    public void SpawnPopUpText(Vector3 pos, string text, Color color, float pushForce=6)
    {
        GameObject obj = Instantiate(popupPrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
        
        PopUpAnim popUp = obj.GetComponent<PopUpAnim>();
        popUp.Push(Vector3.one*pushForce);

        TextMeshProUGUI[] tmps = popUp.GetComponentsInChildren<TextMeshProUGUI>();

        foreach(TextMeshProUGUI tmp in tmps)
        {
            tmp.text = text;
            tmp.color = color;
        }
    }

    public GameObject hitmarkerPrefab;

    public void SpawnHitmarker(Vector3 pos, Color color)
    {
        GameObject obj = Instantiate(hitmarkerPrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
        
        ParticleSystem ps = obj.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = ps.main;
        main.startColor = color;
    }

    public GameObject flashUnlitPrefab;
    public GameObject flashLitPrefab;

    public void SpawnFlash(Vector3 pos, Color color, bool lit=false)
    {
        GameObject prefab = lit ? flashLitPrefab : flashUnlitPrefab;

        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        sr.color = color;

        Destroy(obj, .4f);
    }

    public GameObject shockwavePrefab;

    public void SpawnShockwave(Vector3 pos, Color color)
    {
        GameObject obj = Instantiate(shockwavePrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;

        ParticleSystem ps = obj.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = ps.main;
        main.startColor = color;
    }

    public GameObject aoePrefab;

    public void SpawnGroundExplosion(Vector3 pos, float scaleMult=1)
    {
        GameObject obj = Instantiate(aoePrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;

        ParticleSystem ps = obj.GetComponent<ParticleSystem>();
        ps.transform.localScale*=scaleMult;
        ExpandAnim(obj);
    }

    public GameObject bloodPrefab;

    public void SpawnBlood(Vector3 pos)
    {
        GameObject obj = Instantiate(bloodPrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
    }

    public GameObject greenBloodPrefab;

    public void SpawnGreenBlood(Vector3 pos)
    {
        GameObject obj = Instantiate(greenBloodPrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
    }

    public GameObject purpleBloodPrefab;

    public void SpawnPurpleBlood(Vector3 pos)
    {
        GameObject obj = Instantiate(purpleBloodPrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
    }
    
    public GameObject footprintPrefab;

    public void SpawnPlayerFootprint(Transform footstepTr)
    {
        GameObject obj = Instantiate(footprintPrefab, footstepTr.position, footstepTr.rotation);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
    }
    
    public GameObject impactPrefab;

    public void SpawnImpact(Vector3 pos)
    {
        GameObject obj = Instantiate(impactPrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
    }
    
    public GameObject redImpactPrefab;

    public void SpawnRedImpact(Vector3 pos)
    {
        GameObject obj = Instantiate(redImpactPrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
    }
    
    public GameObject sparksPrefab;

    public void SpawnSparks(Vector3 pos)
    {
        GameObject obj = Instantiate(sparksPrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
    }

    public GameObject mcCritPrefab;

    public void SpawnMcCrit(Vector3 pos)
    {
        GameObject obj = Instantiate(mcCritPrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
    }

    public GameObject mcSmokePrefab;

    public void SpawnMcSmoke(Vector3 pos)
    {
        GameObject obj = Instantiate(mcSmokePrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
    }

    public GameObject mcDigPrefab;

    public void SpawnMcDig(Vector3 pos, Color color)
    {
        GameObject obj = Instantiate(mcDigPrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
        
        ParticleSystem ps = obj.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = ps.main;
        main.startColor = color;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void StopParticles(GameObject obj, float wait)
    {
        StartCoroutine(StoppingParticles(obj, wait));
    }
    IEnumerator StoppingParticles(GameObject obj, float wait)
    {
        if(wait>0) yield return new WaitForSeconds(wait);

        if(!obj) yield break; // you shall not pass

        ParticleSystem ps = obj.GetComponent<ParticleSystem>();
        ps.Stop();
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void Update()
    {
        Testing();
    }
    
    void Testing()
    {
        // if(Input.GetKeyDown(KeyCode.Keypad0)) CameraManager.Current.Shake();
        // if(Input.GetKeyDown(KeyCode.Keypad1)) TimescaleManager.Current.HitStop();
        // if(Input.GetKeyDown(KeyCode.Keypad2)) SpawnHitmarker(PlayerTop(), Color.white);
        // if(Input.GetKeyDown(KeyCode.Keypad3)) SpawnFlash(PlayerTop(), Color.white);
        // if(Input.GetKeyDown(KeyCode.Keypad5)) SpawnGroundExplosion(FindPlayer().transform.position);
        // if(Input.GetKeyDown(KeyCode.Keypad6)) SpawnHeal(FindPlayer());
        // if(Input.GetKeyDown(KeyCode.Keypad7)) SpawnImpact(PlayerTop());
        // if(Input.GetKeyDown(KeyCode.Keypad8)) SpawnSparks(PlayerTop());
        // if(Input.GetKeyDown(KeyCode.Keypad9)) SpawnPopUpText(PlayerTop(), "ABOI", Color.cyan, Vector3.one*2);
        // if(Input.GetKeyDown(KeyCode.KeypadDivide)) SpawnChi(ModelManager.Current.GetColliderCenter(FindPlayer()), Vector3.one*5);
    }

    GameObject FindPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    // Vector3 PlayerTop()
    // {
    //     return ModelManager.Current.GetColliderTop(FindPlayer());
    // }

    void ExpandAnim(GameObject obj, float time=.15f)
    {
        Vector3 defscale = obj.transform.localScale;

        obj.transform.localScale=Vector3.zero;

        LeanTween.scale(obj, defscale, time).setEaseInOutSine();
    }
}
