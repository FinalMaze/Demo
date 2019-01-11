using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStart : FsmBase
{
    Animator animator;
    EnemyData data;
    float timeCount;
    public BallStart(Animator tmpAnimator,EnemyData tmpData)
    {
        animator = tmpAnimator;
        data = tmpData;
    }
    public override void OnEnter()
    {
        data.BallStarting = true;
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount>EnemyData.BallStart)
        {
            timeCount = 0;
            data.BallStarting = false;
        }
    }
    public override void OnExit()
    {
        data.BallStarting = false;
    }
}

public class Balling : FsmBase
{
    Animator animator;
    public Balling(Animator tmpAnimator)
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
public class BallEnd : FsmBase
{
    Animator animator;
    EnemyData data;
    float timeCount;
    public BallEnd(Animator tmpAnimator,EnemyData tmpData)
    {
        animator = tmpAnimator;
        data = tmpData;
    }
    public override void OnEnter()
    {
        animator.SetInteger("Index", 2);
        data.BallEnding = true;
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount>EnemyData.BallEnd)
        {
            timeCount = 0;
            GameObject.Destroy(animator.gameObject);
        }
    }
    public override void OnExit()
    {
    }
}