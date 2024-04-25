using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steve : MonoBehaviour
{
    [HideInInspector] public Collider2D coll;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public ForceVehicle2D vehicle;
    [HideInInspector] public PursuitAI move;
    [HideInInspector] public HPManager hp;
    [HideInInspector] public Radar2D radar;
    [HideInInspector] public Inventory inv;
    [HideInInspector] public CombatAI combat;
    [HideInInspector] public Equipment equip;
    [HideInInspector] public WanderAI wander;

    public SpriteRenderer sr;

    void Awake()
    {
        coll=GetComponent<Collider2D>();
        rb=GetComponent<Rigidbody2D>();
        vehicle=GetComponent<ForceVehicle2D>();
        move=GetComponent<PursuitAI>();
        hp=GetComponent<HPManager>();
        radar=GetComponent<Radar2D>();
        inv=GetComponent<Inventory>();
        combat=GetComponent<CombatAI>();
        equip=GetComponent<Equipment>();
        wander=GetComponent<WanderAI>();

        spawnpoint=transform.position;
    }

    void OnEnable()
    {
        EventManager.Current.HurtEvent += OnHurt;
        EventManager.Current.CraftedEvent += OnCrafted;
        EventManager.Current.LootEvent += OnLoot;
        EventManager.Current.AmmoEvent += OnAmmo;

        ActorManager.Current.npcs.Add(gameObject);
    }
    void OnDisable()
    {
        EventManager.Current.HurtEvent -= OnHurt;
        EventManager.Current.CraftedEvent -= OnCrafted;
        EventManager.Current.LootEvent -= OnLoot;
        EventManager.Current.AmmoEvent -= OnAmmo;

        ActorManager.Current.npcs.Remove(gameObject);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public NPCName npcName;

    [Header("HP")]
    public float lowHPPercent=25;
    public float okHPPercent=50;
    public float sleepRegenInterval=.25f;

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
    public bool IsDead()
    {
        return hp.hp<=0;
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
    public GameObject closestHazard;
    public GameObject closestLoot;

    void UpdateRadar()
    {
        closestEnemy = GetClosestEnemy();
        closestHazard = GetClosestHazard();
        closestLoot = GetClosestLoot();
    }

    public GameObject GetClosestEnemy()
    {
        return radar.GetClosest(radar.GetTargetsWithTag("Enemy"));
    }

    public GameObject GetClosestHazard()
    {
        return radar.GetClosest(radar.GetTargetsWithTag("Hazard"));
    }

    public GameObject GetClosestLoot()
    {
        if(inv.drops.Count>0)
        {
            return radar.GetClosest(inv.drops);
        }
        else
        {
            List<GameObject> loots = new();

            if(!IsOkHP()) loots = radar.GetTargetsWithTag("Food");

            if(loots.Count==0) loots = radar.GetTargetsWithTag("Loot");

            List<GameObject> freeLoots = StationManager.Current.GetFreeTargets(loots, gameObject);

            return radar.GetClosest(freeLoots);
        }
    }

    public GameObject GetClosestBed()
    {
        List<GameObject> beds = StationManager.Current.GetBeds();

        List<GameObject> freeBeds = StationManager.Current.GetFreeTargets(beds, gameObject);

        return radar.GetClosest(freeBeds);
    }

    public GameObject GetClosestCraftingTable()
    {
        List<GameObject> tables = StationManager.Current.GetCraftingTables();

        List<GameObject> freeTables = StationManager.Current.GetFreeTargets(tables, gameObject);

        return radar.GetClosest(freeTables);
    }

    public GameObject GetClosestFurnace()
    {
        List<GameObject> furnaces = StationManager.Current.GetFurnaces();

        List<GameObject> freeFurnaces = StationManager.Current.GetFreeTargets(furnaces, gameObject);

        return radar.GetClosest(freeFurnaces);
    }

    public GameObject GetClosestWood()
    {
        List<GameObject> woods = ResourceManager.Current.GetWoods();

        List<GameObject> freeWoods = StationManager.Current.GetFreeTargets(woods, gameObject);

        return radar.GetClosest(freeWoods);
    }

    public GameObject GetClosestStone()
    {
        List<GameObject> stones = ResourceManager.Current.GetStones();

        List<GameObject> freeStones = StationManager.Current.GetFreeTargets(stones, gameObject);

        return radar.GetClosest(freeStones);
    }

    public GameObject GetClosestCoalOre()
    {
        List<GameObject> coals = ResourceManager.Current.GetCoals();

        if(coals.Count==0) return GetClosestStone();

        List<GameObject> freeCoals = StationManager.Current.GetFreeTargets(coals, gameObject);

        return radar.GetClosest(freeCoals);
    }

    public GameObject GetClosestIronOre()
    {
        List<GameObject> irons = ResourceManager.Current.GetIrons();

        if(irons.Count==0) return GetClosestCoalOre();

        List<GameObject> freeIrons = StationManager.Current.GetFreeTargets(irons, gameObject);

        return radar.GetClosest(freeIrons);
    }

    public GameObject GetClosestDiamondOre()
    {
        List<GameObject> diamonds = ResourceManager.Current.GetDiamonds();

        if(diamonds.Count==0) return GetClosestIronOre(); // these are to clear space if full capacity in quarries

        List<GameObject> freeDiamonds = StationManager.Current.GetFreeTargets(diamonds, gameObject);

        return radar.GetClosest(freeDiamonds);
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
            GetClosestFurnace():
            GetClosestCraftingTable();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        if(victim!=gameObject) return;

        if(gameObject==SpectatorCam.Current.spectatedNPC)
        {
            CameraManager.Current.Shake();
            //TimescaleManager.Current.HitStop(); // headache
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

    void OnLoot(GameObject looter, GameObject loot, LootInfo lootInfo)
    {
        if(looter!=gameObject) return;

        ItemFood food = ItemManager.Current.GetFood(lootInfo.item);

        if(food!=null)
        {
            hp.Add(food.heal);
            return;
        }

        Potion potion = ItemManager.Current.GetPotion(lootInfo.item);

        if(potion!=null)
        {
            EventManager.Current.OnAddBuff(gameObject, potion.buff, potion.duration);
            return;
        }

        inv.AddItem(lootInfo.item, lootInfo.quantity);
    }

    void OnAmmo(GameObject shooter, Item ammoItem, int quantity) // done by animation event
    {
        if(shooter!=gameObject) return;

        inv.RemoveItem(ammoItem, quantity);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Death")]
    public float respawnTime=3;
    public Vector3 spawnpoint;

    public void Respawning()
    {
        Invoke(nameof(Respawn), respawnTime);
    }

    void Respawn()
    {
        transform.position = spawnpoint;

        hp.hp=hp.hpMax;
    }
}
