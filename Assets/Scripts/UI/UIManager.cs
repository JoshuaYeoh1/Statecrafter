using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject inventoryClose;
    public GameObject inventoryOpen;

    void Update()
    {
        if(Input.GetButtonDown("Inventory"))
        {
            inventoryClose.SetActive(inventoryOpen.activeSelf);
            inventoryOpen.SetActive(!inventoryClose.activeSelf);
        }
    }
}
