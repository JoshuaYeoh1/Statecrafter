using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Item
{
    None,
    WoodLog,
    WoodPickaxe,
    Stone,
    StonePickaxe,
    CoalOre,
    IronOre,
    IronIngot,
    IronPickaxe,
    Diamond,
    DiamondPickaxe,
    Arrow,
    DiamondBlock,
    WoodAxe,
    StoneAxe,
    IronAxe,
    DiamondAxe,
    WoodSword,
    StoneSword,
    IronSword,
    DiamondSword,
    Bow,
    String,
    Stick,
    WoodPlanks,
    RottenFlesh,
    Bone,
    SpiderEye,
    GenericFood,
}

[System.Serializable]
public class ItemFood
{
    public string name;
    public Item item;
    public float heal;
}

public class ItemManager : MonoBehaviour
{
    public static ItemManager Current;

    void Awake()
    {
        Current=this;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public GameObject lootPrefab;

    public GameObject Spawn(Item item, Vector3 pos)
    {
        GameObject spawned = Instantiate(lootPrefab, pos, Quaternion.identity);

        if(spawned.TryGetComponent(out Loot2D loot))
        {
            loot.item = item;
        }

        return spawned;
    }

    public List<ItemFood> foods = new();

    public ItemFood GetFood(Item item)
    {
        foreach(ItemFood food in foods)
        {
            if(food.item==item)
            {
                return food;
            }
        }
        return null;
    }
}
