using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEvent : MonoBehaviour
{
    public GameObject toDestroy;

    public void DestroyAnim()
    {
        Destroy(toDestroy);
    }
}
