using System.Runtime.Remoting.Messaging;
using UnityEngine;
using System.Collections;
using System.Linq;

[System.Serializable]
public class Ball : MonoBehaviour {

    public bool Attached = false;
    public Transform Attachee = null;
    public Conditional.WhichHands WhichHand;
    public bool Launched = false;

    public GameObject explosion;
    
	
	// Update is called once per frame
	public void Update () {
        if (Attached)
        {
            if (Attachee != null)
            {
                transform.position = Attachee.transform.position;
            }
            else
            {
                if (!Launched)
                    Destroy(gameObject);
            }
        }
        else
        {
            if (!Launched)
            {
                Destroy(gameObject);
            }
            else
            {

                GameObject closest = FindClosestEnemy();

                if (closest != null)
                {

                    StartCoroutine(Launch(10.0f, closest));
                }
                else
                {
                    StartCoroutine(Launch(10.0f));
                }
            }
        }
	}

    public void OnDestroy()
    {
        //Instantiate particle effect
        if (explosion)
            Instantiate(explosion, transform.position, Quaternion.identity);
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;

    }

    IEnumerator Launch(float f, GameObject closest)
    {
        float travelledDistance = 0;

            while (travelledDistance < 10f)
            {
                Vector3 pos = closest.transform.position;
                pos.y += 2;
                transform.position = Vector3.MoveTowards(transform.position, pos,
                    1.0f*Time.deltaTime);
                yield return 0;
                travelledDistance += Time.deltaTime;

            }
        Destroy(gameObject);

        
    }

    IEnumerator Launch(float f)
    {
        float travelledDistance = 0;
        Transform transformLauncher = BallAimer.Instance.transform;
        while (travelledDistance < 10f)
        {
            rigidbody.AddForce(transformLauncher.forward*1.0f);
            travelledDistance += Time.deltaTime;
            yield return 0;
        }
        //yield return new WaitForSeconds(f);
        Destroy(gameObject);
    }
}
