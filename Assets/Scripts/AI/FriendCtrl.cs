using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendCtrl : MonoBehaviour
{
    public static FriendCtrl Instance;

    Animator animator;
    FSMManager fsmManager;


    Rigidbody2D tmpRgb;
    BoxCollider2D friendC;
    //玩家的位置
    Vector2 player;
    //与玩家相距的坐标点
    public Vector2 distanceV = Vector2.zero;
    //将要被扔到的位置
    private Vector2 target;
    //与玩家x相距的距离
    float distance;
    //跟随的速度
    private Vector2 velocity = Vector2.one;
    //计时器
    float timeCount;
    //是否冲刺
    bool blink = false;
    //是否Back
    bool back = false;
    //是否amass
    bool amass = true;
    bool canMove;
    private void Start()
    {
        Instance = this;
        fsmManager = new FSMManager((int)Data.FriendAnimationCount.Max);
        animator = GetComponent<Animator>();
        //tmpRgb = GetComponent<Rigidbody2D>();
        //tmpRgb.gravityScale = 0;
        friendC = GetComponent<BoxCollider2D>();
        friendC.isTrigger = true;



        #region 注册动画
        FriendIdel friendIdel = new FriendIdel(animator);
        fsmManager.AddState(friendIdel);
        FriendMove friendMove = new FriendMove(animator);
        fsmManager.AddState(friendMove);
        FriendAttack friendAttack = new FriendAttack(animator);
        fsmManager.AddState(friendAttack);
        FriendAmass friendAmass = new FriendAmass(animator);
        fsmManager.AddState(friendAmass);
        FriendAmassing friendAmassing = new FriendAmassing(animator);
        fsmManager.AddState(friendAmassing);
        FriendBack friendBack = new FriendBack(animator);
        fsmManager.AddState(friendBack);
        FriendCast friendCast = new FriendCast(animator);
        fsmManager.AddState(friendCast);
        #endregion

    }
    float ran;
    private void Update()
    {
        fsmManager.OnStay();
        player = AIManager.Instance.Player.transform.position;
        PlayerData.distance = Vector2.Distance(player, transform.position);
        distance = PlayerCtrl.Instance.transform.position.x - transform.position.x;
        timeCount += Time.deltaTime;

        #region 进行动作的前置条件
        //todo
        #endregion

        #region 巡逻
        
        if (FriendData.Biging)
        {
            if (timeCount>3&& !FriendData.Attacking)
            {
                timeCount = 0;
                ran = Random.Range(-5, 5);
                Debug.Log(ran);
                //StartCoroutine("Patrol");
            }
            StartCoroutine("Patrol");
        }
        else
        {
            if (!FriendData.Casting&&!FriendData.Amassing&&!FriendData.Backing)
            {
                #region 跟随
                if (PlayerData.distance > FriendData.followDistance)
                {
                    if (distance > 0)
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    }
                    if (distance < 0)
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    }
                    Debug.Log("跟随中");
                    transform.position = Vector2.SmoothDamp(transform.position, player + distanceV, ref velocity, FriendData.smoothTime);
                }
                else if (PlayerData.distance > FriendData.followDistance * 2)
                {
                    if (PlayerData.Dircetion > 0)
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    }
                    if (PlayerData.Dircetion < 0)
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    }
                    transform.position = Vector2.SmoothDamp(transform.position, player + distanceV, ref velocity, FriendData.smoothTime);
                }
                #endregion
            }
        }
        #endregion

        #region 回到Idel的方法
        //Debug.Log(!FriendData.Attacking);
        //Debug.Log(!FriendData.Amassing);
        //Debug.Log(!FriendData.Backing);
        //Debug.Log(!FriendData.Casting);
        if (!FriendData.Attacking && !FriendData.Amassing && !FriendData.Backing && !FriendData.Casting)
        {
            ChangeState((sbyte)Data.FriendAnimationCount.Idel);
        }
        #endregion

        #region 被投掷的方法
        if (blink)
        {
            StartCoroutine("Blink");
        }
        //添加和销毁刚体
        if (FriendData.Casting)
        {
            StartCoroutine("Rigi");
        }
        if (FriendData.Back)
        {
            friendC.isTrigger = true;
            friendC.size = new Vector2(0.47f, 0.2f);
            friendC.offset = new Vector2(0, 0);
            Destroy(GetComponent<Rigidbody2D>());
            tmpRgb = null;

            back = true;
        }

        if (back)
        {
            StartCoroutine("Back");
        }
        #endregion
    }

    #region 蓄力
    public void Amass()
    {
        GoToPlayer();
        if (!FriendData.Amassing)
        {
            FriendData.Amass = true;
            ChangeState((sbyte)Data.FriendAnimationCount.Amass);
        }
        else
        {
            ChangeState((sbyte)Data.FriendAnimationCount.Amassing);
        }
    }
    #endregion

    #region 冲刺
    IEnumerator Blink()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, 0.2f);
        yield return new WaitForSeconds(1f);
        blink = false;
    }
    #endregion

    #region Back
    IEnumerator Back()
    {
        GoToPlayer();
        ChangeState((sbyte)Data.FriendAnimationCount.Back);
        yield return new WaitForSeconds(1f);
        back = false;
    }
    #endregion

    #region 合体
    public void Fit()
    {

    }
    #endregion

    #region 被扔出去
    public void ThrowFriend(Vector2 target)
    {
        FriendData.Cast = true;
        this.target = target;
        ChangeState((sbyte)Data.FriendAnimationCount.Cast);
        blink = true;
        StartCoroutine("BigTime");
    }
    IEnumerator Rigi()
    {
        yield return new WaitForSeconds(0.2f);
        friendC.isTrigger = false;
        friendC.size = new Vector2(1.8f, 0.8f);
        friendC.offset = new Vector2(0, -0.18f);
        if (tmpRgb == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
            tmpRgb = GetComponent<Rigidbody2D>();
        }
        tmpRgb.mass = 100;
        tmpRgb.gravityScale = 100;
    }
    #endregion

    #region 被召唤到玩家位置
    public void GoToPlayer()
    {
        distance = player.x - transform.position.x;
        //if (distance>0)
        //{
        //    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        //}
        //if (distance<0)
        //{
        //    transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        //}
        if (PlayerData.Dircetion > 0)
        {
            distanceV = new Vector2(-0.25f, -0.27f);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (PlayerData.Dircetion < 0)
        {
            distanceV = new Vector2(0.25f, -0.27f);
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        FriendData.Moving = true;
        transform.position = Vector2.SmoothDamp(transform.position, player + distanceV, ref velocity, FriendData.comeTime);
    }
    #endregion

    #region 巡逻
    IEnumerator Patrol()
    {
        if (ran > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (ran < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + ran, transform.position.y), 0.5f);
        yield return new WaitForSeconds(1.5f);
        FriendData.Moving = false;
    }
    #endregion

    #region 变大的总时长
    IEnumerator BigTime()
    {
        yield return new WaitForSeconds(4f);
        FriendData.Back = true;
    }
    #endregion

    public void ChangeState(sbyte animationCount)
    {
        fsmManager.ChangeState(animationCount);
    }

}
