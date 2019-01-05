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

    public void UpdateHP()
    {
        hp.value = PlayerData.hp / PlayerData.hpMax;
        hp.value = Mathf.Clamp(hp.value, 0, 1);
    }
    public void UpdateMP()
    {
        mp.value = PlayerData.mp / PlayerData.mpMax;
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
