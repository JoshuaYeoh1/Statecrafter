using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightCycle : MonoBehaviour
{
    Light2D skyLight;

    void Awake()
    {
        skyLight=GetComponent<Light2D>();
    }

    public float duration=500;
    public Gradient gradient;

    void Update()
    {
        float percentage = Mathf.Sin(Time.time/duration * Mathf.PI*2) *.5f+.5f;

        percentage = Mathf.Clamp01(percentage);

        skyLight.color = gradient.Evaluate(percentage);
    }
}
