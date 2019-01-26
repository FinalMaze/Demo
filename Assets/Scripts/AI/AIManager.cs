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
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }
            return player;
        }
    }
    Transform tmpFriend;
    GameObject tmpBase1;
    GameObject tmpBase2;
    GameObject tmpBase3;
    GameObject tmpEnemy1;
    GameObject tmpEnemy2;
    GameObject tmpEnemy3;
    private void Awake()
    {
        instance = this;
        InitialEnemy();
    }
    //初始化敌人
    public void InitialEnemy()
    {
        tmpBase1 = GameObject.Find("Enemy1");
        tmpEnemy1 = RandomEnemy(tmpBase1);
        tmpBase2 = GameObject.Find("Enemy2");
        tmpEnemy2 = RandomEnemy(tmpBase2);
        tmpBase3 = GameObject.Find("Enemy3");
        tmpEnemy3 = RandomEnemy(tmpBase3);
    }
    #endregion

    float timeCount1;
    float timeCount2;
    float timeCount3;
    private void Update()
    {
        if (tmpEnemy1 == null)
        {
            timeCount1 += Time.deltaTime;
            if (timeCount1 > EnemyData.bigReTime)
            {
                timeCount1 = 0;
                tmpEnemy1 = RandomEnemy(tmpBase1);
            }
        }
        if (tmpEnemy2 == null)
        {
            timeCount2 += Time.deltaTime;
            if (timeCount2 > EnemyData.bigReTime)
            {
                timeCount2 = 0;
                tmpEnemy2 = RandomEnemy(tmpBase2);
            }
        }
        if (tmpEnemy3 == null)
        {
            timeCount3 += Time.deltaTime;
            if (timeCount3 > EnemyData.smallReTime)
            {
                timeCount3 = 0;
                tmpEnemy3 = RandomEnemy(tmpBase3);
            }
        }
    }

    private GameObject RandomEnemy(GameObject tmpObj)
    {
        int ran = Random.Range(0, 3);
        switch (ran)
        {
            case 0:
                return BulidEnemy("Prefabs/Enemy", tmpObj.transform);
            default:
                return BulidEnemy("Prefabs/Enemy1", tmpObj.transform);
        }
    }

    #region 创建玩家角色
    public void BulidPlayer(string path, Transform tmpBase)
    {
        Object tmpObj = Resources.Load(path);
        GameObject tmpPlayer = GameObject.Instantiate(tmpObj) as GameObject;
        tmpPlayer.AddComponent<PlayerCtrl>();
        tmpPlayer.transform.SetParent(tmpBase, false);
    }
    #endregion

    #region 创建召唤兽
    public void BuildFriend(string path, Transform tmpBase)
    {
        Object tmpObj = Resources.Load(path);
        GameObject tmpFriend = GameObject.Instantiate(tmpObj) as GameObject;
        tmpFriend.transform.SetParent(tmpBase);

    }
    #endregion

    #region 创建敌人
    public GameObject BulidEnemy(string path, Transform tmpBase)
    {
        Object tmpObj = Resources.Load(path);
        GameObject tmpEnemy = GameObject.Instantiate(tmpObj) as GameObject;
        //tmpEnemy.AddComponent<EnemyCtrl>();
        Data.allEnemy.Add(tmpEnemy);
        int ran = Random.Range(-EnemyData.startDis, EnemyData.startDis);
        Vector2 tmpT = new Vector2(ran, tmpEnemy.transform.position.y);
        tmpEnemy.transform.SetParent(tmpBase);
        tmpEnemy.transform.localPosition = tmpT;
        return tmpEnemy;
    }
    #endregion

    #region 销毁敌人
    //销毁指定
    public void DelEnemy(GameObject tmpObject)
    {
        for (int i = 0; i < Data.allEnemy.Count; i++)
        {

            if (Data.allEnemy[i] == tmpObject)
            {
                Del(Data.allEnemy[i]);
            }
        }
    }
    //销毁所有
    List<GameObject> tmpAllEnemy;
    public void DelEnemy()
    {
        if (tmpAllEnemy==null)
        {
            tmpAllEnemy = new List<GameObject>();
        }
        foreach (GameObject item in Data.allEnemy)
        {
            tmpAllEnemy.Add(item);
        }
        for (int i = 0; i < tmpAllEnemy.Count; i++)
        {
            Del(tmpAllEnemy[i]);
        }
        tmpAllEnemy.Clear();
        tmpAllEnemy = null;
    }
    public void Del(GameObject tmpObj)
    {
        Data.allEnemy.Remove(tmpObj);
        Destroy(tmpObj);
    }
    #endregion
}
