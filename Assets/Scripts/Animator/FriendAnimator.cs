using System.Collections;
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
        FriendData.DelRigibody = false;
        FriendData.Biging = false;
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
        FriendData.Amass = false;
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
    float timeCount;
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
        if (FriendData.Cast)
        {
            FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Cast);
        }
        else
        {
            FriendData.Runing = false;
        }
    }
    public override void OnExit()
    {
        
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
        FriendData.DelRigibody = true;
        FriendCtrl.Instance.RigibodyCtrl();
        FriendData.Backing = true;
        FriendData.Back = false;
        animator.SetInteger("Index", 5);
    }
    public override void OnStay()
    {
        FriendCtrl.Instance.GoToPlayer();

        timeCount += Time.deltaTime;
        if (timeCount > FriendData.BackTime)
        {
            timeCount = 0;
            FriendData.CanBack = false;
            FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Idel);
            FriendData.Backing = false;
        }
    }
    public override void OnExit()
    {
        FriendData.CanBack = false;
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
        FriendData.Amassing = false;
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
            //变化到大型的动作流程
            FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Idel2);
        }

    }
    public override void OnExit()
    {
        FriendData.Casting = false;
        //添加刚体
        FriendData.AddRigibody = true;
        FriendCtrl.Instance.RigibodyCtrl();
        //转换为变大状态
        FriendData.Biging = true;
    }
}
public class FriendIdel2 : FsmBase
{
    Animator animator;
    public FriendIdel2(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        FriendData.AddRigibody = false;
        animator.SetInteger("Index", 7);
    }
    public override void OnStay()
    {

    }
    public override void OnExit()
    {

    }
}
public class FriendRun2 : FsmBase
{
    Animator animator;
    public FriendRun2(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("Index", 8);
        FriendData.Runing = true;
    }
    public override void OnStay()
    {

    }
    public override void OnExit()
    {
        FriendData.Runing = false;
    }
}
