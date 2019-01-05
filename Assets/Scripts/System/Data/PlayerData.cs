using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    #region 玩家的动画时间调整
    //起跳动作的总时长
    public static float JumpTime = 0.3f;
    //落地动作的总时长
    public static float JumpEndTime = 0.05f;
    //一段攻击动作的总时长
    public static float AttackTime = 0.5f;
    //一段攻击动作开始后，设置第几秒可以进行二段攻击
    public static float Attack2StartTime = 0f;
    //二段攻击动作的总时长
    public static float Attack2Time = 0.5f;
    //投掷动作的总时长
    public static float CastTime = 0.35f;


    #endregion

    #region 玩家的状态信息
    //现在是什么状态
    public static int State;

    //玩家面向的方向,大于0为正
    public static float Dircetion;

    public static bool Ideling = false;
    //投掷
    public static bool Cast = false;
    public static bool Casting = false;
    //是否蓄力
    public static bool Amass = false;
    //是否正在蓄力
    public static bool Amassing = false;
    //玩家是否在Run状态
    public static bool Runing = false;
    //玩家是否在Walk状态
    public static bool Walking = false;
    //玩家是否在跳跃中
    public static bool Jumping = false;
    //玩家是否可以起跳
    public static bool playerStartJump = false;
    //玩家是否攻击
    public static bool Attack = false;
    //玩家是否正在攻击
    public static bool Attacking = false;
    //开始二段攻击
    public static bool Attack2Start = false;
    //玩家是否二段攻击
    public static bool Attack2 = false;
    //玩家是否正在二段攻击
    public static bool Attacking2 = false;
    //玩家是否在地面上
    public static bool playerIsGround = true;
    //玩家是否被攻击
    public static bool playerAttacked = false;
    #endregion

    #region 玩家的数据信息
    public static float hp = 100f;
    public static float hpMax = 100f;
    public static float mp = 100f;
    public static float mpMax = 100f;

    //移动速度
    public static float runSpeed = 50f;
    //冲刺距离
    public static float BlinkDistance = 5f;
    //冲刺时每次移动的距离
    public static float BlinkTempDistance = 0.5f;
   // public static float jumpSpeed = 1f;

    //玩家的路径
    public static string playerPath = "Prefabs/Player/Player";
    #endregion

    #region 玩家与召唤兽互动的数据信息
    //玩家与召唤兽的距离
    public static float distance;
    //可以直接投掷——需要相距手的位置多远
    public static float CanSimpleThrow = 2f;
    //可以召唤并投掷的与宠物相距的距离
    public static float CanThrow = 2f;
    //投掷的距离
    public static float ThrowDistance = 5f;
    //投掷后的Y轴偏移量
    public static float ThrowEndY = 0.4f;
    #endregion

    #region 蓄力时间相关的数据
    //蓄力多少时间以下进行投掷
    public static float ToCastStayTime = 1f;

    #endregion
}
