using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    #region 玩家的状态信息
    public static bool Cast = false;
    //是否蓄力
    public static bool Amass = false;
    //是否正在蓄力
    public static bool Amassing = false;
    //玩家是否在Run状态
    public static bool playerRun = false;
    //玩家是否在Walk状态
    public static bool playerWalk = false;
    //玩家是否在跳跃中
    public static bool playerJumping = false;
    //玩家是否可以起跳
    public static bool playerStartJump = false;
    //玩家是否攻击
    public static bool Attack = false;
    //玩家是否正在攻击
    public static bool Attacking = false;
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


    public static float runSpeed = 50f;
    public static float jumpSpeed = 1f;

    //玩家的路径
    public static string playerPath = "Prefabs/Player/Player";
    #endregion

    #region 玩家与召唤兽互动的数据信息
    //玩家与召唤兽的距离
    public static float distance;
    //投掷召唤兽的距离
    public static float throwDistance = 5f;
    #endregion
}
