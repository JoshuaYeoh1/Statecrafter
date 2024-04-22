using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderAI : MonoBehaviour
{
    public Transform wanderTr;
    public Vector2 interval = new Vector2(1,4);
    public float innerRadius=1;
    public float outerRadius=5;
    Vector2 currentPos;

    void OnEnable()
    {
        StartCoroutine(Relocating());
    }

    IEnumerator Relocating()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(interval.x, interval.y));

            currentPos = RandomSpotInDoughnut();
        }
    }

    Vector3 RandomSpotInDoughnut()
    {
        float angle = Random.Range(0, Mathf.PI*2);

        float distance = Mathf.Sqrt(Random.Range(innerRadius*innerRadius, outerRadius*outerRadius));

        float x = distance * Mathf.Cos(angle);
        float y = distance * Mathf.Sin(angle);

        return transform.position + new Vector3(x, y);
    }

    void FixedUpdate()
    {
        wanderTr.position = currentPos;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 1, .5f);
        Gizmos.DrawWireSphere(transform.position, innerRadius);
        Gizmos.DrawWireSphere(transform.position, outerRadius);
    }
}
