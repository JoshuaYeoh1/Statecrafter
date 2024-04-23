using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager Current;

    void Awake()
    {
        if(Current!=null && Current!=this)
        {
            Destroy(gameObject);
            return;
        }

        Current = this;
        //DontDestroyOnLoad(gameObject); // Persist across scene changes
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(Current!=this) Destroy(gameObject);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public event Action<GameObject> AttackEvent;
    public event Action<GameObject, GameObject, HurtInfo> HitEvent; // ignores iframe
    public event Action<GameObject, GameObject, HurtInfo> HurtEvent; // respects iframe
    public event Action<GameObject, GameObject, HurtInfo> DeathEvent;
    public event Action<GameObject, Item, int> AmmoEvent;

    public void OnAttack(GameObject attacker)
    {
        AttackEvent?.Invoke(attacker);
    }    
    public void OnHit(GameObject attacker, GameObject victim, HurtInfo hurtInfo)
    {
        HitEvent?.Invoke(attacker, victim, hurtInfo);
    }    
    public void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        HurtEvent?.Invoke(victim, attacker, hurtInfo);
    }
    public void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtInfo)
    {
        DeathEvent?.Invoke(victim, killer, hurtInfo);
    }
    public void OnAmmo(GameObject shooter, Item ammoItem, int quantity)
    {
        AmmoEvent?.Invoke(shooter, ammoItem, quantity);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public event Action<GameObject, float, float> UIBarUpdateEvent;
    public event Action<GameObject> SpectateEvent;

    public void OnUIBarUpdate(GameObject owner, float value, float valueMax)
    {
        UIBarUpdateEvent?.Invoke(owner, value, valueMax);
    }
    public void OnSpectate(GameObject watched)
    {
        SpectateEvent?.Invoke(watched);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public event Action<GameObject, LootInfo> LootEvent;
    public event Action<GameObject, GameObject, Recipe> UpdateCraftEvent;
    public event Action<GameObject> UpdateNotCraftEvent;
    public event Action<GameObject, GameObject, Recipe> CraftedEvent;

    public void OnLoot(GameObject looter, LootInfo lootInfo)
    {
        LootEvent?.Invoke(looter, lootInfo);
    }
    public void OnUpdateCraft(GameObject crafter, GameObject station, Recipe recipe)
    {
        UpdateCraftEvent?.Invoke(crafter, station, recipe);
    }
    public void OnUpdateNotCraft(GameObject station)
    {
        UpdateNotCraftEvent?.Invoke(station);
    }
    public void OnCrafted(GameObject crafter, GameObject station, Recipe recipe)
    {
        CraftedEvent?.Invoke(crafter, station, recipe);
    }
}