using UnityEngine;
using System.Collections;

public class AudioClipHolder : MonoBehaviour
{

    public AudioClip ExplodeClip;

    public static AudioClipHolder Instance;
	// Use this for initialization
	void Start ()
	{
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
