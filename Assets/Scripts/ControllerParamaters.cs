using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ControllerParamaters
{

    public enum JumpBehavior
    {
        CanJumpOnGround,
        CanJumpAnywhere,
        CantJump
    }

    public JumpBehavior JumpRestrictions;

    public Vector2 MaxVelocity = new Vector2(float.MaxValue, float.MaxValue);

    [Range(0, 90)]
    public float SlopeLimit = 30;

    public float Gravity = -25;

    public float JumpFrequency = 0.25f;

    public float JumpMagnitude = 16f;


}
