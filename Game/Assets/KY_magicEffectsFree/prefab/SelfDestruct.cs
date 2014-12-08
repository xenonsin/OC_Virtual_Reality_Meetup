using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    StartCoroutine(AutoDestroy(particleSystem.duration + 0.5f));
	}

    public AudioClip clipToPlay
    {
        set
        {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            if (audioSource != null)
                audioSource.clip = value;
            audioSource.Play();
        }

    }

    IEnumerator AutoDestroy(float f)
    {
        yield return new WaitForSeconds(f);
        Destroy(gameObject);
    }

    void OnDisable()
    {
        Destroy((gameObject));
    }
}
