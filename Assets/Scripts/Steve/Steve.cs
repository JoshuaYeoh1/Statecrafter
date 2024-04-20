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
    [HideInInspector] public CombatAI combat;

    void Awake()
    {
        vehicle=GetComponent<ForceVehicle2D>();
        move=GetComponent<PursuitBehaviour>();
        hp=GetComponent<HPManager>();
        hurt=GetComponent<Hurt2D>();
        radar=GetComponent<Radar2D>();
        inv=GetComponent<Inventory>();
        combat=GetComponent<CombatAI>();
    }

    void OnEnable()
    {
        EventManager.Current.HitEvent += OnHit;
        EventManager.Current.CraftedEvent += OnCrafted;
        EventManager.Current.AmmoEvent += OnAmmo;
    }
    void OnDisable()
    {
        EventManager.Current.HitEvent -= OnHit;
        EventManager.Current.CraftedEvent -= OnCrafted;
        EventManager.Current.AmmoEvent -= OnAmmo;
    }

    public void OnHit(GameObject attacker, GameObject victim, HurtInfo hurtInfo)
    {
        if(victim!=gameObject) return;

        hurt.Hit(attacker, hurtInfo);
    }

    public float lowHPPercent=25;

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

    [Header("Radar")]
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

    [Header("Inventory")]
    public Tool currentTool = Tool.Fist;
    public Tool nextTool = Tool.WoodPickaxe;
    public Item nextItem = Item.WoodPickaxe;

    void UpdateCurrentTool()
    {
        if(inv.inventory.ContainsKey(Item.DiamondPickaxe))
        {
            currentTool = Tool.DiamondPickaxe;
            currentMeleePrefab = diamondPickaxePrefab;
            nextTool = Tool.None;
            nextItem = Item.None;
        }
        else if(inv.inventory.ContainsKey(Item.IronPickaxe))
        {
            currentTool = Tool.IronPickaxe;
            currentMeleePrefab = ironPickaxePrefab;
            nextTool = Tool.DiamondPickaxe;
            nextItem = Item.DiamondPickaxe;
        }
        else if(inv.inventory.ContainsKey(Item.StonePickaxe))
        {
            currentTool = Tool.StonePickaxe;
            currentMeleePrefab = stonePickaxePrefab;
            nextTool = Tool.IronPickaxe;
            nextItem = Item.IronPickaxe;
        }
        else if(inv.inventory.ContainsKey(Item.WoodPickaxe))
        {
            currentTool = Tool.WoodPickaxe;
            currentMeleePrefab = woodPickaxePrefab;
            nextTool = Tool.StonePickaxe;
            nextItem = Item.StonePickaxe;
        }
        else
        {
            currentTool = Tool.Fist;
            currentMeleePrefab = fistPrefab;
            nextTool = Tool.WoodPickaxe;
            nextItem = Item.WoodPickaxe;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public bool HasEnoughResources()
    {
        return RecipeManager.Current.CanCraft(nextItem, inv.inventory);
    }

    public Recipe GetTargetRecipe()
    {
        return RecipeManager.Current.GetRecipe(nextItem);
    }

    public GameObject GetRequiredCraftingStation()
    {
        return GetTargetRecipe().craftingStation == StationType.Furnace ? GetClosestFurnace() : GetClosestCraftingTable();
    }

    void OnCrafted(GameObject crafter, GameObject station, Recipe recipe)
    {
        if(crafter!=gameObject) return;

        RecipeManager.Current.Craft(recipe, inv.inventory, station);

        crafted.Enable();
    }

    readonly Trigger crafted = new();
    public bool HasCrafted() => crafted.Check();

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Combat")]
    public float huggyRange=.05f;
    public float meleeRange=1;
    public float longRange=5;

    public GameObject fistPrefab;
    public GameObject woodPickaxePrefab;
    public GameObject stonePickaxePrefab;
    public GameObject ironPickaxePrefab;
    public GameObject diamondPickaxePrefab;
    public GameObject currentMeleePrefab;
    public GameObject bowPrefab;

    void OnAmmo(GameObject shooter, Item ammoItem, int quantity) // done by animation event
    {
        if(shooter!=gameObject) return;

        inv.RemoveItem(ammoItem, quantity);
    }
}
