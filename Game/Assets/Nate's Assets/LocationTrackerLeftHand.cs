using UnityEngine;
using System.Collections;

public class LocationTrackerLeftHand : MonoBehaviour {

    public static LocationTrackerLeftHand Instance;

	// Use this for initialization
	void Start () {
        Instance = this;
	}

    void OnDestroy()
    {
        Instance = null;
    }
	
	
}
