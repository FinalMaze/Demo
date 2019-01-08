using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    FSMManager fsmManager;
    Animator animator;
    EnemyData enemyData;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyData = new EnemyData();
        fsmManager = new FSMManager((int)Data.EnemyAnimationCount.Max);



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
        EnemyDie enemyDie = new EnemyDie(animator,ref enemyData);
        fsmManager.AddState(enemyDie);
        #endregion
    }
    float distance;
    float direction;
    private void Update()
    {
        fsmManager.OnStay();
        distance = Vector2.Distance(PlayerCtrl.Instance.transform.position, transform.position);
        direction = PlayerCtrl.Instance.transform.position.x - transform.position.x;

        #region 强制进入Idel
        if (!enemyData.Attacking && !enemyData.Hurting && !enemyData.Die)
        {
            ChangeState((sbyte)Data.EnemyAnimationCount.Idel);
        }
        #endregion

        //if (enemyData.HP == 0 && !enemyData.Die)
        //{
        //    ChangeState((sbyte)Data.EnemyAnimationCount.Die);
            
        //}

        //巡逻
        EnemyAI();

        if (Input.GetKeyDown(KeyCode.J))
        {
            Hurt(50);
        }
    }

    #region 巡逻
    bool canPartol;
    float attackTimeCount;
    public void Patrol()
    {
        if (!enemyData.Attacking && !enemyData.Hurting && !enemyData.Die)
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
                    transform.position = Vector2.MoveTowards(transform.position, tmpVec, 0.10f);
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
            }
            //跟随并攻击
            else
            {
                if (distance > enemyData.AttackDistance)
                {
                    if (direction > 0)
                    {
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    if (direction < 0)
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    transform.position = Vector2.MoveTowards(transform.position, PlayerCtrl.Instance.transform.position, 0.08f);
                    ChangeState((sbyte)Data.EnemyAnimationCount.Walk);
                }
                else
                {
                    attackTimeCount += Time.deltaTime;
                    if (attackTimeCount > 1f)
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
    public void Attack()
    {
        ChangeState((sbyte)Data.EnemyAnimationCount.Attack);
        Invoke("Damage", EnemyData.AttackTime / 2);
    }
    private void Damage()
    {
        if (distance<3f)
        {
            PlayerData.hp -= enemyData.Damage;
            GameInterfaceCtrl.Instance.UpdateHP();
            if (!PlayerData.Attacking)
            {
                //播放玩家受击动画
            }
        }
    }
    #endregion

    #region 受击
    public void Hurt(float reduceHP)
    {
        enemyData.HP = Mathf.Clamp(enemyData.HP -= reduceHP, 0, enemyData.MaxHP);
        if (enemyData.HP != 0)
        {
            if (!enemyData.Attacking && !enemyData.Die)
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
    #endregion

    #region 重生
    public void Destory()
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
