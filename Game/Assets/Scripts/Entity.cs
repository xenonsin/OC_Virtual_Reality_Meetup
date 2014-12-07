using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour
{
    public delegate void Action();
    public static event Action HpChange;
    public static event Action GotHit;
    public static event Action Healed;
    public static event Action Died;

    [SerializeField] private string _name;
    public string Name { get { return _name; } set { _name = value; } }
    [SerializeField] private float _health;
    public float Health { get { return _health; } set { _health = value; } }
    [SerializeField] private float _maxHealth;
    public float MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
    [SerializeField] private float _minDamage;
    public float MinDamage{ get { return _minDamage; } set { _minDamage = value; } }
    [SerializeField] private float _maxDamage;
    public float MaxDamage { get { return _maxDamage; } set { _maxDamage = value; } }
    [SerializeField] private float _maxSpeed;
    public float MaxSpeed { get { return _maxSpeed; } set { _maxSpeed = value; } }
    [SerializeField] private int _team;
    public int Team { get { return _team; } set { _team = value; } }
    [SerializeField]  private bool _isDead;
    public bool IsDead { get { return _isDead; } set { _isDead = value; } }


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
	
