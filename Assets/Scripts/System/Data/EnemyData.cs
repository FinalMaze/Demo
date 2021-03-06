﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{
    #region 怪物的动作状态信息
    public bool Summoning = false;
    //是否在攻击中
    public bool Attacking = false;
    //是否在远程攻击中
    public bool Attacking2 = false;
    //是否在被攻击动作中
    public bool Hurting = false;
    //是否在死亡动作中
    public bool Die = false;

    //子弹是否在开始动画中
    public bool BallStarting = false;
    //子弹是否在结束动画中
    public bool BallEnding = false;
    #endregion

    #region 怪物动作数据信息
    //攻击动作总时长
    public static float AttackTime = 1f;
    //受击动作总时长
    public static float HurtTime = 0.28f;
    //死亡动作总时长
    public static float DieTime = 0.4f;
    //远程攻击动作总时长
    public static float AttackTime2 = 0.65f;
    //出生动作总时长
    public static float SummonTime = 0.9f;

    //子弹开始动画的时长
    //public static float BallStart = 0.1f;
    public static float BallStart = 0.1f;
    //子弹结束动画的时长
    public static float BallEnd = 0.3f;
    #endregion

    #region 怪物的基础数据信息
    public float HP = 100f;
    public float MaxHP = 100f;

    public float Damage = 5f;
    

    //攻击后第几秒调用玩家受击（造成伤害）动作
    public float PlayerHurtTime = 0.8f;
    //怪物被攻击的位移距离
    public float HurtDistance = 0.07f;
    //怪物被攻击的位移速度
    public float HurtSpeed = 0.25f;
    //怪物移动速度
    public float MoveSpeed = 0.05f;
    //攻击频率
    public float AttackCD = 2.5f;

    //检测到玩家的距离（追）
    public float FllowDistance = 10f;
    //攻击距离
    public float AttackDistance = 3f;

    //远程攻击动作开始后，多少秒创建子弹
    public static float BallStartTime = 0.3f;
    //子弹的飞行速度
    public static  float BallSpeed = 7f;
    //子弹的伤害
    public static float BallDamage = 5f;

    #endregion

    #region 出生死亡数据 
    //小怪复活间隔
    public static float smallReTime = 2f;
    //大怪复活间隔
    public static float bigReTime = 2f;
    //出生自标准点的随机距离（范围）
    public static int startDis = 3;

    #endregion

    #region 特效是否在播放
    public bool Attacking1E = false;
    public bool HurtingE = false;
    public bool SummoningE = false;
    public bool DieE = false;
    #endregion
    #region 特效信息
    //攻击后第几秒播放特效
    public static float AttackEStartTime=0.75f;
    //攻击特效的时长
    public static float AttackTimeE = 0.25f;
    //受击后第几少播放特效
    public static float HurtEStartTime = 0f;
    //受击特效时长
    public static float HurtTimeE = 0.32f;

    //出生后第几秒播放特效
    public static float SummonEStartTime = 0f;
    //出生特效时长
    public static float SummonTimeE = 0.8f;

    //死亡第几秒播放特效
    public static float DieEStartTime = 0f;
    //死亡特效时长
    public static float DieTimeE = 0.25f * 1.6f;
    #endregion
}
