using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour
{
    public delegate void Action();
    public static event Action HpChange;
    public static event Action GotHit;
    public static event Action Healed;
    public static event Action Died;

    public string Name { get; set; }
    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public float MinDamage{ get; set; }
    public float MaxDamage { get; set; }
    public float Speed { get; set; }
    public int Team { get; set; }
    public bool IsDead { get; set; }


    public virtual void OnEnable()
    {

    }

    public virtual void OnDisable()
    {

    }


    public virtual void Awake()
    {
        IsDead = false;
        Health = MaxHealth;
    }

    public virtual void Update()
    {
        if (Health <= 0)
            Death();
    }

    public virtual void Hit(float damage)
    {
        Health -= damage;
        Debug.Log(Name + " was hit for " + damage + " damage!");
        Debug.Log(Name + " Health: " + Health.ToString());

        if (HpChange != null)
            HpChange();

        if (GotHit != null)
            GotHit();
    }

    public virtual void Heal(float heal)
    {
        Health += heal;
        Debug.Log(Name + " was healed for " + heal + " hp!");
        Debug.Log(Name + " Health: " + Health.ToString());

        if (HpChange != null)
            HpChange();

        if (Healed != null)
            Healed();
    }

    public virtual void Death()
    {
        if (IsDead) return;
        Debug.Log(Name + " has died!");
        IsDead = true;
        if (Died != null)
            Died();
        //Destroy(gameObject);
    }

}
	
