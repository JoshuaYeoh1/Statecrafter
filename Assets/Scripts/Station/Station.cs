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
}

public class Station : MonoBehaviour
{
    public StationType type;

    public float size=2;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, .5f);
        Gizmos.DrawWireSphere(transform.position, size);
    }
}
