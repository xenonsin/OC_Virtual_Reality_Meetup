﻿using UnityEngine;
using System.Collections;

public class LevelSwapper : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButtonDown("space"))
	    {
	        Application.LoadLevel("instructions");
	    }
	    if (Input.GetButtonDown("b"))
	    {
	        Application.LoadLevel("main_menu");
	    }

	}
}
