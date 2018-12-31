using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameInterfaceCtrl : UIBase
{
    public static GameInterfaceCtrl Instance;
    Slider blood;

    public void UpdateBlood()
    {
        blood.value = PlayerData.blood / PlayerData.maxBlood;
    }


    private void Start()
    {
        Instance = this;
        blood = GetControl("Blood_UI").GetComponent<Slider>();
        UpdateBlood();
        
    }


    private void Update()
    {
        
    }


}
