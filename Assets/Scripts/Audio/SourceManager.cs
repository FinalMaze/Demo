﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceManager
{
    List<AudioSource> allSource;
    GameObject tmpObj;

    public SourceManager(GameObject obj)
    {
        tmpObj = obj;
        Initial();
    }

    public void Initial()
    {
        allSource = new List<AudioSource>();
        for (int i = 0; i < 3; i++)
        {
            AudioSource tmpSource = tmpObj.AddComponent<AudioSource>();
            allSource.Add(tmpSource);
        }
    }
    //得到空闲的音频播放器
    public AudioSource GetFreeAudioSource()
    {
        for (int i = 0; i < allSource.Count; i++)
        {
            if (!allSource[i].isPlaying)
            {
                return allSource[i];
            }
        }
        AudioSource tmpSource = tmpObj.AddComponent<AudioSource>();
        allSource.Add(tmpSource);
        return tmpSource;
    }
    //根据音频名称查找播放器
    public AudioSource GetSameClipSource(string clipName)
    {
        for (int i = 0; i < allSource.Count; i++)
        {
            if (allSource[i].clip != null)
            {
                if (allSource[i].clip.name == clipName)
                {
                    return allSource[i];
                }
            }
        }
        return null;
    }

    //停止正在播放的音频
    public void StopClip(string clipName)
    {

        for (int i = 0; i < allSource.Count; i++)
        {
            if (allSource[i].isPlaying && allSource[i].clip.name == clipName)
            {
                allSource[i].Stop();
                allSource[i].clip = null;
            }
        }
    }

    //将没有播放的音频清空
    public void DelFeelClip()
    {
        for (int i = 0; i < allSource.Count; i++)
        {
            if (!allSource[i].isPlaying && allSource[i].clip != null)
            {
                allSource[i].clip = null;
            }
        }
    }

    //删除多余的音频播放器
    public void DelSurplusAudioSource()
    {
        int count = 0;
        List<AudioSource> allSurplus = new List<AudioSource>();
        for (int i = allSource.Count - 1; i >= 0; i--)
        {
            if (!allSource[i].isPlaying)
            {
                count++;
                if (count > 3)
                {
                    allSurplus.Add(allSource[i]);
                }
            }
        }
        for (int i = 0; i < allSurplus.Count; i++)
        {
            RemoveAudio(allSurplus[i]);
        }
        allSurplus.Clear();
        allSurplus = null;

    }

    public void RemoveAudio(AudioSource audio)
    {
        allSource.Remove(audio);
        GameObject.Destroy(audio);
    }
}
