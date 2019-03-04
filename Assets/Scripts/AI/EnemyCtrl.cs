using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    FSMManager fsmManager;
    Animator animator;
    public  EnemyData enemyData;
    SpriteRenderer sprite;
    EnemyEffectCtrl enemyEffect;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        enemyData = new EnemyData();
        fsmManager = new FSMManager((int)Data.EnemyAnimationCount.Max);
        sprite = GetComponentInChildren<SpriteRenderer>();

        sprite.color = new Color(Data.Rb / 255f, Data.Gb / 255f, Data.Bb / 255f, Data.Ab / 255f);

        ball = transform.Find("FirePostion");
        tmpBall = Resources.Load("Prefabs/Ball") as GameObject;
        InvokeRepeating("RandomPos", 2, 2);
        enemyEffect = GetComponentInChildren<EnemyEffectCtrl>();

        #region 注册动画
        EnemyIdel enemyIdel = new EnemyIdel(animator);
        fsmManager.AddState(enemyIdel);
        EnemyWalk enemyWalk = new EnemyWalk(animator);
        fsmManager.AddState(enemyWalk);
        EnemyAttack enemyAttack = new EnemyAttack(animator, enemyData);
        fsmManager.AddState(enemyAttack);
        EnemyHurt enemyHurt = new EnemyHurt(animator, enemyData);
        fsmManager.AddState(enemyHurt);
        EnemyDie enemyDie = new EnemyDie(animator,ref enemyData);
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

        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeState((sbyte)Data.EnemyAnimationCount.Summon);
        }

        #region 强制进入Idel
        //Debug.Log(enemyData.Hurting);
        //Debug.Log(enemyData.Attacking);
        //Debug.Log(enemyData.Attacking2);
        //Debug.Log(enemyData.Die);
        if (!enemyData.Attacking&&!enemyData.Attacking2 && !enemyData.Hurting && !enemyData.Die
            && !enemyData.Summoning)
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
    public void Patrol()
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

                if (distance > enemyData.AttackDistance-0.5f)
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
                else if (distance <= enemyData.AttackDistance-0.5f)
                {
                    if (canAttack)
                    {
                        canAttack = false;
                        Attack();
                    }
                    attackTimeCount += Time.deltaTime;
                    if (attackTimeCount > enemyData.AttackCD)
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
    public void RandomPos()
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
    public void EnemyAI()
    {
        if (!enemyData.Die)
        {
            Patrol();
        }

    }
    #endregion

    #region 攻击
    //近战攻击 
    public void Attack()
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
    public void Attack2()
    {
        if (!enemyData.Attacking && !enemyData.Attacking2 && !enemyData.Hurting && !enemyData.Die)
        {
            ChangeState((sbyte)Data.EnemyAnimationCount.Attack2);
            Invoke("BuildBall", EnemyData.BallStartTime);
        }
    }
    private void BuildBall()
    {
        Instantiate(tmpBall, ball.position, ball.rotation);
    }
    private void Damage()
    {
        if (distance<enemyData.AttackDistance)
        {
            if (PlayerCtrl.Instance.transform.position.x>transform.position.x)
            {
                AudioManager.Instance.StartAudio(Data.Audio.Hurt.ToString());
                PlayerCtrl.Instance.Hurt(enemyData.Damage,1);
            }
            if (PlayerCtrl.Instance.transform.position.x < transform.position.x)
            {
                AudioManager.Instance.StartAudio(Data.Audio.Hurt.ToString());
                PlayerCtrl.Instance.Hurt(enemyData.Damage, -1);
            }

        }
    }
    #endregion

    #region 受击
    float dir;
    float tmpAdd=0;
    public void Hurt(float reduceHP,float tmpDir,float addDistance=0)
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
                enemyEffect.ChangeState((sbyte)Data.EnemyEffect.Hurt);
            }
        }
        else
        {
            StartCoroutine("Red");
            ChangeState((sbyte)Data.EnemyAnimationCount.Die);
            Invoke("Destory", 0.64f);
        }
    }
    IEnumerator Red()
    {
        sprite.color = new Color(Data.R / 255f, Data.G / 255f, Data.B / 255f, Data.A / 255f);
        yield return new WaitForSeconds(Data.IdelRGB);
        sprite.color = new Color(Data.Rb / 255f, Data.Gb / 255f, Data.Bb / 255f, Data.Ab / 255f);
    }
    #endregion

    #region 冲刺的方法
    public void Blink(float distance, float speed)
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
    public void Destory()
    {
        AIManager.Instance.DelEnemy(gameObject);
    }
    #endregion

    #region 变换动作的方法
    public  void ChangeState(sbyte animatorCount)
    {
        fsmManager.ChangeState(animatorCount);
    }
    #endregion
}
