using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap.Interact;


[System.Serializable]
public class GestureController : MonoBehaviour {

    HandController hc = null;
    Leap.Frame currentFrame = null;
    Leap.HandList handList = null;
    Leap.Hand leftHand = null;
    Leap.Hand rightHand = null;
    Leap.Vector leftHandNormal = new Leap.Vector();
    Leap.Vector rightHandNormal = new Leap.Vector();
    bool printIfHandsAreNotDetected = false;
    public HandPalm LeftHand = new HandPalm();
    public HandPalm RightHand = new HandPalm();
    public static HandPalm leftHandPalm = new HandPalm();
    public static HandPalm rightHandPalm = new HandPalm();
    public List<SpellPattern> AvailableActions = new List<SpellPattern>();
    public BallPrefabs ballPrefabList;
    public static GestureController Instance;
    [HideInInspector]
    public Transform leftHandBallLoc;
    public Transform rightHandBallLoc;
    List<SpellPattern> CurrentlyCasting = new List<SpellPattern>();
    List<Ball> BallsMade = new List<Ball>();
    float blinkTimer = 0.0f;

    public Leap.Hand getLeftHand()
    {
        return leftHand;
    }

    public Leap.Hand getRightHand()
    {
        return rightHand;
    }

    [System.Serializable]
    public class HandPalm
    {
        public Hand_ENUM WhichHand;
        public PalmDirection HandOrientation;
        public bool hasBall = false;
        public bool hasMergedBall = false;

        public enum PalmDirection
        {
            Up,
            Down,
            Forward,
            In,

            Other
        }
    }

    public enum Hand_ENUM
    {
        LeftHand,
        RightHand
    }

    void OnDestroy()
    {
        Instance = null;
    }

	void Start () {
        Instance = this;
        foreach (SpellPattern sp in AvailableActions)
        {
            sp.CurrentConditional = null;
        }
        hc = gameObject.GetComponent<HandController>();
        leftHandPalm.WhichHand = Hand_ENUM.LeftHand;
        rightHandPalm.WhichHand = Hand_ENUM.RightHand;
        LeftHand = leftHandPalm;
        RightHand = rightHandPalm;
	}
	
	void Update () {
        if (blinkTimer > 0.0f) blinkTimer -= Time.deltaTime;
        grabHands();
        updateNormals();
        updatePalm(Hand_ENUM.LeftHand);
        updatePalm(Hand_ENUM.RightHand);
        runSpells();
	}

    void grabHands()
    {
        if (hc != null)
        {
            currentFrame = hc.GetFrame();
            if (currentFrame.IsValid)
            {
                handList = currentFrame.Hands;
                for (int i = 0; i < handList.Count; i++)
                {
                    if (handList[i].IsLeft)
                        leftHand = handList[i];
                    if (handList[i].IsRight)
                        rightHand = handList[i];
                }
                if (handList.Count == 0)
                {
                    leftHand = null;
                    rightHand = null;
                }
            }
        }
    }

    void updateNormals()
    {
        if (leftHand != null)
            leftHandNormal = leftHand.PalmNormal;
        if (rightHand != null)
            rightHandNormal = rightHand.PalmNormal;
    }

