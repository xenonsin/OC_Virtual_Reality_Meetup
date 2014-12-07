using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int _killCount = 0;
    private float _playerHP;

    void OnEnable()
    {
        
    }

	// Use this for initialization
	void Start ()
	{
	    Instance = this;
        if (Player.Instance != null)
            _playerHP = Player.Instance.MaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateKillCount()
    {
        _killCount += 1;
    }

    void OnDestroy()
    {
        Instance = null;

    }
}
