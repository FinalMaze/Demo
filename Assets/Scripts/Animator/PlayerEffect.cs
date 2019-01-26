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
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount>PlayerData.AttackEffectTime)
        {
            timeCount = 0;
            PlayerEffectCtrl.Instance.ChangeState((sbyte)Data.PlayerEffect.None);
        }
    }
}
