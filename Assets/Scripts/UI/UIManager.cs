using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    Dictionary<string,Dictionary<string,GameObject>> allChild;
    Dictionary<string, GameObject> allPanel;
    private void Awake()
    {
        Instance = this;
        allChild = new Dictionary<string, Dictionary<string, GameObject>>();
        allPanel = new Dictionary<string, GameObject>();
    }

    #region 注册子控件的方法
    public void RegistControl(GameObject panelName,string controlName,GameObject obj)
    {
        if(!allChild.ContainsKey(panelName.name))
        {
            allChild[panelName.name] = new Dictionary<string, GameObject>();
            allPanel.Add(panelName.name,panelName);
        }
        allChild[panelName.name].Add(controlName, obj);
    }
    #endregion

    #region 得到子控件的方法
    public GameObject GetChild(string panelName,string controlNmae)
    {

        if (allChild.ContainsKey(panelName))
        {
            return allChild[panelName][controlNmae];
        }
        return null;
    }
    #endregion

    #region 得到Panel的方法
    public GameObject GetPanel(string panelName)
    {
        return allPanel[panelName];
    }
    #endregion
}
