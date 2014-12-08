using Pathfinding.RVO;
using UnityEngine;
using System.Collections;

public class Warrior : Entity
{

    private Animator _animator;

    public AudioClip growl;
    
    public override void Awake()
    {
        if (growl)
            audio.PlayOneShot(growl, 0.3f);

        _animator = GetComponent<Animator>();

        base.Awake();
    }

    public override void OnEnable()
    {
        if(GameManager.Instance != null)
            Died += GameManager.Instance.UpdateKillCount;
        base.OnEnable();
    }

    public override void OnDisable()
    {
        if (GameManager.Instance != null)
            Died -= GameManager.Instance.UpdateKillCount;
        base.OnDisable();
    }

    public override void Hit(float damage)
    {
        if (_animator)
            _animator.SetTrigger("Hit");

        

        base.Hit(damage);
    }

    public override void Death()
    {
        if (_animator)
            _animator.SetBool("Dead", true);

        var rvo = GetComponent<RVOController>();

        if (rvo)
            rvo.enabled = false;
        gameObject.tag = "Dead";
        base.Death();
        StartCoroutine(AutoDeath(10.0f));
    }

    public void OnTriggerEnter(Collider co)
    {
        var ball = co.gameObject.GetComponent<Ball>();

        if (ball != null)
        {
            Hit(1f);
            Destroy(ball.gameObject);
        }
    }

    IEnumerator AutoDeath(float f)
    {
        yield return new WaitForSeconds(f);
        Destroy(gameObject);
    }
}
