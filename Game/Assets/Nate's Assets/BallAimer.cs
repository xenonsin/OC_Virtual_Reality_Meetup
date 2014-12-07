using UnityEngine;
using System.Collections;

[System.Serializable]
public class BallAimer : MonoBehaviour {

    public static BallAimer Instance;
	// Use this for initialization
	void Start () {
        Instance = this;
	}

    void OnDestroy()
    {
        Instance = null;
    }
	// Update is called once per frame
	void Update () {
	
	}
}
