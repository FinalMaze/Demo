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
    GameObject tmpBase;
    GameObject tmpEnemy;
    private void Awake()
    {
        instance = this;

        tmpBase = GameObject.Find("Enemy");
        tmpEnemy = BulidEnemy("Prefabs/Enemy", tmpBase.transform);
    }
    #endregion

    float timeCount;
    private void Update()
    {
        if (tmpEnemy==null)
        {
            timeCount += Time.deltaTime;
            if (timeCount>2f)
            {
                timeCount = 0;
                tmpEnemy = BulidEnemy("Prefabs/Enemy", tmpBase.transform);
            }
        }
    }


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
        tmpFriend.transform.SetParent(tmpBase);

    }
    #endregion

    #region 创建敌人
    public GameObject BulidEnemy(string path,Transform tmpBase)
    {
        Object tmpObj = Resources.Load(path);
        GameObject tmpEnemy = GameObject.Instantiate(tmpObj) as GameObject;
        //tmpEnemy.AddComponent<EnemyCtrl>();
        Data.allEnemy.Add(tmpEnemy);
        tmpEnemy.transform.SetParent(tmpBase, false);
        return tmpEnemy;
    }
    #endregion

    #region 销毁敌人
    public void DelEnemy(GameObject gameObject)
    {
        for (int i = 0; i < Data.allEnemy.Count; i++)
        {
            if (Data.allEnemy[i]==gameObject)
            {
                //Data.allEnemy.Remove(Data.allEnemy[i]);
                //Destroy(Data.allEnemy[i]);
                Del(Data.allEnemy[i]);
                //Destroy(gameObject);
            }
        }
    }
    public void Del(GameObject tmpObj)
    {
        Data.allEnemy.Remove(tmpObj);
        Destroy(tmpObj);
    }
    #endregion
}