    void updatePalm(Hand_ENUM hand)
    {
        switch (hand)
        {
            case Hand_ENUM.LeftHand:
                {
                    if (leftHand != null)
                    {
                        if (leftHandNormal.y > 0)
                        {
                            leftHandPalm.HandOrientation = HandPalm.PalmDirection.Up;
                        }
                        else if (leftHandNormal.y < 0)
                        {
                            leftHandPalm.HandOrientation = HandPalm.PalmDirection.Down;
                        }
                        if (leftHandNormal.z < -0.75)
                        {
                            leftHandPalm.HandOrientation = HandPalm.PalmDirection.Forward;
                            spellLaunchBall(Hand_ENUM.LeftHand);
                        }
                        if (leftHandNormal.x > 0.80)
                        {
                            leftHandPalm.HandOrientation = HandPalm.PalmDirection.In;
                        }

                    }
                    else
                    {
                        leftHandPalm.HandOrientation = HandPalm.PalmDirection.Other;
                        leftHandPalm.hasBall = false;
                        leftHandPalm.hasMergedBall = false;
                        if(printIfHandsAreNotDetected)
                            Debug.Log("No left hand detected!");
                    }
                }
                break;
            case Hand_ENUM.RightHand:
                {
                    if (rightHand != null)
                    {
                        if (rightHandNormal.y > 0)
                        {
                            rightHandPalm.HandOrientation = HandPalm.PalmDirection.Up;
                        }
                        else if (rightHandNormal.y < 0)
                        {
                            rightHandPalm.HandOrientation = HandPalm.PalmDirection.Down;
                        }
                        if (rightHandNormal.z < -0.75)
                        {
                            rightHandPalm.HandOrientation = HandPalm.PalmDirection.Forward;
                            spellLaunchBall(Hand_ENUM.RightHand);
                        }
                        if( rightHandNormal.x < -0.80)
                            rightHandPalm.HandOrientation = HandPalm.PalmDirection.In;
                    }
                    else
                    {
                        rightHandPalm.HandOrientation = HandPalm.PalmDirection.Other;
                        rightHandPalm.hasBall = false;
                        rightHandPalm.hasMergedBall = false;
                        if (printIfHandsAreNotDetected)
                            Debug.Log("No right hand detected!");
                    }
                }
                break;
        }
        if (leftHandPalm != null & rightHandPalm != null)
        {
            if (leftHandPalm.HandOrientation == HandPalm.PalmDirection.In && rightHandPalm.HandOrientation == HandPalm.PalmDirection.In)
            {
                if (leftHand.PalmPosition.DistanceTo(rightHand.PalmPosition) < 50.0f)
                {
                    spellBlink();
                }
                else if (leftHand.PalmPosition.DistanceTo(rightHand.PalmPosition) < 150.0f)
                {
                    spellMergeBall();
                }
            }
        }
        if (LocationTrackerLeftHand.Instance == null) leftHandPalm.hasBall = false;
        if (LocationTrackerRightHand.Instance == null) rightHandPalm.hasBall = false;
    }

    void runSpells()
    {
        foreach (SpellPattern sequence in AvailableActions)
        {
            if (sequence != null)
            {
                if (CurrentlyCasting.Contains(sequence))
                {
                    updateSequence(sequence);
                }
                else
                {
                    CurrentlyCasting.Add(sequence);
                    updateSequence(sequence);
                }
            }
        }
    }

    void updateSequence(SpellPattern sequence)
    {
        // hasn't started, so start it
        if (sequence != null)
        {
            if (sequence.CurrentConditional == null)
            {
                Debug.Log("New sequence started!");
                sequence.CurrentConditional = sequence.Conditionals.ToArray()[0];
            }
            //sequence.CurrentConditional = sequence.Conditionals.ToArray()[0];
            // update it's variables and check if it's ready to move on
            Conditional cur = sequence.CurrentConditional;
            cur.copyFrom(sequence.CurrentConditional);
            switch (sequence.CurrentConditional.HandConditional)
            {
                case Conditional.WhichHands.Both_Hands:
                    {
                        if (cur.LeftHandDirection != leftHandPalm.HandOrientation)
                        {
                            if (cur.Action == Conditional.SpellAction.Create_Ball)
                                resetSequence(sequence);
                        }
                        else
                        {
                            if (cur.RightHandDirection != rightHandPalm.HandOrientation)
                            {
                                if (cur.Action == Conditional.SpellAction.Create_Ball)
                                    resetSequence(sequence);
                            }
                            else
                            {
                                cur.Timer += Time.deltaTime;
                                if (cur.Timer >= cur.MoveOnAfterHowLong)
                                {
                                    cur.NeedsToMoveOn = true;
                                }
                            }
                        }
                    }
                    break;
                case Conditional.WhichHands.Left_Hand_Only:
                    {
                        if (cur.LeftHandDirection != leftHandPalm.HandOrientation)
                        {
                            if (cur.Action == Conditional.SpellAction.Create_Ball)
                                resetSequence(sequence);
                        }
                        else
                        {
                            cur.Timer += Time.deltaTime;
                            if (cur.Timer >= cur.MoveOnAfterHowLong)
                            {
                                cur.NeedsToMoveOn = true;
                            }
                        }
                    }
                    break;
                case Conditional.WhichHands.Right_Hand_Only:
                    {
                        if (cur.RightHandDirection != rightHandPalm.HandOrientation)
                        {
                            if (cur.Action == Conditional.SpellAction.Create_Ball)
                                resetSequence(sequence);
                        }
                        else
                        {
                            cur.Timer += Time.deltaTime;
                            if (cur.Timer >= cur.MoveOnAfterHowLong)
                            {
                                cur.NeedsToMoveOn = true;
                            }
                        }
                    }
                    break;
                case Conditional.WhichHands.No_Hands:
                    {
                        if (leftHandPalm.HandOrientation == HandPalm.PalmDirection.Other && rightHandPalm.HandOrientation == HandPalm.PalmDirection.Other)
                        {
                            cur.Timer += Time.deltaTime;
                            if (cur.Timer >= cur.MoveOnAfterHowLong)
                                cur.NeedsToMoveOn = true;
                        }
                    }
                    break;
            }
            if (cur.NeedsToMoveOn)
            {
                doAction(cur);
                int i = 0;
                foreach (Conditional c in sequence.Conditionals)
                {
                    if (c == sequence.CurrentConditional)
                    {
                        if (i == sequence.Conditionals.Count - 1)
                        {
                            // this is the last conditional in the sequence, need to reset
                            resetSequence(sequence);
                        }
                        else
                        {
                            // this is NOT the last conditonal in the sequence, need to move forward to next conditional
                            sequence.CurrentConditional = sequence.Conditionals.ToArray()[i + 1];
                        }
                    }
                    i++;
                }
            }
        }
    }

