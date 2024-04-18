using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StationType
{
    Bed,
    CraftingTable,
    Furnace,
    Quarry    
}

public class Station : MonoBehaviour
{
    public StationType type;
}
