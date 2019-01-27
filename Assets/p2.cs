using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class p2 : MonoBehaviour
{
    public void P2(Boolean tmpBool)
    {
        toggle.isOn = !tmpBool;
    }
    Toggle toggle;
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }
}
