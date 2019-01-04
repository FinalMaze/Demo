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

    #region 临时数据 
    //巡逻目标点
    Vector2 tmpVec;
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
    //上次记录的时间
    float lastTime;
    //是否冲刺
    bool blink = false;
    //是否Back
    bool back = false;
    //是否amass
    bool amass = true;
    bool canMove = false;
    bool canPartol = false;

    #endregion

    private void Awake()
    {
        #region 初始化
        Instance = this;
        fsmManager = new FSMManager((int)Data.FriendAnimationCount.Max);
        animator = GetComponent<Animator>();
        friendC = GetComponent<BoxCollider2D>();
        #endregion

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
        FriendIdel2 friendIdel2 = new FriendIdel2(animator);
        fsmManager.AddState(friendIdel2);
        FriendRun2 friendRun2 = new FriendRun2(animator);
        fsmManager.AddState(friendRun2);
        #endregion

    }
    float ran;
    private void Update()
    {
        fsmManager.OnStay();
        player = PlayerCtrl.Instance.transform.position;
        distance = PlayerCtrl.Instance.transform.position.x - transform.position.x;


        #region 巡逻与跟随
        if (!blink)
        {
            Partol();
        }
        #endregion

        #region 回到Idel的方法
        //Debug.Log(!FriendData.Attacking);
        //Debug.Log(!FriendData.Amassing);
        //Debug.Log(!FriendData.Backing);
        //Debug.Log(!FriendData.Casting);
        if (FriendData.Biging)
        {
            if (!FriendData.Attacking && !FriendData.Backing && !FriendData.Casting && !FriendData.Runing)
            {
                //Debug.Log("强制Idel2");
                ChangeState((sbyte)Data.FriendAnimationCount.Idel2);
            }
        }
        else
        {
            if (!FriendData.Attacking && !FriendData.Amassing && !FriendData.Backing && !FriendData.Casting && !FriendData.Biging)
            {
                //Debug.Log("强制Idel1");
                ChangeState((sbyte)Data.FriendAnimationCount.Idel);
            }
        }
        #endregion


        #region 被投掷的位移
        if (blink)
        {
            StartCoroutine("Blink");
        }
        #endregion

        #region 判断什么时候变小
        if (FriendData.Biging)
        {
            timeCount += Time.deltaTime;
            if (timeCount > FriendData.BigTime)
            {
                timeCount = 0;
                Small();
            }
        }
        else
        {
            timeCount = 0;
        }
        #endregion

        //#region 判断什么时候召回
        //Back();
        //#endregion
    }

    #region 蓄力
    public void Amass()
    {
        GoToPlayer(0.01f);
        if (!FriendData.Amassing)
        {
            FriendData.Amass = true;
            ChangeState((sbyte)Data.FriendAnimationCount.Amass);
        }
        ChangeState((sbyte)Data.FriendAnimationCount.Amassing);
    }
    #endregion

    #region 变小和召回

    #region 变小
    private void Small()
    {
        DelRigibody();
        ChangeState((sbyte)Data.FriendAnimationCount.Idel);
    }
    #endregion

    #region 召回
    public void Back()
    {
        ChangeState((sbyte)Data.FriendAnimationCount.Back);
        GoToPlayer();
    }
    #endregion

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
        if (animator.GetInteger("Index") != 4)
        {
            ChangeState((sbyte)Data.FriendAnimationCount.Amassing);
            #region 移动到玩家手的位置
            if (PlayerData.Dircetion > 0)
            {
                if (back)
                {
                    distanceV = Vector2.zero;
                }
                else
                {
                    distanceV = new Vector2(-0.25f, -0.27f);
                }
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            if (PlayerData.Dircetion < 0)
            {
                if (back)
                {
                    distanceV = Vector2.zero;
                }
                else
                {
                    distanceV = new Vector2(0.25f, -0.27f);
                }
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            transform.position = player + distanceV;

            #endregion
        }
        else
        {
            ChangeState((sbyte)Data.FriendAnimationCount.Cast);
        }
        blink = true;
    }



    IEnumerator Blink()
    {
        if (target.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (target.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        transform.position = Vector2.MoveTowards(transform.position, target, 0.2f);
        yield return new WaitForSeconds(0.5f);
        blink = false;
    }
    #endregion

    #region 添加和销毁刚体

    public void RigibodyCtrl()
    {
        if (FriendData.AddRigibody)
        {
            AddRigibody();
        }
        if (FriendData.DelRigibody)
        {
            DelRigibody();
        }
    }

    //添加刚体
    private void AddRigibody()
    {
        friendC.size = new Vector2(1.8f, 0.8f);
        friendC.offset = new Vector2(0, -0.3f);

        if (tmpRgb == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
            tmpRgb = GetComponent<Rigidbody2D>();
        }
        tmpRgb.freezeRotation = true;
        tmpRgb.mass = 100;
        tmpRgb.gravityScale = 100;
    }

    //销毁刚体
    private void DelRigibody()
    {
        friendC.size = new Vector2(0.47f, 0.2f);
        friendC.offset = new Vector2(0, 0);
        Destroy(GetComponent<Rigidbody2D>());
        tmpRgb = null;
        //yield return new WaitForSeconds(0.5f);
        //canPartol = false;
    }
    #endregion

    #region 被召唤到玩家位置
    public void GoToPlayer(float timeRatio = 1)
    {
        if (FriendData.Backing)
        {
            if (distance > 0)
            {
                if (back)
                {
                    distanceV = Vector2.zero;
                }
                else
                {
                    distanceV = new Vector2(-0.25f, -0.27f);
                }
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            if (distance < 0)
            {
                if (back)
                {
                    distanceV = Vector2.zero;
                }
                else
                {
                    distanceV = new Vector2(0.25f, -0.27f);
                }
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }

        }
        else
        {
            if (PlayerData.Dircetion > 0)
            {
                if (back)
                {
                    distanceV = Vector2.zero;
                }
                else
                {
                    distanceV = new Vector2(-0.25f, -0.27f);
                }
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            if (PlayerData.Dircetion < 0)
            {
                if (back)
                {
                    distanceV = Vector2.zero;
                }
                else
                {
                    distanceV = new Vector2(0.25f, -0.27f);
                }
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
        }
        transform.position = Vector2.SmoothDamp(transform.position, player + distanceV, ref velocity, FriendData.comeTime * timeRatio);
    }
    #endregion

    #region 巡逻
    private void Partol()
    {
        //Debug.Log(FriendData.Biging);
        if (FriendData.Biging)
        {
            if (Time.time - lastTime > FriendData.PartolTime && !FriendData.Attacking)
            {
                lastTime = Time.time;
                ran = Random.Range(-5, 5);
                if (ran < 0)
                {
                    ran = Mathf.Clamp(ran, -5, -3);
                }
                if (ran > 0)
                {
                    ran = Mathf.Clamp(ran, 3, 5);
                }
                tmpVec = new Vector2(transform.position.x + ran, transform.position.y);

            }
            if (!FriendData.Backing)
            {
                StartCoroutine("IEPatrol");
            }
        }
        else
        {
            //Debug.Log(!FriendData.Amassing);
            //Debug.Log(!back);
            if (!FriendData.Casting && !FriendData.Amassing && !FriendData.Backing)
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
                    transform.position = Vector2.SmoothDamp(transform.position, player + distanceV, ref velocity, FriendData.smoothTime * 0.5f);
                }
                #endregion
            }
        }

    }

    IEnumerator IEPatrol()
    {
        if (ran > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (ran < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        if (canPartol)
        {
            transform.position = Vector2.MoveTowards(transform.position, tmpVec, 0.08f);
            if (Vector2.Distance(transform.position, tmpVec) != 0f)
            {
                //Debug.Log("巡逻中");
                ChangeState((sbyte)Data.FriendAnimationCount.Run2);
            }
            else
            {
                ChangeState((sbyte)Data.FriendAnimationCount.Idel2);
            }
        }
        yield return new WaitForSeconds(0.4f);
        canPartol = true;
    }
    #endregion

    public void ChangeState(sbyte animationCount)
    {
        fsmManager.ChangeState(animationCount);
    }

}
