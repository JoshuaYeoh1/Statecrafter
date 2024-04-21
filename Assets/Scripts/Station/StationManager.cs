using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationManager : MonoBehaviour
{
    public static StationManager Current;

    void Awake()
    {
        Current=this;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public List<GameObject> GetStations(StationType type)
    {
        List<GameObject> stations = new();

        Station[] allStations = FindObjectsOfType<Station>();

        foreach(Station station in allStations)
        {
            if(station.type==type)
            {
                stations.Add(station.gameObject);
            }
        }

        return stations;
    }

    public List<GameObject> GetBeds()
    {
        return GetStations(StationType.Bed);
    }
    public List<GameObject> GetCraftingTables()
    {
        return GetStations(StationType.CraftingTable);
    }
    public List<GameObject> GetFurnaces()
    {
        return GetStations(StationType.Furnace);
    }
    public List<GameObject> GetTrees()
    {
        return GetStations(StationType.Tree);
    }
    public List<GameObject> GetQuarries()
    {
        return GetStations(StationType.Quarry);
    }
}
