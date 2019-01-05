using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameInterfaceCtrl : UIBase
{
    public static GameInterfaceCtrl Instance;
    Slider hp;
    Slider mp;
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

    private void Start()
    {
        Instance = this;
        hp = GetControl("HP_UI").GetComponent<Slider>();
        mp= GetControl("MP_UI").GetComponent<Slider>();
        UpdateHP();
        UpdateMP();
    }


    private void Update()
    {
        
    }


}
