using UnityEngine;
using System.Collections;

public class TrollSpawner : MonoBehaviour {

    public Transform troll;
    public Transform player;

    public bool canSpawn = true;
    public float spawnTimeRandom;
    public float spawnTime = 0.1f;

    private float spawnTimer;
    private AudioSource _audio;

	// Use this for initialization
	void Start ()
	{
        if(Player.Instance != null)
	    player = Player.Instance.transform;

	    _audio = GetComponent<AudioSource>();
        ResetSpawnTimer();
	
	}
	
	// Update is called once per frame
	void Update () {
        if (canSpawn && player && troll)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0.0f)
            {
                if (_audio)
                    _audio.Play();

                var trolls = troll.GetComponent<AIPath>();
                trolls.target = player;
                Instantiate(troll, transform.position, Quaternion.identity);
                ResetSpawnTimer();
            }
        }
	
	}

    void ResetSpawnTimer()
    {
        spawnTimer = (float)(spawnTime + Random.Range(0, spawnTimeRandom * 100) / 100.0);
    }

    void StopSpawning()
    {
        canSpawn = false;
    }
}
