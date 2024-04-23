using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WiggleLight2D : MonoBehaviour
{
    Light2D myLight;

    [Header("Wiggle")]
    public bool wiggle=true;
    public float frequency=3;
    public float magnitudeMult=.5f;

    float seed, defIntensity;

    void Awake()
    {
        myLight=GetComponent<Light2D>();
        defIntensity=myLight.intensity;

        seed=Random.Range(-9999f,9999f);
    }

    void Update()
    {
        if(!wiggle) return;

        float noise = (Mathf.PerlinNoise(seed, Time.time * frequency) *2-1) * (defIntensity * magnitudeMult);

        myLight.intensity = defIntensity + noise;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void Shake(float t=.2f)
    {
        if(shakeRt!=null) StopCoroutine(shakeRt);
        shakeRt = StartCoroutine(Shaking(t));
    }
    Coroutine shakeRt;
    IEnumerator Shaking(float t)
    {
        wiggle=true;
        yield return new WaitForSeconds(t);
        wiggle=false;
        myLight.intensity = defIntensity;
    }
}
