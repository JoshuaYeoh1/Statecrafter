using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCName
{
    Steve,
    Alex,
}

public enum EnemyName
{
    Zombie,
    Skeleton,
    Spider,
}

public class ActorManager : MonoBehaviour
{
    public static ActorManager Current;

    void Awake()
    {
        Current=this;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public List<GameObject> npcs = new();
    public List<GameObject> enemies = new();
}
