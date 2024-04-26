using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }

    ////////////////////////////////////////////////////////////////////////////////////

    [Header("Chivalry")]
    public AudioClip[] sfxUIHeartbeat;

    [Header("CSGO")]
    public AudioClip[] sfxUIClockIn;

    [Header("FEAR")]
    public AudioClip[] sfxCage;
    public AudioClip[] sfxCageBreak;
    public AudioClip[] sfxCamBtn;
    public AudioClip[] sfxDoor;
    public AudioClip[] sfxUIExpel;
    public AudioClip[] sfxShutter;

    [Header("L4D2")]
    public AudioClip[] sfxCageBtn;
    public AudioClip[] sfxUIExorcistUpdate;
    public AudioClip[] sfxFlashBtn;
    public AudioClip[] sfxLightSwitch;
    public AudioClip[] sfxReportBtn;
    public AudioClip[] sfxShutterBreak;
    public AudioClip[] sfxShutterHit;
    public AudioClip[] sfxSoundPitchBtn;

    [Header("Lego")]
    public AudioClip[] sfxPcBtn;

    [Header("Minecraft")]
    public AudioClip[] sfxUIAppearAtWindow;
    public AudioClip[] sfxUIExorcise;

    [Header("PvZ")]
    public AudioClip[] sfxUIPaper;

    [Header("SpookyHouse")]
    public AudioClip[] sfxToyolLoop;    

    [Header("Yeoh")]
    public AudioClip[] sfxSoundPitchLoop;
    public AudioClip[] sfxExorciseFailScream;
    public AudioClip[] sfxStaticLoop;
    public AudioClip[] sfxJumpscare;
}
