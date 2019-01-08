using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public static bool EasyTouch = false;

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


        Max
    }

}
