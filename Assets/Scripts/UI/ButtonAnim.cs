using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnim : MonoBehaviour
{
    public float animTime=.3f, scaleMult=.1f;
    bool inBtn, pressedBtn;

    Vector2 defScale;

    //public bool doSFX=true;

    void Awake()
    {
        defScale = transform.localScale;
    }

    void OnDisable()
    {
        LeanTween.cancel(gameObject);
        ResetButton();
        inBtn=pressedBtn=false;
    }

    public void OnMouseEnter()
    {
        inBtn=true;

        if(!pressedBtn)
        {
            LeanTween.cancel(gameObject);
            LeanTween.scale(gameObject, defScale*(1+scaleMult), animTime).setEaseOutExpo().setIgnoreTimeScale(true);
            
            // if(doSFX)
            // AudioManager.Current.PlaySFX(SFXManager.Current.sfxUIHover, transform.position, false);
        }
    }

    public void OnMouseExit()
    {
        inBtn=false;

        if(!pressedBtn)
        {
            LeanTween.cancel(gameObject);
            LeanTween.scale(gameObject, defScale, animTime).setEaseOutExpo().setIgnoreTimeScale(true);

            // if(doSFX)
            // AudioManager.Current.PlaySFX(SFXManager.Current.sfxUIHover, transform.position, false);
        }
    }

    public void OnMouseDown()
    {
        if(inBtn)
        {
            pressedBtn=true;

            LeanTween.cancel(gameObject);
            transform.localScale = defScale*(1+scaleMult);
            LeanTween.scale(gameObject, defScale*(1-scaleMult), animTime/2).setEaseOutExpo().setIgnoreTimeScale(true);

            // if(doSFX)
            // AudioManager.Current.PlaySFX(SFXManager.Current.sfxUIClick, transform.position, false);
        }
    }

    public void OnMouseUp()
    {
        pressedBtn=false;

        LeanTween.cancel(gameObject);

        if(inBtn)
        LeanTween.scale(gameObject, defScale*(1+scaleMult), animTime/2).setDelay(animTime/2).setEaseOutBack().setIgnoreTimeScale(true);

        else
        LeanTween.scale(gameObject, defScale, animTime).setEaseOutBack().setIgnoreTimeScale(true);
    }

    public void ResetButton()
    {
        transform.localScale = defScale;
    }

    // public void SfxUIBack()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxUIBack, transform.position, false);
    // }
}
