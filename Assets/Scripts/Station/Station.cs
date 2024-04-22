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
}

public class Station : MonoBehaviour
{
    public StationType type;

    public float stationSize=2;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, .5f);
        Gizmos.DrawWireSphere(transform.position, stationSize);
    }

    void OnEnable()
    {
        StationManager.Current.occupiedSpots.Add(transform.position);
    }
    void OnDisable()
    {
        StationManager.Current.occupiedSpots.Remove(transform.position);
    }
}
