using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    #region 单例
    private static AIManager instance;
    public static AIManager Instance
    {
        get { return instance; }
    }
    Transform player;
    public Transform Player
    {
        get
        {
            if(player==null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }
            return player;
        }
    }
    Transform tmpFriend;
    private void Awake()
    {
        instance = this;
        GameObject tmpEnemy = GameObject.Find("Enemy");
        BulidEnemy("Prefabs/Enemy", tmpEnemy.transform);
    }
    #endregion

    #region 创建玩家角色
    public void BulidPlayer(string path,Transform tmpBase)
    {
        Object tmpObj = Resources.Load(path);
        GameObject tmpPlayer = GameObject.Instantiate(tmpObj) as GameObject;
        tmpPlayer.AddComponent<PlayerCtrl>();
        tmpPlayer.transform.SetParent(tmpBase, false);
    }
    #endregion

    #region 创建召唤兽
    public void BuildFriend(string path,Transform tmpBase)
    {
        Object tmpObj = Resources.Load(path);
        GameObject tmpFriend = GameObject.Instantiate(tmpObj) as GameObject;
        tmpFriend.AddComponent<FriendCtrl>();
        tmpFriend.transform.SetParent(tmpBase,false);

    }
    #endregion

    #region 创建敌人
    public void BulidEnemy(string path,Transform tmpBase)
    {
        Object tmpObj = Resources.Load(path);
        GameObject tmpEnemy = GameObject.Instantiate(tmpObj) as GameObject;
        tmpEnemy.AddComponent<EnemyCtrl>();
        Data.allEnemy.Add(tmpEnemy);
        tmpEnemy.transform.SetParent(tmpBase, false);
    }
    #endregion
}
