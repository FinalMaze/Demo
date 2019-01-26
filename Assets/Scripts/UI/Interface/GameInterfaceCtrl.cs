using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameInterfaceCtrl : UIBase
{
    public static GameInterfaceCtrl Instance;
    //角色血量
    Slider hp;
    //角色蓝量
    Slider mp;
    //击杀数
    Text killCount;
    float tmpHP;
    float tmpMP;

    public void UpdateHP()
    {
        PlayerData.hp = Mathf.Clamp(PlayerData.hp, 0, PlayerData.hpMax);
        tmpHP = PlayerData.hp / PlayerData.hpMax;
        hp.value = Mathf.Clamp(tmpHP, 0, 1);
    }
    public void UpdateMP()
    {
        PlayerData.mp = Mathf.Clamp(PlayerData.mp, 0, PlayerData.mpMax);
        tmpMP = PlayerData.mp / PlayerData.mpMax;
        mp.value = Mathf.Clamp(tmpMP, 0, 1);

    }
    int killC = 0;
    public void AddKillCount(int count)
    {
        killC += count;
        killCount.text = killC.ToString() + " Kill";
    }
    GameObject main;
    GameObject startGame;
    Vector2 player0;
    public void StartGame(BaseEventData data)
    {
        main.SetActive(false);
        Time.timeScale = 1;
        //foreach (GameObject enemy in Data.allEnemy)
        //{
        //    EnemyTest tmp = enemy.GetComponent<EnemyTest>();
        //    if (tmp != null)
        //    {
        //        tmp.ChangeState((sbyte)Data.EnemyAnimationCount.Summon);
        //    }
        //    else
        //    {
        //        enemy.GetComponent<EnemyCtrl>().ChangeState((sbyte)Data.EnemyAnimationCount.Summon);
        //    }
        //}

    }

    GameObject gameOver;
    public void GameOver()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0;
    }

    public void NewGame(BaseEventData data)
    {
        gameOver.SetActive(false);
        main.SetActive(true);
        PlayerData.hp = PlayerData.hpMax;
        PlayerData.mp = PlayerData.mpMax;
        UpdateHP();
        UpdateMP();
        PlayerCtrl.Instance.ChangeState((sbyte)Data.AnimationCount.Idel);
        PlayerCtrl.Instance.transform.position = player0;
        killC = 0;
        killCount.text = null;
        AIManager.Instance.DelEnemy();
        AIManager.Instance.InitialEnemy();
    }
    private void Start()
    {
        Instance = this;
        player0 = PlayerCtrl.Instance.transform.position;

        hp = GetControl("HP_UI").GetComponent<Slider>();
        mp= GetControl("MP_UI").GetComponent<Slider>();
        killCount = GetControl("KillCount_UI").GetComponent<Text>();
        gameOver = GetControl("GameOver_UI");
        AddPointClick("GameOver_UI", NewGame);
        main = GetControl("Main_UI");
        startGame = GetControl("Start_UI");
        AddPointClick("Start_UI", StartGame);
        gameOver.SetActive(false);
        UpdateHP();
        UpdateMP();
    }

}
