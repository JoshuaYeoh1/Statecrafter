using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<Item, int> inventory = new();

    public void AddItem(Item item, int quantity)
    {
        if(HasItem(item))
        {
            inventory[item] += quantity;
        }
        else if(quantity>0)
        {
            inventory.Add(item, quantity);
        }
    }
    
    public void RemoveItem(Item item, int quantity)
    {
        if(!HasItem(item)) return;

        inventory[item] -= quantity;

        if(inventory[item]<=0)
        {
            inventory.Remove(item);
        }
    }

    public bool HasItem(Item item, int quantity=1)
    {
        return inventory.ContainsKey(item) && inventory[item]>=quantity;
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

    public void Drop(Item item, int quantity)
    {
        if(!HasItem(item)) return;

        int left = inventory[item];

        for(int i=0; i<quantity && left>0; i++)
        {
            left--;

            GameObject spawned = ItemManager.Current.Spawn(item, transform.position);

            StationManager.Current.OccupyTarget(spawned, gameObject);

            drops.Add(spawned);
        }

        RemoveItem(item, quantity);
    }

    public void DropAll()
    {
        List<Item> itemsToRemove = new();

        foreach(Item item in inventory.Keys)
        {
            itemsToRemove.Add(item);
        }

        foreach(Item item in itemsToRemove)
        {
            Drop(item, inventory[item]);
        }
    }

    public List<GameObject> drops = new();

    void Update()
    {
        RemoveNulls(drops);
    }

    void RemoveNulls(List<GameObject> list)
    {
        list.RemoveAll(item => item == null);
    }
}
