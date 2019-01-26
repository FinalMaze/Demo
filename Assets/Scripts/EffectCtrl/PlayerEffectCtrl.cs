using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectCtrl : MonoBehaviour
{
    Animator animator;
    FSMManager fsmManager;
    public static PlayerEffectCtrl Instance;
    private void Awake()
    {
        #region 初始化
        Instance = this;
        fsmManager = new FSMManager((int)Data.PlayerEffect.Max);
        animator = GetComponent<Animator>();
        #endregion
        #region 注册动画
        None None = new None(animator);
        fsmManager.AddState(None);
        PlayerAttackEffect PlayerAttackEffect = new PlayerAttackEffect(animator);
        fsmManager.AddState(PlayerAttackEffect);
        #endregion

    }
    private void Update()
    {
        fsmManager.OnStay();
    }
    public void ChangeState(sbyte animatorCount)
    {
        fsmManager.ChangeState(animatorCount);
    }
}
