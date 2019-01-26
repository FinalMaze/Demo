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
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount> EnemyData.AttackTime)
        {
            enemyData.Attacking = false;
            timeCount = 0;
        }
    }
    public override void OnExit()
    {
        enemyData.Attacking = false;
    }
}

public class EnemyHurt : FsmBase
{
    Animator animator;
    EnemyData enemyData;
    EnemyCtrl enemyCtrl;
    float timeCount;
    public EnemyHurt(Animator tmpAnimator, EnemyData tmpEnemyData,EnemyCtrl tmpCtrl)
    {
        animator = tmpAnimator;
        enemyData = tmpEnemyData;
        enemyCtrl = tmpCtrl;
    }
    public override void OnEnter()
    {
        enemyData.Hurting = true;
        animator.SetInteger("Index", 3);
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount> EnemyData.HurtTime)
        {
            timeCount = 0;
            enemyData.Hurting = false;
        }
    }
    public override void OnExit()
    {
        enemyData.Hurting = false;
    }
}

public class EnemyDie : FsmBase
{
    Animator animator;
    EnemyData enemyData;
    EnemyCtrl enemyCtrl;
    float timeCount;
    public EnemyDie(Animator tmpAnimator,ref EnemyData tmpEnemyData,EnemyCtrl tmpCtrl)
    {
        animator = tmpAnimator;
        this.enemyData = tmpEnemyData;
        enemyCtrl = tmpCtrl;
    }
    public override void OnEnter()
    {
        GameInterfaceCtrl.Instance.AddKillCount(1);

        enemyData.Die = true;
        animator.SetInteger("Index", 4);
    }
    public override void OnStay()
    {
        enemyCtrl.Blink(enemyData.HurtDistance, enemyData.HurtSpeed);
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