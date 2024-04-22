using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutSpawner : MonoBehaviour
{
    public List<GameObject> prefabs = new();
    public Vector2Int spawnCount = new Vector2Int(10, 10);
    public float innerRadius=4;
    public float outerRadius=30;
    
    void Start()
    {
        for(int i=0; i<Random.Range(spawnCount.x, spawnCount.y+1); i++)
        {
            GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
            Spawn(prefab);
        }
    }

    void Spawn(GameObject prefab)
    {
        Vector2 randomSpot;

        bool gotSpace;

        do
        {
            randomSpot = RandomSpotInDoughnut();

            gotSpace=true;

            Station station = prefab.GetComponent<Station>();

            if(station)
            {
                foreach(Vector2 spot in StationManager.Current.occupiedSpots)
                {                
                    float distance = Vector3.Distance(spot, randomSpot);

                    if(distance <= station.stationSize*2)
                    {
                        gotSpace=false;
                    }
                }
            }

        } while(!gotSpace);
        
        GameObject spawned = Instantiate(prefab, randomSpot, Quaternion.identity);
        spawned.name = prefab.gameObject.name;
    }

    Vector3 RandomSpotInDoughnut()
    {
        float angle = Random.Range(0, Mathf.PI*2);

        float distance = Mathf.Sqrt(Random.Range(innerRadius*innerRadius, outerRadius*outerRadius));

        float x = distance * Mathf.Cos(angle);
        float y = distance * Mathf.Sin(angle);

        return transform.position + new Vector3(x, y);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, .5f);
        Gizmos.DrawWireSphere(transform.position, innerRadius);
        Gizmos.DrawWireSphere(transform.position, outerRadius);
    }
}
