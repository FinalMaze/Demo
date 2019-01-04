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
    //是否可以蓄力
    public static bool Amass = false;
    //是否在蓄力中
    public static bool Amassing = false;
    //是否Back
    public static bool Back = false;
    //是否在Back中
    public static bool Backing = false;
    //是否可以被投掷
    public static bool Cast = false;
    //是否在投掷中
    public static bool Casting = false;
    //是否在变大的状态
    public static bool Biging = false;
    //是否在Run2状态
    public static bool Runing = false;

    #endregion

    #region 动作时间数据
    //攻击动作的总时长
    public static float AttackTime = 0.6f;
    //蓄力动作的总时长
    public static float AmassTime = 0.1f;
    //Back动作的总时长
    public static float BackTime = 0.3f;
    //投掷动作的总时长
    public static float CastTime = 0.5f;

    #endregion

    #region 组件增减条件
    public static bool AddRigibody = false;
    public static bool DelRigibody = false;
    #endregion

    #region 基础数据
    //巡逻的时间间隔
    public static float PartolTime = 1.5f;
    //进行跟随的距离
    public static float followDistance = 3f;
    //普通多长时间跟随到玩家
    public static float smoothTime = 2f;
    //被召唤或者Back时，移动到玩家位置的时间
    public static float comeTime = 0.1f;
    //变小时,离开可蓄力距离，召回需要按住攻击键的时长
    public static float ComeStayTime = 0.5f;
    //变大的持续时间
    public static float BigTime = 4f;
    //变大时召回需要按住攻击键的时长
    public static float BackStayTime = 0.3f;
    #endregion
}
