using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffectCtrl : MonoBehaviour
{
    Animator animator;
    FSMManager fsmManager;
    EnemyData data;
    private void Awake()
    {
        #region 初始化
        data = new EnemyData();
        fsmManager = new FSMManager((int)Data.EnemyEffect.Max);
        animator = GetComponent<Animator>();
        #endregion
        #region 注册动画
        None None = new None(animator);
        fsmManager.AddState(None);
        EnemyAttack1Effect EnemyAttack1Effect = new EnemyAttack1Effect(animator,ref data);
        fsmManager.AddState(EnemyAttack1Effect);
        EnemyHurtEffect EnemyHurtEffect = new EnemyHurtEffect(animator, ref data);
        fsmManager.AddState(EnemyHurtEffect);
        EnemySummonEffect EnemySummonEffect = new EnemySummonEffect(animator, ref data);
        fsmManager.AddState(EnemySummonEffect);
        #endregion

    }
    private void Update()
    {
        fsmManager.OnStay();
        #region    强制为空
        if (!data.Attacking1E&&!data.HurtingE&&!data.SummoningE)
        {
            ChangeState((sbyte)Data.EnemyEffect.None);
        }
        #endregion
    }
    public void ChangeState(sbyte animatorCount)
    {
        fsmManager.ChangeState(animatorCount);
    }
}
