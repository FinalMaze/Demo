using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class None : FsmBase
{
    Animator animator;
    public None(Animator tmpAnimator)
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
public class PlayerAttackEffect : FsmBase
{
    Animator animator;
    float timeCount;
    public PlayerAttackEffect(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("Index", 1);
        PlayerData.AttackingE = true;
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount>PlayerData.AttackEffectTime)
        {
            timeCount = 0;
            PlayerData.AttackingE = false;
        }
    }
    public override void OnExit()
    {
        timeCount = 0;
        PlayerData.AttackingE = false;
    }
}
