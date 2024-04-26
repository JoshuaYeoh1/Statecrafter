using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : MonoBehaviour
{
    void OnEnable()
    {
        EventManager.Current.UpdateCraftEvent += OnUpdateCraft;
        EventManager.Current.UpdateNotCraftEvent += OnUpdateNotCraft;
    }
    void OnDisable()
    {
        EventManager.Current.UpdateCraftEvent -= OnUpdateCraft;
        EventManager.Current.UpdateNotCraftEvent -= OnUpdateNotCraft;
    }

    float progress;
    public float craftingSpeedMult=1;
    
    void OnUpdateCraft(GameObject crafter, GameObject station, Recipe recipe)
    {
        if(station!=gameObject) return;

        progress += craftingSpeedMult * Time.deltaTime;

        if(progress >= recipe.craftingTime)
        {
            progress=0;

            EventManager.Current.OnCrafted(crafter, gameObject, recipe);
        }

        EventManager.Current.OnUIBarUpdate(gameObject, progress, recipe.craftingTime);
    }

    void OnUpdateNotCraft(GameObject station, Recipe recipe)
    {
        if(station!=gameObject) return;

        progress=0;

        EventManager.Current.OnUIBarUpdate(gameObject, progress, progress+1);
    }
}
