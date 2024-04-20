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
}

public enum Tool
{
    None,
    Fist,
    WoodPickaxe,
    StonePickaxe,
    IronPickaxe,
    DiamondPickaxe,
}

public class Inventory : MonoBehaviour
{
    public Dictionary<Item, int> inventory = new();

    void OnEnable()
    {
        EventManager.Current.LootEvent += OnLoot;
    }
    void OnDisable()
    {
        EventManager.Current.LootEvent -= OnLoot;
    }
    
    void OnLoot(GameObject looter, LootInfo lootInfo)
    {
        if(looter!=gameObject) return;

        AddItem(lootInfo.item, lootInfo.quantity);
    }

    public void AddItem(Item item, int quantity)
    {
        inventory[item] += quantity;
    }
    
    public void RemoveItem(Item item, int quantity)
    {
        if(!inventory.ContainsKey(item)) return;

        inventory[item] -= quantity;

        if(inventory[item]<=0)
        {
            inventory.Remove(item);
        }
    }

    public bool HasItem(Item item)
    {
        return inventory.ContainsKey(item) && inventory[item]>0;
    }

    public int GetItemQuantity(Item item)
    {
        if(!HasItem(item)) return 0;

        return inventory[item];
    }
    
    public void Clear()
    {
        inventory.Clear();
    }
}
