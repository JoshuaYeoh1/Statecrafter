using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////

    public AudioMixer mixer;
    public const string MASTER_KEY = "masterVolume";
    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";

    [Header("Defaults")]
    public float masterVolume=1;
    public float musicVolume=1;
    public float sfxVolume=1;

    void Start()
    {
        LoadSettings();
    }

    void LoadSettings()
    {   
        masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, masterVolume);
        musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, musicVolume);
        sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, sfxVolume);

        mixer.SetFloat(MASTER_KEY, Log10(masterVolume));
        mixer.SetFloat(MUSIC_KEY, Log10(musicVolume));
        mixer.SetFloat(SFX_KEY, Log10(sfxVolume));
    }

    float Log10(float value)
    {
        return Mathf.Log10(value)*20;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////

    Dictionary<AudioSource, int> tweenVolumeIds = new Dictionary<AudioSource, int>();
    
    public void TweenVolume(AudioSource source, float to, float time=3)
    {
        if(tweenVolumeIds.ContainsKey(source))
        {
            LeanTween.cancel(tweenVolumeIds[source]);
        }
        
        if(time>0)
        {
            tweenVolumeIds[source] = LeanTween.value(source.volume, to, time)
                .setEaseInOutSine()
                .setIgnoreTimeScale(true)
                .setOnUpdate( (float value)=>{source.volume=value;} )
                .id;
        }
        else source.volume=to;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("SFX")]
    public GameObject SFXObjectPrefab;
    public bool hideInHierarchy=true;

    void SetAudioSettings(AudioSource source, bool spatialBlend=true, bool randPitch=true, float panStereo=0, float volume=1, float minRadius=15)
    {
        source.spatialBlend = spatialBlend ? 1 : 0;
        if(randPitch) source.pitch = 1+Random.Range(-.1f,.1f);
        source.panStereo = panStereo;
        source.volume = volume;
        source.maxDistance = minRadius * 2;
        source.minDistance = minRadius;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void PlaySFX(AudioClip[] clips, Vector3 pos, bool spatialBlend=true, bool randPitch=true, float panStereo=0, float volume=1, float minRadius=15)
    {   
        GameObject obj = Instantiate(SFXObjectPrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;

        AudioSource source = obj.GetComponent<AudioSource>();

        source.clip = clips[Random.Range(0,clips.Length)];

        SetAudioSettings(source, spatialBlend, randPitch, panStereo, volume, minRadius);

        source.Play();

        Destroy(source.gameObject, source.clip.length);
    }

    public void PlaySFX(AudioClip clip, Vector3 pos, bool spatialBlend=true, bool randPitch=true, float panStereo=0, float volume=1, float minRadius=15)
    {
        AudioClip[] clips = {clip};

        PlaySFX(clips, pos, spatialBlend, randPitch, panStereo, volume, minRadius);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void PlayVoice(AudioSource voiceSource, AudioClip[] clips, bool randPitch=true, float volume=1, bool spatialBlend=true, float minRadius=15, float panStereo=0)
    {   
        voiceSource.clip = clips[Random.Range(0,clips.Length)];
        
        SetAudioSettings(voiceSource, spatialBlend, randPitch, panStereo, volume, minRadius);

        voiceSource.Play();
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public AudioSource LoopSFX(GameObject owner, AudioClip[] loopClip, bool spatialBlend=true, bool randPitch=true, float panStereo=0, float volume=1, float minRadius=15)
    {
        return LoopSFX(owner, null, loopClip, spatialBlend, randPitch, panStereo, volume, minRadius);
    }

    public AudioSource LoopSFX(GameObject owner, AudioClip[] inClips, AudioClip[] loopClips, bool spatialBlend=true, bool randPitch=true, float panStereo=0, float volume=1, float minRadius=15)
    {   
        GameObject obj = Instantiate(SFXObjectPrefab, owner.transform.position, owner.transform.rotation);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;

        AudioSource loopSource = obj.GetComponent<AudioSource>();
        loopSource.transform.parent = owner.transform;
        
        SetAudioSettings(loopSource, spatialBlend, randPitch, panStereo, volume, minRadius);

        if(HasClips(inClips))
        {
            loopSource.clip = inClips[Random.Range(0,inClips.Length)];
            loopSource.Play();

            if(startingLoopRts.ContainsKey(loopSource))
            {
                if(startingLoopRts[loopSource]!=null)
                StopCoroutine(startingLoopRts[loopSource]);
            }

            startingLoopRts[loopSource] = StartCoroutine(StartingLoop(loopSource, loopClips, loopSource.clip.length));
        }
        else
        {
            StartLoop(loopSource, loopClips);
        }

        return loopSource;
    }

    Dictionary<AudioSource, Coroutine> startingLoopRts = new Dictionary<AudioSource, Coroutine>();

    IEnumerator StartingLoop(AudioSource loopSource, AudioClip[] loopClips, float delay)
    {
        if(delay>0) yield return new WaitForSeconds(delay);
        StartLoop(loopSource, loopClips);
    }

    void StartLoop(AudioSource loopSource, AudioClip[] loopClips)
    {
        loopSource.clip = loopClips[Random.Range(0,loopClips.Length)];
        loopSource.loop = true;
        loopSource.Play();
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void StopLoop(AudioSource loopSource, AudioClip[] outClips)
    {
        if(!loopSource) return;
        
        if(HasClips(outClips))
        {
            loopSource.clip = outClips[Random.Range(0,outClips.Length)];
            loopSource.loop = false;
            loopSource.Play();
        }

        Destroy(loopSource.gameObject, HasClips(outClips) ? loopSource.clip.length : 0); // Only delay destroy if there was an outClip
    }

    public void StopLoop(AudioSource loopSource)
    {
        StopLoop(loopSource, null);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public bool HasClips(AudioClip[] clips)
    {
        return clips!=null && clips.Length>0;
    }
}
