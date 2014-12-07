using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour {


    public static Global Instance;
    public Transform LeftHand;
    public Transform RightHand;

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
