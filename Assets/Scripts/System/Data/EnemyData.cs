﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{
    #region 怪物的动作状态信息
    //是否在攻击中
    public bool Attacking = false;
    //是否在远程攻击中
    public bool Attacking2 = false;
    //是否在被攻击动作中
    public bool Hurting = false;
    //是否在死亡动作中
    public bool Die = false;
    #endregion

    #region 怪物动作数据信息
    //攻击动作总时长
    public static float AttackTime = 1f;
    //受击动作总时长
    public static float HurtTime = 0.20f;
    //死亡动作总时长
    public static float DieTime = 0.4f;
    //远程攻击动作总时长
    public static float AttackTime2 = 0.65f;
    #endregion

    #region 怪物的基础数据信息
    public float HP = 100f;
    public float MaxHP = 100f;

    public float Damage = 10f;
    

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

    #endregion
}
