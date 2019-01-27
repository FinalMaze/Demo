using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdel : FsmBase
{
    Animator animator;
    public EnemyIdel(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("Index", 0);
    }
    public override void OnStay()
    {

    }
    public override void OnExit()
    {

    }
}

public class EnemyWalk : FsmBase
{
    Animator animator;
    public EnemyWalk(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("Index", 1);
    }
    public override void OnStay()
    {

    }
    public override void OnExit()
    {

    }
}

public class EnemyAttack : FsmBase
{
    Animator animator;
    EnemyData enemyData;
    float timeCount;
    public EnemyAttack(Animator tmpAnimator,EnemyData tmpEnemyData)
    {
        animator = tmpAnimator;
        enemyData = tmpEnemyData;
    }
    public override void OnEnter()
    {
        enemyData.Attacking = true;
        animator.SetInteger("Index", 2);

        effect = true;
        baseA = animator.GetComponentInParent<Rigidbody2D>();
        tmp = baseA.GetComponentInChildren<EnemyEffectCtrl>();
    }
    bool effect = true;
    Rigidbody2D baseA;
    EnemyEffectCtrl tmp;
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (effect&&timeCount>EnemyData.AttackEStartTime)
        {
            effect = false;
            if (tmp==null)
            {
                Debug.Log("没找到");
            }
            else
            {
                tmp.ChangeState((sbyte)Data.EnemyEffect.Attack1);
            }
        }
        if (timeCount> EnemyData.AttackTime)
        {
            enemyData.Attacking = false;
            timeCount = 0;
        }
    }
    public override void OnExit()
    {
        effect = true;
        enemyData.Attacking = false;
    }
}

public class EnemyHurt : FsmBase
{
    Animator animator;
    EnemyData enemyData;
    float timeCount;
    public EnemyHurt(Animator tmpAnimator, EnemyData tmpEnemyData)
    {
        animator = tmpAnimator;
        enemyData = tmpEnemyData;
    }
    public override void OnEnter()
    {
        enemyData.Hurting = true;
        animator.SetInteger("Index", 3);

        //effect = true;
        //baseA = animator.GetComponentInParent<Rigidbody2D>();
        //tmp = baseA.GetComponentInChildren<EnemyEffectCtrl>();
    }
    //bool effect = true;
    //Rigidbody2D baseA;
    //EnemyEffectCtrl tmp;
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        //if (effect && timeCount > EnemyData.HurtEStartTime)
        //{
        //    effect = false;
        //    if (tmp == null)
        //    {
        //        Debug.Log("没找到");
        //    }
        //    else
        //    {
        //        tmp.ChangeState((sbyte)Data.EnemyEffect.Hurt);
        //    }
        //}

        if (timeCount> EnemyData.HurtTime)
        {
            timeCount = 0;
            enemyData.Hurting = false;
        }
    }
    public override void OnExit()
    {
        timeCount = 0;
        enemyData.Hurting = false;
    }
}

public class EnemyDie : FsmBase
{
    Animator animator;
    EnemyData enemyData;
    float timeCount;
    public EnemyDie(Animator tmpAnimator,ref EnemyData tmpEnemyData)
    {
        animator = tmpAnimator;
        this.enemyData = tmpEnemyData;
    }
    public override void OnEnter()
    {
        GameInterfaceCtrl.Instance.AddKillCount(1);

        enemyData.Die = true;
        animator.SetInteger("Index", 4);

        AudioManager.Instance.StartAudio(Data.Audio.Die.ToString());
        //tmpCtrl = animator.GetComponent<EnemyTest>();
    }
    //EnemyTest tmpCtrl;
    public override void OnStay()
    {
        //if (tmpCtrl==null)
        //{
        //    tmpCtrl.Blink(enemyData.HurtDistance, enemyData.HurtSpeed);
        //}
        //else
        //{
        //    animator.GetComponent<EnemyTest>().Blink(enemyData.HurtDistance, enemyData.HurtSpeed);
        //}
        timeCount += Time.deltaTime;
        if (timeCount > 0.64f)
        {
            timeCount = 0;
        }
    }
    public override void OnExit()
    {

    }
}
public class EnemyAttack2 : FsmBase
{
    Animator animator;
    EnemyData enemyData;
    float timeCount;
    public EnemyAttack2(Animator tmpAnimator, EnemyData tmpEnemyData)
    {
        animator = tmpAnimator;
        enemyData = tmpEnemyData;
    }
    public override void OnEnter()
    {
        enemyData.Attacking2 = true;
        animator.SetInteger("Index", 5);
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount > EnemyData.AttackTime2)
        {
            timeCount = 0;
            enemyData.Attacking2 = false;
        }
    }
    public override void OnExit()
    {
        enemyData.Attacking2 = false;
    }
}

public class EnemySummon : FsmBase
{
    Animator animator;
    EnemyData enemyData;
    float timeCount;
    public EnemySummon(Animator tmpAnimator, ref EnemyData tmpEnemyData)
    {
        animator = tmpAnimator;
        this.enemyData = tmpEnemyData;
    }
    public override void OnEnter()
    {
        enemyData.Summoning = true;
        animator.SetInteger("Index", 6);

        effect = true;
        baseA = animator.GetComponentInParent<Rigidbody2D>();
        tmp = baseA.GetComponentInChildren<EnemyEffectCtrl>();
    }
    bool effect = true;
    Rigidbody2D baseA;
    EnemyEffectCtrl tmp;
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (effect && timeCount > EnemyData.SummonEStartTime)
        {
            effect = false;
            if (tmp == null)
            {
                Debug.Log("没找到");
            }
            else
            {
                tmp.ChangeState((sbyte)Data.EnemyEffect.Summon);
            }
        }


        if (timeCount > EnemyData.SummonTime)
        {
            timeCount = 0;
            enemyData.Summoning = false;
        }
    }
    public override void OnExit()
    {

    }
}
