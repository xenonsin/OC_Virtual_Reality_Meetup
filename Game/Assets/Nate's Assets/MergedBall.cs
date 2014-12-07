using UnityEngine;
using System.Collections;

public class MergedBall : Ball {

    public float blastRange = 5f;
    public float damage = 3f;

    GestureController gc;
	// Use this for initialization
	void Start () {
        gc = GestureController.Instance;
        //StartCoroutine(Autodeath(6.0f));
        Invoke("GetBlastRadius", particleSystem.duration);
	}
	new void Update () {
        

	}

    private void GetBlastRadius()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, blastRange);
        foreach (var hit in hitColliders)
        {
            if (hit && hit.tag == "Enemy")
            {
                DealDamage(hit);
            }
        }
    }

    void DealDamage(Collider hit)
    {
        var objectThatWasHit = hit.transform.GetComponent<Entity>();

        if (objectThatWasHit)
            objectThatWasHit.Hit(damage);
    }

    IEnumerator Autodeath(float f)
    {
        yield return new WaitForSeconds(f);
        Destroy(gameObject);
    }
}
