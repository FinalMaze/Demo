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
        PlayerData.Ideling = true;

        PlayerData.State = 0;
        animator.SetInteger("Index", 0);
    }
    public override void OnExit()
    {
        PlayerData.Ideling = false;
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
        PlayerData.Ideling = false;
        PlayerData.Jumping = false;
        PlayerData.Walking = true;
        PlayerData.Runing = false;

        PlayerData.State = 1;
        animator.SetInteger("Index", 1);
    }
    public override void OnExit()
    {
        PlayerData.Walking = false;
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
        PlayerData.Runing = true;

        PlayerData.State = 2;
        animator.SetInteger("Index", 2);
    }
    public override void OnExit()
    {
        PlayerData.Runing = false;
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
        PlayerData.Jumping = true;

        PlayerData.State = 3;
        animator.SetInteger("Index", 3);
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;

        if (timeCount>0.1f&& PlayerData.playerIsGround)
        {
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.JumpEnd);
        }
        if (timeCount>PlayerData.JumpTime)
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
        PlayerData.Jumping = true;

        PlayerData.State = 4;
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
        PlayerData.Jumping = true;

        PlayerData.State = 9;
        animator.SetInteger("Index", 9);
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount > PlayerData.JumpEndTime)
        {
            timeCount = 0;
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Idel);
        }
    }
    public override void OnExit()
    {
        PlayerData.Jumping = false;
    }
}
public class PlayerAttack : FsmBase
{
    Animator animator;
    public static float timeCount;
    public PlayerAttack(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        PlayerData.Attack = false;
        PlayerData.Attacking = true;
        PlayerData.Attacking1 = true;
        PlayerData.Ideling = false;
        PlayerData.Amassing = false;
        PlayerData.Casting = false;
        PlayerData.Jumping = false;
        PlayerData.Walking = false;
        PlayerData.Runing = false;
        PlayerData.Attacking2 = false;

        PlayerData.State = 5;
        animator.SetInteger("Index", 5);
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount > PlayerData.AttackTime)
        {
            if (PlayerData.Attack2)
            {
                PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Attack2);
            }
            else
            {
                PlayerData.Attacking = false;
                PlayerData.Attack2 = false;
                timeCount = 0;
                PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Idel);
            }
        }
    }
    public override void OnExit()
    {
        PlayerData.Attacking1 = false;
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
        PlayerData.Attack2 = false;
        PlayerData.Attacking = true;
        PlayerData.Ideling = false;
        PlayerData.Amassing = false;
        PlayerData.Casting = false;
        PlayerData.Jumping = false;
        PlayerData.Walking = false;
        PlayerData.Runing = false;
        PlayerData.Attacking2 = true;


        PlayerAttack.timeCount = 0;
        PlayerData.State = 6;
        animator.SetInteger("Index", 6);
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount > PlayerData.Attack2Time)
        {
            PlayerData.Attacking = false;
            PlayerData.Attacking2 = false;
            timeCount = 0;
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Idel);
        }
    }
    public override void OnExit()
    {
        PlayerData.Attacking = false;
        PlayerData.Attacking2 = false;
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
        PlayerData.Amassing = true;

        PlayerData.State = 7;
        animator.SetInteger("Index", 7);
    }
    public override void OnStay()
    {

    }
    public override void OnExit()
    {
        PlayerData.Amassing = false;
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
        PlayerData.Cast = false;

        PlayerData.Casting = true;

        PlayerData.State = 8;
        animator.SetInteger("Index", 8);
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount>PlayerData.CastTime)
        {
            timeCount = 0;
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Idel);
        }
    }
    public override void OnExit()
    {
        PlayerData.Casting = false;
    }
}

public class PlayerBack : FsmBase
{
    Animator animator;
    float timeCount;
    public PlayerBack(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        PlayerData.Backing = true;

        PlayerData.State = 10;
        animator.SetInteger("Index", 10);
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount > PlayerData.BackTime)
        {
            timeCount = 0;
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Idel);
        }
    }
    public override void OnExit()
    {
        PlayerData.Backing = false;
    }
}

public class PlayerHurt : FsmBase
{
    Animator animator;
    float timeCount;
    public PlayerHurt(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        PlayerData.Hurting = true;

        PlayerData.State = 11;
        animator.SetInteger("Index", 11);
    }
    public override void OnStay()
    {
        PlayerCtrl.Instance.Blink(PlayerData.HurtDistance, PlayerData.HurtSpeed);
        timeCount += Time.deltaTime;
        if (timeCount > PlayerData.HurtTime)
        {
            timeCount = 0;
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Idel);
        }
    }
    public override void OnExit()
    {
        PlayerData.Hurting = false;
    }
}

public class PlayerBlow : FsmBase
{
    Animator animator;
    float timeCount;
    public PlayerBlow(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        PlayerData.Blowing = true;

        PlayerData.State = 12;
        animator.SetInteger("Index", 12);
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount > PlayerData.BlowTime)
        {
            timeCount = 0;
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Idel);
        }
    }
    public override void OnExit()
    {
        PlayerData.Blowing = false;
    }
}