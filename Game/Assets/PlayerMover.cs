using System;
using System.Security.Cryptography;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;

[System.Serializable]
public class PlayerMover : MonoBehaviour
{
    public static PlayerMover Instance;
    private bool goingUp = true;
    private bool goingDown = false;
    private float zMod = 1.0f;
    private int i = 1;
	// Use this for initialization
	void Start ()
	{
	    Instance = this;
	    goingUp = true;
	    i = 0;
	    goingDown = false;
	}

    private void OnDisable()
    {
        Instance = null;
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    // Update is called once per frame
	public void Rotate ()
	{
        Debug.LogWarning("ROTATED");
        transform.Rotate(0, UnityEngine.Random.Range(60f, 180f), 0);
        GetComponent<AudioSource>().Play();
	}
}
