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

    #region 临时变量
    //巡逻目标点
    Vector2 tmpVec;
    //玩家的位置
    Vector2 player;
    //玩家手的位置
    public Vector2 distanceV = Vector2.zero;
    //将要被扔到的位置
    private Vector2 target;
    //与玩家x相距的距离
    float distance;
    //跟随的速度
    private Vector2 velocity = Vector2.one;
    //计时器
    float timeCount;
    //跳跃计时器
    float jumpTimeCount;
    //上次记录的时间
    float lastTime;
    //是否冲刺
    bool blink = false;
    //是否Back
    bool back = false;
    //是否可以巡逻
    bool canPartol = false;
    //巡逻用的随机数
    float ran;
    GameObject playerFoot;
    #endregion

    private void Awake()
    {
        #region 初始化
        Instance = this;
        fsmManager = new FSMManager((int)Data.FriendAnimationCount.Max);
        animator = GetComponent<Animator>();
        friendC = GetComponent<BoxCollider2D>();
        playerFoot = GameObject.FindGameObjectWithTag("PlayerDown");
        #endregion

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
        FriendIdel2 friendIdel2 = new FriendIdel2(animator);
        fsmManager.AddState(friendIdel2);
        FriendRun2 friendRun2 = new FriendRun2(animator);
        fsmManager.AddState(friendRun2);
        #endregion

    }
    private void Update()
    {
        fsmManager.OnStay();
        player = PlayerCtrl.Instance.transform.position;
        distance = PlayerCtrl.Instance.transform.position.x - transform.position.x;

        FriendData.JumpDistance = Vector2.Distance(playerFoot.transform.position, transform.position);

        if (FriendData.Jumped)
        {
            StartCoroutine("Jumped");
        }

        #region 巡逻与跟随
        if (!blink)
        {
            Partol();
        }
        #endregion

        #region 回到Idel的方法
        //Debug.Log(!FriendData.Attacking);
        //Debug.Log(!FriendData.Amassing);
        //Debug.Log(!FriendData.Backing);
        //Debug.Log(!FriendData.Casting);
        if (FriendData.Biging)
        {
            if (!FriendData.Attacking && !FriendData.Backing && !FriendData.Casting && !FriendData.Runing
                &&!FriendData.Amassing&&!FriendData.Smalling)
            {
                //Debug.Log("强制Idel2");
                ChangeState((sbyte)Data.FriendAnimationCount.Idel2);
            }
        }
        else
        {
            if (!FriendData.Attacking && !FriendData.Amassing && !FriendData.Backing && !FriendData.Casting && !FriendData.Biging)
            {
                //Debug.Log("强制Idel1");
                ChangeState((sbyte)Data.FriendAnimationCount.Idel);
            }
        }
        #endregion


        #region 被投掷的位移
        if (blink)
        {
            StartCoroutine("Blink");
        }
        #endregion

        #region 判断什么时候变小
        if (FriendData.Biging)
        {
            timeCount += Time.deltaTime;
            if (timeCount > FriendData.BigTime)
            {
                timeCount = 0;
                Small();
            }
        }
        else
        {
            timeCount = 0;
        }
        #endregion

    }

    #region 蓄力
    public void Amass()
    {
        GoToPlayer(player,0);
        if (!FriendData.Amassing)
        {
            FriendData.Amass = true;
            ChangeState((sbyte)Data.FriendAnimationCount.Amass);
        }
        ChangeState((sbyte)Data.FriendAnimationCount.Amassing);
    }
    #endregion

    #region 变小和召回

    #region 变小
    private void Small()
    {
        DelRigibody();
        ChangeState((sbyte)Data.FriendAnimationCount.Idel);
    }
    #endregion

    #region 召回
    public void Back()
    {
        ChangeState((sbyte)Data.FriendAnimationCount.Back);
    }
    #endregion

    #endregion

    #region 合体
    public void Fit()
    {

    }
    #endregion

    #region 被扔出去
    public void ThrowFriend(Vector2 target)
    {
        FriendData.Cast = true;
        this.target = target;
        if (animator.GetInteger("Index") != 4)
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
    }



    IEnumerator Blink()
    {
        if (target.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (target.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        transform.position = Vector2.MoveTowards(transform.position, target, PlayerData.ThrowSpeed);
        yield return new WaitForSeconds(PlayerData.ThrowLongTime);
        blink = false;
    }
    #endregion

    #region 添加和销毁刚体

    public void RigibodyCtrl()
    {
        if (FriendData.AddRigibody)
        {
            AddRigibody();
        }
        if (FriendData.DelRigibody)
        {
            DelRigibody();
        }
    }

    //添加刚体
    private void AddRigibody()
    {
        friendC.size = new Vector2(1.8f, 0.8f);
        friendC.offset = new Vector2(0, -0.3f);

        if (tmpRgb == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
            tmpRgb = GetComponent<Rigidbody2D>();
        }
        tmpRgb.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Material/Friend");
        tmpRgb.freezeRotation = true;
        tmpRgb.mass = 100;
        tmpRgb.gravityScale = 100;
    }

    //销毁刚体
    private void DelRigibody()
    {
        friendC.size = new Vector2(0.47f, 0.2f);
        friendC.offset = new Vector2(0, 0);
        Destroy(GetComponent<Rigidbody2D>());
        tmpRgb = null;
    }
    #endregion

    #region 被召唤到玩家位置
    public void GoToPlayer(Vector2 playerPostion, float timeRatio = 1,float x=0.25f,float y=-0.27f)
    {
        if (FriendData.Backing)
        {
            if (distance > 0)
            {
                if (back)
                {
                    distanceV = Vector2.zero;
                }
                else
                {
                    distanceV = new Vector2(-x, y);
                }
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            if (distance < 0)
            {
                if (back)
                {
                    distanceV = Vector2.zero;
                }
                else
                {
                    distanceV = new Vector2(x, y);
                }
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }

        }
        else
        {
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
        }
        transform.position = Vector2.SmoothDamp(transform.position, playerPostion + distanceV, ref velocity, FriendData.comeTime * timeRatio);
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
                if (ran < 0)
                {
                    ran = Mathf.Clamp(ran, -5, -3);
                    tmpVec = new Vector2(transform.position.x + ran, transform.position.y);

                }
                if (ran > 0)
                {
                    ran = Mathf.Clamp(ran, 3, 5);
                    tmpVec = new Vector2(transform.position.x + ran, transform.position.y);
                }
            }
            if (!FriendData.Backing)
            {
                StartCoroutine("IEPatrol");
            }
        }
        else
        {
            //Debug.Log(!FriendData.Amassing);
            //Debug.Log(!back);
            if (!FriendData.Casting && !FriendData.Amassing && !FriendData.Backing)
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
                    //Debug.Log("跟随中");
                    transform.position = Vector2.SmoothDamp(transform.position, player + distanceV, ref velocity, FriendData.smoothTime);
                }
                transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, player.y-0.3f),PlayerData.distance*0.001f);
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
        if (canPartol)
        {
            transform.position = Vector2.MoveTowards(transform.position, tmpVec, 0.08f);
            if (Vector2.Distance(transform.position, tmpVec) != 0f)
            {
                //Debug.Log("巡逻中");
                ChangeState((sbyte)Data.FriendAnimationCount.Run2);
            }
            else
            {
                ChangeState((sbyte)Data.FriendAnimationCount.Idel2);
            }
        }
        yield return new WaitForSeconds(0.4f);
        if (FriendData.Biging)
        {
            canPartol = true;
        }
        else
        {
            canPartol = false;
        }
    }
    #endregion

    IEnumerator Jumped()
    {
        transform.position = Vector2.Lerp(transform.position,FriendData.Jump2Target,0.8f);
        yield return new WaitForSeconds(0.1f);
        transform.position = Vector2.SmoothDamp(transform.position,
            new Vector2(FriendData.Jump2Target.x, FriendData.Jump2Target.y + FriendData.Jump2TargetY),ref velocity, 0.2f);
        yield return new WaitForSeconds(0.2f);
        FriendData.Jumped = false;
    }

    public void ChangeState(sbyte animationCount)
    {
        fsmManager.ChangeState(animationCount);
    }

}
