using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public static bool EasyTouch = false;

    //受击的颜色
    public static byte R = 255;
    public static byte G = 150;
    public static byte B = 150;
    public static byte A = 255;
    //受击多少秒后返回正常
    public static float IdelRGB = 0.1f;

    public static List<GameObject> allEnemy=new List<GameObject>();

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
        Blow,
        Hurt,

        Max
    }

    public enum FriendAnimationCount
    {
        Idel,
        Move,
        Attack,
        Amass,
        Amassing,
        Back,
        Cast,
        Idel2,
        Run2,


        Max
    }

    public enum EnemyAnimationCount
    {
        Idel,
        Walk,
        Attack,
        //Attack2,
        Hurt,
        Die,
        Attack2,


        Max
    }

    public enum BallAnimationCount
    {
        BallStart,
        Balling,
        BallEnd,

        Max
    }
}
