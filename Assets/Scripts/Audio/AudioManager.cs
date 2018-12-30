using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    SourceManager source;
    ClipManager clipManager;

    private void Awake()
    {
        Instance = this;
        source = new SourceManager(gameObject);
        clipManager = new ClipManager();
    }

    //循环播放
    AudioSource bgmSouce;
    AudioClip bgmClip;
    public void LoopBgm(string clipName)
    {
        if (bgmSouce == null)
        {
            bgmSouce = source.GetFreeAudioSource();
        }
        if (bgmClip == null)
        {
            bgmClip = clipManager.FindClip(clipName);
        }
        if (bgmSouce.clip == null)
        {
            bgmSouce.clip = bgmClip;
        }
        if (!bgmSouce.isPlaying)
        {
            bgmSouce.Play();
        }
    }

    //开始播放
    public void StartAudio(string clipName)
    {
        AudioSource freeSouce = source.GetFreeAudioSource();
        AudioClip clip = clipManager.FindClip(clipName);
        freeSouce.clip = clip;
        freeSouce.Play();
    }
    //停止播放
    public void StopAudio(string clipName)
    {
        source.StopClip(clipName);
        source.DelSurplusAudioSource();

    }

    private void Start()
    {
       // LoopBgm("translate");
    }

    private void Update()
    {
        
    }

}
