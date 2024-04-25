using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceSlam : MonoBehaviour
{
    Explosion2D explosion;

    void Awake()
    {
        explosion=GetComponent<Explosion2D>();
    }

    public void Slam()
    {
        EventManager.Current.OnMaceSlam(gameObject);

        explosion.Explode();
    }
}
