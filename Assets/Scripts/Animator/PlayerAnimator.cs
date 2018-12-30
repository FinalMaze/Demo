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
        if (!PlayerData.playerRun)
        {
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Idel);
        }
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
        Debug.Log("Jump");
        PlayerData.playerJumping = true;
        animator.SetInteger("Index", 3);
    }
    public override void OnStay()
    {
        PlayerData.playerStartJump = false;
        timeCount += Time.deltaTime;
        if (timeCount>1.07f)
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
public class PlayerSway : FsmBase
{
    Animator animator;
    float timeCount;
    public PlayerSway(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("Index", 4);
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount>1.25f)
        {
            PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Idel);
        }
    }
    public override void OnExit()
    {

    }
}