using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public static bool EasyTouch = false;

    public enum Audio
    {
        translate
    }

    public enum AnimationCount
    {
        Idel,
        Walk,
        Run,
        Jump,
        Jumping,
        Attack,
        Attack2,
        Amass,
        Cast,
        JumpEnd,

        Max
    }
}
