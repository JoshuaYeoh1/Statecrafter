using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }

    ////////////////////////////////////////////////////////////////////////////////////

    [Header("Minecraft")]
    public AudioClip[] sfxHitNpc;
    public AudioClip[] sfxHitWood;
    public AudioClip[] sfxFstZombie;
    public AudioClip[] voiceSteveHurt;
    public AudioClip[] voiceZombieDeath;
    public AudioClip[] voiceZombieHurt;
    public AudioClip[] voiceZombieIdle;

    [Header("Terraria")]
    public AudioClip[] sfxDeathEnemy;
    public AudioClip[] sfxDeathNpc;
    public AudioClip[] sfxHitFlesh;
    public AudioClip[] voiceAlexHurt;

    ////////////////////////////////////////////////////////////////////////////////////

    void OnEnable()
    {
        EventManager.Current.HurtEvent += OnHurt;
        EventManager.Current.DeathEvent += OnDeath;
        EventManager.Current.LootEvent += OnLoot;
        EventManager.Current.AddBuffEvent += OnAddBuff;
        EventManager.Current.EnderPearlEvent += OnEnderPearl;
        EventManager.Current.MaceSlamEvent += OnMaceSlam;
    }
    void OnDisable()
    {
        EventManager.Current.HurtEvent -= OnHurt;
        EventManager.Current.DeathEvent -= OnDeath;
        EventManager.Current.LootEvent -= OnLoot;
        EventManager.Current.AddBuffEvent -= OnAddBuff;
        EventManager.Current.EnderPearlEvent -= OnEnderPearl;
        EventManager.Current.MaceSlamEvent -= OnMaceSlam;
    }
    
    void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        if(victim.TryGetComponent(out Steve steve))
        {
            AudioManager.Current.PlaySFX(sfxHitNpc, victim.transform.position);

            switch(steve.npcName)
            {
                case NPCName.Steve: AudioManager.Current.PlayVoice(steve.voicebox, voiceSteveHurt); break;
                case NPCName.Alex: AudioManager.Current.PlayVoice(steve.voicebox, voiceAlexHurt); break;
            }
        }

        else if(victim.TryGetComponent(out Enemy enemy))
        {
            switch(enemy.enemyName)
            {
                case EnemyName.Zombie: ; break;
                case EnemyName.Spider: ; break;
                case EnemyName.Skeleton: ; break;
            }

            if(enemy.enemyName!=EnemyName.Skeleton) AudioManager.Current.PlaySFX(sfxHitFlesh, victim.transform.position);
        }

        else if(victim.TryGetComponent(out Resource resource))
        {
            switch(resource.type)
            {
                case Item.WoodLog: ; break;
                case Item.Stone: ; break;
                case Item.CoalOre: ; break;
                case Item.IronOre: ; break;
                case Item.Diamond: ; break;
            }
        }
    }

    void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtInfo)
    {
        if(victim.TryGetComponent(out Steve steve))
        {
            AudioManager.Current.PlaySFX(sfxDeathNpc, victim.transform.position);
        }

        else if(victim.TryGetComponent(out Enemy enemy))
        {
            switch(enemy.enemyName)
            {
                case EnemyName.Zombie: ; break;
                case EnemyName.Spider: ; break;
                case EnemyName.Skeleton: ; break;
            }
        }

        else if(victim.TryGetComponent(out Resource resource))
        {
            switch(resource.type)
            {
                case Item.WoodLog: ; break;
                case Item.Stone: ; break;
                case Item.CoalOre: ; break;
                case Item.IronOre: ; break;
                case Item.Diamond: ; break;
            }
        }
    }    

    public void OnLoot(GameObject looter, GameObject loot, LootInfo lootInfo)
    {
        ItemFood food = ItemManager.Current.GetFood(lootInfo.item);

        if(food!=null)
        {

            return;
        }
        
        Potion potion = ItemManager.Current.GetPotion(lootInfo.item);

        if(potion!=null)
        {

            return;
        }
            
        string suffix = lootInfo.quantity>1 ? $"({lootInfo.quantity})" : "";
    }

    void OnAddBuff(GameObject target, Buff newBuff, float duration)
    {
        if(newBuff==Buff.Speed)
        {
            
        }
    }

    void OnEnderPearl(GameObject teleporter, float teleportTime)
    {

    }

    void OnMaceSlam(GameObject mace)
    {

    }
}
