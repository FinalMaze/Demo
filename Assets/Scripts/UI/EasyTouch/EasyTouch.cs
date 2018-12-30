using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Easy
{
    float tmpRadius;
    Vector2 tmpCore;
    GameObject tmpRocker;
    GameObject tmpPlayer;
    GameObject tmpClick;
    //构造时写入圆心位置、摇杆物体、摇杆移动半径、摇杆控制的玩家、被点击物体
    public Easy(Vector2 core, GameObject rocker, float radius, GameObject player,GameObject click)
    {
        tmpClick = click;
        tmpPlayer = player;
        tmpCore = core;
        tmpRocker = rocker;
        tmpRadius = radius;
    }
    #region 背景轮盘点击事件
    public void OnClickBGDown(BaseEventData eventData)
    {
        Data.EasyTouch = true;
        if (tmpPlayer == null && AIManager.Instance.Player != null)
        {
            tmpPlayer = AIManager.Instance.Player.gameObject;
            Debug.Log(tmpPlayer);
        }

        PointerEventData tmpDate = (PointerEventData)eventData;
        Vector2 tmpDistance = tmpDate.position - tmpCore;
        if (tmpDistance.magnitude < tmpRadius)
        {
            tmpRocker.transform.position = tmpDate.position;
        }
        else
        {
            tmpRocker.transform.position = tmpCore + tmpDistance.normalized * tmpRadius;
        }
    }
    public void OnClickBGUP(BaseEventData eventData)
    {
        Data.EasyTouch = false;
        tmpRocker.transform.position = tmpClick.transform.position;
    }
    #endregion

    #region 背景轮盘拖拽事件
    public void OnDragBG(BaseEventData eventData)
    {
        Data.EasyTouch = true;
        if (tmpPlayer == null && AIManager.Instance.Player != null)
        {
            tmpPlayer = AIManager.Instance.Player.gameObject;
            Debug.Log(tmpPlayer);
        }

        PointerEventData tmpDate = (PointerEventData)eventData;
        Vector2 tmpDistance = tmpDate.position - tmpCore;
        if (tmpDistance.magnitude < tmpRadius)
        {
            tmpRocker.transform.position = tmpDate.position;
        }
        else
        {
            tmpRocker.transform.position = tmpCore + tmpDistance.normalized * tmpRadius;
        }
        //if (PlayerData.blood!=0&&!PlayerData.playerAttacked)
        //{
        //    Vector2 rocker = tmpPlayer.transform.localEulerAngles;
        //    rocker.y = 90 - Mathf.Atan2(tmpDistance.y, tmpDistance.x) * Mathf.Rad2Deg;
        //    tmpPlayer.transform.localEulerAngles = rocker;
        //}
    }
    public void OnEndDragBG(BaseEventData eventData)
    {
        Data.EasyTouch = false;
        tmpRocker.transform.position = tmpCore;
    }
    #endregion

    //摇杆拖拽事件
    public void OnDrag(BaseEventData eventData)
    {
        Data.EasyTouch = true;
        if (tmpPlayer == null && AIManager.Instance.Player != null)
        {
            tmpPlayer = AIManager.Instance.Player.gameObject;
            Debug.Log(tmpPlayer);
        }

        PointerEventData tmpDate = (PointerEventData)eventData;
        Vector2 tmpDistance = tmpDate.position - tmpCore;
        if (tmpDistance.magnitude < tmpRadius)
        {
            tmpRocker.transform.position = tmpDate.position;
        }
        else
        {
            tmpRocker.transform.position = tmpCore + tmpDistance.normalized * tmpRadius;
        }
        //if (PlayerData.blood!=0&&!PlayerData.playerAttacked)
        //{
        //    Vector2 rocker = tmpPlayer.transform.localEulerAngles;
        //    rocker.y = 90 - Mathf.Atan2(tmpDistance.y, tmpDistance.x) * Mathf.Rad2Deg;
        //    tmpPlayer.transform.localEulerAngles = rocker;
        //}
    }
    //摇杆结束拖拽时归位
    public void OnEndDrag(BaseEventData eventData)
    {
        Data.EasyTouch = false;
        tmpRocker.transform.position = tmpClick.transform.position;
    }
    float tmpX;
    float tmpY;
    void OnStay()
    {
    }
}