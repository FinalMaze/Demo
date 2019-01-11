using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBallCtrl : MonoBehaviour {
    Rigidbody2D ball;
    FSMManager fsmManager;
    Animator animator;
    EnemyData data;
    private void Awake()
    {
        fsmManager = new FSMManager((int)Data.BallAnimationCount.Max);
        ball = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        data = new EnemyData();

        #region 注册动画
        BallStart ballStart = new BallStart(animator,data);
        fsmManager.AddState(ballStart);
        Balling Balling = new Balling(animator);
        fsmManager.AddState(Balling);
        BallEnd BallEnd = new BallEnd(animator,data);
        fsmManager.AddState(BallEnd);
        #endregion
    }
    void Start ()
    {
        ball.velocity = transform.right * EnemyData.BallSpeed;
        ChangeState((sbyte)Data.BallAnimationCount.BallStart);
	}
    private void Update()
    {
        fsmManager.OnStay();
        if (!data.BallStarting && !data.BallEnding)
        {
            ChangeState((sbyte)Data.BallAnimationCount.Balling);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.tag=="PlayerParent")
        {
            ball.velocity = Vector2.zero;
            ChangeState((sbyte)Data.BallAnimationCount.BallEnd);
            Damage();
        }
    }
    private void Damage()
    {
        if (PlayerCtrl.Instance.transform.position.x>transform.position.x)
        {
            PlayerCtrl.Instance.Hurt(EnemyData.BallDamage,1);
        }
        if (PlayerCtrl.Instance.transform.position.x < transform.position.x)
        {
            PlayerCtrl.Instance.Hurt(EnemyData.BallDamage, -1);
        }
    }
    private void ChangeState(sbyte animator)
    {
        fsmManager.ChangeState(animator);
    }
}
