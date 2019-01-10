﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    FSMManager fsmManager;
    Animator animator;
    EnemyData enemyData;
    SpriteRenderer sprite;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        enemyData = new EnemyData();
        fsmManager = new FSMManager((int)Data.EnemyAnimationCount.Max);
        sprite = GetComponentInChildren<SpriteRenderer>();


        InvokeRepeating("RandomPos", 2, 2);

        #region 注册动画
        EnemyIdel enemyIdel = new EnemyIdel(animator);
        fsmManager.AddState(enemyIdel);
        EnemyWalk enemyWalk = new EnemyWalk(animator);
        fsmManager.AddState(enemyWalk);
        EnemyAttack enemyAttack = new EnemyAttack(animator, enemyData);
        fsmManager.AddState(enemyAttack);
        EnemyHurt enemyHurt = new EnemyHurt(animator, enemyData,this);
        fsmManager.AddState(enemyHurt);
        EnemyDie enemyDie = new EnemyDie(animator,ref enemyData);
        fsmManager.AddState(enemyDie);
        EnemyAttack2 enemyAttack2 = new EnemyAttack2(animator, enemyData);
        fsmManager.AddState(enemyAttack2);
        #endregion
    }
    float distance;
    float direction;
    private void Update()
    {
        fsmManager.OnStay();
        distance = Mathf.Abs( PlayerCtrl.Instance.transform.position.x- transform.position.x);
        direction = PlayerCtrl.Instance.transform.position.x - transform.position.x;

        #region 强制进入Idel
        if (!enemyData.Attacking&&!enemyData.Attacking2 && !enemyData.Hurting && !enemyData.Die)
        {
            ChangeState((sbyte)Data.EnemyAnimationCount.Idel);
        }
        #endregion

        EnemyAI();

    }

    #region 巡逻
    bool canPartol;
    float attackTimeCount;
    float attack2TimeCount;
    bool canAttack=true;
    public void Patrol()
    {
        if (!enemyData.Attacking&&!enemyData.Attacking2 && !enemyData.Hurting && !enemyData.Die)
        {
            //如果大于怪物的可跟随距离，进行巡逻
            if (distance > enemyData.FllowDistance)
            {
                if (ran > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                if (ran < 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                if (canPartol)
                {
                    transform.position = Vector2.MoveTowards(transform.position, tmpVec, enemyData.MoveSpeed);
                    if (Vector2.Distance(transform.position, tmpVec) >= 0.01f)
                    {
                        ChangeState((sbyte)Data.EnemyAnimationCount.Walk);
                    }
                    else
                    {
                        canPartol = false;
                        ChangeState((sbyte)Data.EnemyAnimationCount.Idel);
                    }
                }
            }
            //跟随并攻击
            else
            {
                if (direction > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                if (direction < 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }

                if (distance > enemyData.AttackDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, PlayerCtrl.Instance.transform.position, enemyData.MoveSpeed);
                    ChangeState((sbyte)Data.EnemyAnimationCount.Walk);
                }
                if (distance > enemyData.AttackDistance*2)
                {
                    canAttack = true;
                    attack2TimeCount += Time.deltaTime;
                    if (attack2TimeCount > enemyData.AttackCD)
                    {
                        attack2TimeCount = 0;
                        Attack2();
                    }

                }
                else if (distance <= enemyData.AttackDistance)
                {
                    if (canAttack)
                    {
                        canAttack = false;
                        Attack();
                    }
                    attackTimeCount += Time.deltaTime;
                    if (attackTimeCount > enemyData.AttackCD)
                    {
                        attackTimeCount = 0;
                        Attack();
                    }
                }
            }
        }
    }

    int ran;
    Vector2 tmpVec;
    public void RandomPos()
    {
        if (distance > enemyData.FllowDistance)
        {
            ran = Random.Range(-5, 5);
            if (ran < 0)
            {
                ran = Mathf.Clamp(ran, -5, -3);
                tmpVec.x = transform.position.x + ran;
                tmpVec.y = transform.position.y;
            }
            if (ran > 0)
            {
                ran = Mathf.Clamp(ran, 3, 5);
                tmpVec.x = transform.position.x + ran;
                tmpVec.y = transform.position.y;
            }
            canPartol = true;
        }
    }


    #endregion

    #region 怪物行为
    public void EnemyAI()
    {
        if (!enemyData.Die)
        {
            Patrol();
        }

    }
    #endregion

    #region 攻击
    public void Attack()
    {
        ChangeState((sbyte)Data.EnemyAnimationCount.Attack);
        if (enemyData.Attacking&&enemyData.HP!=0)
        {
            Invoke("Damage", enemyData.PlayerHurtTime);
        }
    }
    public void Attack2()
    {
        ChangeState((sbyte)Data.EnemyAnimationCount.Attack2);
        Debug.Log("二段攻击!!! ");
    }
    private void Damage()
    {
        if (distance<3f)
        {
            if (PlayerCtrl.Instance.transform.position.x>transform.position.x)
            {
                PlayerCtrl.Instance.Hurt(enemyData.Damage,1);
            }
            if (PlayerCtrl.Instance.transform.position.x < transform.position.x)
            {
                PlayerCtrl.Instance.Hurt(enemyData.Damage, -1);
            }

        }
    }
    #endregion

    #region 受击
    float dir;
    public void Hurt(float reduceHP,float tmpDir)
    {
        dir = tmpDir;
        enemyData.HP = Mathf.Clamp(enemyData.HP -= reduceHP, 0, enemyData.MaxHP);
        if (enemyData.HP != 0)
        {
            StartCoroutine("Red");
            if (!enemyData.Attacking && !enemyData.Die)
            {
                ChangeState((sbyte)Data.EnemyAnimationCount.Hurt);
            }
        }
        else
        {
            ChangeState((sbyte)Data.EnemyAnimationCount.Die);
            Invoke("Destory", 0.64f);
        }
    }
    IEnumerator Red()
    {
        sprite.color = new Color(Data.R / 255f, Data.G / 255f, Data.B / 255f, Data.A / 255f);
        yield return new WaitForSeconds(Data.IdelRGB);
        sprite.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
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
    #endregion

    #region 重生
    public void Destory()
    {
        AIManager.Instance.DelEnemy(gameObject);
    }
    #endregion

    #region 变换动作的方法
    void ChangeState(sbyte animatorCount)
    {
        fsmManager.ChangeState(animatorCount);
    }
    #endregion
}
