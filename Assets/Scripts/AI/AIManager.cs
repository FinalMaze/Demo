using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
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
    private void Awake()
    {
        instance = this;
        //GameObject tmpBase = GameObject.FindGameObjectWithTag("PlayerParent");
        //BulidPlayer(Data.playerPath, tmpBase.transform);
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

    #region 创建敌人
    public void BulidEnemy(string path,Transform tmpBase)
    {
        Object tmpObj = Resources.Load(path);
        GameObject tmpEnemy = GameObject.Instantiate(tmpObj) as GameObject;
        tmpEnemy.AddComponent<EnemyCtrl>();
        tmpEnemy.transform.SetParent(tmpBase, false);
    }
    #endregion
}
