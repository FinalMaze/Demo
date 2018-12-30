using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        GameObject tmpClick = GetControl("Click_UI");
        easyTouch = new Easy(tmpClick.transform.position, tmpObj, 60f, AIManager.Instance.Player.gameObject, tmpClick);

        AddDrag("Click_UI", easyTouch.OnDragBG);
        AddOnEndDrag("Click_UI", easyTouch.OnEndDrag);
    }

    private void Update()
    {
    }


}
