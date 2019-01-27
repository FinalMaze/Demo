using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCtrl : MonoBehaviour
{
	void Update ()
    {
        AudioManager.Instance.LoopBgm(Data.Audio.BGM.ToString());
	}
}
