using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendPlayerCtrl : MonoBehaviour
{
    public static FriendPlayerCtrl Instance;

    Animator animator;
    FSMManager fsmManager;


    Rigidbody2D tmpRgb;
    BoxCollider2D friendC;

    #region 临时变量
    //巡逻目标点
    Vector2 tmpVec;
    //玩家的位置
    Vector2 player;
    //玩家手的位置
    public Vector2 distanceV = Vector2.zero;
    //与玩家x相距的距离
    float distance;
    //宠物的朝向
    float direction;
    //跟随的速度
    private Vector2 velocity = Vector2.one;
    //计时器
    float timeCount;
    //跳跃计时器
    float jumpTimeCount;
    //是否Back
    bool back = false;
    //是否可以巡逻
    bool canPartol = false;
    //巡逻用的随机数
    float ran;
    float run = 0;
    float up = 0;
    float moveSpeed;
    #endregion

    private void Awake()
    {
        #region 初始化
        Instance = this;
        fsmManager = new FSMManager((int)Data.FriendAnimationCount.Max);
        animator = GetComponentInChildren<Animator>();
        friendC = GetComponent<BoxCollider2D>();
        moveSpeed = FriendData.MoveSpeed;
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
        FriendBlow FriendBlow = new FriendBlow(animator);
        fsmManager.AddState(FriendBlow);
        FriendAttack2 friendAttack2 = new FriendAttack2(animator);
        fsmManager.AddState(friendAttack2);
        FriendRunAttack FriendRunAttack = new FriendRunAttack(animator);
        fsmManager.AddState(FriendRunAttack);

        #endregion

    }
    private void Update()
    {
        fsmManager.OnStay();
        player = PlayerCtrl.Instance.transform.position;
        direction = transform.rotation.y;
        distance = PlayerCtrl.Instance.transform.position.x - transform.position.x;
        PlayerData.distance = Vector2.Distance(PlayerCtrl.Instance.transform.position, transform.position);



        #region 检测什么时候被踩
        if (FriendData.Jumped)
        {
            StartCoroutine("Jumped");
        }
        #endregion

        #region 大型和小型的加减蓝
        Partol();
        #endregion

        #region 回到Idel的方法
        //Debug.Log(!FriendData.Attacking);
        //Debug.Log(!FriendData.Amassing);
        //Debug.Log(!FriendData.Backing);
        //Debug.Log(!FriendData.Casting);
        if (FriendData.Biging)
        {
            if (!FriendData.Attacking && !FriendData.Backing && !FriendData.Casting
                && !FriendData.Amassing && !FriendData.Smalling && !FriendData.Blowing
                &&!FriendData.RunAttacking)
            {

                ChangeState((sbyte)Data.FriendAnimationCount.Idel2);
            }
        }
        else
        {
            if (!FriendData.Attacking && !FriendData.Blowing && !FriendData.Amassing && !FriendData.Backing && !FriendData.Casting && !FriendData.Biging && !FriendData.Moving)
            {
                //Debug.Log("强制Idel1");
                ChangeState((sbyte)Data.FriendAnimationCount.Idel);
            }
        }
        #endregion
        Ctrl();
        Move();

    }

    private void Ctrl()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if ( !FriendData.Backing && !FriendData.Casting&& !FriendData.Amassing && !FriendData.Blowing
                && !FriendData.RunAttacking)
            {
                if (FriendData.Biging )
                {
                    Attack();
                }
                if (FriendData.Smalling)
                {
                    if (PlayerData.mp > FriendData.AttackMP)
                    {
                        PlayerData.mp -= FriendData.AttackMP;
                        GameInterfaceCtrl.Instance.UpdateMP();
                        ChangeState((sbyte)Data.FriendAnimationCount.Attack2);
                        Invoke("Damage", FriendData.EnemyHurtTime);
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!FriendData.Backing && !FriendData.Casting && !FriendData.Amassing && !FriendData.Blowing)
            {
                if (FriendData.Biging)
                {
                    Back();
                }
                if (FriendData.Smalling)
                {
                    ChangeState((sbyte)Data.FriendAnimationCount.Move);
                }
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            run = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            run = 1;
        }
        else
        {
            run = 0;
        }
    }

    private void Move()
    {
        if (!FriendData.Attacking && !FriendData.Backing && !FriendData.Casting
    && !FriendData.Amassing && !FriendData.Blowing&&!FriendData.RunAttacking)
        {
            if (run == 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                if (FriendData.Biging)
                {
                    ChangeState((sbyte)Data.FriendAnimationCount.Run2);
                }
                transform.Translate(transform.right * moveSpeed * Time.deltaTime);
            }
            if (run == -1)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                if (FriendData.Biging)
                {
                    ChangeState((sbyte)Data.FriendAnimationCount.Run2);
                }
                transform.Translate(-transform.right * moveSpeed * Time.deltaTime);
            }
        }
        if (!FriendData.Biging)
        {
            if (up == 1)
            {
                transform.Translate(transform.up * moveSpeed * Time.deltaTime * 0.5f);
            }
            if (up == -1)
            {
                transform.Translate(-transform.up * moveSpeed * Time.deltaTime * 0.5f);
            }
        }
    }

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
    }


    #endregion

    #endregion

    #region 攻击
    public void Attack()
    {
        if (PlayerData.mp>FriendData.AttackMP)
        {
            PlayerData.mp -= FriendData.AttackMP;
            GameInterfaceCtrl.Instance.UpdateMP();

            ChangeState((sbyte)Data.FriendAnimationCount.Attack);
            Invoke("Damage", FriendData.EnemyHurtTime);
        }
    }
    Rigidbody2D baseA;
    EnemyEffectCtrl tmpA;
    public void Damage()
    {
        for (int i = 0; i < Data.allEnemy.Count; i++)
        {
            if (Mathf.Abs(Data.allEnemy[i].transform.position.x - transform.position.x) < FriendData.AttackDistance)
            {
                if (Data.allEnemy[i].transform.position.x > transform.position.x)
                {
                    EnemyTest tmp = Data.allEnemy[i].GetComponent<EnemyTest>();
                    if (tmp != null)
                    {
                        tmp.Hurt(FriendData.Damage, 1);
                        baseA = tmp.GetComponentInParent<Rigidbody2D>();
                        tmpA = baseA.GetComponentInChildren<EnemyEffectCtrl>();
                        tmpA.ChangeState((sbyte)Data.EnemyEffect.Hurt);
                    }
                    else
                    {
                        Data.allEnemy[i].GetComponent<EnemyCtrl>().Hurt(FriendData.Damage, 1);
                        baseA = Data.allEnemy[i].GetComponent<EnemyCtrl>().GetComponentInParent<Rigidbody2D>();
                        tmpA = baseA.GetComponentInChildren<EnemyEffectCtrl>();
                        tmpA.ChangeState((sbyte)Data.EnemyEffect.Hurt);
                    }
                }
                if (Data.allEnemy[i].transform.position.x < transform.position.x)
                {
                    EnemyTest tmp = Data.allEnemy[i].GetComponent<EnemyTest>();
                    if (tmp != null)
                    {
                        tmp.Hurt(FriendData.Damage, -1);
                        baseA = tmp.GetComponentInParent<Rigidbody2D>();
                        tmpA = baseA.GetComponentInChildren<EnemyEffectCtrl>();
                        tmpA.ChangeState((sbyte)Data.EnemyEffect.Hurt);
                    }
                    else
                    {
                        Data.allEnemy[i].GetComponent<EnemyCtrl>().Hurt(FriendData.Damage, -1);
                        baseA = Data.allEnemy[i].GetComponent<EnemyCtrl>().GetComponentInParent<Rigidbody2D>();
                        tmpA = baseA.GetComponentInChildren<EnemyEffectCtrl>();
                        tmpA.ChangeState((sbyte)Data.EnemyEffect.Hurt);
                    }
                }
            }
        }
    }

    //检测攻击距离内是否有敌人
    public Transform CheckEnemy(float distance)
    {
        for (int i = 0; i < Data.allEnemy.Count; i++)
        {
            if (Mathf.Abs(Data.allEnemy[i].transform.position.x - transform.position.x) < distance)
            {
                return Data.allEnemy[i].transform;
            }
        }
        return null;
    }
    #endregion

    #region 冲刺攻击
    public void RunAttack(Vector2 target)
    {
        FriendData.Target = target;
        FriendData.StartPos = transform.position;
        ChangeState((sbyte)Data.FriendAnimationCount.RunAttack);
    }
    #endregion

    #region 被扔出去
    public void ThrowFriend(Vector2 target)
    {
        FriendData.Cast = true;
        FriendData.Target = target;
        FriendData.StartPos = transform.position;
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
    }

    #endregion

    #region 添加和销毁刚体

    public void RigibodyCtrl()
    {
        if (FriendData.AddRigibody)
        {
            FriendData.AddRigibody = false;
            AddRigibody();
        }
        if (FriendData.DelRigibody)
        {
            FriendData.DelRigibody = false;
            DelRigibody();
        }
    }

    //添加刚体
    private void AddRigibody()
    {
        friendC.size = new Vector2(1.8f, 0.8f);
        friendC.offset = new Vector2(0, -0.3f);
        friendC.isTrigger = false;

        if (tmpRgb == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
            tmpRgb = GetComponent<Rigidbody2D>();
        }
        tmpRgb.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Material/Friend");
        tmpRgb.freezeRotation = true;
        //tmpRgb.mass = 100;
        tmpRgb.gravityScale = 3;
    }

    //销毁刚体
    private void DelRigibody()
    {
        friendC.size = new Vector2(0.47f, 0.2f);
        friendC.offset = new Vector2(0, 0);
        friendC.isTrigger = true;

        Destroy(GetComponent<Rigidbody2D>());
        tmpRgb = null;
    }
    #endregion

    #region 冲刺   重写
    public void Blink(float distance, float speed)
    {
        if (transform.rotation.y < 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2((transform.position.x - distance), transform.position.y), speed);
        }
        if (transform.rotation.y == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2((transform.position.x + distance), transform.position.y), speed);
        }
    }
    public void GoToPlayer(Vector2 playerPostion, float timeRatio = 1, float x = 0.25f, float y = -0.27f)
    {
        if (FriendData.Backing)
        {
            if (playerPostion.x - transform.position.x > 0)
            {
                if (back)
                {
                    distanceV = Vector2.zero;
                }
                else
                {
                    distanceV = new Vector2(-x, y);
                }
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            if (playerPostion.x - transform.position.x < 0)
            {
                if (back)
                {
                    distanceV = Vector2.zero;
                }
                else
                {
                    distanceV = new Vector2(x, y);
                }
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

        }
        else
        {
            if (playerPostion.x - transform.position.x > 0)
            {
                distanceV = Vector2.zero;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (playerPostion.x - transform.position.x < 0)
            {
                distanceV = Vector2.zero;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        transform.position = Vector2.SmoothDamp(transform.position, playerPostion + distanceV, ref velocity, FriendData.comeTime * timeRatio);
    }
    #endregion

    #region 加减蓝
    float bigMPTimeCount;
    float smallMPTimeCount;
    private void Partol()
    {
        if (FriendData.Biging)
        {
            //大型时持续耗蓝
            bigMPTimeCount += Time.deltaTime;
            if (bigMPTimeCount > FriendData.UpdataTime * 1.6f)
            {
                bigMPTimeCount = 0;
                PlayerData.mp -= FriendData.BigMP;
                GameInterfaceCtrl.Instance.UpdateMP();
                if (PlayerData.mp <= 0)
                {
                    Small();
                }
                if (Vector2.Distance(transform.position, PlayerCtrl.Instance.transform.position) > FriendData.BigToSmallDistance)
                {
                    Small();
                }
            }
        }
        else
        {
            //小型时持续 回蓝
            //smallMPTimeCount += Time.deltaTime;
            //if (smallMPTimeCount > FriendData.UpdataTime * 1.6f)
            //{
            //    smallMPTimeCount = 0;
            //    PlayerData.mp += FriendData.SmallMP;
            //    GameInterfaceCtrl.Instance.UpdateMP();
            //}
            if (!FriendData.Casting && !FriendData.Amassing && !FriendData.Backing && run == 0)
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
                    //Debug.Log("跟随中");
                    transform.position = Vector2.SmoothDamp(transform.position, player + distanceV, ref velocity, FriendData.smoothTime * 0.5f);
                }
                transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, player.y + 0.3f), PlayerData.distance * 0.001f);
                #endregion
            }

        }
    }


    #endregion

    #region 二段跳被踩
    IEnumerator Jumped()
    {
        transform.position = Vector2.Lerp(transform.position, FriendData.Jump2Target, 0.8f);
        yield return new WaitForSeconds(0.1f);
        transform.position = Vector2.SmoothDamp(transform.position,
            new Vector2(FriendData.Jump2Target.x, FriendData.Jump2Target.y + FriendData.Jump2TargetY), ref velocity, 0.2f);
        yield return new WaitForSeconds(0.2f);
        FriendData.Jumped = false;
    }
    #endregion

    public void ChangeState(sbyte animationCount)
    {
        fsmManager.ChangeState(animationCount);
    }

}
