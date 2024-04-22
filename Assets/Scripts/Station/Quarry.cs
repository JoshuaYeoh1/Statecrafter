using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quarry : MonoBehaviour
{
    public Bounds spawnBounds;

    void OnEnable()
    {
        StartCoroutine(CheckingOres());
        StartCoroutine(CheckingEnemies());
    }

    void Update()
    {
        RemoveNulls(currentResources);
        RemoveNulls(currentEnemies);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Resource")]
    public List<GameObject> resourcePrefabs = new();
    public int maxResources=3;
    public List<GameObject> currentResources = new();
    public Vector2 resourceSpawnTime = new Vector2(1, 5);

    IEnumerator CheckingOres()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(resourceSpawnTime.x, resourceSpawnTime.y));

            if(!IsInView() || !visibleOnlySpawn)
            CheckOres();
        }
    }

    void CheckOres()
    {
        if(resourcePrefabs.Count==0) return;

        if(currentResources.Count<maxResources)
        {
            GameObject spawned = Spawn(resourcePrefabs, true);

            spawned.transform.position = GetRandomSpot(spawnBounds);

            currentResources.Add(spawned);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Enemy")]
    public List<GameObject> enemyPrefabs = new();
    public int maxEnemies=3;
    public List<GameObject> currentEnemies = new();
    public Vector2 enemySpawnTime = new Vector2(10, 15);

    IEnumerator CheckingEnemies()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(enemySpawnTime.x, enemySpawnTime.y));

            if(!IsInView() || !visibleOnlySpawn)
            CheckEnemies();
        }
    }

    void CheckEnemies()
    {
        if(enemyPrefabs.Count==0) return;

        if(currentEnemies.Count<maxEnemies)
        {
            GameObject spawned = Spawn(enemyPrefabs, false);

            spawned.transform.position = GetRandomSpot(spawnBounds);

            currentEnemies.Add(spawned);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject Spawn(List<GameObject> prefabs, bool parent)
    {
        GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];

        GameObject spawned = Instantiate(prefab, transform.position, Quaternion.identity);
        if(parent) spawned.transform.parent = transform;
        spawned.name = prefab.name;

        return spawned;
    }

    Vector3 GetRandomSpot(Bounds bounds)
    {
        Vector3 randomSpot = transform.position + new Vector3
        (
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );

        return randomSpot;
    }

    void RemoveNulls(List<GameObject> list)
    {
        list.RemoveAll(item => item == null);
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, .5f);
        Gizmos.DrawWireCube(transform.position + spawnBounds.center, spawnBounds.size);
    }

    public bool visibleOnlySpawn=true;
    
    bool IsInView()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        return viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 && viewportPosition.y < 1;
    }
}
