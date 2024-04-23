using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StationType
{
    Bed,
    CraftingTable,
    Furnace,
    Tree,
    Quarry,
    Group,
    Plant,
}

public class Station : MonoBehaviour
{
    public StationType type;
    public float stationSize=2;

    void OnEnable()
    {
        StationManager.Current.occupiedSpots.Add(transform, this);
    }
    void OnDisable()
    {
        StationManager.Current.occupiedSpots.Remove(transform);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, .5f);
        Gizmos.DrawWireSphere(transform.position, stationSize);
    }
}
