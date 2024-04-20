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

    public Dictionary<StationType, List<GameObject>> stations = new();

    void Start()
    {
        RefreshStations();
    }

    public void RefreshStations()
    {
        stations.Clear();

        Station[] allStations = FindObjectsOfType<Station>();

        foreach(Station station in allStations)
        {
            if(!stations.ContainsKey(station.type))
            {
                stations[station.type] = new();
            }

            stations[station.type].Add(station.gameObject);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public List<GameObject> GetStations(StationType type)
    {
        if(!stations.ContainsKey(type)) return null;

        return stations[type];
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

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public List<GameObject> GetQuarryStations()
    {
        return GetStations(StationType.Quarry);
    }

    public List<Quarry> GetQuarries(QuarryType type)
    {
        List<Quarry> matches = new();

        foreach(GameObject station in GetQuarryStations())
        {
            if(station.TryGetComponent<Quarry>(out Quarry quarry))
            {
                if(quarry.type==type)
                {
                    matches.Add(quarry);
                }
            }
        }

        return matches;
    }

    public List<Quarry> GetTrees()
    {
        return GetQuarries(QuarryType.Tree);
    }

    public List<Quarry> GetStoneQuarries()
    {
        return GetQuarries(QuarryType.StoneQuarry);
    }

    public List<Quarry> GetCoalQuarries()
    {
        return GetQuarries(QuarryType.CoalQuarry);
    }

    public List<Quarry> GetIronQuarries()
    {
        return GetQuarries(QuarryType.IronQuarry);
    }

    public List<Quarry> GetDiamondQuarries()
    {
        return GetQuarries(QuarryType.DiamondQuarry);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public List<GameObject> GetTreeLogs()
    {
        List<GameObject> logs = new();

        foreach(Quarry quarry in GetTrees())
        {
            logs.AddRange(quarry.currentOres);
        }

        return logs;
    }

    public List<GameObject> GetStones()
    {
        List<GameObject> stones = new();

        foreach(Quarry quarry in GetStoneQuarries())
        {
            stones.AddRange(quarry.currentOres);
        }

        return stones;
    }

    public List<GameObject> GetCoalOres()
    {
        List<GameObject> coalOres = new();

        foreach(Quarry quarry in GetCoalQuarries())
        {
            coalOres.AddRange(quarry.currentOres);
        }

        return coalOres;
    }

    public List<GameObject> GetIronOres()
    {
        List<GameObject> ironOres = new();

        foreach(Quarry quarry in GetIronQuarries())
        {
            ironOres.AddRange(quarry.currentOres);
        }

        return ironOres;
    }

    public List<GameObject> GetDiamondOres()
    {
        List<GameObject> diamondOres = new();

        foreach(Quarry quarry in GetDiamondQuarries())
        {
            diamondOres.AddRange(quarry.currentOres);
        }

        return diamondOres;
    }
}
