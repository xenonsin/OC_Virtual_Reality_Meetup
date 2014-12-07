using UnityEngine;
using System.Collections;

public class Warrior : Entity
{

    public string nameString;
    public float maxhp = 1f;
    public float speed = 1f;
    public float minDamage = 1f;
    public float maxDamage = 2f;
    public int team = 1;

    private Animator _animator;

    public override void Awake()
    {
        Name = nameString;
        MaxHealth = maxhp;
        MaxDamage = maxDamage;
        MinDamage = minDamage;
        Speed = speed;
        Team = team;

        _animator = GetComponent<Animator>();

        base.Awake();
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

        base.Death();
    }
}
