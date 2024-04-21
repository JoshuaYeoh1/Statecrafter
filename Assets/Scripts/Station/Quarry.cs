using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuarryType
{
    Tree,
    StoneQuarry,
    CoalQuarry,
    IronQuarry,
    DiamondQuarry,
}

public class Quarry : MonoBehaviour
{
    public QuarryType type;

    public Bounds spawnBounds;

    void OnEnable()
    {
        StartCoroutine(CheckingOres());
        StartCoroutine(CheckingEnemies());
    }

    void Update()
    {
        RemoveNulls(currentOres);
        RemoveNulls(currentEnemies);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Resource")]
    public List<GameObject> orePrefabs = new();
    public int maxOres=3;
    public List<GameObject> currentOres = new();
    public Vector2 oreSpawnTime = new Vector2(1, 5);

    IEnumerator CheckingOres()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(oreSpawnTime.x, oreSpawnTime.y));
            CheckOres();
        }
    }

    void CheckOres()
    {
        if(orePrefabs.Count==0) return;

        if(currentOres.Count<maxOres)
        {
            GameObject spawned = Spawn(orePrefabs, true);

            spawned.transform.position = GetRandomSpot(spawnBounds);

            currentOres.Add(spawned);
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
}
