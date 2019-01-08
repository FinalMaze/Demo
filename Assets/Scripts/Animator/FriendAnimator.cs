using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendIdel : FsmBase
{
    Animator animator;
    float timeCount;
    public FriendIdel(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        FriendData.DelRigibody = false;

        FriendData.Smalling = true;
        FriendData.Biging = false;

        FriendData.Attacking = false;
        FriendData.Amassing = false;
        FriendData.Casting = false;
        FriendData.Backing = false;
        FriendData.Runing = false;
        FriendData.Moving = false;

        FriendData.State = 0;
        animator.SetInteger("Index", 0);
    }
    public override void OnStay()
    {
        if (FriendData.CanBack&&PlayerData.Blowing)
        {
            timeCount += Time.deltaTime;
            FriendCtrl.Instance.GoToPlayer(PlayerCtrl.Instance.transform.position);
            if (timeCount>FriendData.comeTime)
            {
                FriendData.CanBack = false;
            }
        }
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
        FriendData.Attacking = false;
        FriendData.Amassing = false;
        FriendData.Casting = false;
        FriendData.Backing = false;
        FriendData.Runing = false;

        FriendData.State = 1;
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
        FriendData.Smalling = false;
        FriendData.Biging = true;
        FriendData.Attacking = true;
        FriendData.Amassing = false;
        FriendData.Casting = false;
        FriendData.Backing = false;
        FriendData.Runing = false;
        FriendData.Moving = false;

        FriendData.State = 2;
        animator.SetInteger("Index", 2);
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount > FriendData.AttackTime)
        {
            timeCount = 0;
            FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Idel2);
        }
    }
    public override void OnExit()
    {
        FriendData.Attacking = false;
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
        FriendData.Attacking = false;
        FriendData.Casting = false;
        FriendData.Backing = false;
        FriendData.Runing = false;
        FriendData.Moving = false;

        FriendData.State = 3;
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
        FriendData.Amassing = true;
        FriendData.Attacking = false;
        FriendData.Casting = false;
        FriendData.Backing = false;
        FriendData.Runing = false;
        FriendData.Moving = false;

        FriendData.State = 4;
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
        FriendData.DelRigibody = true;
        FriendCtrl.Instance.RigibodyCtrl();

        FriendData.Biging = false;
        FriendData.Backing = true;
        FriendData.Back = false;
        FriendData.Attacking = false;
        FriendData.Amassing = false;
        FriendData.Casting = false;
        FriendData.Runing = false;
        FriendData.Moving = false;

        FriendData.State = 5;
        animator.SetInteger("Index", 5);
    }
    public override void OnStay()
    {
        FriendCtrl.Instance.GoToPlayer(PlayerCtrl.Instance.transform.position);

        timeCount += Time.deltaTime;
        if (timeCount > FriendData.BackTime)
        {
            timeCount = 0;
            //FriendData.CanBack = false;
            FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Idel);
        }
    }
    public override void OnExit()
    {
        FriendData.Backing = false;
        FriendData.Smalling = true;
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
        FriendData.Biging = true;
        FriendData.Smalling = false;
        FriendData.Amassing = false;
        FriendData.Cast = false;
        FriendData.Casting = true;
        FriendData.Backing = false;
        FriendData.Attacking = false;
        FriendData.Amassing = false;
        FriendData.Runing = false;
        FriendData.Moving = false;


        FriendData.State = 6;
        animator.SetInteger("Index", 6);
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount > FriendData.CastTime)
        {
            timeCount = 0;
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

        FriendData.Biging = true;
        FriendData.Backing = false;
        FriendData.Attacking = false;
        FriendData.Amassing = false;
        FriendData.Casting = false;
        FriendData.Runing = false;
        FriendData.Moving = false;


        FriendData.State = 7;
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
        FriendData.State = 8;
        animator.SetInteger("Index", 8);

        FriendData.Biging = true;

        FriendData.Runing = true;
        FriendData.Backing = false;
        FriendData.Attacking = false;
        FriendData.Amassing = false;
        FriendData.Casting = false;
        FriendData.Moving = false;
    }
    public override void OnStay()
    {

    }
    public override void OnExit()
    {
        FriendData.Runing = false;
    }
}
