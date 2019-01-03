using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgehogTeam.EasyTouch;

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
    Transform playerR;
    Rigidbody2D rgb;

    bool leftBlink = false;
    bool rightBlink = false;

    private void Awake()
    {
        #region 初始化数据
        instance = this;
        control = GetComponent<CharacterController2D>();
        fsmManager = new FSMManager((int)Data.AnimationCount.Max);
        animator = GetComponent<Animator>();
        playerR = GetComponent<Transform>();
        rgb = GetComponent<Rigidbody2D>();
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
        PlayerJumping playerJumping = new PlayerJumping(animator);
        fsmManager.AddState(playerJumping);
        PlayerAttack playerAttack = new PlayerAttack(animator);
        fsmManager.AddState(playerAttack);
        PlayerAttack2 playerAttack2 = new PlayerAttack2(animator);
        fsmManager.AddState(playerAttack2);
        PlayerAmass playerAmass = new PlayerAmass(animator);
        fsmManager.AddState(playerAmass);
        PlayerCast playerCast = new PlayerCast(animator);
        fsmManager.AddState(playerCast);
        PlayerJumpEnd playerJumpEnd = new PlayerJumpEnd(animator);
        fsmManager.AddState(playerJumpEnd);
        #endregion

        // todo
        //AIManager.Instance.BuildFriend("",transform);
    }
    float run;
    float moveSpeed = PlayerData.runSpeed;
    private void Update()
    {
        fsmManager.OnStay();
        PlayerData.Dircetion = transform.localScale.x;


        #region 判断是否能进行动作的前置条件
        //判断是否在地面上
        if (!PlayerData.playerIsGround)
        {
            //PlayerData.playerWalk = false;
            PlayerData.playerRun = false;
        }
        #endregion

        #region 蓄力 中锁移动
        if (PlayerData.Amassing)
        {
            rgb.constraints = RigidbodyConstraints2D.FreezePositionX|RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rgb.constraints = ~RigidbodyConstraints2D.FreezePosition;
        }
        #endregion
        

        #region 判断是否进行投掷
        #endregion

        #region 转向并移动
        if (ETCInput.GetAxis("Horizontal") > 0f)
        {
            PlayerData.playerWalk = true;
            run = 1f;
        }
        //else if (ETCInput.GetAxis("Horizontal") > 0.5f)
        //{
        //    PlayerData.playerWalk = true;
        //    run = 1;
        //}
        if (ETCInput.GetAxis("Horizontal") < 0f)
        {
            PlayerData.playerWalk = true;
            run = -1f;
        }
        //if (ETCInput.GetAxis("Horizontal") < -0.5f)
        //{
        //    PlayerData.playerWalk = true;
        //    run = -1;
        //}
        if (ETCInput.GetAxis("Horizontal") == 0)
        {
            run = 0;
            Data.EasyTouch = false;
        }
        if (run != 0)
        {
            Data.EasyTouch = true;
        }
        #endregion

        #region 冲刺
        if (!PlayerData.playerJumping)
        {
            if (rightBlink)
            {
                StartCoroutine("RightBlink");
            }
            if (leftBlink)
            {
                StartCoroutine("LeftBlink");
            }
        }
        #endregion

        #region 在地上的动作判断
        if (PlayerData.playerIsGround && !PlayerData.playerJumping)
        {
            //Debug.Log(PlayerData.Attacking);
            //Debug.Log(PlayerData.Attacking2);

            #region 跳跃
            if (PlayerData.playerStartJump)
            {
                Debug.Log("Jump!!!");
                ChangeState((sbyte)Data.AnimationCount.Jump);
            }
            #endregion

            #region 判断是否播放Walk或Run动画
            //如果在Walk状态，变换成Walk动作
            if (Data.EasyTouch && !PlayerData.Attacking&&!PlayerData.Attacking2&&!PlayerData.Amassing&&!PlayerData.Casting)
            {
                ChangeState((sbyte)Data.AnimationCount.Walk);
            }
            //如果横轴为0，那么变成Idel状态
            else if (!Data.EasyTouch && !PlayerData.Attacking && !PlayerData.Casting && !PlayerData.Amassing && !PlayerData.Attacking2)
            {
                ChangeState((sbyte)Data.AnimationCount.Idel);
            }
            ////如果在run状态，变换成跑步动作
            //if (PlayerData.playerRun)
            //{
            //    ChangeState((sbyte)Data.AnimationCount.Run);
            //}
            ////如果横轴为0，那么变成Idel状态
            //else if (run == 0 && !PlayerData.Attacking)
            //{
            //    ChangeState((sbyte)Data.AnimationCount.Idel);
            //}
            #endregion

        }
        #endregion

    }
    private void FixedUpdate()
    {

        control.Move(run * moveSpeed * Time.fixedDeltaTime, false, PlayerData.playerStartJump);
        PlayerData.playerStartJump = false;
    }


    #region 长按时的攻击变化
    public void StayAttack(Gesture gesture)
    {
        if (!FriendData.Biging)
        {
            if (PlayerData.distance < PlayerData.throwDistance)
            {
                FriendCtrl.Instance.GoToPlayer();
                if (gesture.actionTime > 0.5f)
                {
                    Amass();
                }
            }
            else if (PlayerData.distance > PlayerData.throwDistance && gesture.actionTime > 1f)
            {
                FriendCtrl.Instance.GoToPlayer();
            }
        }
        else
        {
            Debug.Log("变大中，暂时还不能召回");
        }
    }
    #endregion

    #region 松开时的攻击方法
    public void AttackAI(Gesture gesture)
    {

        float timeCount = gesture.actionTime;
        if (timeCount >0.5f)
        {
            if (PlayerData.distance < PlayerData.throwDistance)
            {
                if (PlayerData.Amassing)
                {
                    ThrowFriend();
                }
                else
                {
                    Debug.LogWarning("没在进行Amass");
                }
            }
            else
            {
                Attack();
            }
        }
        else
        {
            Debug.Log("0.5秒以下");
        }
        //else if (timeCount >= 1f)
        //{
        //    if (PlayerData.distance < PlayerData.throwDistance)
        //    {
        //        Boom();
        //    }
        //    else
        //    {
        //        FriendCtrl.Instance.GoToPlayer();
        //    }
        //}
        //else if (timeCount >= 3f)
        //{
        //    Fit();
        //}
    }
    #endregion

    #region 蓄力
    public void Amass()
    {
        FriendCtrl.Instance.Amass();
        ChangeState((sbyte)Data.AnimationCount.Amass);
    }
    #endregion

    #region 普通攻击
    public void Attack()
    {
        if (PlayerData.distance<1f)
        {
            ThrowFriend();
        }
        else if (PlayerData.Attacking && PlayerData.Attack2)
        {
            ChangeState((sbyte)Data.AnimationCount.Attack2);
        }
        else if (!PlayerData.Attacking && !PlayerData.Attack2)
        {
            PlayerData.Attack = true;
            //todo 伤害计算
            ChangeState((sbyte)Data.AnimationCount.Attack);
        }
    }
    #endregion

    #region 扔召唤兽
    //直接扔

    //蓄力扔
    public void ThrowFriend()
    {
        //变化动作   To Do
        PlayerData.Cast = true;
        ChangeState((sbyte)Data.AnimationCount.Cast);
        if (transform.localScale.x > 0)
        {
            FriendCtrl.Instance.ThrowFriend(new Vector2(transform.position.x + 5, transform.position.y+0.4f));
        }
        if (transform.localScale.x < 0)
        {
            FriendCtrl.Instance.ThrowFriend(new Vector2(transform.position.x - 5, transform.position.y+0.4f));
        }

    }
    #endregion

    #region 爆破
    public void Boom()
    {
        Debug.Log("Boom");
        PlayerData.Cast = true;
        ChangeState((sbyte)Data.AnimationCount.Cast);

        if (transform.localScale.x > 0)
        {
            FriendCtrl.Instance.ThrowFriend(new Vector2(transform.position.x + 5, 0));
        }
        if (transform.localScale.x < 0)
        {
            FriendCtrl.Instance.ThrowFriend(new Vector2(transform.position.x - 5, 0));
        }

    }
    #endregion

    #region 合体
    public void Fit()
    {
        Debug.Log("合体！！！");
    }
    #endregion

    #region 变化动作的方法
    public void ChangeState(sbyte animationCount)
    {
        fsmManager.ChangeState(animationCount);
    }
    #endregion

    #region 冲刺的方法
    Vector2 tmp;
    public void Blink()
    {
        if (!rightBlink && !leftBlink && !PlayerData.playerJumping)
        {
            tmp = transform.position;
            if (playerR.localScale.x > 0)
            {
                rightBlink = true;
            }
            if (playerR.localScale.x < 0)
            {
                leftBlink = true;
            }
        }
    }
    IEnumerator RightBlink()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(tmp.x + 5, tmp.y), 0.5f);
        yield return new WaitForSeconds(1f);
        rightBlink = false;
    }
    IEnumerator LeftBlink()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(tmp.x - 5, tmp.y), 0.5f);
        yield return new WaitForSeconds(1f);
        leftBlink = false;
    }
    #endregion

    #region 跳跃的方法
    public void StartJump()
    {
        PlayerData.playerStartJump = true;
        ChangeState((sbyte)Data.AnimationCount.Jump);
    }
    #endregion

    #region 协程延迟跳跃
    IEnumerator Jump()
    {
        //ChangeState((sbyte)Data.AnimationCount.Jump);


        //推迟多少时间进行跳跃
        yield return new WaitForSeconds(0.1f);
        PlayerData.playerStartJump = false;
    }
    #endregion

}
