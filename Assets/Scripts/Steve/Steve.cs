using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steve : MonoBehaviour
{
    [HideInInspector] public ForceVehicle2D vehicle;
    [HideInInspector] public PursuitBehaviour move;
    [HideInInspector] public HPManager hp;
    [HideInInspector] public Hurt2D hurt;
    [HideInInspector] public Radar2D radar;
    [HideInInspector] public Inventory inv;

    void Awake()
    {
        vehicle=GetComponent<ForceVehicle2D>();
        move=GetComponent<PursuitBehaviour>();
        hp=GetComponent<HPManager>();
        hurt=GetComponent<Hurt2D>();
        radar=GetComponent<Radar2D>();
        inv=GetComponent<Inventory>();
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

    public float lowHPPercent=30;

    public bool IsLowHP()
    {
        return hp.GetHPPercent()<=lowHPPercent;
    }

    public bool IsFullHP()
    {
        return hp.hp==hp.hpMax;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Update()
    {
        UpdateClosest();
        UpdateCurrentTool();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public GameObject closestEnemy;
    public GameObject closestLoot;

    void UpdateClosest()
    {
        closestEnemy = GetClosestEnemy();
        closestLoot = GetClosestLoot();
    }

    public GameObject GetClosestEnemy()
    {
        return radar.GetClosest(radar.GetTargetsWithTag("Enemy"));
    }

    public GameObject GetClosestLoot()
    {
        return radar.GetClosest(radar.GetTargetsWithTag("Loot"));
    }

    public GameObject GetClosestBed()
    {
        return radar.GetClosest(StationManager.Current.GetBeds());
    }

    public GameObject GetClosestCraftingTable()
    {
        return radar.GetClosest(StationManager.Current.GetCraftingTables());
    }

    public GameObject GetClosestFurnace()
    {
        return radar.GetClosest(StationManager.Current.GetFurnaces());
    }

    public GameObject GetClosestWood()
    {
        return radar.GetClosest(StationManager.Current.GetTreeLogs());
    }

    public GameObject GetClosestStone()
    {
        return radar.GetClosest(StationManager.Current.GetStones());
    }

    public GameObject GetClosestCoalOre()
    {
        return radar.GetClosest(StationManager.Current.GetCoalOres());
    }

    public GameObject GetClosestIronOre()
    {
        return radar.GetClosest(StationManager.Current.GetIronOres());
    }

    public GameObject GetClosestDiamondOre()
    {
        return radar.GetClosest(StationManager.Current.GetDiamondOres());
    }

    public GameObject GetTargetResource()
    {
        if(nextTool==Tool.DiamondPickaxe)
        {
            return GetClosestDiamondOre();
        }
        else if(nextTool==Tool.IronPickaxe)
        {
            int eachRequired = RecipeManager.Current.GetRecipe(Item.IronPickaxe).quantities[0];

            if(inv.inventory.ContainsKey(Item.Coal))
            {
                if(inv.inventory[Item.Coal] >= eachRequired)
                {
                    if(inv.inventory.ContainsKey(Item.IronOre))
                    {
                        if(inv.inventory[Item.IronOre] < eachRequired)
                        {
                            return GetClosestIronOre();
                        }
                    }
                    else return GetClosestIronOre();
                }
                else return GetClosestCoalOre();
            }
            else return GetClosestCoalOre();
        }
        else if(nextTool==Tool.StonePickaxe)
        {
            return GetClosestStone();
        }
        else if(nextTool==Tool.WoodPickaxe)
        {
            return GetClosestWood();
        }

        return null;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public Tool currentTool = Tool.Fist;
    public Tool nextTool = Tool.WoodPickaxe;
    public Item nextItem = Item.WoodPickaxe;

    void UpdateCurrentTool()
    {
        if(inv.inventory.ContainsKey(Item.DiamondPickaxe))
        {
            currentTool = Tool.DiamondPickaxe;
            nextTool = Tool.None;
            nextItem = Item.None;
        }
        else if(inv.inventory.ContainsKey(Item.IronPickaxe))
        {
            currentTool = Tool.IronPickaxe;
            nextTool = Tool.DiamondPickaxe;
            nextItem = Item.DiamondPickaxe;
        }
        else if(inv.inventory.ContainsKey(Item.StonePickaxe))
        {
            currentTool = Tool.StonePickaxe;
            nextTool = Tool.IronPickaxe;
            nextItem = Item.IronPickaxe;
        }
        else if(inv.inventory.ContainsKey(Item.WoodPickaxe))
        {
            currentTool = Tool.WoodPickaxe;
            nextTool = Tool.StonePickaxe;
            nextItem = Item.StonePickaxe;
        }
        else
        {
            currentTool = Tool.Fist;
            nextTool = Tool.WoodPickaxe;
            nextItem = Item.WoodPickaxe;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public bool HasEnoughResources()
    {
        return RecipeManager.Current.CanCraft(nextItem, inv.inventory);
    }

    public bool NeedsFurnace()
    {
        return nextTool == Tool.IronPickaxe;
    }

}
