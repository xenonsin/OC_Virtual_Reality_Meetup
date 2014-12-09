using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPGUI : MonoBehaviour
{

    private Text _text;

	// Use this for initialization
	void Start ()
	{

	    _text = GetComponent<Text>();

	}
	
	// Update is called once per frame
	void Update ()
	{
        if (Player.Instance != null && _text)
	        _text.text = "Health: " + Player.Instance.Health.ToString();

	}
}
