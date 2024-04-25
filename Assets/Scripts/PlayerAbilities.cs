using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAbility
{
    SpawnFood,
    FlintAndSteel,
    SpeedPotion,
    EnderPearl,
    MaceSlam,
    Herobrine,
}

public class PlayerAbilities : MonoBehaviour
{
    MouseManager mouseManager;

    void Start()
    {
        mouseManager = MouseManager.Current;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public PlayerAbility currentAbility;

    void OnEnable()
    {
        EventManager.Current.Click2DEvent += OnClick2D;
    }
    void OnDisable()
    {
        EventManager.Current.Click2DEvent -= OnClick2D;
    }

    public GameObject foodPrefab;
    public GameObject firePrefab;
    public GameObject speedPotPrefab;
    public GameObject enderPearlPrefab;
    public GameObject macePrefab;
    
    void OnClick2D(Vector3 pos)
    {
        switch(currentAbility)
        {
            case PlayerAbility.SpawnFood:
            {
                Spawn(foodPrefab, pos);
            } break;

            case PlayerAbility.FlintAndSteel:
            {
                Spawn(firePrefab, pos);
            } break;
            
            case PlayerAbility.SpeedPotion:
            {
                
            } break;
            
            case PlayerAbility.EnderPearl:
            {
                
            } break;
            
            case PlayerAbility.MaceSlam:
            {
                
            } break;
        }
    }

    void Spawn(GameObject prefab, Vector3 pos)
    {
        GameObject spawned = Instantiate(prefab, pos, Quaternion.identity);
        spawned.name = prefab.name;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public GameObject herobrine;

    void Update()
    {
        if(currentAbility==PlayerAbility.Herobrine)
        {
            herobrine.SetActive(true);

            herobrine.transform.position = mouseManager.mousePos;
        }
        else herobrine.SetActive(false);
    }
}
