using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Current;

    void Awake()
    {
        if(!Current)
        {
            Current=this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        
        //Invoke("UnlockFPS", .1f); // 45-60FPS FREEZES MY S10 AFTER PLAYING A WHILE
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void UnlockFPS()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public bool IsWindows()
    {
        return Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    
}