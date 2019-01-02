using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendCtrl : MonoBehaviour
{
    public static FriendCtrl Instance;

    Animator animator;
    FSMManager fsmManager;


    Rigidbody2D tmpRgb;
    CircleCollider2D friendC;
    //玩家的位置
    Vector2 player;
    //与玩家相距的坐标点
    public Vector2 distanceV=Vector2.zero;
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
    bool canMove;
    private void Start()
    {
        Instance = this;
        fsmManager = new FSMManager((int)Data.FriendAnimationCount.Max);
        animator = GetComponent<Animator>();
        //tmpRgb = GetComponent<Rigidbody2D>();
        //tmpRgb.gravityScale = 0;
        friendC = GetComponent<CircleCollider2D>();
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
    private void Update()
    {
        fsmManager.OnStay();
        player = AIManager.Instance.Player.transform.position;
        PlayerData.distance = Vector2.Distance(player, transform.position);
        timeCount += Time.deltaTime;


        #region 进行动作的前置条件
        //todo
        #endregion

        #region 跟随的方法
        if (PlayerData.distance > FriendData.followDistance&&!FriendData.Moving)
        {
            FriendData.Moving = true;
            if (PlayerData.Dircetion>0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            if (PlayerData.Dircetion < 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            transform.position = Vector2.SmoothDamp(transform.position, player + distanceV, ref velocity, FriendData.smoothTime);
        }
        if (PlayerData.distance > FriendData.followDistance*2)
        {
            FriendData.Moving = true;
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

        #region 巡逻的方法
        if (!FriendData.Moving)
        {
            StartCoroutine("Ran");
            StartCoroutine("Patrol");
        }
        #endregion

        if (FriendData.Casting&&tmpRgb!=null)
        {
            Debug.Log("Cast");
            StartCoroutine("Rigi");
        }
        if (FriendData.Backing)
        {
            friendC.isTrigger = true;
            Destroy(tmpRgb);
            tmpRgb = null;
        }

        if (FriendData.Cast)
        {
            ChangeState((sbyte)Data.FriendAnimationCount.Cast);
            blink = true;

        }
        if (blink)
        {
            StartCoroutine("Blink");
        }
    }

    #region 蓄力
    public void Amass()
    {
        ChangeState((sbyte)Data.FriendAnimationCount.Amass);
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

    #region 合体
    public void Fit()
    {

    }
    #endregion

    #region 扔召唤兽
    public void ThrowFriend(Vector2 target)
    {
        FriendData.Cast = true;
        this.target = target;
    }
    IEnumerator Rigi()
    {
        yield return new WaitForSeconds(0.2f);
        friendC.isTrigger = false;
        tmpRgb= gameObject.AddComponent<Rigidbody2D>();
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
        if (PlayerData.Dircetion>0)
        {
            distanceV = new Vector2(-0.25f, -0.27f);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (PlayerData.Dircetion<0)
        {
            distanceV = new Vector2(0.25f, -0.27f);
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        Debug.Log("对碰到的敌人造成伤害");
        FriendData.Moving = true;
        transform.position = Vector2.SmoothDamp(transform.position, player + distanceV, ref velocity, FriendData.comeTime);
    }
    #endregion

    #region 巡逻
    int ran;
    bool random;
    IEnumerator Ran()
    {
        if (!random)
        {
            random = true;
            ran = Random.Range(-5, 5);
        }
        yield return new WaitForSeconds(1);
        random = false;
    }
    IEnumerator Patrol()
    {
        if (ran>0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (ran<0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + ran, transform.position.y), 0.5f);
        yield return new WaitForSeconds(1.5f);
        FriendData.Moving = false;
    }
    #endregion

    public void ChangeState(sbyte animationCount)
    {
        fsmManager.ChangeState(animationCount);
    }

}
