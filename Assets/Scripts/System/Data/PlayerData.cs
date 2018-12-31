using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public static bool playerRun = false;
    public static bool playerWalk = false;
    public static bool playerJumping = false;
    public static bool playerStartJump = false;
    public static bool playerIsGround = true;
    public static string playerPath = "Prefabs/Player/Player";

    public static bool playerAttacked = false;

    public static float blood = 100f;
    public static float maxBlood = 100f;
    public static float runSpeed = 50f;
    public static float jumpSpeed = 1f;


}
