using System.Collections;
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

    }

    #region 长按开始时调用的方法
    float tmpDis;
    public void GetDistance()
    {
        tmpDis = PlayerData.distance;
    }
    
    #endregion

    #region 长按时的攻击变化
    public void StayAttack(Gesture gesture)
    {
        //if (!FriendData.Biging && !FriendData.Backing)
        //{
        //    if (tmpDis < PlayerData.CanThrow)
        //    {
        //        FriendCtrl.Instance.GoToPlayer();
        //        if (gesture.actionTime > 0.5f)
        //        {
        //            PlayerCtrl.Instance.Amass();
        //        }
        //    }
        //    else if (tmpDis > PlayerData.CanThrow && gesture.actionTime > 1f)
        //    {
        //        FriendCtrl.Instance.GoToPlayer();
        //    }
        //}
        //else
        //{
        //    if (gesture.actionTime > 0.3f)
        //    {
        //        FriendData.Back = true;
        //        return;
        //    }
        //}
    }
    #endregion

    #region 松开时的攻击方法
    public void AttackAI(Gesture gesture)
    {

        Debug.Log(gesture.actionTime);
        //if (!FriendData.Biging && !FriendData.Backing)
        //{
        //    float timeCount = gesture.actionTime;
        //    if (timeCount > 0.5f)
        //    {
        //        if (PlayerData.distance < PlayerData.CanThrow)
        //        {
        //            if (PlayerData.Amassing)
        //            {
        //                PlayerCtrl.Instance.Throw();
        //            }
        //            else
        //            {
        //                Debug.LogWarning("没在进行Amass");
        //            }
        //        }
        //        else
        //        {
        //            PlayerCtrl.Instance.Attack();
        //        }
        //    }
        //    else
        //    {
        //        Debug.Log("0.5秒以下");
        //    }
        //}
        //else
        //{
        //    Debug.Log("在Biging状态");
        //    Debug.Log(FriendData.Biging);
        //    Debug.Log(FriendData.Backing);
        //}
    }
    #endregion

    #region 普通攻击
    public void SimpleAttack()
    {
        if (PlayerData.distance < PlayerData.CanSimpleThrow)
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
