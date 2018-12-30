using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private void Awake()
    {
        gameObject.AddComponent<AIManager>();
        gameObject.AddComponent<AudioManager>();
        gameObject.AddComponent<UIManager>();
        DontDestroyOnLoad(gameObject);
    }
}
