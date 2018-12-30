using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIBase : MonoBehaviour
{
    private void Awake()
    {
        Transform[] tmpChild = transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < tmpChild.Length; i++)
        {

            if (tmpChild[i].name.EndsWith("_UI"))
            {
                tmpChild[i].gameObject.AddComponent<UIBehaviour>();
            }
        }
    }
    public GameObject GetControl(string controlName)
    {
        return UIManager.Instance.GetChild(transform.name, controlName);
    }
    public UIBehaviour GetBehaviour(string controlName)
    {
        GameObject tmpObj = GetControl(controlName);
        if (tmpObj != null)
        {
            return tmpObj.GetComponent<UIBehaviour>();
        }
        return null;
    }
    //添加Button的事件
    public void AddButtonListen(string controlName, UnityAction action)
    {
        UIBehaviour tmpBehaviour = GetBehaviour(controlName);
        if (tmpBehaviour != null)
        {
            tmpBehaviour.AddButtonListen(action);
        }
    }
    //拖拽事件添加方法
    public void AddDrag(string controlName, UnityAction<BaseEventData> action)
    {
        UIBehaviour tmpBehaviour = GetBehaviour(controlName);
        if (tmpBehaviour != null)
        {
            tmpBehaviour.AddDrag(action);
        }
    }
    //结束拖拽事件添加方法
    public void AddOnEndDrag(string controlName, UnityAction<BaseEventData> action)
    {
        UIBehaviour tmpBehaviour = GetBehaviour(controlName);
        if (tmpBehaviour != null)
        {
            tmpBehaviour.AddOnEndDrag(action);
        }
    }
    //开始拖拽事件添加方法
    public void AddOnBeginDrag(string controlName, UnityAction<BaseEventData> action)
    {
        UIBehaviour tmpBehaviour = GetBehaviour(controlName);
        if (tmpBehaviour != null)
        {
            tmpBehaviour.AddOnBeginDrag(action);
        }
    }
    //点击事件添加方法
    public void AddPointClick(string controlName, UnityAction<BaseEventData> action)
    {
        
        UIBehaviour tmpBehaviour = GetBehaviour(controlName);

        if (tmpBehaviour != null)
        {
            tmpBehaviour.AddPointClick(action);
            
        }
    }
    //点击按下事件
    public void AddPointClickDown(string controlName, UnityAction<BaseEventData> action)
    {

        UIBehaviour tmpBehaviour = GetBehaviour(controlName);

        if (tmpBehaviour != null)
        {
            tmpBehaviour.AddPointClickDown(action);

        }
    }
    //点击抬起事件
    public void AddPointClickUP(string controlName, UnityAction<BaseEventData> action)
    {

        UIBehaviour tmpBehaviour = GetBehaviour(controlName);

        if (tmpBehaviour != null)
        {
            tmpBehaviour.AddPointClickUP(action);

        }
    }
}
