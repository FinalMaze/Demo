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
    public static float AttackTime = 0.6f;
    //一段攻击动作开始后，设置第几秒可以进行二段攻击
    public static float Attack2StartTime = 0.2f;
    //二段攻击动作的总时长
    public static float Attack2Time = 0.4f;
    //投掷动作的总时长
    public static float CastTime = 0.35f;
    //召回动作的总时长
    public static float BackTime = 0.24f;
    //爆破动作的总时长
    public static float BlowTime = 0.48f;
    //受击动作的总时长
    public static float HurtTime = 0.17f;
    //冲刺攻击动作的总时长
    public static float RunAttackTime = 0.5f;

    #endregion

    #region 玩家的状态信息
    //现在是什么状态
    public static int State;
    //是否在下落中
    public static bool Downing = false;
    //是否在召回动作中
    public static bool Backing = false;
    //是否在受击动作中
    public static bool Hurting = false;

    public static bool Blowing = false;
    //是否可以二段跳
    public static bool Jump2 = false;
    public static bool Jump2ing = false;
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
    public static bool Attacking1 = false;
    //玩家是否在冲刺攻击中
    public static bool RunAttacking = false;
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
    //攻击力
    public static float Damage = 10f;
    //攻击距离
    public static float AttackDistance = 4f;
    //攻击后第几秒调用怪物被 攻击(造成伤害)动画
    public static float EnemyHurtTime = 0.3f;
    //攻击回蓝量
    public static float AddMP = 2f;

    //第一段攻击位移距离
    public static float AttackDistance1 = 0.7f;
    //第一段攻击位移速度
    public static float AttackSpeed1 = 0.5f;
    //第二段攻击位移距离
    public static float AttackDistance2 = 0.7f;
    //第二段攻击位移速度
    public static float AttackSpeed2 = 0.5f;

    //攻击可以造成伤害的距离
    public static float DamageDistance = 2f;

    //玩家被攻击的位移距离
    public static float HurtDistance = 0.1f;
    //玩家被攻击的位移速度
    public static float HurtSpeed = 0.2f;


    //蓄力掉mp的速度
    public static float AmassingMP = 0.1f;
    //投掷的mp消耗量
    public static float CastMP = 5f;
    //小型时召回的mp消耗量
    public static float GoToPlayerMP = 3f;
    //大型时召回的mp消耗量
    public static float BackMP = 3f;

    //移动速度
    public static float runSpeed = 50f;
    //冲刺距离
    public static float BlinkDistance = 4f;
    //冲刺的CD
    public static float BlinkTime = 1f;
    //冲刺时每次移动的距离
    public static float BlinkTempDistance = 0.5f;
   // public static float jumpSpeed = 1f;

    //玩家的路径
    public static string playerPath = "Prefabs/Player/Player";

    //走路速度
    public static float walkspeed = 0.5f;
    //跑步速度
    public static float runspeed = 1f;
    //走多少秒后变为跑
    public static float walkToRun = 0.5f;
    //冲刺的距离 
    public static float RunAttackDistance = 8f;

    #endregion

    #region 玩家与召唤兽互动的数据信息
    //玩家与召唤兽的距离
    public static float distance;
    //玩家可二段跳，所需的脚的位置与宠物距离
    public static float CanJump = 0.5f;
    //可以直接投掷——需要相距手的位置多远
    public static float CanSimpleThrow = 3.5f;
    //可以召唤并投掷的与宠物相距的距离
    public static float CanThrow = 3.5f;
    //投掷的距离
    public static float ThrowDistance = 10f;
    //投掷到目标点所需的时间（0.1f*下面的数值）<时间长度必须小于动画持续时间>
    public static float ThrowSpeed = 2.3f;
    //投掷过程的最大持续时间(尽量和投掷持续时间相等)
    public static float ThrowLongTime = 0.6f;

    //玩家爆破动作几秒后，播放宠物爆破动作
    public static float FriendBlowStartTime = 0.1f;

    //投掷后的Y轴偏移量(根据玩家的Y轴，向下的偏移量)
    public static float ThrowEndY = 0.6f;


    //召回时所需的时间（0.1f*下面的数值）<时间长度必须小于动画持续时间>
    public static float BackSpeed = 1.5f;
    #endregion

    #region 蓄力时间相关的数据
    //蓄力多少时间以下进行投掷
    public static float ToCastStayTime = 1f;

    #endregion

    #region 滑动相关的数据
    //触发冲刺的最小滑动距离
    public static float SwipeDistance = 80f;
    //触发冲刺的最少滑动时间
    public static float SwipeTime = 0.4f;
    #endregion
}
