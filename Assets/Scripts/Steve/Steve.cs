using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steve : MonoBehaviour
{
    [HideInInspector] public ForceVehicle2D vehicle;
    [HideInInspector] public PursuitAI move;
    [HideInInspector] public HPManager hp;
    [HideInInspector] public Radar2D radar;
    [HideInInspector] public Inventory inv;
    [HideInInspector] public CombatAI combat;

    void Awake()
    {
        vehicle=GetComponent<ForceVehicle2D>();
        move=GetComponent<PursuitAI>();
        hp=GetComponent<HPManager>();
        radar=GetComponent<Radar2D>();
        inv=GetComponent<Inventory>();
        combat=GetComponent<CombatAI>();
    }

    void OnEnable()
    {
        EventManager.Current.CraftedEvent += OnCrafted;
        EventManager.Current.AmmoEvent += OnAmmo;
    }
    void OnDisable()
    {
        EventManager.Current.CraftedEvent -= OnCrafted;
        EventManager.Current.AmmoEvent -= OnAmmo;
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
        UpdateTool();
        UpdateGoal();
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
        return radar.GetClosest(ResourceManager.Current.GetWoods());
    }

    public GameObject GetClosestStone()
    {
        GameObject target = radar.GetClosest(ResourceManager.Current.GetStones());

        if(!target) target = GetClosestCoalOre();

        return target;
    }

    public GameObject GetClosestCoalOre()
    {
        GameObject target = radar.GetClosest(ResourceManager.Current.GetCoals());

        if(!target) target = GetClosestStone();

        return target;
    }

    public GameObject GetClosestIronOre()
    {
        GameObject target = radar.GetClosest(ResourceManager.Current.GetIrons());

        if(!target) target = GetClosestCoalOre();

        if(!target) target = GetClosestStone();

        return target;
    }

    public GameObject GetClosestDiamondOre()
    {
        GameObject target = radar.GetClosest(ResourceManager.Current.GetDiamonds());

        if(!target) target = GetClosestIronOre(); // these are to clear space if full capacity in quarries

        if(!target) target = GetClosestCoalOre();

        if(!target) target = GetClosestStone();

        return target;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Inventory")]
    public GameObject currentMeleePrefab;

    void UpdateTool()
    {
        if(inv.HasItem(Item.DiamondPickaxe))
        {
            currentMeleePrefab = diamondPickaxePrefab;
        }
        else if(inv.HasItem(Item.IronPickaxe))
        {
            currentMeleePrefab = ironPickaxePrefab;
        }
        else if(inv.HasItem(Item.StonePickaxe))
        {
            currentMeleePrefab = stonePickaxePrefab;
        }
        else if(inv.HasItem(Item.WoodPickaxe))
        {
            currentMeleePrefab = woodPickaxePrefab;
        }
        else
        {
            currentMeleePrefab = fistPrefab;
        }
    }

    [Header("Goal")]
    public Item goalItem = Item.WoodPickaxe;

    void UpdateGoal()
    {
        if(inv.HasItem(Item.DiamondPickaxe))
        {
            goalItem = Item.DiamondBlock;
        }
        else if(inv.HasItem(Item.IronPickaxe))
        {
            goalItem = Item.DiamondPickaxe;
        }
        else if(inv.HasItem(Item.StonePickaxe))
        {
            if(RecipeManager.Current.CanCraft(Item.IronPickaxe, inv))
            {
                goalItem = Item.IronPickaxe;
            }
            else
            {
                goalItem = RecipeManager.Current.GetRecipe(Item.IronPickaxe).ingredients[0];
            }
        }
        else if(inv.HasItem(Item.WoodPickaxe))
        {
            goalItem = Item.StonePickaxe;
        }
        else
        {
            goalItem = Item.WoodPickaxe;
        }
    }

    public GameObject GetGoalResource()
    {
        Recipe goalRecipe = GetGoalRecipe();

        if(goalRecipe!=null && !CanCraftGoalItem())
        {
            for(int i=0; i<goalRecipe.ingredients.Count; i++)
            {
                if(!inv.HasItem(goalRecipe.ingredients[i], goalRecipe.quantities[i]))
                {
                    switch(goalRecipe.ingredients[i])
                    {
                        case Item.Wood: return GetClosestWood();
                        case Item.Stone: return GetClosestStone();
                        case Item.Coal: return GetClosestCoalOre();
                        case Item.IronOre: return GetClosestIronOre();
                        case Item.Diamond: return GetClosestDiamondOre();
                    }
                }
            }
        }

        return null;
    }

    public Recipe GetGoalRecipe()
    {
        return RecipeManager.Current.GetRecipe(goalItem);
    }

    public GameObject GetRequiredCraftingStation()
    {
        return GetGoalRecipe().craftingStation == StationType.Furnace ? GetClosestFurnace() : GetClosestCraftingTable();
    }

    public bool CanCraftGoalItem()
    {
        return RecipeManager.Current.CanCraft(goalItem, inv);
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void OnCrafted(GameObject crafter, GameObject station, Recipe recipe)
    {
        if(crafter!=gameObject) return;

        RecipeManager.Current.Craft(recipe, inv, station);

        crafted.Enable();
    }

    readonly Trigger crafted = new();
    public bool HasCrafted() => crafted.Check();

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Combat")]
    public float huggyRange=.05f;
    public float useRange=.75f;
    public float meleeRange=1.5f;
    public float longRange=7;

    public GameObject fistPrefab;
    public GameObject woodPickaxePrefab;
    public GameObject stonePickaxePrefab;
    public GameObject ironPickaxePrefab;
    public GameObject diamondPickaxePrefab;
    public GameObject bowPrefab;

    void OnAmmo(GameObject shooter, Item ammoItem, int quantity) // done by animation event
    {
        if(shooter!=gameObject) return;

        inv.RemoveItem(ammoItem, quantity);
    }
}
