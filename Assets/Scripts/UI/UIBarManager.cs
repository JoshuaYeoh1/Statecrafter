using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIHider))]

public class UIBarManager : MonoBehaviour
{
    public GameObject owner;

    UIHider hider;

    void Awake()
    {
        hider=GetComponent<UIHider>();
    }

    void OnEnable()
    {
        EventManager.Current.UIBarUpdateEvent += OnUIBarUpdate;
    }
    void OnDisable()
    {
        EventManager.Current.UIBarUpdateEvent -= OnUIBarUpdate;

        LeanTween.cancel(gameObject);
    }
    
    void OnUIBarUpdate(GameObject owner, float value, float valueMax)
    {
        if(!owner && owner!=this.owner) return;

        TweenFilledImage(value/valueMax, .2f);
        TweenSlider(value/valueMax, .2f);

        if(hider)
        {
            hider.value = value;
            hider.valueMax = valueMax;
        }
    }

    public Image filledImage; 

    int tweenFilledImageId=0;

    public void TweenFilledImage(float to, float time)
    {
        if(!filledImage) return;

        LeanTween.cancel(tweenFilledImageId);

        tweenFilledImageId = LeanTween.value(filledImage.fillAmount, to, time)
            .setEaseInOutSine()
            .setIgnoreTimeScale(true)
            .setOnUpdate( (float value)=>{if(filledImage) filledImage.fillAmount=value;} )
            .id;
    }

    public Slider slider; 

    int tweenSliderId=0;

    public void TweenSlider(float to, float time)
    {
        if(!slider) return;

        LeanTween.cancel(tweenSliderId);

        tweenSliderId = LeanTween.value(slider.value, to, time)
            .setEaseInOutSine()
            .setIgnoreTimeScale(true)
            .setOnUpdate( (float value)=>{if(slider) slider.value=value;} )
            .id;
    }
}
