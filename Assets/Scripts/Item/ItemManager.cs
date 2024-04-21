using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Item
{
    None,
    Wood,
    WoodPickaxe,
    Stone,
    StonePickaxe,
    Coal,
    IronOre,
    IronIngot,
    IronPickaxe,
    Diamond,
    DiamondPickaxe,
    Arrow,
    DiamondBlock,
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

    public void Spawn(Item item, Vector3 pos)
    {
        Loot2D loot = Instantiate(lootPrefab, pos, Quaternion.identity).GetComponent<Loot2D>();

        loot.item = item;
    }
}
