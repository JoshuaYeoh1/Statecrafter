using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationSpawner : MonoBehaviour
{
    public float innerRadius = 4;
    public float outerRadius = 30;

    Vector2 RandomSpotInDoughnut()
    {
        float angle = Random.Range(0, Mathf.PI*2);

        float distance = Mathf.Sqrt(Random.Range(innerRadius*innerRadius, outerRadius*outerRadius));

        float x = distance * Mathf.Cos(angle);
        float y = distance * Mathf.Sin(angle);

        return new Vector2(x, y);
    }

    public Station treePrefab;
    public Vector2Int treeCount = new Vector2Int(20, 20);

    public List<Station> quarryPrefabs = new();
    public Vector2Int quarryCount = new Vector2Int(10, 10);
    
    List<Vector3> occupiedSpots = new();

    void Start()
    {
        for(int i=0; i<Random.Range(treeCount.x, treeCount.y+1); i++)
        {
            Spawn(treePrefab);
        }
        
        for(int i=0; i<Random.Range(quarryCount.x, quarryCount.y+1); i++)
        {
            Station quarryPrefab = quarryPrefabs[Random.Range(0, quarryPrefabs.Count)];
            Spawn(quarryPrefab);
        }
    }

    void Spawn(Station station)
    {
        Vector2 randomSpot;

        bool gotSpace;

        do
        {
            randomSpot = RandomSpotInDoughnut();

            gotSpace=true;

            foreach(Vector2 spot in occupiedSpots)
            {
                float distance = Vector3.Distance(spot, randomSpot);

                if(distance <= station.size*2)
                {
                    gotSpace=false;
                }
            }

        } while(!gotSpace);
        
        Station spawned = Instantiate(station, randomSpot, Quaternion.identity);
        spawned.name = station.gameObject.name;

        occupiedSpots.Add(spawned.transform.position);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, .5f);
        Gizmos.DrawWireSphere(transform.position, innerRadius);
        Gizmos.DrawWireSphere(transform.position, outerRadius);
    }
}
