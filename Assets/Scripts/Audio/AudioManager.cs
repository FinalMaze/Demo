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
        DelAudioSource();
        AudioSource freeSouce = source.GetFreeAudioSource();
        AudioClip clip = clipManager.FindClip(clipName);
        freeSouce.clip = clip;
        freeSouce.Play();
    }
    //停止播放
    public void StopAudio(string clipName)
    {
        source.StopClip(clipName);
        source.DelFeelClip();
        source.DelSurplusAudioSource();
    }
    //将播放完的音频清空，将多出的音频播放器删除
    public void DelAudioSource()
    {
        source.DelFeelClip();
        source.DelSurplusAudioSource();
    }

    private void Start()
    {
    }

    private void Update()
    {
        //LoopBgm("A");
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartAudio(Data.Audio.A.ToString());
        }
    }

}
