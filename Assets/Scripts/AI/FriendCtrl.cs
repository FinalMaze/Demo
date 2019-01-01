using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendCtrl : MonoBehaviour
{
    public static FriendCtrl Instance;

    Animator animator;
    FSMManager fsmManager;

    //玩家的位置
    Vector2 player;
    //与玩家相距的坐标点
    public Vector2 distanceV=Vector2.one;
    //跟随的速度
    private Vector2 velocity = Vector2.one;
    //计时器
    float timeCount;
    bool canMove;
    private void Start()
    {
        Instance = this;
        fsmManager = new FSMManager((int)Data.FriendAnimationCount.Max);
        animator = GetComponent<Animator>();


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
        if (PlayerData.distance > FriendData.followDistance&&!FriendData.moving)
        {
            FriendData.moving = true;
            if (player.x>transform.position.x)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            transform.position = Vector2.SmoothDamp(transform.position, player + distanceV, ref velocity, FriendData.smoothTime);
        }
        if (PlayerData.distance > FriendData.followDistance*2)
        {
            FriendData.moving = true;
            if (player.x < transform.position.x)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            transform.position = Vector2.SmoothDamp(transform.position, player + distanceV, ref velocity, FriendData.smoothTime);
        }
        #endregion

        #region 巡逻的方法
        if (!FriendData.moving)
        {
            StartCoroutine("Ran");
            StartCoroutine("Patrol");
        }
        #endregion

    }

    #region 合体
    public void Fit()
    {

    }
    #endregion

    #region 扔召唤兽
    public void ThrowFriend(Vector2 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, 0.5f);
    }
    #endregion

    #region 被召唤到玩家位置
    public void GoToPlayer()
    {
        Debug.Log("对碰到的敌人造成伤害");
        FriendData.moving = true;
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
        FriendData.moving = false;
    }
    #endregion

    public void ChangeState(sbyte animationCount)
    {
        fsmManager.ChangeState(animationCount);
    }

}
