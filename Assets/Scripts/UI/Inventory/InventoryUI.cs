using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    SpectatorCam spectatorCam;
    GameObject npc;

    void Start()
    {
        spectatorCam = SpectatorCam.Current;
    }

    public List<InvSlotUI> slots = new();

    void Update()
    {
        npc = spectatorCam.spectatedNPC;

        if(npc.TryGetComponent(out Inventory inv))
        {
            SortedDictionary<Item, int> inventory = new(inv.inventory);

            int i=0;

            foreach(var dict in inventory)
            {
                if(i<slots.Count)
                {
                    slots[i].item = dict.Key;
                    slots[i].count = dict.Value;
                    i++;
                }
                else break;
            }

            for(; i<slots.Count; i++)
            {
                slots[i].item = Item.None;
                slots[i].count=0;
            }
        }
    }

    [ContextMenu("Assign Slots")]
    void AssignSlots()
    {
        slots.Clear();
        foreach(Transform child in transform)
        {
            if(child.TryGetComponent(out InvSlotUI slot))
            {
                slots.Add(slot);
            }
        }
    }
}
