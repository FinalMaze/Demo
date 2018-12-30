using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClipManager
{
    string[] allClip;
    AudioClip[] allAudioClip;
    public ClipManager()
    {
        Initial();
        LoadClip();
    }

    public void Initial()
    {
        allClip = Enum.GetNames(typeof(Data.Audio));
    }
    //加载音频
    public void LoadClip()
    {
        allAudioClip = new AudioClip[allClip.Length];
        for (int i = 0; i < allClip.Length; i++)
        {

            AudioClip tmpClip = Resources.Load<AudioClip>("Audio/" + allClip[i]);
            allAudioClip[i] = tmpClip;
        }
    }
    //找到音频
    public AudioClip FindClip(string clipName)
    {
        for (int i = 0; i < allAudioClip.Length; i++)
        {
            if (allAudioClip[i].name==clipName)
            {
                return allAudioClip[i];
            }
        }
        return null;
    }

}
