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
        control = GetComponent<CharacterController2D>();
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
    float moveSpeed=PlayerData.runSpeed;
    private void Update()
    {
        fsmManager.OnStay();

        if (ETCInput.GetAxis("Horizontal") > 0f)
        {

            run = 1;
        }
        if (ETCInput.GetAxis("Horizontal") < 0f)
        {
            run = -1;
        }
        if (ETCInput.GetAxis("Horizontal")==0)
        {
            run = 0;
        }
     


        #region 判断移动和跳跃
        //判断是否在地面上
        if (!PlayerData.playerIsGround)
        {
            //PlayerData.playerWalk = false;
            PlayerData.playerJumping = true;
            PlayerData.playerRun = false;
        }
        else
        {
            PlayerData.playerJumping = false;
        }

        //如果不是跳跃状态，可以移动
        if (!PlayerData.playerJumping)
        {
            if (PlayerData.playerRun)
            {
                ChangeState((sbyte)Data.AnimationCount.Run);
            }
            else if (!Data.EasyTouch)
            {
                ChangeState((sbyte)Data.AnimationCount.Idel);
            }
        }
        #endregion
    }

    #region 协程跳跃
    IEnumerator Jump()
    {
        //ChangeState((sbyte)Data.AnimationCount.Jump);

        
        //推迟多少时间进行跳跃
        yield return new WaitForSeconds(0.1f);
        PlayerData.playerStartJump = false;
    }
    #endregion

    private void FixedUpdate()
    {
        PlayerData.playerStartJump = false;
        control.Move(run * moveSpeed * Time.fixedDeltaTime, false, PlayerData.playerStartJump);
    }
    public void ChangeState(sbyte animationCount)
    {
        fsmManager.ChangeState(animationCount);
    }
    public void StartJump()
    {
       
        PlayerData.playerStartJump = true;
        //StartCoroutine("Jump");
    }
}
