using UnityEngine;
using System.Collections;

public class TeleportPad : MonoBehaviour {

    public static TeleportPad Instance;
    public static TeleportPad Instance2;
    public static TeleportPad Instance3;
    public static int i = 1;
	// Use this for initialization
	void Start () {
        if (Instance == null)
            Instance = this;
        else if (Instance2 == null)
            Instance2 = this;
        else
            Instance3 = this;

	}

    void OnDestroy()
    {
        Instance = null;
        Instance2 = null;
        Instance3 = null;
    }

    public Transform GetNextParent()
    {
        i++;
        if (i > 3) i = 1;
        switch (i)
        {
            case 1:
                return Instance.gameObject.transform;
            case 2:
                return Instance2.gameObject.transform;
            case 3:
                return Instance3.gameObject.transform;
        }
        return gameObject.transform;
    }

	// Update is called once per frame
	void Update () {
        
	}
}
