using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgehogTeam.EasyTouch;
using System;

public class PlayerManager : MonoBehaviour
{
    #region 单例
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
        player = PlayerCtrl.Instance.gameObject;
        friend = GameObject.FindGameObjectWithTag("FriendParent");

    }
    #endregion

    #region 设置宠物AI是否开启
    public void FriendAI(Boolean tmpBool)
    {
        Data.FriendAI = tmpBool;
        if (Data.FriendAI)
        {
            Destroy(friend.GetComponent<FriendPlayerCtrl>());
            friend.AddComponent<FriendCtrl>();
        }
        else
        {
            Destroy(friend.GetComponent<FriendCtrl>());
            friend.AddComponent<FriendPlayerCtrl>();
        }
    }
    #endregion

    #region 初始化
    private void Start()
    {
        playerFoot = GameObject.FindGameObjectWithTag("PlayerDown");

        allBlink = new QuickSwipe[2];
        GameObject tmpEasy = GameObject.FindGameObjectWithTag("EasyTouch");
        allBlink = tmpEasy.GetComponents<QuickSwipe>();
        for (int i = 0; i < allBlink.Length; i++)
        {
            allBlink[i].enabled = false;
        }

    }
    #endregion

    GameObject playerFoot;
    private void Update()
    {

        //玩家与宠物的距离
        PlayerData.distance = Vector2.Distance(player.transform.position, friend.transform.position);
        FriendData.JumpDistance = PlayerCtrl.Instance.transform.position.y - 0.538f - friend.transform.position.y;

        //检测什么时候二段跳
        JumpAgain();

        if (FriendData.Smalling && !FriendData.Biging && !FriendData.Casting && !PlayerData.Casting)
        {
            CancelInvoke("RandomPos");
        }

    }


    #region 宠物按键相关

    public void SimpleFriend()
    {
        if (up)
        {
            if (FriendData.Biging)
            {
                if (!PlayerData.Attacking && !FriendData.Backing && !FriendData.Casting && !PlayerData.Casting && !PlayerData.Backing && !PlayerData.Blowing)
                {
                    if (PlayerData.mp != 0 && PlayerData.mp >= PlayerData.BackMP)
                    {
                        if (!PlayerData.Jumping)
                        {
                            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Back);
                        }
                        Back();

                        PlayerData.mp -= PlayerData.BackMP;
                        GameInterfaceCtrl.Instance.UpdateMP();
                    }
                }
            }
            if (FriendData.Smalling)
            {
                if (!FriendData.Backing && !FriendData.Casting && !PlayerData.Casting && !PlayerData.Backing && !PlayerData.Attacking && !PlayerData.Blowing)
                {
                    if (PlayerData.distance < PlayerData.CanSimpleThrow)
                    {

                    }
                    else
                    {
                        if (PlayerData.mp != 0 && PlayerData.mp >= PlayerData.BackMP)
                        {
                            PlayerData.mp -= PlayerData.BackMP;
                            GameInterfaceCtrl.Instance.UpdateMP();
                            if (!PlayerData.Jumping)
                            {
                                PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Back);
                            }
                            Move();
                        }
                    }
                }
            }
        }
        //下加宠物键的技能
        else if (down)
        {
            if (!FriendData.Backing && !FriendData.Casting && !PlayerData.Casting && !PlayerData.Backing && !PlayerData.Attacking && !PlayerData.Blowing)
            {
                if (FriendData.Biging)
                {
                    if (!PlayerData.Attacking && !FriendData.Backing && !FriendData.Casting && !PlayerData.Casting && !PlayerData.Backing && !PlayerData.Blowing)
                    {
                        if (PlayerData.mp != 0 && PlayerData.mp >= PlayerData.BackMP)
                        {
                            if (!PlayerData.Jumping)
                            {
                                PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Back);
                            }
                            Back();

                            PlayerData.mp -= PlayerData.BackMP;
                            GameInterfaceCtrl.Instance.UpdateMP();
                        }
                    }
                }

                if (FriendData.Smalling)
                {
                    if (!FriendData.Backing && !FriendData.Casting && !PlayerData.Casting && !PlayerData.Backing && !PlayerData.Attacking && !PlayerData.Blowing)
                    {
                        if (PlayerData.distance < PlayerData.CanSimpleThrow)
                        {
                            Blow();
                        }
                        else
                        {
                            if (PlayerData.mp != 0 && PlayerData.mp >= PlayerData.BackMP)
                            {
                                PlayerData.mp -= PlayerData.BackMP;
                                GameInterfaceCtrl.Instance.UpdateMP();
                                if (!PlayerData.Jumping)
                                {
                                    PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Back);
                                }
                                Move();
                            }
                        }
                    }
                }
            }
        }
        //不按上下时的宠物键
        else if (!up && !down)
        {
            if (FriendData.Biging)
            {
                if (!PlayerData.Attacking && !FriendData.Backing && !FriendData.Casting && !PlayerData.Casting && !PlayerData.Backing && !PlayerData.Blowing)
                {
                    if (PlayerData.mp != 0 && PlayerData.mp >= PlayerData.BackMP)
                    {
                        if (!PlayerData.Jumping)
                        {
                            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Back);
                        }
                        Back();

                        PlayerData.mp -= PlayerData.BackMP;
                        GameInterfaceCtrl.Instance.UpdateMP();
                    }
                }
            }
            if (FriendData.Smalling)
            {
                if (!FriendData.Backing && !FriendData.Casting && !PlayerData.Casting && !PlayerData.Backing && !PlayerData.Attacking && !PlayerData.Blowing)
                {
                    if (PlayerData.distance < PlayerData.CanSimpleThrow)
                    {
                        Throw();
                    }
                    else
                    {
                        if (PlayerData.mp != 0 && PlayerData.mp >= PlayerData.BackMP)
                        {
                            PlayerData.mp -= PlayerData.BackMP;
                            GameInterfaceCtrl.Instance.UpdateMP();
                            if (!PlayerData.Jumping)
                            {
                                PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Back);
                            }
                            Move();
                        }
                    }
                }
            }

        }
    }
    #endregion

    #region 普通攻击按键相关
    public void SimpleAttack()
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
    #endregion


    #region 投掷动作
    public void Throw()
    {
        if (!PlayerData.Jumping && !PlayerData.Backing && !PlayerData.Blowing && !PlayerData.Attacking && !FriendData.Backing && FriendData.Smalling)
        {
            if (PlayerData.distance < PlayerData.CanSimpleThrow && !FriendData.Biging && !FriendData.Blowing && !PlayerData.Blowing)
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
                    if (Data.FriendAI)
                    {
                        FriendCtrl.Instance.ThrowFriend(new Vector2(player.transform.position.x + PlayerData.ThrowDistance, player.transform.position.y + PlayerData.ThrowEndY));
                    }
                    else
                    {
                        FriendPlayerCtrl.Instance.ThrowFriend(new Vector2(player.transform.position.x + PlayerData.ThrowDistance, player.transform.position.y + PlayerData.ThrowEndY));
                    }
                }
                if (player.transform.localScale.x < 0)
                {
                    if (Data.FriendAI)
                    {
                        FriendCtrl.Instance.ThrowFriend(new Vector2(player.transform.position.x - PlayerData.ThrowDistance, player.transform.position.y + PlayerData.ThrowEndY));
                    }
                    else
                    {
                        FriendPlayerCtrl.Instance.ThrowFriend(new Vector2(player.transform.position.x - PlayerData.ThrowDistance, player.transform.position.y + PlayerData.ThrowEndY));
                    }
                }
                if (Data.FriendAI)
                {
                    InvokeRepeating("RandomPos", 1, 1.5f);
                }
            }
        }
    }
    #endregion

    public void Move()
    {
        if (Data.FriendAI)
        {
            FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Move);
        }
        else
        {
            FriendPlayerCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Move);
        }
    }

    public void Back()
    {
        if (Data.FriendAI)
        {
            FriendCtrl.Instance.Back();
        }
        else
        {
            FriendPlayerCtrl.Instance.Back();
        }
    }

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

    #region 二段跳的方法
    private void JumpAgain()
    {
        //Debug.Log(PlayerData.Jumping);
        //Debug.Log(PlayerData.Downing);
        //Debug.Log(FriendData.JumpDistance<0.2f && FriendData.JumpDistance > 0);
        //Debug.Log(Mathf.Abs(player.transform.position.x - friend.transform.position.x) < 2f);
        //Debug.Log(FriendData.Smalling);
        if (PlayerData.Jumping && PlayerData.Downing && FriendData.JumpDistance < 1.5f && FriendData.JumpDistance > 0f
            && Mathf.Abs(player.transform.position.x - friend.transform.position.x) < 2f && FriendData.Smalling)
        {
            if (!PlayerData.Jump2ing)
            {

                PlayerData.Jump2ing = true;
                player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1300));
                FriendData.Jump2Target = new Vector2(friend.transform.position.x, friend.transform.position.y - FriendData.Jump2TargetY);
                FriendData.Jumped = true;
            }

        }
    }
    #endregion

    #region 给宠物巡逻随机一个目标点
    private void RandomPos()
    {
        FriendCtrl.Instance.RandomPos();
    }
    #endregion

    private void Blow()
    {
        if (!PlayerData.Jumping && !PlayerData.Backing && !PlayerData.Blowing && !PlayerData.Attacking && !FriendData.Backing && FriendData.Smalling)
        {
            friend.transform.position = PlayerCtrl.Instance.transform.position;
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Blow);
            Invoke("FriendBlow", PlayerData.FriendBlowStartTime);
        }
    }
    private void FriendBlow()
    {
        if (Data.FriendAI)
        {
            FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Blow);
        }
        else
        {
            FriendPlayerCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Blow);

        }
    }


    #region 方向键下触发中
    bool down = false;
    public void Down(Vector2 vector2)
    {
        if (vector2.y > 0.4f)
        {
            down = true;
        }
        else
        {
            down = false;
        }
    }
    #endregion

    #region 方向键上触发中
    bool up = false;
    public void UP(Vector2 vector2)
    {
        if (vector2.y < -0.4f)
        {
            up = true;
        }
        else
        {
            up = false;
        }

    }
    #endregion

    #region 按键归位
    public void PressUP()
    {
        down = false;
        up = false;
    }
    #endregion


}
