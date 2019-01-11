﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgehogTeam.EasyTouch;

public class PlayerCtrl : MonoBehaviour
{
    private static PlayerCtrl instance;
    public static PlayerCtrl Instance
    {
        get { return instance; }
    }
    FSMManager fsmManager;
    Animator animator;
    CharacterController2D control;
    Rigidbody2D rgb;
    SpriteRenderer sprite;

    bool leftBlink = false;
    bool rightBlink = false;

    private void Awake()
    {
        #region 初始化数据
        instance = this;
        control = GetComponent<CharacterController2D>();
        fsmManager = new FSMManager((int)Data.AnimationCount.Max);
        animator = GetComponentInChildren<Animator>();
        rgb = GetComponent<Rigidbody2D>();
        #endregion
        #region 添加玩家动画
        PlayerIdel playerIdel = new PlayerIdel(animator);
        fsmManager.AddState(playerIdel);
        PlayerWalk playerWalk = new PlayerWalk(animator);
        fsmManager.AddState(playerWalk);
        PlayerRun playerRun = new PlayerRun(animator);
        fsmManager.AddState(playerRun);
        PlayerJump playerJump = new PlayerJump(animator);
        fsmManager.AddState(playerJump);
        PlayerJumping playerJumping = new PlayerJumping(animator);
        fsmManager.AddState(playerJumping);
        PlayerAttack playerAttack = new PlayerAttack(animator);
        fsmManager.AddState(playerAttack);
        PlayerAttack2 playerAttack2 = new PlayerAttack2(animator);
        fsmManager.AddState(playerAttack2);
        PlayerAmass playerAmass = new PlayerAmass(animator);
        fsmManager.AddState(playerAmass);
        PlayerCast playerCast = new PlayerCast(animator);
        fsmManager.AddState(playerCast);
        PlayerJumpEnd playerJumpEnd = new PlayerJumpEnd(animator);
        fsmManager.AddState(playerJumpEnd);
        PlayerBlow PlayerBlow = new PlayerBlow(animator);
        fsmManager.AddState(PlayerBlow);
        PlayerHurt PlayerHurt = new PlayerHurt(animator);
        fsmManager.AddState(PlayerHurt);
        #endregion

        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    float run;
    float moveSpeed = PlayerData.runSpeed;
    float lastTestJump;
    float lastJumpPos;
    private void Update()
    {
        fsmManager.OnStay();
        PlayerData.Dircetion = transform.localScale.x;


        #region 检测是否在下落过程中
        if (PlayerData.Jumping)
        {
            if (Time.time - lastTestJump > 0.01f)
            {
                lastTestJump = Time.time;
                if (transform.position.y >= lastJumpPos || PlayerData.playerIsGround)
                {
                    PlayerData.Downing = false;
                }
                else
                {
                    PlayerData.Downing = true;
                }
                lastJumpPos = transform.position.y;
            }
        }
        #endregion

        #region 判断是否能进行动作的前置条件
        //判断是否在地面上
        //if (!PlayerData.playerIsGround)
        //{
        //    PlayerData.Attacking = false;
        //    PlayerData.Ideling = false;
        //    PlayerData.Amassing = false;
        //    PlayerData.Casting = false;
        //    PlayerData.Jumping = true;
        //    PlayerData.Walking = false;
        //    PlayerData.Runing = false;
        //    PlayerData.Attacking = false;
        //    PlayerData.Attacking2 = false;
        //}
        #endregion

        #region 锁移动
        if (PlayerData.Amassing || PlayerData.Attacking || PlayerData.Casting || PlayerData.Blowing || PlayerData.Hurting)
        {
            rgb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rgb.constraints = ~RigidbodyConstraints2D.FreezePosition;
        }
        #endregion

        #region 判断是否进行投掷
        #endregion

        #region 转向并移动
        if (ETCInput.GetAxis("Horizontal") > 0f)
        {
            run = 1f;
        }
        //else if (ETCInput.GetAxis("Horizontal") > 0.5f)
        //{
        //    PlayerData.playerWalk = true;
        //    run = 1;
        //}
        if (ETCInput.GetAxis("Horizontal") < 0f)
        {
            run = -1f;
        }
        //if (ETCInput.GetAxis("Horizontal") < -0.5f)
        //{
        //    PlayerData.playerWalk = true;
        //    run = -1;
        //}
        if (ETCInput.GetAxis("Horizontal") == 0)
        {
            PlayerData.Walking = false;
            run = 0;
            Data.EasyTouch = false;
        }
        if (run != 0)
        {
            Data.EasyTouch = true;
        }
        #endregion

        #region 冲刺
        if (!PlayerData.Jumping)
        {
            if (rightBlink && !leftBlink)
            {
                StartCoroutine("RightBlink");
            }
            if (leftBlink && !rightBlink)
            {
                StartCoroutine("LeftBlink");
            }
        }
        #endregion

        #region 在地上的动作判断
        if (PlayerData.playerIsGround && !PlayerData.Jumping && !PlayerData.Amassing && !PlayerData.Attacking
            && !PlayerData.Casting && !PlayerData.Blowing && !PlayerData.Hurting)
        {
            //Debug.Log(PlayerData.Attacking);
            //Debug.Log(PlayerData.Attacking2);
            PlayerData.Jump2ing = false;
            #region 跳跃
            if (PlayerData.playerStartJump)
            {
                ChangeState((sbyte)Data.AnimationCount.Jump);
            }
            #endregion

            #region 判断是否播放Walk或Run动画
            //如果在Walk状态，变换成Walk动作
            if (Data.EasyTouch)
            {
                ChangeState((sbyte)Data.AnimationCount.Walk);
            }
            //如果横轴为0，那么变成Idel状态
            else if (!Data.EasyTouch && !PlayerData.Walking)
            {
                ChangeState((sbyte)Data.AnimationCount.Idel);
            }
            #endregion

        }
        #endregion

        #region 攻击的位移
        //if (canAttackBlink)
        //{
        if (PlayerData.Attacking1)
        {
            transform.position = Vector2.MoveTowards(transform.position, tmpAttackTarget, PlayerData.AttackSpeed1);
        }
        if (PlayerData.Attacking2)
        {
            transform.position = Vector2.MoveTowards(transform.position, tmpAttackTarget, PlayerData.AttackSpeed2);
        }

        //}
        #endregion

    }
    private void FixedUpdate()
    {
        control.Move(run * moveSpeed * Time.fixedDeltaTime, false, PlayerData.playerStartJump);
        PlayerData.playerStartJump = false;
    }

    #region 蓄力
    public void Amass()
    {
        if (FriendData.Smalling && !FriendData.Biging && !FriendData.Attacking && !FriendData.Casting)
        {
            ChangeState((sbyte)Data.AnimationCount.Amass);
        }
    }
    #endregion

    #region 普通攻击
    Vector2 tmpAttackTarget;
    public void Attack()
    {
        if (!PlayerData.Jumping && !PlayerData.Casting && !PlayerData.Blowing && !PlayerData.Hurting)
        {
            PlayerData.Attack = true;
            ChangeState((sbyte)Data.AnimationCount.Attack);
            //如果攻击距离内有敌人，取消攻击位移
            if (CheckEnemy())
            {
                Invoke("Damage", PlayerData.EnemyHurtTime);
                tmpAttackTarget.x = transform.position.x;
                tmpAttackTarget.y = transform.position.y;
            }
            else
            {
                if (PlayerData.Dircetion > 0)
                {
                    tmpAttackTarget.x = transform.position.x + PlayerData.AttackDistance1;
                    tmpAttackTarget.y = transform.position.y;
                }
                if (PlayerData.Dircetion < 0)
                {
                    tmpAttackTarget.x = transform.position.x - PlayerData.AttackDistance1;
                    tmpAttackTarget.y = transform.position.y;
                }
            }
        }
    }
    public void Attack2()
    {
        PlayerData.Attack2 = true;
        //如果攻击距离内有敌人，取消攻击位移
        if (CheckEnemy())
        {
            Invoke("Damage", PlayerData.EnemyHurtTime);
            tmpAttackTarget.x = transform.position.x;
            tmpAttackTarget.y = transform.position.y;
        }
        else
        {
            if (PlayerData.Dircetion > 0)
            {
                tmpAttackTarget.x = transform.position.x + PlayerData.AttackDistance1;
                tmpAttackTarget.y = transform.position.y;
            }
            if (PlayerData.Dircetion < 0)
            {
                tmpAttackTarget.x = transform.position.x - PlayerData.AttackDistance1;
                tmpAttackTarget.y = transform.position.y;
            }
        }

    }
    #endregion

    #region 攻击造成伤害
    public bool CheckEnemy()
    {
        for (int i = 0; i < Data.allEnemy.Count; i++)
        {
            if (Mathf.Abs(Data.allEnemy[i].transform.position.x - transform.position.x) < PlayerData.AttackDistance)
            {
                if (PlayerData.Dircetion>0&&Data.allEnemy[i].transform.position.x > transform.position.x)
                {
                    return true;
                }
                if (PlayerData.Dircetion < 0 && Data.allEnemy[i].transform.position.x < transform.position.x)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Damage()
    {
        for (int i = 0; i < Data.allEnemy.Count; i++)
        {
            if (Mathf.Abs(Data.allEnemy[i].transform.position.x - transform.position.x) < PlayerData.AttackDistance)
            {
                if (PlayerData.Dircetion > 0 && Data.allEnemy[i].transform.position.x > transform.position.x)
                {
                    Data.allEnemy[i].GetComponent<EnemyCtrl>().Hurt(PlayerData.Damage, 1);
                }
                if (PlayerData.Dircetion < 0 && Data.allEnemy[i].transform.position.x < transform.position.x)
                {
                    Data.allEnemy[i].GetComponent<EnemyCtrl>().Hurt(PlayerData.Damage, -1);
                }
                PlayerData.mp = Mathf.Clamp(PlayerData.mp += PlayerData.AddMP, 0, PlayerData.mpMax);
                GameInterfaceCtrl.Instance.UpdateMP();
            }
        }
    }
    #endregion

    #region 被攻击
    float dir;
    public void Hurt(float reduceHP, float tmpDir)
    {
        PlayerData.hp -= reduceHP;
        GameInterfaceCtrl.Instance.UpdateHP();
        StartCoroutine("Red");
        if (!PlayerData.Attacking && !PlayerData.Jumping)
        {
            dir = tmpDir;
            ChangeState((sbyte)Data.AnimationCount.Hurt);
        }
    }
    IEnumerator Red()
    {
        sprite.color = new Color(Data.R / 255f, Data.G / 255f, Data.B / 255f, Data.A / 255f);
        yield return new WaitForSeconds(Data.IdelRGB);
        sprite.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
    }
    #endregion

    #region 扔召唤兽
    public void Throw()
    {
        //变化动作   To Do
        PlayerData.Cast = true;
        if (!PlayerData.Jumping)
        {
            ChangeState((sbyte)Data.AnimationCount.Cast);
        }
    }
    #endregion

    #region 爆破
    public void Boom()
    {
        Debug.Log("Boom");
        //PlayerData.Cast = true;
        //ChangeState((sbyte)Data.AnimationCount.Cast);

        //if (transform.localScale.x > 0)
        //{
        //    FriendCtrl.Instance.ThrowFriend(new Vector2(transform.position.x + 5, 0));
        //}
        //if (transform.localScale.x < 0)
        //{
        //    FriendCtrl.Instance.ThrowFriend(new Vector2(transform.position.x - 5, 0));
        //}

    }
    #endregion

    #region 合体
    public void Fit()
    {
        Debug.Log("合体！！！");
    }
    #endregion

    #region 变化动作的方法
    public void ChangeState(sbyte animationCount)
    {
        fsmManager.ChangeState(animationCount);
    }
    #endregion

    #region 冲刺的方法
    public void Blink(float distance, float speed)
    {
        if (dir > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2((transform.position.x + distance), transform.position.y), speed);
        }
        if (dir < 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2((transform.position.x - distance), transform.position.y), speed);
        }
    }


    Vector2 tmp;
    public void BlinkR(Gesture gesture)
    {
        if (gesture.swipeLength > 80f)
        {
            if (!PlayerData.Jumping)
            {
                if (rightBlink || leftBlink)
                {
                    return;
                }
                tmp = transform.position;
                if (gesture.actionTime < 0.4f)
                {
                    rightBlink = true;
                }
            }
        }
    }
    public void BlinkF(Gesture gesture)
    {
        if (gesture.swipeLength > 80f)
        {
            if (!PlayerData.Jumping)
            {
                if (rightBlink || leftBlink)
                {
                    return;
                }
                tmp = transform.position;
                if (gesture.actionTime < 0.4f)
                {
                    leftBlink = true;
                }
            }
        }
    }
    float timeCount;
    IEnumerator RightBlink()
    {
        timeCount += Time.deltaTime;
        if (timeCount < 0.3f)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(tmp.x + PlayerData.BlinkDistance, tmp.y), PlayerData.BlinkTempDistance);
        }
        yield return new WaitForSeconds(PlayerData.BlinkTime);
        timeCount = 0;
        rightBlink = false;
    }
    IEnumerator LeftBlink()
    {
        timeCount += Time.deltaTime;
        if (timeCount < 0.3f)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(tmp.x - PlayerData.BlinkDistance, tmp.y), PlayerData.BlinkTempDistance);
        }
        yield return new WaitForSeconds(PlayerData.BlinkTime);
        timeCount = 0;
        leftBlink = false;
    }
    #endregion

    #region 跳跃的方法
    public void StartJump()
    {
        //Debug.Log(PlayerData.playerIsGround);
        //Debug.Log(PlayerData.Jumping);
        //Debug.Log(PlayerData.Amassing);
        //Debug.Log(PlayerData.Attacking);
        //Debug.Log(PlayerData.Casting);
        if (PlayerData.playerIsGround && !PlayerData.Jumping && !PlayerData.Amassing && !PlayerData.Attacking
            && !PlayerData.Casting&&!PlayerData.Hurting&&!PlayerData.Blowing)
        {
            PlayerData.playerStartJump = true;
            ChangeState((sbyte)Data.AnimationCount.Jump);
        }
    }
    #endregion

}
