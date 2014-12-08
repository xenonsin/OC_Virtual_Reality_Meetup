using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Conditional : MonoBehaviour
{
    public enum WhichHands
    {
        Left_Hand_Only,
        Right_Hand_Only,
        Both_Hands,
        No_Hands
    }
    public enum SpellAction
    {
        Create_Ball,
        Launch_Ball,
        Merge_Ball,
        Launch_Merged_Ball,
        Blink,
        None
    }
    public enum Element
    {
        Fire,
        Ice,
        Lightning,
        None
    }

    public WhichHands HandConditional;
    public SpellAction Action;
    public GestureController.HandPalm.PalmDirection LeftHandDirection;
    public GestureController.HandPalm.PalmDirection RightHandDirection;
    public Element element;
    public float MoveOnAfterHowLong = 0.0f;
    [HideInInspector]
    public float Timer = 0.0f;
    [HideInInspector]
    public bool NeedsToMoveOn = false;

    void Awake()
    {
        Timer = 0.0f;
        NeedsToMoveOn = false;
    }

    //public Conditional()
    //{
    //    HandConditional = WhichHands.No_Hands;
    //    Action = SpellAction.None;
    //    LeftHandDirection = GestureController.HandPalm.PalmDirection.Other;
    //    RightHandDirection = GestureController.HandPalm.PalmDirection.Other;
    //    MoveOnAfterHowLong = 0.0f;
    //    NeedsToMoveOn = false;
    //}

    public void copyFrom(Conditional c)
    {
        this.HandConditional = c.HandConditional;
        this.Action = c.Action;
        this.LeftHandDirection = c.LeftHandDirection;
        this.RightHandDirection = c.RightHandDirection;
        this.element = c.element;
        this.MoveOnAfterHowLong = c.MoveOnAfterHowLong;
        this.Timer = c.Timer;
        this.NeedsToMoveOn = c.NeedsToMoveOn;
    }

}