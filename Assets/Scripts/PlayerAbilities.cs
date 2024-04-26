using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAbility
{
    None=0,
    SpawnFood=1,
    FlintAndSteel=2,
    SpeedPotion=3,
    EnderPearl=4,
    MaceSlam=5,
    Herobrine=6,
}

public class PlayerAbilities : MonoBehaviour
{
    public PlayerAbility currentAbility;

    void OnEnable()
    {
        EventManager.Current.Click2DEvent += OnClick2D;
        EventManager.Current.Swipe2DEvent += OnSwipe2D;
        EventManager.Current.SelectPlayerAbilityEvent += OnSelectPlayerAbility;
    }
    void OnDisable()
    {
        EventManager.Current.Click2DEvent -= OnClick2D;
        EventManager.Current.Swipe2DEvent -= OnSwipe2D;
        EventManager.Current.SelectPlayerAbilityEvent -= OnSelectPlayerAbility;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

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

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

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

    void Update()
    {
        MoveHerobrine();
        CheckNumberButtons();
    }

    public GameObject herobrine;

    void MoveHerobrine()
    {
        herobrine.SetActive(currentAbility==PlayerAbility.Herobrine);

        herobrine.transform.position = MouseManager.Current.mousePos;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Start()
    {
        EventManager.Current.OnSelectPlayerAbility(PlayerAbility.SpawnFood);
    }

    void OnSelectPlayerAbility(PlayerAbility ability)
    {
        currentAbility = ability;

        Cursor.visible = currentAbility!=PlayerAbility.Herobrine;
    }

    void CheckNumberButtons()
    {
        for(int i=0; i<=9; i++)
        {
            if(Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                if(i>6)
                {
                    EventManager.Current.OnSelectPlayerAbility(PlayerAbility.None);
                }
                else
                {
                    EventManager.Current.OnSelectPlayerAbility((PlayerAbility)i);
                }
            }
        }
    }
}
