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
}
