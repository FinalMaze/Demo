using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendData
{
    //是否在移动中
    public static bool moving = false;
    //进行跟随的距离
    public static float followDistance = 3f;
    //普通多长时间跟随到玩家
    public static float smoothTime = 2f;
    //被召唤时，移动到玩家位置的时间
    public static float comeTime = 0.5f;
}
