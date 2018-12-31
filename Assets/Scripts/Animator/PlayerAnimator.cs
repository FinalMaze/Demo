using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdel : FsmBase
{
    Animator animator;
    public PlayerIdel(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("Index", 0);
    }
    public override void OnStay()
    {
        if (PlayerData.playerStartJump)
        {
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Jump);
        }
        if (PlayerData.Attack)
        {
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Attack);
        }
        if (PlayerData.Cast)
        {
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Cast);
        }
        if (PlayerData.Amass)
        {
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Amass);
        }
        if (PlayerData.playerWalk)
        {
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Walk);
        }
    }
    public override void OnExit()
    {

    }
}
public class PlayerWalk : FsmBase
{

    Animator animator;
    public PlayerWalk(Animator tmpAnimator)
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
        PlayerData.playerWalk = false;
    }
}
public class PlayerRun : FsmBase
{
    Animator animator;
    public PlayerRun(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("Index", 2);
    }
    public override void OnStay()
    {
    }
    public override void OnExit()
    {

    }
}
public class PlayerJump : FsmBase
{
    float timeCount;
    
    Animator animator;
    public PlayerJump(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        PlayerData.playerJumping = true;
        animator.SetInteger("Index", 3);
    }
    public override void OnStay()
    {
        if (PlayerData.playerIsGround)
        {
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.JumpEnd);
        }
        timeCount += Time.deltaTime;
        if (timeCount>0.29f)
        {
            timeCount = 0;
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Jumping);
        }
    }
    public override void OnExit()
    {
        
    }
}
public class PlayerJumping : FsmBase
{
    Animator animator;
    float timeCount;
    public PlayerJumping(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("Index", 4);
    }
    public override void OnStay()
    {
        
        if (PlayerData.playerIsGround)
        {
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.JumpEnd);
        }
    }
    public override void OnExit()
    {

    }
}
public class PlayerJumpEnd : FsmBase
{
    float timeCount;

    Animator animator;
    public PlayerJumpEnd(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        PlayerData.playerJumping = true;
        animator.SetInteger("Index", 9);
    }
    public override void OnStay()
    {
        PlayerData.playerStartJump = false;
        timeCount += Time.deltaTime;
        if (timeCount > 0.05f)
        {
            PlayerData.playerJumping = false;
            timeCount = 0;
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Idel);
        }
    }
    public override void OnExit()
    {

    }
}
public class PlayerAttack : FsmBase
{
    Animator animator;
    float timeCount;
    public PlayerAttack(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("Index", 5);
    }
    public override void OnStay()
    {
        PlayerData.Attack = false;
        timeCount += Time.deltaTime;
        if (timeCount > 0.35f)
        {
            PlayerData.Attacking = false;
            timeCount = 0;
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Idel);
        }
    }
    public override void OnExit()
    {

    }
}
public class PlayerAttack2 : FsmBase
{
    Animator animator;
    float timeCount;
    public PlayerAttack2(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("Index", 6);
    }
    public override void OnStay()
    {

    }
    public override void OnExit()
    {

    }
}
public class PlayerAmass : FsmBase
{
    Animator animator;
    float timeCount;
    public PlayerAmass(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("Index", 7);
    }
    public override void OnStay()
    {

    }
    public override void OnExit()
    {

    }
}
public class PlayerCast : FsmBase
{
    Animator animator;
    float timeCount;
    public PlayerCast(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("Index", 8);
    }
    public override void OnStay()
    {

    }
    public override void OnExit()
    {

    }
}