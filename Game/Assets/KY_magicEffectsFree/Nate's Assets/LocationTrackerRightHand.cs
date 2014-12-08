using UnityEngine;
using System.Collections;

public class LocationTrackerRightHand : MonoBehaviour
{

    public static LocationTrackerRightHand Instance;

    // Use this for initialization
    void Start()
    {
        Instance = this;
    }

    void OnDestroy()
    {
        Instance = null;
    }


}
