﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgehogTeam.EasyTouch;

public class PlayerManager : MonoBehaviour
{
    private GameObject player;
    private GameObject friend;
    private static PlayerManager instance;
    public static PlayerManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        player = PlayerCtrl.Instance.gameObject;
        friend = FriendCtrl.Instance.gameObject;
        allBlink = new QuickSwipe[2];
        GameObject tmpEasy = GameObject.FindGameObjectWithTag("EasyTouch");
        allBlink = tmpEasy.GetComponents<QuickSwipe>();
        for (int i = 0; i < allBlink.Length; i++)
        {
            allBlink[i].enabled = false;
        }

    }
    private void Update()
    {
        //玩家与宠物的距离
        PlayerData.distance = Vector2.Distance(player.transform.position, friend.transform.position);

        JumpAgain();
        //if (PlayerData.Jump2)
        //{
        //    PlayerData.Jump2 = false;
        //    player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1000));
        //    FriendData.Jump2Target = new Vector2(friend.transform.position.x, friend.transform.position.y - FriendData.Jump2TargetY);
        //    FriendData.Jumped = true;
        //}
    }

    #region 长按开始时调用的方法
    float tmpDis;
    bool tmpBiging;
    bool canThrow;
    Vector2 tmpPlayer;
    float reduceMP;
    public void GetDistance()
    {
        tmpDis = PlayerData.distance;
        tmpBiging = FriendData.Biging;
        tmpPlayer = player.transform.position;
        reduceMP = PlayerData.mp - PlayerData.GoToPlayerMP;
        if (PlayerData.distance <= PlayerData.CanThrow)
        {
            canThrow = true;
        }
        else
        {
            canThrow = false;
        }
    }

    #endregion

    #region 长按时的攻击变化
    public void StayAttack(Gesture gesture)
    {
        //大型时，长按触发的方法
        if (tmpBiging || FriendData.Biging)
        {
            if (!FriendData.Backing && FriendData.State != (int)Data.FriendAnimationCount.Back && FriendData.Biging)
            {
                if (PlayerData.mp != 0 && PlayerData.mp >= PlayerData.BackMP)
                {
                    FriendCtrl.Instance.Back();
                    PlayerData.mp -= PlayerData.BackMP;
                    GameInterfaceCtrl.Instance.UpdateMP();
                }
            }
            else
            {
            }
        }
        //小型时，长按触发的方法
        else if (!tmpBiging && FriendData.Smalling)
        {
            if (tmpDis < PlayerData.CanThrow)
            {
                //需要长按的时间
                if (gesture.actionTime > FriendData.ComeStayTime)
                {
                    if (PlayerData.playerIsGround && !PlayerData.Jumping)
                    {
                        Amass();
                    }
                }
            }
            else
            {
                if (PlayerData.mp!=0)
                {
                    PlayerData.mp = reduceMP;
                    GameInterfaceCtrl.Instance.UpdateMP();
                    Debug.Log(PlayerData.mp);
                    FriendCtrl.Instance.GoToPlayer(tmpPlayer);
                }
            }
        }
    }
    #endregion

    #region 松开时的攻击方法
    public void AttackAI(Gesture gesture)
    {
        if (tmpBiging)
        {

        }
        else
        {
            if (canThrow)
            {
                Throw();
            }
            else
            {
                PlayerCtrl.Instance.Attack();
            }
        }

        tmpBiging = FriendData.Biging;
    }
    #endregion

    #region 普通攻击
    public void SimpleAttack()
    {
        if (!PlayerData.Jumping)
        {

            if (PlayerData.distance < PlayerData.CanSimpleThrow && !FriendData.Biging)
            {
                Throw();
            }
            else
            {
                if (PlayerData.Attacking && !PlayerData.Attacking2)
                {
                    PlayerCtrl.Instance.Attack2();
                }
                else if (!PlayerData.Attacking)
                {
                    PlayerCtrl.Instance.Attack();
                }
            }
        }
    }
    #endregion

    #region 蓄力
    private void Amass()
    {
        if (PlayerData.mp<=0)
        {
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Idel);

            FriendData.Amassing = false;
            FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Idel);
            return;
        }
        if (!FriendData.Biging && !tmpBiging && !FriendData.Backing)
        {
            PlayerData.mp -= PlayerData.AmassingMP;
            GameInterfaceCtrl.Instance.UpdateMP();

            PlayerCtrl.Instance.Amass();
            FriendCtrl.Instance.Amass();
        }
    }
    #endregion

    #region 投掷动作
    public void Throw()
    {
        if (PlayerData.mp <= 0) 
        {
            #region mp=0时，调用的攻击方法
            if (PlayerData.Attacking && !PlayerData.Attacking2)
            {
                PlayerCtrl.Instance.Attack2();
            }
            else if (!PlayerData.Attacking)
            {
                PlayerCtrl.Instance.Attack();
            }
            #endregion
            return;
        }
        PlayerData.mp -= PlayerData.CastMP;
        GameInterfaceCtrl.Instance.UpdateMP();

        //让玩家进入投掷状态
        PlayerCtrl.Instance.Throw();
        //将投掷的目标位置传给宠物
        if (player.transform.localScale.x > 0)
        {
            FriendCtrl.Instance.ThrowFriend(new Vector2(player.transform.position.x + PlayerData.ThrowDistance, player.transform.position.y + PlayerData.ThrowEndY));
        }
        if (player.transform.localScale.x < 0)
        {
            FriendCtrl.Instance.ThrowFriend(new Vector2(player.transform.position.x - PlayerData.ThrowDistance, player.transform.position.y + PlayerData.ThrowEndY));
        }
    }
    #endregion

    #region 冲刺相关
    QuickSwipe[] allBlink;
    public void BlinkStart()
    {
        for (int i = 0; i < allBlink.Length; i++)
        {
            allBlink[i].enabled = true;
        }
    }
    public void BlinkEnd()
    {
        for (int i = 0; i < allBlink.Length; i++)
        {
            allBlink[i].enabled = false;
        }
    }
    #endregion

    private void JumpAgain()
    {
        if (PlayerData.Jumping&&PlayerData.Downing&&FriendData.JumpDistance<1f
            &&Mathf.Abs(player.transform.position.x-friend.transform.position.x)<2f)
        {
            if (!PlayerData.Jump2ing)
            {
                PlayerData.Jump2ing = true;
                player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1000));
                FriendData.Jump2Target = new Vector2(friend.transform.position.x, friend.transform.position.y - FriendData.Jump2TargetY);
                FriendData.Jumped = true;
            }

        }
    }
}
