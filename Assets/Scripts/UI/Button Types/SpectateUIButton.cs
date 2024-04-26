using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectateUIButton : MonoBehaviour
{
    public void SwitchSpectate()
    {
        EventManager.Current.OnSwitchSpectate();
    }
}
