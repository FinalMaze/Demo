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
}
public class FriendMove : FsmBase
{
    Animator animator;
    float timeCount;
    bool canDamage;
    bool canLoop;
    int tmpI = -1;
    public FriendMove(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        canDamage = true;
        canLoop = true;
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
        if (canLoop)
        {
            Loop();
        }
        if (Data.FriendAI)
        {
            FriendCtrl.Instance.GoToPlayer(PlayerCtrl.Instance.transform.position, 1f);
        }
        else
        {
            FriendPlayerCtrl.Instance.GoToPlayer(PlayerCtrl.Instance.transform.position, PlayerData.BackSpeed);
        }


        timeCount += Time.deltaTime;
        if (timeCount > FriendData.MoveTime)
        {
            timeCount = 0;
            //FriendData.CanBack = false;
            if (Data.FriendAI)
            {
                FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Idel);
            }
            else
            {
                FriendPlayerCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Idel);
            }
        }

    }
    public override void OnExit()
    {
        FriendData.Moving = false;
        canLoop = true;
    }

    private bool Loop()
    {
        for (int i = 0; i < Data.allEnemy.Count; i++)
        {
            if (Data.allEnemy[i].transform.position.x > animator.transform.position.x
                && Data.allEnemy[i].transform.position.x < PlayerCtrl.Instance.transform.position.x)
            {
                if (tmpI != i)
                {
                    canDamage = true;
                    tmpI = i;
                }
                if (canDamage)
                {
                    canDamage = false;
                    if (Data.allEnemy[i].transform.position.x > animator.transform.position.x)
                    {
                        EnemyTest tmp = Data.allEnemy[i].GetComponent<EnemyTest>();
                        if (tmp != null)
                        {
                            if (!tmp.enemyData.Hurting)
                            {
                                tmp.Hurt(FriendData.Damage, 1);
                            }

                        }
                        else
                        {
                            if (!Data.allEnemy[i].GetComponent<EnemyCtrl>().enemyData.Hurting)
                            {
                                Data.allEnemy[i].GetComponent<EnemyCtrl>().Hurt(FriendData.Damage, 1);
                            }
                        }
                    }
                }

            }
            else if (Data.allEnemy[i].transform.position.x < animator.transform.position.x
                && Data.allEnemy[i].transform.position.x > PlayerCtrl.Instance.transform.position.x)
            {
                if (tmpI != i)
                {
                    canDamage = true;
                    tmpI = i;
                }
                if (canDamage)
                {
                    canDamage = false;
                    if (Data.allEnemy[i].transform.position.x < animator.transform.position.x)
                    {
                        EnemyTest tmp = Data.allEnemy[i].GetComponent<EnemyTest>();
                        if (tmp != null)
                        {
                            if (!tmp.enemyData.Hurting)
                            {
                                tmp.Hurt(FriendData.Damage, -1);
                            }

                        }
                        else
                        {
                            if (!Data.allEnemy[i].GetComponent<EnemyCtrl>().enemyData.Hurting)
                            {
                                Data.allEnemy[i].GetComponent<EnemyCtrl>().Hurt(FriendData.Damage, -1);
                            }
                        }
                    }
                }
            }
        }
        return canLoop = false;
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
        if (Data.FriendAI)
        {
            FriendCtrl.Instance.Blink(FriendData.AttackBlinkDistance, FriendData.AttackBlinkSpeed);
        }
        else
        {
            FriendPlayerCtrl.Instance.Blink(FriendData.AttackBlinkDistance, FriendData.AttackBlinkSpeed);
        }
        timeCount += Time.deltaTime;
        if (timeCount > FriendData.AttackTime)
        {
            timeCount = 0;
            if (Data.FriendAI)
            {
                FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Idel2);
            }
            else
            {
                FriendPlayerCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Idel2);
            }
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
            if (Data.FriendAI)
            {
                FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Amassing);
            }
            else
            {
                FriendPlayerCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Amassing);
            }
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
            if (Data.FriendAI)
            {
                FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Cast);
            }
            else
            {
                FriendPlayerCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Cast);
            }
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
    bool canDamage;
    bool canLoop;
    int tmpI = -1;
    public FriendBack(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        canDamage = true;
        canLoop = true;

        FriendData.DelRigibody = true;
        if (Data.FriendAI)
        {
            FriendCtrl.Instance.RigibodyCtrl();
        }
        else
        {
            FriendPlayerCtrl.Instance.RigibodyCtrl();
        }

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
        if (Data.FriendAI)
        {
            FriendCtrl.Instance.GoToPlayer(PlayerCtrl.Instance.transform.position, PlayerData.BackSpeed);
        }
        else
        {
            FriendPlayerCtrl.Instance.GoToPlayer(PlayerCtrl.Instance.transform.position, PlayerData.BackSpeed);

        }

        if (canLoop)
        {
            Loop();
        }

        timeCount += Time.deltaTime;
        if (timeCount > FriendData.BackTime)
        {
            timeCount = 0;
            //FriendData.CanBack = false;
            if (Data.FriendAI)
            {
                FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Idel);
            }
            else
            {
                FriendPlayerCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Idel);
            }
        }
    }
    public override void OnExit()
    {
        FriendData.Backing = false;
        FriendData.Smalling = true;
        canLoop = true;
    }
    private bool Loop()
    {
        for (int i = 0; i < Data.allEnemy.Count; i++)
        {
            if (Data.allEnemy[i].transform.position.x > animator.transform.position.x
                && Data.allEnemy[i].transform.position.x < PlayerCtrl.Instance.transform.position.x)
            {
                if (tmpI != i)
                {
                    canDamage = true;
                    tmpI = i;
                }
                if (canDamage)
                {
                    canDamage = false;
                    if (Data.allEnemy[i].transform.position.x > animator.transform.position.x)
                    {
                        EnemyTest tmp = Data.allEnemy[i].GetComponent<EnemyTest>();
                        if (tmp != null)
                        {
                            if (!tmp.enemyData.Hurting)
                            {
                                tmp.Hurt(FriendData.Damage, 1);
                            }
                            
                        }
                        else
                        {
                            if (!Data.allEnemy[i].GetComponent<EnemyCtrl>().enemyData.Hurting)
                            {
                                Data.allEnemy[i].GetComponent<EnemyCtrl>().Hurt(FriendData.Damage, 1);
                            }
                        }
                    }
                }

            }
            else if (Data.allEnemy[i].transform.position.x < animator.transform.position.x
                && Data.allEnemy[i].transform.position.x > PlayerCtrl.Instance.transform.position.x)
            {
                if (tmpI != i)
                {
                    canDamage = true;
                    tmpI = i;
                }
                if (canDamage)
                {
                    canDamage = false;
                    if (Data.allEnemy[i].transform.position.x < animator.transform.position.x)
                    {
                        EnemyTest tmp = Data.allEnemy[i].GetComponent<EnemyTest>();
                        if (tmp != null)
                        {
                            if (!tmp.enemyData.Hurting)
                            {
                                tmp.Hurt(FriendData.Damage, -1);
                            }

                        }
                        else
                        {
                            if (!Data.allEnemy[i].GetComponent<EnemyCtrl>().enemyData.Hurting)
                            {
                                Data.allEnemy[i].GetComponent<EnemyCtrl>().Hurt(FriendData.Damage, -1);
                            }
                        }
                    }
                }
            }
        }
        return canLoop = false;
    }
}
public class FriendCast : FsmBase
{
    Animator animator;
    float timeCount;
    bool canDamage;
    bool canLoop;
    int tmpI = -1;

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

