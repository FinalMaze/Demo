using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : EnemyCtrl
{
    FSMManager fsmManager;
    Animator animator;
    public new EnemyData enemyData;
    SpriteRenderer sprite;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        enemyData = new EnemyData();
        enemyData.HP = 30;
        enemyData.MaxHP = 30;

        fsmManager = new FSMManager((int)Data.EnemyAnimationCount.Max);
        sprite = GetComponentInChildren<SpriteRenderer>();

        sprite.color = new Color(Data.Rm / 255f, Data.Gm / 255f, Data.Bm / 255f, Data.Am / 255f);

        ball = transform.Find("FirePostion");
        tmpBall = Resources.Load("Prefabs/Ball") as GameObject;
        InvokeRepeating("RandomPos", 2, 2);

        #region 注册动画
        EnemyIdel enemyIdel = new EnemyIdel(animator);
        fsmManager.AddState(enemyIdel);
        EnemyWalk enemyWalk = new EnemyWalk(animator);
        fsmManager.AddState(enemyWalk);
        EnemyAttack enemyAttack = new EnemyAttack(animator, enemyData);
        fsmManager.AddState(enemyAttack);
        EnemyHurt enemyHurt = new EnemyHurt(animator, enemyData);
        fsmManager.AddState(enemyHurt);
        EnemyDie enemyDie = new EnemyDie(animator,ref enemyData,this);
        fsmManager.AddState(enemyDie);
        EnemyAttack2 enemyAttack2 = new EnemyAttack2(animator, enemyData);
        fsmManager.AddState(enemyAttack2);
        EnemySummon EnemySummon = new EnemySummon(animator, ref enemyData);
        fsmManager.AddState(EnemySummon);
        #endregion
    }
    private void Start()
    {
        ChangeState((sbyte)Data.EnemyAnimationCount.Summon);
    }
    float distance;
    float direction;
    private void Update()
    {
        fsmManager.OnStay();
        distance = Mathf.Abs( PlayerCtrl.Instance.transform.position.x- transform.position.x);
        direction = PlayerCtrl.Instance.transform.position.x - transform.position.x;

        #region 强制进入Idel
        //Debug.Log(enemyData.Hurting);
        //Debug.Log(enemyData.Attacking);
        //Debug.Log(enemyData.Attacking2);
        //Debug.Log(enemyData.Die);
        if (!enemyData.Attacking&&!enemyData.Attacking2 && !enemyData.Hurting && !enemyData.Die
            &&!enemyData.Summoning)
        {
            ChangeState((sbyte)Data.EnemyAnimationCount.Idel);
        }
        #endregion
        EnemyAI();

        if (enemyData.Hurting)
        {
            Blink(enemyData.HurtDistance, enemyData.HurtSpeed);
        }

    }

    #region 巡逻
    bool canPartol;
    float attackTimeCount;
    float attack2TimeCount;
    bool canAttack=true;
    bool canLongAttack = true;
    public new void Patrol()
    {
        if (!enemyData.Attacking&&!enemyData.Attacking2 && !enemyData.Hurting && !enemyData.Die
            && !enemyData.Summoning)
        {
            //如果大于怪物的可跟随距离，进行巡逻
            if (distance > enemyData.FllowDistance)
            {
                if (ran > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                if (ran < 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                if (canPartol)
                {
                    transform.position = Vector2.MoveTowards(transform.position, tmpVec, enemyData.MoveSpeed);
                    if (Vector2.Distance(transform.position, tmpVec) >= 0.01f)
                    {
                        ChangeState((sbyte)Data.EnemyAnimationCount.Walk);
                    }
                    else
                    {
                        canPartol = false;
                        ChangeState((sbyte)Data.EnemyAnimationCount.Idel);
                    }
                }
                if (distance > enemyData.FllowDistance*1.3f)
                {
                    canLongAttack = true;
                }
            }
            //跟随并攻击
            else
            {
                if (direction > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                if (direction < 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }

                if (distance > enemyData.AttackDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, PlayerCtrl.Instance.transform.position, enemyData.MoveSpeed);
                    ChangeState((sbyte)Data.EnemyAnimationCount.Walk);
                }
                if (distance > enemyData.AttackDistance*1.5f)
                {
                    canAttack = true;
                    if (canLongAttack)
                    {
                        canLongAttack = false;
                        Attack2();
                    }
                    attack2TimeCount += Time.deltaTime;
                    if (attack2TimeCount > enemyData.AttackCD)
                    {
                        attack2TimeCount = 0;
                        Attack2();
                    }

                }
                else if (distance <= enemyData.AttackDistance)
                {
                    //if (canAttack)
                    //{
                    //    canAttack = false;
                    //    Attack();
                    //}
                    attackTimeCount += Time.deltaTime;
                    if (attackTimeCount > enemyData.AttackCD+1)
                    {
                        attackTimeCount = 0;
                        Attack();
                    }
                }
            }
        }
    }

    int ran;
    Vector2 tmpVec;
    public new void RandomPos()
    {
        if (distance > enemyData.FllowDistance)
        {
            ran = Random.Range(-5, 5);
            if (ran < 0)
            {
                ran = Mathf.Clamp(ran, -5, -3);
                tmpVec.x = transform.position.x + ran;
                tmpVec.y = transform.position.y;
            }
            if (ran > 0)
            {
                ran = Mathf.Clamp(ran, 3, 5);
                tmpVec.x = transform.position.x + ran;
                tmpVec.y = transform.position.y;
            }
            canPartol = true;
        }
    }


    #endregion

    #region 怪物行为
    public new void EnemyAI()
    {
        if (!enemyData.Die)
        {
            Patrol();
        }

    }
    #endregion

    #region 攻击
    //近战攻击 
    public new void Attack()
    {
        if (!enemyData.Attacking&&!enemyData.Attacking2&&!enemyData.Hurting&&!enemyData.Die
            && !enemyData.Summoning)
        {
            ChangeState((sbyte)Data.EnemyAnimationCount.Attack);
        }
        if (enemyData.Attacking&&enemyData.HP!=0)
        {
            Invoke("Damage", enemyData.PlayerHurtTime);
        }
    }
    //远程攻击
    Transform ball;
    GameObject tmpBall;
    public new void Attack2()
    {
        
    }
    private void BuildBall()
    {
        Instantiate(tmpBall, ball.position, ball.rotation);
    }
    private void Damage()
    {
        if (distance<3f)
        {
            if (PlayerCtrl.Instance.transform.position.x>transform.position.x)
            {
                PlayerCtrl.Instance.Hurt(enemyData.Damage,1);
            }
            if (PlayerCtrl.Instance.transform.position.x < transform.position.x)
            {
                PlayerCtrl.Instance.Hurt(enemyData.Damage, -1);
            }

        }
    }
    #endregion

    #region 受击
    float dir;
    float tmpAdd=0;
    public new void Hurt(float reduceHP,float tmpDir,float addDistance=0)
    {
        dir = tmpDir;
        tmpAdd = addDistance;
        enemyData.HP = Mathf.Clamp(enemyData.HP -= reduceHP, 0, enemyData.MaxHP);
        if (enemyData.HP > 0)
        {
            StartCoroutine("Red");
            if (!enemyData.Attacking || addDistance != 0)
            {
                ChangeState((sbyte)Data.EnemyAnimationCount.Hurt);
            }
        }
        else
        {
            ChangeState((sbyte)Data.EnemyAnimationCount.Die);
            Invoke("Destory", 0.64f);
        }
    }
    IEnumerator Red()
    {
        sprite.color = new Color(Data.mR / 255f, Data.mG / 255f, Data.mB / 255f, Data.mA / 255f);
        yield return new WaitForSeconds(Data.IdelRGB);
        sprite.color = new Color(Data.Rm / 255f, Data.Gm / 255f, Data.Bm / 255f, Data.Am / 255f);
    }
    #endregion

    #region 冲刺的方法
    public new void Blink(float distance, float speed)
    {
        if (dir > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2((transform.position.x + distance+ tmpAdd), transform.position.y), speed);
        }
        if (dir < 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2((transform.position.x - distance- tmpAdd), transform.position.y), speed);
        }
    }
    #endregion

    #region 死亡
    public new void Destory()
    {
        AIManager.Instance.DelEnemy(gameObject);
    }
    #endregion

    #region 变换动作的方法
    void ChangeState(sbyte animatorCount)
    {
        fsmManager.ChangeState(animatorCount);
    }
    #endregion
}
