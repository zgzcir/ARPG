using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSvc : MonoBehaviour
{
    public static AudioSvc Instance;

    public AudioSource bgAudio;
    public AudioSource uiAudio;

    public void InitSvc()
    {
        Instance = this;


        CommonTool.Log("AudioSvc Connected");
    }

     public void PlayBgAudio(string name, bool isLoop)
    {
        AudioClip audioClip = ResSvc.Instance.LoadAudio(name, true);
        if (bgAudio.clip == null || bgAudio.clip.name != audioClip.name)
        {
            bgAudio.clip = audioClip;
            bgAudio.loop = isLoop;
            bgAudio.Play();
        }
    }
    public void PlayUIAudio(string name)            
    {
        AudioClip audioClip = ResSvc.Instance.LoadAudio(name, true);
        uiAudio.clip = audioClip;
        uiAudio.Play();
    }
}            