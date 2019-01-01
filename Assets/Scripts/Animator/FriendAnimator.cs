﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendIdel : FsmBase
{
    Animator animator;
    public FriendIdel(Animator tmpAnimator)
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
public class FriendMove : FsmBase
{
    Animator animator;
    public FriendMove(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        FriendData.Moving = true;
        animator.SetInteger("Index", 1);
    }
    public override void OnStay()
    {

    }
    public override void OnExit()
    {
        FriendData.Moving = false;
    }
}
public class FriendAttack : FsmBase
{
    Animator animator;
    float timeCount;
    public FriendAttack(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        FriendData.Attacking = true;
        animator.SetInteger("Index", 2);
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount > FriendData.AttackTime)
        {
            timeCount = 0;
            FriendData.Attacking = false;
            FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Back);
        }
    }
    public override void OnExit()
    {

    }
}
public class FriendAmass : FsmBase
{
    Animator animator;
    float timeCount;
    public FriendAmass(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        FriendData.Amassing = true;
        animator.SetInteger("Index", 3);
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount > FriendData.AmassTime)
        {
            timeCount = 0;
            FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Amassing);
        }
    }
    public override void OnExit()
    {

    }
}
public class FriendAmassing : FsmBase
{
    Animator animator;
    public FriendAmassing(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("Index", 4);
    }
    public override void OnStay()
    {

    }
    public override void OnExit()
    {
        FriendData.Amassing = false;
    }
}
public class FriendBack : FsmBase
{
    Animator animator;
    float timeCount;
    public FriendBack(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        FriendData.Backing = true;
        animator.SetInteger("Index", 5);
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        FriendCtrl.Instance.GoToPlayer();
        if (timeCount > FriendData.BackTime)
        {
            timeCount = 0;
            FriendData.Backing = false;
            FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Idel);
        }
    }
    public override void OnExit()
    {

    }
}
public class FriendCast : FsmBase
{
    Animator animator;
    float timeCount;
    public FriendCast(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        FriendData.Cast = false;
        FriendData.Casting = true;
        animator.SetInteger("Index", 6);
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount > FriendData.CastTime)
        {
            timeCount = 0;
            FriendData.Casting = false;
            FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Attack);
        }

    }
    public override void OnExit()
    {

    }
}