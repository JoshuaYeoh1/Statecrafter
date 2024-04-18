using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Wood,
    Stone,
    Coal,
    IronOre,
    IronIngot,
    Diamond,
    Arrow,
}

public class Inventory : MonoBehaviour
{
    public Dictionary<ItemType, int> inventory = new();

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

        inventory[lootInfo.item] += lootInfo.quantity;
    }

    public void RemoveItem(ItemType item, int quantity)
    {
        if(!inventory.ContainsKey(item)) return;

        if(inventory[item]>=quantity)
        {
            inventory[item] -= quantity;

            if(inventory[item]<=0)
            {
                inventory.Remove(item);
            }
        }
        else inventory.Remove(item);
    }
}
