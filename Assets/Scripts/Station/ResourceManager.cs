using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Current;

    void Awake()
    {
        Current=this;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public List<GameObject> GetResources(Item item)
    {
        List<GameObject> resources = new();

        Resource[] allResources = FindObjectsOfType<Resource>();

        foreach(Resource resource in allResources)
        {
            if(resource.type==item)
            {
                resources.Add(resource.gameObject);
            }
        }

        return resources;
    }

    public List<GameObject> GetWoods()
    {
        return GetResources(Item.WoodLog);
    }
    public List<GameObject> GetStones()
    {
        return GetResources(Item.Stone);
    }
    public List<GameObject> GetCoals()
    {
        return GetResources(Item.CoalOre);
    }
    public List<GameObject> GetIrons()
    {
        return GetResources(Item.IronOre);
    }
    public List<GameObject> GetDiamonds()
    {
        return GetResources(Item.Diamond);
    }
}
