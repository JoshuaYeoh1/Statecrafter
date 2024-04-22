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

    public List<Vector3> occupiedSpots = new();
    
    public Dictionary<GameObject, GameObject> occupiedStations = new(); // station, user

    public void OccupyStation(GameObject station, GameObject user)
    {
        if(!occupiedStations.ContainsKey(station))
        {
            occupiedStations[station] = user;
        }
    }

    public void UnoccupyStation(GameObject station, GameObject user)
    {
        if(occupiedStations.ContainsKey(station))
        {
            if(occupiedStations[station]==user)
            {
                occupiedStations.Remove(station);
            }
        }
    }

    public bool IsOccupied(GameObject station, GameObject pendingUser)
    {
        if(!occupiedStations.ContainsKey(station))
        {
            return false; // Station is not occupied
        }
        
        if(occupiedStations[station] != pendingUser)
        {
            return true; // Station is occupied by another user
        }

        return false; // its free to use for the pending user
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
