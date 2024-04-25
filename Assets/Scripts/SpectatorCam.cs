using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SpectatorCam : MonoBehaviour
{
    public static SpectatorCam Current;
    CinemachineVirtualCamera vcam;

    void Awake()
    {
        Current=this;
        vcam=GetComponent<CinemachineVirtualCamera>();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void OnEnable()
    {
        EventManager.Current.SpectateEvent += OnSpectate;
    }
    void OnDisable()
    {
        EventManager.Current.SpectateEvent -= OnSpectate;
    }
    
    void OnSpectate(GameObject watched)
    {
        vcam.m_Follow = watched.transform;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public GameObject spectatedNPC;
    public int spectatedIndex;

    void Start()
    {
        Spectate(NPCName.Steve);
    }

    public void Spectate(int i)
    {
        spectatedNPC = ActorManager.Current.npcs[i];
        spectatedIndex = i;
        EventManager.Current.OnSpectate(spectatedNPC);
    }

    public void Spectate(GameObject toWatch)
    {
        for(int i=0; i<ActorManager.Current.npcs.Count; i++)
        {
            if(ActorManager.Current.npcs[i]==toWatch)
            {
                Spectate(i);
                break;
            }
        }
    }

    public void Spectate(NPCName npcName)
    {
        foreach(GameObject npc in ActorManager.Current.npcs)
        {
            if(npc.TryGetComponent(out Steve steve))
            {
                if(steve.npcName==npcName)
                {
                    Spectate(npc);
                    break;
                }
            }
        }
    }

    void Update()
    {
        if(Input.GetButtonDown("Spectate"))
        {
            SpectateUp();
        }
    }    

    public void SpectateUp()
    {
        spectatedIndex++;

        if(spectatedIndex >= ActorManager.Current.npcs.Count)
        {
            spectatedIndex=0;
        }

        Spectate(spectatedIndex);
    }

    public void SpectateDown()
    {
        spectatedIndex--;

        if(spectatedIndex<0)
        {
            spectatedIndex = ActorManager.Current.npcs.Count-1;
        }

        Spectate(spectatedIndex);
    }

    
}
