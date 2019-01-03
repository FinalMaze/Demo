﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendCtrl : MonoBehaviour
{
    public static FriendCtrl Instance;

    Animator animator;
    FSMManager fsmManager;


    Rigidbody2D tmpRgb;
    BoxCollider2D friendC;

    //巡逻目标点
    Vector2 tmpVec;
    //玩家的位置
    Vector2 player;
    //与玩家相距的坐标点
    public Vector2 distanceV = Vector2.zero;
    //将要被扔到的位置
    private Vector2 target;
    //与玩家x相距的距离
    float distance;
    //跟随的速度
    private Vector2 velocity = Vector2.one;
    //计时器
    float timeCount;
    //上次记录的时间
    float lastTime;
    //是否冲刺
    bool blink = false;
    //是否Back
    bool back = false;
    //是否amass
    bool amass = true;
    bool canMove=false;
    bool canPartol=false;
    private void Start()
    {
        Instance = this;
        fsmManager = new FSMManager((int)Data.FriendAnimationCount.Max);
        animator = GetComponent<Animator>();
        //tmpRgb = GetComponent<Rigidbody2D>();
        //tmpRgb.gravityScale = 0;
        friendC = GetComponent<BoxCollider2D>();
        //friendC.isTrigger = true;



        #region 注册动画
        FriendIdel friendIdel = new FriendIdel(animator);
        fsmManager.AddState(friendIdel);
        FriendMove friendMove = new FriendMove(animator);
        fsmManager.AddState(friendMove);
        FriendAttack friendAttack = new FriendAttack(animator);
        fsmManager.AddState(friendAttack);
        FriendAmass friendAmass = new FriendAmass(animator);
        fsmManager.AddState(friendAmass);
        FriendAmassing friendAmassing = new FriendAmassing(animator);
        fsmManager.AddState(friendAmassing);
        FriendBack friendBack = new FriendBack(animator);
        fsmManager.AddState(friendBack);
        FriendCast friendCast = new FriendCast(animator);
        fsmManager.AddState(friendCast);
        #endregion

    }
    float ran;
    private void Update()
    {
        fsmManager.OnStay();
        player = AIManager.Instance.Player.transform.position;
        PlayerData.distance = Vector2.Distance(player, transform.position);
        distance = PlayerCtrl.Instance.transform.position.x - transform.position.x;
        timeCount += Time.deltaTime;

        #region 进行动作的前置条件
        //todo
        #endregion

        #region 巡逻
        Partol();
        #endregion

        #region 回到Idel的方法
        //Debug.Log(!FriendData.Attacking);
        //Debug.Log(!FriendData.Amassing);
        //Debug.Log(!FriendData.Backing);
        //Debug.Log(!FriendData.Casting);
        if (!FriendData.Attacking && !FriendData.Amassing && !FriendData.Backing && !FriendData.Casting)
        {
            ChangeState((sbyte)Data.FriendAnimationCount.Idel);
        }
        #endregion

        #region 判断什么时候被投掷
        Throwed();
        #endregion

        #region 判断什么时候召回
        Back();
        #endregion
    }

    #region 蓄力
    public void Amass()
    {
        if (FriendData.Biging)
        {
            FriendData.Back = true;
        }
        GoToPlayer();
        if (!FriendData.Amassing)
        {
            FriendData.Amass = true;
            ChangeState((sbyte)Data.FriendAnimationCount.Amass);
        }
        else
        {
            ChangeState((sbyte)Data.FriendAnimationCount.Amassing);
        }
    }
    #endregion

    #region Back
    private void Back()
    {
        if (FriendData.Back)
        {
            //friendC.isTrigger = true;
            friendC.size = new Vector2(0.47f, 0.2f);
            friendC.offset = new Vector2(0, 0);
            Destroy(GetComponent<Rigidbody2D>());
            tmpRgb = null;

            back = true;
        }

        if (back)
        {
            StartCoroutine("IEBack");
        }

    }

    IEnumerator IEBack()
    {
        GoToPlayer(2);
        FriendData.Biging = false;
        if (distance>0)
        {
            transform.rotation =Quaternion.Euler(0, 180, 0);
        }
        if (distance < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        ChangeState((sbyte)Data.FriendAnimationCount.Back);
        yield return new WaitForSeconds(0.5f);
        back = false;
    }
    #endregion

    #region 合体
    public void Fit()
    {

    }
    #endregion

    #region 被扔出去
    private void Throwed()
    {
        if (blink)
        {
            StartCoroutine("Blink");
        }
        if (FriendData.Cast)
        {
            ChangeState((sbyte)Data.FriendAnimationCount.Cast);
        }
        //添加和销毁刚体
        if (FriendData.Casting)
        {
            StartCoroutine("AddRigibody");
        }
    }



    public void ThrowFriend(Vector2 target)
    {
        Debug.Log("进入被扔的方法");
        FriendData.Cast = true;
        this.target = target;
        if (animator.GetInteger("Index")!=4)
        {
            ChangeState((sbyte)Data.FriendAnimationCount.Amassing);
            #region 移动到玩家手的位置
            if (PlayerData.Dircetion > 0)
            {
                if (back)
                {
                    distanceV = Vector2.zero;
                }
                else
                {
                    distanceV = new Vector2(-0.25f, -0.27f);
                }
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            if (PlayerData.Dircetion < 0)
            {
                if (back)
                {
                    distanceV = Vector2.zero;
                }
                else
                {
                    distanceV = new Vector2(0.25f, -0.27f);
                }
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            transform.position = player + distanceV;

            #endregion
        }
        else
        {
            ChangeState((sbyte)Data.FriendAnimationCount.Cast);
        }
        blink = true;
        StartCoroutine("BigTime");
    }



    IEnumerator Blink()
    {
        if (target.x<transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (target.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        transform.position = Vector2.MoveTowards(transform.position, target, 0.2f);
        yield return new WaitForSeconds(1f);
        blink = false;
    }

    IEnumerator AddRigibody()
    {
        //0.5秒后更改碰撞器大小，添加刚体组件并设置
        yield return new WaitForSeconds(0.5f);
        friendC.size = new Vector2(1.8f, 0.8f);
        friendC.offset = new Vector2(0, -0.3f);
        if (tmpRgb == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
            tmpRgb = GetComponent<Rigidbody2D>();
        }
        tmpRgb.freezeRotation = true;
        tmpRgb.mass = 100;
        tmpRgb.gravityScale = 100;
    }
    #endregion

    #region 被召唤到玩家位置
    public void GoToPlayer(float timeRatio=1)
    {
        distance = player.x - transform.position.x;
        //if (distance>0)
        //{
        //    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        //}
        //if (distance<0)
        //{
        //    transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        //}
        if (PlayerData.Dircetion > 0)
        {
            if (back)
            {
                distanceV = Vector2.zero;
            }
            else
            {
                distanceV = new Vector2(-0.25f, -0.27f);
            }
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (PlayerData.Dircetion < 0)
        {
            if (back)
            {
                distanceV = Vector2.zero;
            }
            else
            {
                distanceV = new Vector2(0.25f, -0.27f);
            }
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        FriendData.Moving = true;
        transform.position = Vector2.SmoothDamp(transform.position, player + distanceV, ref velocity, FriendData.comeTime* timeRatio);
    }
    #endregion

    #region 巡逻
    private void Partol()
    {
        //Debug.Log(FriendData.Biging);
        if (FriendData.Biging)
        {
            if (Time.time - lastTime > FriendData.PartolTime && !FriendData.Attacking)
            {
                lastTime = Time.time;
                ran = Random.Range(-5, 5);
                tmpVec = new Vector2(transform.position.x + ran, transform.position.y);
                //Debug.Log(ran);
                //StartCoroutine("Patrol");
            }

            StartCoroutine("IEPatrol");
        }
        else
        {
            //Debug.Log(!FriendData.Amassing);
            //Debug.Log(!back);
            if (!FriendData.Casting && !FriendData.Amassing && !FriendData.Backing&&!back)
            {
                #region 跟随
                if (PlayerData.distance > FriendData.followDistance)
                {
                    if (distance > 0)
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    }
                    if (distance < 0)
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    }
                    Debug.Log("跟随中");
                    transform.position = Vector2.SmoothDamp(transform.position, player + distanceV, ref velocity, FriendData.smoothTime);
                }
                else if (PlayerData.distance > FriendData.followDistance * 2)
                {
                    if (PlayerData.Dircetion > 0)
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    }
                    if (PlayerData.Dircetion < 0)
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    }
                    transform.position = Vector2.SmoothDamp(transform.position, player + distanceV, ref velocity, FriendData.smoothTime*0.5f);
                }
                #endregion
            }
        }

    }

    IEnumerator IEPatrol()
    {
        if (ran > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (ran < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        
        transform.position = Vector2.MoveTowards(transform.position, tmpVec, 0.08f);
        yield return null;
    }
    #endregion

    #region 变大的总时长
    IEnumerator BigTime()
    {
        yield return new WaitForSeconds(4f);
        FriendData.Back = true;
    }
    #endregion

    public void ChangeState(sbyte animationCount)
    {
        fsmManager.ChangeState(animationCount);
    }

}
