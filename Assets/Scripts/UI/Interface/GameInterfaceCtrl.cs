using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInterfaceCtrl : UIBase
{
    public static GameInterfaceCtrl Instance;
    Easy easyTouch;
    private void Start()
    {
        Instance = this;

        EasyTouchInitial();
    }

    private void EasyTouchInitial()
    {
        GameObject tmpObj = GetControl("EasyTouch_UI");
        easyTouch = new Easy(tmpObj.transform.position, tmpObj, 60f, AIManager.Instance.Player.gameObject);
        
        AddDrag("EasyTouch_UI", easyTouch.OnDrag);
        AddOnEndDrag("EasyTouch_UI", easyTouch.OnEndDrag);
    }



}
