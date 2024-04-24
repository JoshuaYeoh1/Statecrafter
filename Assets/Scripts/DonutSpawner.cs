using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutSpawner : MonoBehaviour
{
    public List<GameObject> prefabs = new();
    public Vector2Int spawnCount = new Vector2Int(10, 10);
    public float innerRadius=4;
    public float outerRadius=30;
    public float spawnDelay;
    public bool hideInHierarchy;
    
    void Start()
    {
        Invoke(nameof(StartSpawn), spawnDelay);
    }

    void StartSpawn()
    {
        for(int i=0; i<Random.Range(spawnCount.x, spawnCount.y+1); i++)
        {
            GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
            Spawn(prefab);
        }
    }

    void Spawn(GameObject prefab, int retries=50)
    {
        Vector3 randomSpot = Vector3.zero;
        bool foundSpot=false;

        for(int i=0; i<retries; i++)
        {
            randomSpot = RandomSpotInDoughnut();
            bool gotSpace;

            float stationSize = prefab.TryGetComponent(out Station station) ? station.stationSize : 0;

            gotSpace = StationManager.Current.HasSpace(stationSize, randomSpot);

            if(gotSpace)
            {
                foundSpot=true;
                break;
            }
        }

        if(foundSpot)
        {
            GameObject spawned = Instantiate(prefab, randomSpot, Quaternion.identity);
            spawned.name = prefab.gameObject.name;
            if(hideInHierarchy) spawned.hideFlags = HideFlags.HideInHierarchy;
        }
        else Debug.LogWarning($"No space to spawn {prefab.name} after {retries} retries");
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
