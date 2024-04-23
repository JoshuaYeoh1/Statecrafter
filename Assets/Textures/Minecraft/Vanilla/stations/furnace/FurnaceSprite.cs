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
    public GameObject furnaceLight;
    
    void OnUpdateCraft(GameObject crafter, GameObject station, Recipe recipe)
    {
        if(station!=gameObject) return;

        sr.sprite = onSprite;

        furnaceLight.SetActive(true);
    }

    void OnUpdateNotCraft(GameObject station)
    {
        if(station!=gameObject) return;

        sr.sprite = offSprite;

        furnaceLight.SetActive(false);
    }
}