    void resetSequence(SpellPattern sequence)
    {
        foreach(Conditional c in sequence.Conditionals)
        {
            c.Timer = 0.0f;
            c.NeedsToMoveOn = false;
        }
        sequence.CurrentConditional = sequence.Conditionals.ToArray()[0];
    }

    void doAction(Conditional spell)
    {
        switch (spell.Action)
        {
            case Conditional.SpellAction.Create_Ball:
                {
                    spellCreateBall(spell);
                }
                break;
        }
    }

    void spellCreateBall(Conditional spell)
    {
        bool launch = false;
        GameObject go = null;
        switch(spell.HandConditional)
        {
            case Conditional.WhichHands.Left_Hand_Only:
                {
                    if (!leftHandPalm.hasBall && !leftHandPalm.hasMergedBall)
                    {
                        launch = true;
                        leftHandPalm.hasBall = true;
                    }
                }
                break;
            case Conditional.WhichHands.Right_Hand_Only:
                {
                    if (!rightHandPalm.hasBall && !rightHandPalm.hasMergedBall)
                    {
                        launch = true;
                        rightHandPalm.hasBall = true;
                    }
                }
                break;
        }
            switch (spell.element)
            {
                case Conditional.Element.Fire:
                    {
                        go = ballPrefabList.FireBallPrefab;
                    }
                    break;
                case Conditional.Element.Ice:
                    {
                        go = ballPrefabList.IceBallPrefab;
                    }
                    break;
                case Conditional.Element.Lightning:
                    {
                        go = ballPrefabList.LightningBallPrefab;
                    }
                    break;
                case Conditional.Element.None:
                    {
                        Debug.LogWarning("You tried to launch a non elemental ball!");
                    }
                    break;
            }
            if (launch && go != null)
            {
                GameObject ball = (GameObject)Instantiate(go, CalcPos(spell), new Quaternion());
                Ball ballComponent = ball.gameObject.GetComponent<Ball>();
                if (ballComponent != null)
                {
                    ballComponent.Attached = true;
                    ballComponent.Attachee = CalcParent(spell);
                    ballComponent.WhichHand = spell.HandConditional;
                    BallsMade.Add(ballComponent);
                }
            }
    }

    Vector3 CalcPos(Conditional spell)
    {
        if (spell.HandConditional == Conditional.WhichHands.Left_Hand_Only)
        {
            if (LocationTrackerLeftHand.Instance != null)
                return LocationTrackerLeftHand.Instance.transform.position;
        }
        else if (spell.HandConditional == Conditional.WhichHands.Right_Hand_Only)
        {
            if (LocationTrackerRightHand.Instance != null)
                return LocationTrackerRightHand.Instance.transform.position;
        }
        else if (spell.HandConditional == Conditional.WhichHands.Both_Hands)
        {
            if (LocationTrackerRightHand.Instance != null && LocationTrackerLeftHand.Instance != null)
            {
                return (LocationTrackerLeftHand.Instance.transform.position + (LocationTrackerLeftHand.Instance.transform.position - LocationTrackerRightHand.Instance.transform.position));
            }
        }
        return new Vector3();
    }

    Transform CalcParent(Conditional spell)
    {
        if (spell.HandConditional == Conditional.WhichHands.Left_Hand_Only)
        {
            if (LocationTrackerLeftHand.Instance != null)
                return LocationTrackerLeftHand.Instance.transform;
        }
        else if (spell.HandConditional == Conditional.WhichHands.Right_Hand_Only)
        {
            if (LocationTrackerRightHand.Instance != null)
                return LocationTrackerRightHand.Instance.transform;
        }
        else if (spell.HandConditional == Conditional.WhichHands.Both_Hands)
        {
            if (LocationTrackerRightHand.Instance != null && LocationTrackerLeftHand.Instance != null)
            {
                return (LocationTrackerLeftHand.Instance.transform);
            }
        }
        return null;
    }

