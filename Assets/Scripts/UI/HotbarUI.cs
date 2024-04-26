using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarUI : MonoBehaviour
{
    void OnEnable()
    {
        EventManager.Current.SelectPlayerAbilityEvent += OnSelectPlayerAbility;
    }
    void OnDisable()
    {
        EventManager.Current.SelectPlayerAbilityEvent -= OnSelectPlayerAbility;
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void OnSelectPlayerAbility(PlayerAbility ability)
    {
        HighlightItem((int)ability-1);
    }

    public void SelectPlayerAbility(int i)
    {
        EventManager.Current.OnSelectPlayerAbility((PlayerAbility)i);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public GameObject[] selectFrames;

    void HighlightItem(int i)
    {
        if(i<0 || i>=selectFrames.Length)
        {
            foreach(GameObject select in selectFrames)
            {
                select.SetActive(false);
            }
            return;
        }

        foreach(GameObject select in selectFrames)
        {
            if(select!=selectFrames[i])
            select.SetActive(false);
        }

        selectFrames[i].SetActive(true);
    }
}
