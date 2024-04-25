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
        EventManager.Current.Swipe2DEvent += OnSwipe2D;
    }
    void OnDisable()
    {
        EventManager.Current.Click2DEvent -= OnClick2D;
        EventManager.Current.Swipe2DEvent -= OnSwipe2D;
    }

    public GameObject foodPrefab;
    public GameObject firePrefab;
    public EnderPearl enderPearlPrefab;
    public GameObject macePrefab;
    
    void OnClick2D(Vector3 pos)
    {
        switch(currentAbility)
        {
            case PlayerAbility.SpawnFood: Spawn(foodPrefab, pos); break;

            case PlayerAbility.FlintAndSteel: Spawn(firePrefab, pos); break;
            
            case PlayerAbility.SpeedPotion: ItemManager.Current.Spawn(Item.SpeedPotion, pos); break;
            
            case PlayerAbility.EnderPearl: SpawnEnderPearl(pos, 0, Vector3.zero); break;
            
            case PlayerAbility.MaceSlam: Spawn(macePrefab, pos); break;
        }
    }

    void OnSwipe2D(Vector3 startPos, float magnitude, Vector3 direction, Vector3 endPos, Vector3 midPos)
    {
        switch(currentAbility)
        {
            case PlayerAbility.EnderPearl: SpawnEnderPearl(SpectatorCam.Current.spectatedNPC.transform.position, magnitude, direction); break;
        }
    }

    void Spawn(GameObject prefab, Vector3 pos)
    {
        GameObject spawned = Instantiate(prefab, pos, Quaternion.identity);
        spawned.name = prefab.name;
    }

    void SpawnEnderPearl(Vector3 pos, float force, Vector3 dir)
    {
        EnderPearl pearl = Instantiate(enderPearlPrefab, pos, Quaternion.identity);
        pearl.gameObject.name = enderPearlPrefab.name;

        pearl.Throw(force, dir);
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
