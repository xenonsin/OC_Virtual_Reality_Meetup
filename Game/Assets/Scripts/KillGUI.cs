using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KillGUI : MonoBehaviour {

    private Text _text;

	// Use this for initialization
	void Start () {
        _text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        if (GameManager.Instance != null)
            _text.text = "Kills: " + GameManager.Instance._killCount.ToString();
	
	}
}
