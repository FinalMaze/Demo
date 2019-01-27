using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendAttackEffect : FsmBase
{
    Animator animator;
    float timeCount;
    public FriendAttackEffect(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("Index", 1);
        FriendData.AttackingE = true;
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount > FriendData.AttackEffectTime)
        {
            timeCount = 0;
            FriendData.AttackingE = false;
        }
    }
    public override void OnExit()
    {
        timeCount = 0;
        FriendData.AttackingE = false;
    }
}
