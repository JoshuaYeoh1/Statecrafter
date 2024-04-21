using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceSprite : MonoBehaviour
{
    public SpriteRenderer sr;

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

    public Sprite offSprite;
    public Sprite onSprite;
    
    void OnUpdateCraft(GameObject crafter, GameObject station, Recipe recipe)
    {
        if(station!=gameObject) return;

        sr.sprite = onSprite;
    }

    void OnUpdateNotCraft(GameObject station)
    {
        if(station!=gameObject) return;

        sr.sprite = offSprite;
    }
}
