using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartUIButton : MonoBehaviour
{
    public void Restart()
    {
        ScenesManager.Current.ReloadScene();
    }
}
