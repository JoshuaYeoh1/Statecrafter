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

    void Update()
    {
        RemoveDictNulls(occupiedTargets);
    }

    void RemoveDictNulls<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
    {
        List<TKey> keysToRemove = new List<TKey>();

        foreach(var item in dictionary)
        {
            if(item.Key==null || item.Value==null) // if either is null
            {
                keysToRemove.Add(item.Key);
            }
        }

        foreach(var key in keysToRemove)
        {
            dictionary.Remove(key);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public Dictionary<Transform, Station> occupiedSpots = new();

    public bool HasSpace(float pendingStationSize, Vector3 pendingSpot)
    {
        foreach(Transform spot in occupiedSpots.Keys)
        {
            float distance = Vector3.Distance(spot.position, pendingSpot);

            if(distance < occupiedSpots[spot].stationSize + pendingStationSize)
            {
                return false;
            }
        }
        return true;
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public Dictionary<GameObject, GameObject> occupiedTargets = new(); // target, user

    public void OccupyTarget(GameObject target, GameObject user)
    {
        if(!occupiedTargets.ContainsKey(target))
        {
            occupiedTargets[target] = user;
        }
    }

    public void UnoccupyTarget(GameObject target, GameObject user)
    {
        if(occupiedTargets.ContainsKey(target))
        {
            if(occupiedTargets[target]==user)
            {
                occupiedTargets.Remove(target);
            }
        }
    }

    public bool IsOccupied(GameObject target, GameObject pendingUser)
    {
        if(!occupiedTargets.ContainsKey(target))
        {
            return false; // not occupied
        }
        
        if(occupiedTargets[target] != pendingUser)
        {
            return true; // occupied by another user
        }

        return false; // its free to target for the pending user
    }

    public List<GameObject> GetFreeTargets(List<GameObject> targets, GameObject pendingUser)
    {
        List<GameObject> freeTargets = new();

        foreach(GameObject target in targets)
        {
            if(!IsOccupied(target, pendingUser))
            {
                freeTargets.Add(target);
            }
        }

        return freeTargets;
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
