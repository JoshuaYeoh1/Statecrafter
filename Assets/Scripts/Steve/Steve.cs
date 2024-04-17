using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steve : MonoBehaviour
{
    [HideInInspector] public ForceVehicle2D vehicle;
    [HideInInspector] public PursuitBehaviour move;

    void Awake()
    {
        vehicle=GetComponent<ForceVehicle2D>();
        move=GetComponent<PursuitBehaviour>();
    }
}
