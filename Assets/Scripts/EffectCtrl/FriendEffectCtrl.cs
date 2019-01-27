using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendEffectCtrl : MonoBehaviour
{
    Animator animator;
    FSMManager fsmManager;
    public static FriendEffectCtrl Instance;
    private void Awake()
    {
        #region 初始化
        Instance = this;
        fsmManager = new FSMManager((int)Data.FriendEffect.Max);
        animator = GetComponent<Animator>();
        #endregion
        #region 注册动画
        None None = new None(animator);
        fsmManager.AddState(None);
        FriendAttackEffect FriendAttackEffect = new FriendAttackEffect(animator);
        fsmManager.AddState(FriendAttackEffect);
        #endregion

    }
    private void Update()
    {
        fsmManager.OnStay();
        #region    强制为空
        if (!FriendData.AttackingE||FriendData.Smalling)
        {
            ChangeState((sbyte)Data.FriendEffect.None);
        }
        #endregion
    }
    public void ChangeState(sbyte animatorCount)
    {
        fsmManager.ChangeState(animatorCount);
    }
}
