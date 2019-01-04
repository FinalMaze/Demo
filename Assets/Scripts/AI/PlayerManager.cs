﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgehogTeam.EasyTouch;

public class PlayerManager : MonoBehaviour
{
    private GameObject player;
    private GameObject friend;
    private void Start()
    {
        player = PlayerCtrl.Instance.gameObject;
        friend = FriendCtrl.Instance.gameObject;
    }
    private void Update()
    {
        //玩家与宠物的距离
        PlayerData.distance = Vector2.Distance(player.transform.position, friend.transform.position);
        if (FriendData.CanBack)
        {
            FriendCtrl.Instance.Back();
        }
    }

    #region 长按开始时调用的方法
    float tmpDis;
    bool tmpBiging;
    public void GetDistance()
    {
        tmpDis = PlayerData.distance;
        tmpBiging = FriendData.Biging;
    }

    #endregion

    #region 长按时的攻击变化
    public void StayAttack(Gesture gesture)
    {
        //大型时，长按触发的方法
        if (tmpBiging)
        {
            if (gesture.actionTime>FriendData.BackStayTime)
            {
                FriendData.CanBack = true;
            }
        }
        //小型时，长按触发的方法
        else
        {
            if (!FriendData.Backing)
            {
                if (tmpDis < PlayerData.CanThrow)
                {
                    FriendCtrl.Instance.GoToPlayer();
                    //需要长按的时间
                    if (gesture.actionTime > FriendData.ComeStayTime)
                    {
                        Amass();
                    }
                }
                else
                {
                    FriendCtrl.Instance.GoToPlayer();
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
            if (gesture.actionTime < PlayerData.ToCastStayTime)
            {
                Throw();
            }
            else
            {
                Debug.Log("虽然应该是爆破，但是先用投掷代替");
                Throw();
            }
        }
    }
    #endregion

    #region 普通攻击
    public void SimpleAttack()
    {
        if (PlayerData.distance < PlayerData.CanSimpleThrow&&!FriendData.Biging)
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
    #endregion

    #region 蓄力
    private void Amass()
    {
        FriendCtrl.Instance.GoToPlayer(0.1f);
        PlayerCtrl.Instance.Amass();
        FriendCtrl.Instance.Amass();
    }
    #endregion

    #region 投掷动作
    public void Throw()
    {
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

}
