using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tool
{
    public string name;
    public Item item;
    public GameObject prefab;
}

public class Equipment : MonoBehaviour
{
    public List<Tool> tools = new();

    public Tool GetTool(Item item)
    {
        foreach(Tool tool in tools)
        {
            if(item==tool.item)
            {
                return tool;
            }
        }
        return null;
    }

    public Tool GetTool(string name)
    {
        foreach(Tool tool in tools)
        {
            if(name==tool.name)
            {
                return tool;
            }
        }
        return null;
    }

    public Tool GetPriority(List<Item> priorityList, Inventory inv)
    {
        for(int i=0; i<priorityList.Count; i++)
        {
            if(inv.HasItem(priorityList[i]))
            {
                return GetTool(priorityList[i]);
            }
        }
        return null;
    }
}