        canDamage = true;
        canLoop = true;


        FriendData.State = 6;
        animator.SetInteger("Index", 6);
    }
    public override void OnStay()
    {
        Loop();
        if (Data.FriendAI)
        {
            FriendCtrl.Instance.GoToPlayer(FriendData.Target, PlayerData.ThrowSpeed);
        }
        else
        {
            FriendPlayerCtrl.Instance.GoToPlayer(FriendData.Target, PlayerData.ThrowSpeed);
        }
        timeCount += Time.deltaTime;
        if (timeCount > FriendData.CastTime)
        {
            timeCount = 0;
            //变化到大型的动作流程
            if (Data.FriendAI)
            {
                FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Idel2);
            }
            else
            {
                FriendPlayerCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Idel2);
            }
        }

    }
    public override void OnExit()
    {
        FriendData.Casting = false;
        //添加刚体
        FriendData.AddRigibody = true;
        if (Data.FriendAI)
        {
            FriendCtrl.Instance.RigibodyCtrl();
        }
        else
        {
            FriendPlayerCtrl.Instance.RigibodyCtrl();
        }

        //转换为变大状态
    }
    private bool Loop()
    {
        for (int i = 0; i < Data.allEnemy.Count; i++)
        {
            if (Data.allEnemy[i].transform.position.x > animator.transform.position.x&&
                Data.allEnemy[i].transform.position.x <FriendData.StartPos.x)
            {
                if (tmpI != i)
                {
                    canDamage = true;
                    tmpI = i;
                }
                if (canDamage)
                {
                    canDamage = false;
                    if (Mathf.Abs(Data.allEnemy[i].transform.position.x - animator.transform.position.x) < 0.3f)
                    {
                        if (Data.allEnemy[i].transform.position.x > animator.transform.position.x)
                        {
                            EnemyTest tmp = Data.allEnemy[i].GetComponent<EnemyTest>();
                            if (tmp != null)
                            {
                                if (!tmp.enemyData.Hurting)
                                {
                                    tmp.Hurt(FriendData.Damage, -1);
                                }
                            }
                            else
                            {
                                if (!Data.allEnemy[i].GetComponent<EnemyCtrl>().enemyData.Hurting)
                                {
                                    Data.allEnemy[i].GetComponent<EnemyCtrl>().Hurt(FriendData.Damage, -1);
                                }
                            }
                        }
                    }
                }

            }
            else if (Data.allEnemy[i].transform.position.x < animator.transform.position.x
                && Data.allEnemy[i].transform.position.x > FriendData.StartPos.x)
            {
                if (tmpI != i)
                {
                    canDamage = true;
                    tmpI = i;
                }
                if (canDamage)
                {
                    canDamage = false;
                    if (Mathf.Abs(Data.allEnemy[i].transform.position.x - animator.transform.position.x) < 1f)
                    {
                        if (Data.allEnemy[i].transform.position.x < animator.transform.position.x)
                        {
                            EnemyTest tmp = Data.allEnemy[i].GetComponent<EnemyTest>();
                            if (tmp != null)
                            {
                                if (!tmp.enemyData.Hurting)
                                {
                                    tmp.Hurt(FriendData.Damage, 1);
                                }
                            }
                            else
                            {
                                if (!Data.allEnemy[i].GetComponent<EnemyCtrl>().enemyData.Hurting)
                                {
                                    Data.allEnemy[i].GetComponent<EnemyCtrl>().Hurt(FriendData.Damage, 1);
                                }
                            }
                        }
                    }
                }
            }
        }
        return canLoop = false;
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
public class FriendBlow : FsmBase
{
    Animator animator;
    float timeCount;
    bool canDamage;
    public FriendBlow(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        FriendData.Blowing = true;
        canDamage = true;

        FriendData.State = 9;
        animator.SetInteger("Index", 9);
    }
    public override void OnStay()
    {
        timeCount += Time.deltaTime;
        if (timeCount > FriendData.BlowDamageTime && canDamage)
        {
            Loop();
        }

        if (timeCount > FriendData.BlowTime)
        {
            timeCount = 0;
            canDamage = true;
            if (Data.FriendAI)
            {
                FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Idel);
            }
            else
            {
                FriendPlayerCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Idel);
            }
        }
    }
    public override void OnExit()
    {
        FriendData.Blowing = false;
    }

    private bool Loop()
    {
        for (int i = 0; i < Data.allEnemy.Count; i++)
        {
            if (Vector2.Distance(Data.allEnemy[i].transform.position, animator.transform.position) < FriendData.BlowRadius)
            {
                if (Data.allEnemy[i].transform.position.x > animator.transform.position.x)
                {
                    if (Data.allEnemy[i].transform.position.x > animator.transform.position.x)
                    {
                        EnemyTest tmp = Data.allEnemy[i].GetComponent<EnemyTest>();
                        if (tmp != null)
                        {
                            tmp.Hurt(FriendData.Damage, 1);
                        }
                        else
                        {
                            Data.allEnemy[i].GetComponent<EnemyCtrl>().Hurt(FriendData.Damage, 1);
                        }
                    }
                }
                if (Data.allEnemy[i].transform.position.x < animator.transform.position.x)
                {
                    if (Data.allEnemy[i].transform.position.x > FriendCtrl.Instance .transform.position.x)
                    {
                        EnemyTest tmp = Data.allEnemy[i].GetComponent<EnemyTest>();
                        if (tmp != null)
                        {
                            tmp.Hurt(FriendData.Damage, -1);
                        }
                        else
                        {
                            Data.allEnemy[i].GetComponent<EnemyCtrl>().Hurt(FriendData.Damage, -1);
                        }
                    }
                }
            }
        }
        return canDamage = false;
    }

}
public class FriendAttack2 : FsmBase
{
    Animator animator;
    float timeCount;
    public FriendAttack2(Animator tmpAnimator)
    {
        animator = tmpAnimator;
    }
    public override void OnEnter()
    {
        //添加刚体
        FriendData.AddRigibody = true;
        if (Data.FriendAI)
        {
            FriendCtrl.Instance.RigibodyCtrl();
        }
        else
        {
            FriendPlayerCtrl.Instance.RigibodyCtrl();
        }

        FriendData.Smalling = false;
        FriendData.Biging = true;
        FriendData.Attacking = true;
        FriendData.Amassing = false;
        FriendData.Casting = false;
        FriendData.Backing = false;
        FriendData.Runing = false;
        FriendData.Moving = false;
        FriendData.State = 10;
        animator.SetInteger("Index", 10);
    }
    public override void OnStay()
    {
        if (Data.FriendAI)
        {
            FriendCtrl.Instance.Blink(FriendData.AttackBlinkDistance, FriendData.AttackBlinkSpeed);
        }
        else
        {
            FriendPlayerCtrl.Instance.Blink(FriendData.AttackBlinkDistance, FriendData.AttackBlinkSpeed);
        }
        timeCount += Time.deltaTime;
        if (timeCount > FriendData.AttackTime2)
        {
            timeCount = 0;
            if (Data.FriendAI)
            {
                FriendCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Idel2);
            }
            else
            {
                FriendPlayerCtrl.Instance.ChangeState((sbyte)Data.FriendAnimationCount.Idel2);
            }
        }
    }
    public override void OnExit()
    {
        FriendData.Attacking = false;
    }
}

