using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestInCtrl : UIBase
{
    GameObject tmpObj;
    private void Start()
    {
        tmpObj = GetControl("Thumb_UI");
    }

    public void Move(PointerEventData vector)
    {
        tmpObj.transform.position = vector.position;
        Debug.Log(tmpObj.transform.position + "           "+vector);
    }
}
