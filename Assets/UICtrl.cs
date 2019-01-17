using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgehogTeam.EasyTouch;
using UnityEngine.EventSystems;

public class UICtrl : MonoBehaviour
{
    public PointerEventData eventData;

    public void Touch(PointerEventData tmpData)
    {
        eventData = tmpData;
    }

    public void Thumb(Gesture gesture)
    {
        ETCJoystick.Instance.OnPointerDown(eventData);
        ETCJoystick.Instance.OnDrag(eventData);
    }

}