    void spellLaunchBall(Hand_ENUM hand)
    {
        Ball ballToLaunch = null;
        foreach (Ball b in BallsMade)
        {
            if (b != null)
            {
                if (b.WhichHand == Conditional.WhichHands.Left_Hand_Only && hand == Hand_ENUM.LeftHand)
                {
                    ballToLaunch = b;
                    leftHandPalm.hasBall = false;
                    leftHandPalm.hasMergedBall = false;
                }
                else if (b.WhichHand == Conditional.WhichHands.Right_Hand_Only && hand == Hand_ENUM.RightHand)
                {
                    ballToLaunch = b;
                    rightHandPalm.hasBall = false;
                    rightHandPalm.hasMergedBall = false;
                }
            }
        }
        if (ballToLaunch != null)
        {
            ballToLaunch.Launched = true;
            ballToLaunch.Attached = false;
            ballToLaunch.Attachee = null;
            //Transform transformLauncher = BallAimer.Instance.transform;
            //ballToLaunch.transform.parent = transformLauncher;
            //ballToLaunch.rigidbody.AddForce(transformLauncher.forward * 40.0f);
            // get Player instance, and launch in direction of the oculus but only forward(not up or down)
            // Player.Instance
        }
    }

    void spellLaunchMergedBall(Conditional spell)
    {
        Ball ballToLaunch = null;
        foreach (Ball b in BallsMade)
        {
            if (b.WhichHand == Conditional.WhichHands.Both_Hands)
                ballToLaunch = b;
        }
        if (ballToLaunch != null)
        {
            // get Player instance, and launch in direction of the oculus but only forward(not up or down)
            // Player.Instance
        }
    }

    void spellMergeBall()
    {
        if ((leftHandPalm.hasBall && rightHandPalm.hasBall) && (!leftHandPalm.hasMergedBall && !rightHandPalm.hasMergedBall))
        {
            foreach(Ball b in BallsMade)
            {
                if(b != null)
                Destroy(b.gameObject);
            }
            BallsMade.Clear();
            if (LocationTrackerLeftHand.Instance != null)
            {
                GameObject ball =
                    (GameObject)
                        Instantiate(ballPrefabList.LightningBallPrefab,
                            LocationTrackerLeftHand.Instance.transform.position, new Quaternion());
                MergedBall ballComponent = ball.gameObject.GetComponent<MergedBall>();
                if (ballComponent != null)
                {
                    ballComponent.Attached = true;
                    ballComponent.Attachee = LocationTrackerLeftHand.Instance.transform;
                    ballComponent.WhichHand = Conditional.WhichHands.Both_Hands;
                    BallsMade.Add(ballComponent);
                }
            }
            if (LocationTrackerRightHand.Instance != null)
            {
                GameObject bball =
                    (GameObject)
                        Instantiate(ballPrefabList.LightningBallPrefab,
                            LocationTrackerRightHand.Instance.transform.position, new Quaternion());
                MergedBall bballComponent = bball.gameObject.GetComponent<MergedBall>();
                if (bballComponent != null)
                {
                    bballComponent.Attached = true;
                    bballComponent.Attachee = LocationTrackerRightHand.Instance.transform;
                    bballComponent.WhichHand = Conditional.WhichHands.Both_Hands;
                    BallsMade.Add(bballComponent);
                }
                StartCoroutine(DelayHasBallFalse(2.0f));
            }


        }
    }

    IEnumerator DelayHasBallFalse(float f)
    {
        yield return new WaitForSeconds(f);
        leftHandPalm.hasBall = false;
        rightHandPalm.hasBall = false;
    }

    Vector3 GetMergePos()
    {
        Vector3 left = new Vector3(leftHand.PalmPosition.x, leftHand.PalmPosition.y, leftHand.PalmPosition.z);
        Vector3 right = new Vector3(rightHand.PalmPosition.x, rightHand.PalmPosition.y, rightHand.PalmPosition.z);
        return Vector3.Lerp(left, right,0.5f);
    }

    void spellBlink()
    {
        if (blinkTimer <= 0.0f)
        {
            Player.Instance.transform.parent.parent = TeleportPad.Instance.GetNextParent();
            blinkTimer = 20.0f;
            // launch visual effects
        }
    }
}
