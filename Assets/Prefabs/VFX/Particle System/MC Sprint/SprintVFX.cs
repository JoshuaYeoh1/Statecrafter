using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintVFX : MonoBehaviour
{
    public Rigidbody2D rb;
    ParticleSystem ps;

    void Awake()
    {
        ps=GetComponent<ParticleSystem>();
    }

    public float minVelocity=2;

    void Update()
    {
        if(rb.velocity.magnitude>=minVelocity)
        {
            ToggleParticles(true);
        }
        else ToggleParticles(false);
    }

    void ToggleParticles(bool toggle)
    {
        if(ps.isEmitting!=toggle)
        {
            if(toggle) ps.Play();
            else ps.Stop();
        }
    }
}
