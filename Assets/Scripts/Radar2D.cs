using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar2D : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(SlowUpdate());
    }
    
    IEnumerator SlowUpdate()
    {
        while(true)
        {
            Scan();
            yield return new WaitForSeconds(.1f);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public float range=5;
    public LayerMask layers;
    public List<GameObject> targets = new();

    public void Scan()
    {
        targets.Clear();

        Collider2D[] others = Physics2D.OverlapCircleAll(transform.position, range, layers);
        
        foreach(Collider2D other in others)
        {
            if(other.attachedRigidbody) //if collider has rigidbody
            {
                targets.Add(other.attachedRigidbody.gameObject);
            }
            //else targets.Add(other.gameObject); 
        }
    }

    public GameObject GetClosest(List<GameObject> targets)
    {
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;

        foreach(GameObject target in targets) // go through all detected colliders
        {
            float distance = Vector3.Distance(target.transform.position, transform.position);

            if(distance<closestDistance) // find and replace with the nearer one
            {
                closestDistance = distance;
                closest = target;
            }
        }

        return closest;
    }

    public List<GameObject> GetTargetsWithTag(string tag)
    {
        List<GameObject> matches = new();

        foreach(GameObject target in targets)
        {
            if(target.tag==tag)
            {
                matches.Add(target);
            }
        }

        return matches;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, .5f);
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
