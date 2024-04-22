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
    [HideInInspector] public Equipment equip;

    void Awake()
    {
        vehicle=GetComponent<ForceVehicle2D>();
        move=GetComponent<PursuitAI>();
        hp=GetComponent<HPManager>();
        radar=GetComponent<Radar2D>();
        inv=GetComponent<Inventory>();
        combat=GetComponent<CombatAI>();
        equip=GetComponent<Equipment>();
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
        UpdateRadar();
        UpdateTools();
        UpdateGoals();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Radar")]
    public GameObject closestEnemy;
    public GameObject closestLoot;

    void UpdateRadar()
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
        return radar.GetClosest(ResourceManager.Current.GetStones());
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

    [Header("Ranges")]
    public float huggyRange=.05f;
    public float useRange=.75f;
    public float meleeRange=1.5f;
    public float longRange=7;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Equipments")]
    public List<Item> pickaxePriority = new();
    public List<Item> axePriority = new();
    public List<Item> swordPriority = new();
    public List<Item> bowPriority = new();

    public Tool currentTool;
    public Tool currentSword;
    public Tool currentBow;

    void UpdateTools()
    {
        Resource resource = goalResource.GetComponent<Resource>();

        if(resource.type==Item.WoodLog)
        {
            currentTool = equip.GetPriority(axePriority, inv);
        }
        else currentTool = equip.GetPriority(pickaxePriority, inv);

        currentSword = equip.GetPriority(swordPriority, inv);
        currentBow = equip.GetPriority(bowPriority, inv);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Goals")]
    public List<Item> goalPriority = new();
    public Item goalItem = Item.WoodAxe;
    public Recipe goalRecipe;
    public GameObject goalResource;
    public GameObject goalCraftingStation;

    void UpdateGoals()
    {
        if(inv.HasItem(Item.String, 3))
        {
            goalItem = RecipeManager.Current.GetGoalIngredient(Item.Bow, inv);
        }
        else
        {
            goalItem = RecipeManager.Current.GetGoalIngredient(GetNextItem(), inv);
        }

        goalRecipe = RecipeManager.Current.GetRecipe(goalItem);

        goalResource = GetGoalResource();

        goalCraftingStation = GetGoalCraftingStation();
    }

    Item GetNextItem()
    {
        for(int i=goalPriority.Count-1; i>=0; i--)
        {
            if(!inv.HasItem(goalPriority[i]))
            {
                return goalPriority[i];
            }
        }
        return goalPriority[goalPriority.Count-1];
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public GameObject GetGoalResource()
    {
        switch(goalItem)
        {
            case Item.WoodLog: return GetClosestWood();
            case Item.Stone: return GetClosestStone();
            case Item.IronOre: return GetClosestIronOre();
            case Item.CoalOre: return GetClosestCoalOre();
            case Item.Diamond: return GetClosestDiamondOre();
        }
        return null;
    }

    public bool CanCraftGoalItem()
    {
        return RecipeManager.Current.CanCraft(goalItem, inv);
    }

    public GameObject GetGoalCraftingStation()
    {
        Recipe recipe = RecipeManager.Current.GetRecipe(goalItem);

        if(recipe==null) return null;

        return recipe.craftingStation == StationType.Furnace ?
            GetClosestFurnace():
            GetClosestCraftingTable();
    }
    
    void OnCrafted(GameObject crafter, GameObject station, Recipe recipe)
    {
        if(crafter!=gameObject) return;

        RecipeManager.Current.Craft(recipe, inv, station);

        crafted.Enable();
    }

    readonly Trigger crafted = new();
    public bool HasCrafted() => crafted.Check();

    void OnAmmo(GameObject shooter, Item ammoItem, int quantity) // done by animation event
    {
        if(shooter!=gameObject) return;

        inv.RemoveItem(ammoItem, quantity);
    }
}
