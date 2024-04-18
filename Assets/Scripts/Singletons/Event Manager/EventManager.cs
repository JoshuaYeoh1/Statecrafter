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

    public event Action<GameObject, GameObject, HurtInfo> HitEvent; // ignores iframe
    public event Action<GameObject, GameObject, HurtInfo> HurtEvent; // respects iframe
    public event Action<GameObject, GameObject, HurtInfo> DeathEvent;

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

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public event Action<GameObject, float, float> UIBarUpdateEvent;

    public void OnUIBarUpdate(GameObject owner, float value, float valueMax)
    {
        UIBarUpdateEvent?.Invoke(owner, value, valueMax);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public event Action<GameObject, LootInfo> LootEvent;

    public void OnLoot(GameObject looter, LootInfo lootInfo)
    {
        LootEvent?.Invoke(looter, lootInfo);
    }
}