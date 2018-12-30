using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public static bool playerRun=false;
    public static bool playerWalk = false;
    public static bool playerJumping = false;
    public static bool playerStartJump = false;
    public static bool playerIsGround = true;
    public static float playerWalkSpeed = 30f;
    public static float playerRunSpeed = 50f;
    public static float playerJumpSpeed = 4f;
    public static string playerPath = "Prefabs/Player/Player";

    public enum Audio
    {
        translate
    }

    public enum AnimationCount
    {
        Idel,
        Walk,
        Run,
        Jump,
        Sway,


        Max
    }
}
