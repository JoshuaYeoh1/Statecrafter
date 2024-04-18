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

    public float spawnRadius=3;

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
            GameObject prefab = orePrefabs[Random.Range(0, orePrefabs.Count)];

            GameObject spawned = Instantiate(prefab, transform);

            Vector2 randomSpot = Random.insideUnitCircle * spawnRadius;

            spawned.transform.position += new Vector3(randomSpot.x, randomSpot.y, 0);

            currentOres.Add(spawned);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

            GameObject spawned = Instantiate(prefab);

            Vector2 randomSpot = Random.insideUnitCircle * spawnRadius;

            spawned.transform.position += new Vector3(randomSpot.x, randomSpot.y, 0);

            currentEnemies.Add(spawned);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void RemoveNulls(List<GameObject> list)
    {
        list.RemoveAll(item => item == null);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, .5f);
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
