using UnityEngine;
using System.Collections;

public class TrollSpawner : MonoBehaviour {

    public Transform troll;
    public Transform player;

    public bool canSpawn = true;
    public float spawnTimeRandom;
    private float spawnTime = 0.1f;

    private float spawnTimer;

	// Use this for initialization
	void Start ()
	{

	    player = GameObject.FindGameObjectWithTag("Player").transform;
        ResetSpawnTimer();
	
	}
	
	// Update is called once per frame
	void Update () {
        if (canSpawn && player && troll)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0.0f)
            {
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
