using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public enum SoundClipPrefab
    {
        Bow_String, Arrow_Hit_Target, Arrow_Release,
        Cannon_Wheels, CannonFire,
        Human_FootSteps, Human_Get_Hit, Human_Death, Human_Sword, Humans_Fighting,
        Horse_Spawn, Horse_Get_Hit, Horse_Death, Horse_Footsteps
    }
    [SerializeField]
    public AudioClip Bow_String, Arrow_Hit_Target, Arrow_Release,
        Cannon_Wheels, CannonFire,
        Human_FootSteps, Human_Get_Hit, Human_Death, Human_Sword, Humans_Fighting,
        Horse_Spawn, Horse_Get_Hit, Horse_Death, Horse_Footsteps;
    
    bool playLoopFootSteps = false;
    private static AudioManager inst = null;
    private AudioClip theClip = null;

    void Awake()
    {
        inst = this;
    }
    private void Start()
    {
        // Listen for messages
        MessageDispatch.GetInstance().AddMessageHandler(MessageReceived);
    }
    public static AudioManager GetInstance()
    {
        return inst;
    }
    public void PlayTheSound(AudioClip sound, AudioSource source)
    {
        if (source != null)
        {
#if UNITY_EDITOR
            Debug.Log("Sound played: " + sound);
#endif
            source.clip = sound;
            source.Play();
        }

    }
   

    public void MessageReceived(SoundClipPrefab clip, AudioSource source)
    {
        source.pitch = 1;

        switch (clip)
        {
            case SoundClipPrefab.Bow_String:
                PlayTheSound(Bow_String, source);
                break;
            case SoundClipPrefab.Arrow_Hit_Target:
                PlayTheSound(Arrow_Hit_Target, source);
                break;
            case SoundClipPrefab.Arrow_Release:
                PlayTheSound(Arrow_Release, source);
                break;
            case SoundClipPrefab.Cannon_Wheels:
                source.loop = true;
                PlayTheSound(Cannon_Wheels, source);
                break;
            case SoundClipPrefab.CannonFire:
                source.loop = false;
                PlayTheSound(CannonFire, source);
                break;
            case SoundClipPrefab.Human_FootSteps:

                PlayTheSound(Human_FootSteps, source);
                break;
            case SoundClipPrefab.Human_Get_Hit:
                PlayTheSound(Human_Get_Hit, source);
                break;
            case SoundClipPrefab.Human_Death:
                PlayTheSound(Human_Death, source);
                break;
            case SoundClipPrefab.Human_Sword:
                PlayTheSound(Human_Sword, source);
                break;
            case SoundClipPrefab.Humans_Fighting:
                PlayTheSound(Humans_Fighting, source);
                break;
            case SoundClipPrefab.Horse_Spawn:
                PlayTheSound(Horse_Spawn, source);
                break;
            case SoundClipPrefab.Horse_Get_Hit:
                PlayTheSound(Horse_Get_Hit, source);
                break;
            case SoundClipPrefab.Horse_Death:
                PlayTheSound(Horse_Death, source);
                break;
            case SoundClipPrefab.Horse_Footsteps:
                PlayTheSound(Horse_Footsteps, source);
                break;

            default:
                break;
        }





    }

    public void OnAuidioEvent(string eventName)
    {
        throw new System.NotImplementedException();
    }
    public bool GetIfLoopingFsteps() {
        return playLoopFootSteps;
    }
    /*
    public void SetIfLoopingFsteps(bool b) {
        playLoopFootSteps = b;
        if (playLoopFootSteps)
        {
            if (!audioSourceFootSteps.isPlaying)
            {
                audioSourceFootSteps.loop = true;
                audioSourceFootSteps.Play();
            }
        }
        else {
            audioSourceFootSteps.Stop();
        }
    }
    */
}
