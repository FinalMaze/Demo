using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendData
{
    #region 动作状态信息
    //现在是什么状态
    public static int State;

    //是否二段跳
    public static bool Jumped = false;
    public static bool Jumpeding = false;
    //是否在小型状态
    public static bool Smalling = false;
    //是否在大型的状态
    public static bool Biging = false;

    public static bool Blowing = false;
    //是否在移动中
    public static bool Moving = false;
    //是否在攻击中
    public static bool Attacking = false;
    //是否可以蓄力
    public static bool Amass = false;
    //是否在蓄力中
    public static bool Amassing = false;
    public static bool CanBack = false;
    //是否Back
    public static bool Back = false;
    //是否在Back中
    public static bool Backing = false;
    //是否可以被投掷
    public static bool Cast = false;
    //是否在投掷中
    public static bool Casting = false;
    //是否在Run2状态
    public static bool Runing = false;

    #endregion

    #region 动作时间数据
    //攻击动作的总时长
    public static float AttackTime = 0.4f;
    //蓄力动作的总时长
    public static float AmassTime = 0.1f;
    //Back动作的总时长
    public static float BackTime = 0.3f;
    //投掷动作的总时长
    public static float CastTime = 0.6f;
    //移动动作的总时长
    public static float MoveTime = 0.4f;
    public static float BlowTime = 0.48f;
    #endregion

    #region 组件增减条件
    public static bool AddRigibody = false;
    public static bool DelRigibody = false;
    #endregion

    #region 基础数据
    //攻击力
    public static float Damage = 15;
    //攻击距离
    public static float AttackDistance = 4f;
    //攻击的位移距离 
    public static float AttackBlinkDistance = 0.3f;
    //攻击位移的速度（也会影响距离）
    public static float AttackBlinkSpeed = 0.1f;
    //移动速度
    public static float MoveSpeed = 8f;
    

    //爆破动画几秒后，可以造成伤害
    public static float BlowDamageTime = 0.05f;
    //爆破的范围半径
    public static float BlowRadius = 4f;
    //爆破的击退距离
    public static float BlowDistance = 5f;
    //爆破的伤害
    public static float BlowDamage = 10f;

    //寻找到怪物的距离
    public static float FllowDistance = 10f;
    //攻击后第几秒调用怪物被 攻击(造成伤害)动画
    public static float EnemyHurtTime = 0.3f;

    //冲刺的起点
    public static Vector2 StartPos;
    //冲刺的目标点
    public static Vector2 Target;
    //冲刺的击退距离
    public static float CastDistance = 4f;

    //巡逻的时间间隔
    public static float PartolTime = 1.5f;
    //进行跟随的距离
    public static float followDistance = 1.2f;
    //普通多长时间跟随到玩家
    public static float smoothTime = 0.3f;
    //被召唤或者Back时，移动到玩家位置的时间
    public static float comeTime = 0.1f;
    //变小时,离开可蓄力距离，召回需要按住攻击键的时长
    public static float ComeStayTime = 0.5f;


    //持续回蓝的判定是多少秒（单位是秒不是f）
    public static float UpdataTime = 0.1f;
    //变小时持续回蓝量
    public static float SmallMP = 1f;

    //变大的持续时间
    public static float BigTime = 4f;
    //变大时的持续耗蓝量
    public static float BigMP = 1f;
    //变大时召回需要按住攻击键的时长
    public static float BackStayTime = 0.3f;
    //与玩家的脚相距的距离
    public static float JumpDistance;

    //被二段跳时下降到的目标点
    public static Vector2 Jump2Target;
    //被跳时的Y轴偏移量
    public static float Jump2TargetY=0.5f;


    //变大时离玩家多远变小
    public static float BigToSmallDistance = 20f;
    #endregion
}
