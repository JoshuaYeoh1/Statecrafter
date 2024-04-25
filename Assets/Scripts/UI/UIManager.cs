using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject inventoryCanvas;

    void Update()
    {
        if(Input.GetButtonDown("Inventory"))
        {
            inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
        }
    }
}
