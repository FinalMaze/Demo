using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendData
{
    #region 动作状态信息
    //是否在移动中
    public static bool Moving = false;
    //是否在攻击中
    public static bool Attacking = false;
    //是否在蓄力中
    public static bool Amassing = false;
    //是否在Back中
    public static bool Backing = false;
    //是否可以被投掷
    public static bool Cast = false;
    //是否在投掷中
    public static bool Casting = false;

    #endregion

    #region 动作时间数据
    //攻击动作的总时长
    public static float AttackTime = 0.6f;
    //蓄力动作的总时长
    public static float AmassTime = 0.2f;
    //Back动作的总时长
    public static float BackTime = 0.5f;
    //投掷动作的总时长
    public static float CastTime = 0.31f;

    #endregion


    //进行跟随的距离
    public static float followDistance = 3f;
    //普通多长时间跟随到玩家
    public static float smoothTime = 2f;
    //被召唤时，移动到玩家位置的时间
    public static float comeTime = 0.1f;
}
