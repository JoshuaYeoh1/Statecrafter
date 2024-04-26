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

    public event Action<GameObject> SpawnEvent;
    public event Action<GameObject> AttackEvent;
    public event Action<GameObject, GameObject, HurtInfo> HitEvent; // ignores iframe
    public event Action<GameObject, GameObject, HurtInfo> HurtEvent; // respects iframe
    public event Action<GameObject, GameObject, HurtInfo> DeathEvent;
    public event Action<GameObject, Item, int> AmmoEvent;
    public event Action<GameObject, Buff, float> AddBuffEvent;
    public event Action<GameObject, Buff> RemoveBuffEvent;

    public void OnSpawn(GameObject spawned)
    {
        SpawnEvent?.Invoke(spawned);
    }    
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
    public void OnAddBuff(GameObject target, Buff newBuff, float duration)
    {
        AddBuffEvent?.Invoke(target, newBuff, duration);
    }
    public void OnRemoveBuff(GameObject target, Buff buff)
    {
        RemoveBuffEvent?.Invoke(target, buff);
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public event Action<GameObject, float, float> UIBarUpdateEvent;
    public event Action<GameObject> SpectateEvent;
    public event Action SwitchSpectateEvent;
    public event Action<Vector3> Click2DEvent;
    public event Action<Vector3, float, Vector3, Vector3, Vector3> Swipe2DEvent;
    public event Action<GameObject> ClickObjectEvent;
    public event Action<PlayerAbility> SelectPlayerAbilityEvent;

    public void OnUIBarUpdate(GameObject owner, float value, float valueMax)
    {
        UIBarUpdateEvent?.Invoke(owner, value, valueMax);
    }
    public void OnSpectate(GameObject watched)
    {
        SpectateEvent?.Invoke(watched);
    }
    public void OnSwitchSpectate()
    {
        SwitchSpectateEvent?.Invoke();
    }
    public void OnClick2D(Vector3 pos)
    {
        Click2DEvent?.Invoke(pos);
    }
    public void OnSwipe2D(Vector3 startPos, float magnitude, Vector3 direction, Vector3 endPos)
    {
        Vector3 midPos = Vector3.Lerp(startPos, endPos, .5f);

        Swipe2DEvent?.Invoke(startPos, magnitude, direction, endPos, midPos);
    }
    public void OnClickObject(GameObject clicked)
    {
        ClickObjectEvent?.Invoke(clicked);
    }
    public void OnSelectPlayerAbility(PlayerAbility ability)
    {
        SelectPlayerAbilityEvent?.Invoke(ability);
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public event Action<GameObject, GameObject, LootInfo> LootEvent;
    public event Action<GameObject, GameObject, Recipe> UpdateCraftEvent;
    public event Action<GameObject, Recipe> UpdateNotCraftEvent;
    public event Action<GameObject, GameObject, Recipe> CraftedEvent;
    public event Action<GameObject, float, Vector3, Vector3> EnderPearlEvent;
    public event Action<GameObject> MaceSlamEvent;
    public event Action<GameObject> IdleVoiceEvent;

    public void OnLoot(GameObject looter, GameObject loot, LootInfo lootInfo)
    {
        LootEvent?.Invoke(looter, loot, lootInfo);
    }
    public void OnUpdateCraft(GameObject crafter, GameObject station, Recipe recipe)
    {
        UpdateCraftEvent?.Invoke(crafter, station, recipe);
    }
    public void OnUpdateNotCraft(GameObject station, Recipe recipe)
    {
        UpdateNotCraftEvent?.Invoke(station, recipe);
    }
    public void OnCrafted(GameObject crafter, GameObject station, Recipe recipe)
    {
        CraftedEvent?.Invoke(crafter, station, recipe);
    }
    public void OnEnderPearl(GameObject teleporter, float teleportTime, Vector3 from, Vector3 to)
    {
        EnderPearlEvent?.Invoke(teleporter, teleportTime, from, to);
    }
    public void OnMaceSlam(GameObject mace)
    {
        MaceSlamEvent?.Invoke(mace);
    }
    public void OnIdleVoice(GameObject subject)
    {
        IdleVoiceEvent?.Invoke(subject);
    }
    
}