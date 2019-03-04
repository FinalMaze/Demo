using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack1Effect : FsmBase
{
    Animator animator;
    float timeCount;
    EnemyData tmpData;
    public EnemyAttack1Effect(Animator tmpAnimator,ref EnemyData data)
    {
        animator = tmpAnimator;
        tmpData = data;
    }
    public override void OnEnter()
    {
        animator.SetInteger("Index", 1);
        tmpData.Attacking1E = true;
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount > EnemyData.AttackTimeE)
        {
            timeCount = 0;
            tmpData.Attacking1E = false;
        }
    }
    public override void OnExit()
    {
        timeCount = 0;
        tmpData.Attacking1E = false;
    }
}
public class EnemyHurtEffect : FsmBase
{
    Animator animator;
    float timeCount;
    EnemyData tmpData;
    public EnemyHurtEffect(Animator tmpAnimator, ref EnemyData data)
    {
        animator = tmpAnimator;
        tmpData = data;
    }
    public override void OnEnter()
    {
        animator.SetInteger("Index", 2);
        tmpData.HurtingE = true;
        AudioManager.Instance.StartAudio(Data.Audio.Hurt.ToString());
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount > EnemyData.HurtTimeE)
        {
            timeCount = 0;
            tmpData.HurtingE = false;
        }
    }
    public override void OnExit()
    {
        timeCount = 0;
        tmpData.HurtingE = false;
    }
}
public class EnemySummonEffect : FsmBase
{
    Animator animator;
    float timeCount;
    EnemyData tmpData;
    public EnemySummonEffect(Animator tmpAnimator, ref EnemyData data)
    {
        animator = tmpAnimator;
        tmpData = data;
    }
    public override void OnEnter()
    {
        animator.SetInteger("Index", 3);
        tmpData.SummoningE = true;
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount > EnemyData.SummonTimeE)
        {
            Debug.Log(timeCount);
            timeCount = 0;
            tmpData.SummoningE = false;
        }
    }
    public override void OnExit()
    {
    }
}