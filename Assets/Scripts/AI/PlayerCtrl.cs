using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private static PlayerCtrl instance;
    public static PlayerCtrl Instance
    {
        get { return instance; }
    }
    FSMManager fsmManager;
    Animator animator;
    CharacterController2D control;
    private void Awake()
    {
        #region 初始化数据
        instance = this;
        control= GetComponent<CharacterController2D>();
        fsmManager = new FSMManager();
        animator = GetComponent<Animator>();
        #endregion
        #region 添加玩家动画
        PlayerIdel playerIdel = new PlayerIdel(animator);
        fsmManager.AddState(playerIdel);
        PlayerWalk playerWalk = new PlayerWalk(animator);
        fsmManager.AddState(playerWalk);
        PlayerRun playerRun = new PlayerRun(animator);
        fsmManager.AddState(playerRun);
        PlayerJump playerJump = new PlayerJump(animator);
        fsmManager.AddState(playerJump);
        PlayerSway playerSway = new PlayerSway(animator);
        fsmManager.AddState(playerSway);
        #endregion

    }
    float run;
    float moveSpeed;
    private void Update()
    {
        fsmManager.OnStay();
        run = Input.GetAxisRaw("Horizontal");

        #region 判断移动和跳跃
        //判断是否在地面上
        if (!Data.playerIsGround)
        {
            Data.playerWalk = false;
            Data.playerRun = false;
        }
        //判断是否按下左shift键
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Data.playerRun = true;
        }
        else
        {
            Data.playerRun = false;
        }
        //如果在地上，可以移动
        if (Data.playerIsGround)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                Data.playerWalk = true;
            }
            else
            {
                Data.playerWalk = false;
            }
        }
        //跳跃
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("Jump");
        }
        //如果不是跳跃状态，可以移动
        if (!Data.playerJumping)
        {
            if (Data.playerWalk)
            {
                if (Data.playerRun)
                {
                    moveSpeed = Data.playerRunSpeed;
                    ChangeState((sbyte)Data.AnimationCount.Run);
                }
                else
                {
                    moveSpeed = Data.playerWalkSpeed;
                    ChangeState((sbyte)Data.AnimationCount.Walk);
                }
            }
            else if (!Data.playerWalk)
            {
                ChangeState((sbyte)Data.AnimationCount.Idel);
            }
        }
        #endregion
    }

    #region 协程跳跃
    IEnumerator Jump()
    {
        ChangeState((sbyte)Data.AnimationCount.Jump);
        //推迟多少时间进行跳跃
        yield return new WaitForSeconds(0.25f);
        Data.playerStartJump = true;
    }
    #endregion

    private void FixedUpdate()
    {
        control.Move(run * moveSpeed * Time.fixedDeltaTime, false, Data.playerStartJump);
    }
    public void ChangeState(sbyte animationCount)
    {
        fsmManager.ChangeState(animationCount);
    }

}
