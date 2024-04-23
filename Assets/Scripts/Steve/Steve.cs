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
    [HideInInspector] public WanderAI wander;

    void Awake()
    {
        vehicle=GetComponent<ForceVehicle2D>();
        move=GetComponent<PursuitAI>();
        hp=GetComponent<HPManager>();
        radar=GetComponent<Radar2D>();
        inv=GetComponent<Inventory>();
        combat=GetComponent<CombatAI>();
        equip=GetComponent<Equipment>();
        wander=GetComponent<WanderAI>();
    }

    void OnEnable()
    {
        EventManager.Current.HurtEvent += OnHurt;
        EventManager.Current.CraftedEvent += OnCrafted;
        EventManager.Current.AmmoEvent += OnAmmo;

        ActorManager.Current.npcs.Add(gameObject);
    }
    void OnDisable()
    {
        EventManager.Current.HurtEvent -= OnHurt;
        EventManager.Current.CraftedEvent -= OnCrafted;
        EventManager.Current.AmmoEvent -= OnAmmo;

        ActorManager.Current.npcs.Remove(gameObject);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public NPCName npcName;

    [Header("HP")]
    public float lowHPPercent=25;
    public float okHPPercent=75;
    public float sleepRegen=.25f;

    public bool IsLowHP()
    {
        return hp.GetHPPercent()<=lowHPPercent;
    }
    public bool IsOkHP()
    {
        return hp.GetHPPercent()>=okHPPercent;
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

    public GameObject GetClosestAvailableBed()
    {
        List<GameObject> beds = StationManager.Current.GetBeds();

        List<GameObject> freeBeds = new();

        foreach(GameObject bed in beds)
        {
            if(!StationManager.Current.IsOccupied(bed, gameObject))
            {
                freeBeds.Add(bed);
            }
        }

        return radar.GetClosest(freeBeds);
    }

    public GameObject GetClosestAvailableCraftingTable()
    {
        List<GameObject> tables = StationManager.Current.GetCraftingTables();

        List<GameObject> freeTables = new();

        foreach(GameObject table in tables)
        {
            if(!StationManager.Current.IsOccupied(table, gameObject))
            {
                freeTables.Add(table);
            }
        }

        return radar.GetClosest(freeTables);
    }

    public GameObject GetClosestAvailableFurnace()
    {
        List<GameObject> furnaces = StationManager.Current.GetFurnaces();

        List<GameObject> freeFurnaces = new();

        foreach(GameObject furnace in furnaces)
        {
            if(!StationManager.Current.IsOccupied(furnace, gameObject))
            {
                freeFurnaces.Add(furnace);
            }
        }

        return radar.GetClosest(freeFurnaces);
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
        if(goalResource)
        {
            Resource resource = goalResource.GetComponent<Resource>();

            if(resource.type==Item.WoodLog)
            {
                currentTool = equip.GetPriority(axePriority, inv);
            }
            else currentTool = equip.GetPriority(pickaxePriority, inv);
        }

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
        if(inv.HasItem(Item.String, 3) && !inv.HasItem(Item.Bow))
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
        return goalPriority[0];
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
            GetClosestAvailableFurnace():
            GetClosestAvailableCraftingTable();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        if(victim!=gameObject) return;

        if(gameObject==SpectatorCam.Current.spectatedNPC)
        {
            CameraManager.Current.Shake();
            TimescaleManager.Current.HitStop();
        }
    }
    
    readonly Trigger crafted = new();
    public bool HasCrafted() => crafted.Check();

    void OnCrafted(GameObject crafter, GameObject station, Recipe recipe)
    {
        if(crafter!=gameObject) return;

        RecipeManager.Current.Craft(recipe, inv, station);

        crafted.Enable();
    }

    void OnAmmo(GameObject shooter, Item ammoItem, int quantity) // done by animation event
    {
        if(shooter!=gameObject) return;

        inv.RemoveItem(ammoItem, quantity);
    }
}
