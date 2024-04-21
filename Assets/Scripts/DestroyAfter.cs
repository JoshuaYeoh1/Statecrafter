using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public bool destroyOnEnabled;
    public Vector2 destroyAfter = new Vector2(3, 4);

    [Header("Shrink Anim")]
    public List<GameObject> shrinkObjects = new();
    public float shrinkTime=.5f;

    void OnEnable()
    {
        if(destroyOnEnabled) Destroying();
    }

    public void Destroying()
    {
        float waitTime = Random.Range(destroyAfter.x, destroyAfter.y);

        ShrinkAnim(waitTime);

        Destroy(gameObject, waitTime+shrinkTime);
    }

    void ShrinkAnim(float waitTime)
    {
        List<GameObject> objectsToShrink = new List<GameObject>();

        if(shrinkObjects.Count>0) objectsToShrink = shrinkObjects;
        else objectsToShrink.Add(gameObject);

        foreach(GameObject obj in objectsToShrink)
        {
            LeanTween.scale(obj, Vector3.zero, shrinkTime).setDelay(waitTime).setEaseInOutSine();
        }
    }

    public void DestroyNoAnim()
    {
        Destroy(gameObject);
    }
}
