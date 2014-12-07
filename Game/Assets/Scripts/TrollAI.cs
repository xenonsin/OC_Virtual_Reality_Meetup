using UnityEngine;
using System.Collections;

public class TrollAI : MonoBehaviour
{

    public float minAtkRange = 10.0f;
    public float attackDelay = 1.0f;
    public float meleeRange = 3.0f;
    public float meleeAngle = 50.0f;


    private AIPath _trollAIPath;
    private Entity _troll;
    private Animator _animator;
    private bool IsCurrentlyAttacking = false;


    private enum State
    {
        Idle,
        Following,
        Attacking
    }

    private State currentState = State.Idle;

	// Use this for initialization
	void Start ()
	{

	    _trollAIPath = GetComponent<AIPath>();
	    _troll = GetComponent<Warrior>();
	    _animator = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
        if (_trollAIPath && _troll && !_troll.IsDead)
	        GetDistance();

	    switch (currentState)
	    {
	        case State.Idle:
	        {
	            //animation.Play("Idle");
	            break;
	        }    	           
            case State.Following:
	        {
	            _trollAIPath.canMove = true;
	            _animator.SetBool("Following", true);
	            break;
	        }
            case State.Attacking:
	        {
	            _trollAIPath.canMove = false;
                _animator.SetBool("Is Attacking", true);
                if (!IsCurrentlyAttacking)
	                StartCoroutine(AttackDelay(attackDelay));
	            break;
	        }
                
	            

	    }
	    
	}

    void GetDistance()
    {
        if (_trollAIPath.target == null) return;

        var playerDistance = Vector3.Distance(transform.position, _trollAIPath.target.position);

        if (playerDistance > meleeRange)
        {
            
            if (currentState == State.Attacking)
            {
                StopAllCoroutines();
                IsCurrentlyAttacking = false;
                _animator.SetBool("Is Attacking", false);
            }
            currentState = State.Following;
        }
        else
            currentState = State.Attacking;
    }

    IEnumerator AttackDelay(float delay)
    {
        IsCurrentlyAttacking = true;
        yield return new WaitForSeconds(0.9f);
       // _spriteManager.IsAttacking = true;
        CheckRange();

        yield return new WaitForSeconds(0.55f);
        IsCurrentlyAttacking = false;
    }

    void CheckRange()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, meleeRange);

        foreach (var hit in hitColliders)
        {
            if (hit && hit.tag == "Player")
            {
                var cone = Mathf.Cos(meleeAngle * Mathf.Deg2Rad);
                Vector3 dir = (hit.transform.position - transform.position).normalized;

                if (Vector3.Dot(transform.forward, dir) > cone)
                {
                    //Target is within the cone.
                    Debug.Log("Attack hit!");
                    DealDamage(hit);
                }
            }
            else
            {
                //some how this plays even though it hits. Not sure why
               // PlaySound("miss3");
            }
        }
    }

    void DealDamage(Collider hit)
    {
        var objectThatWasHit = hit.transform.GetComponent<Entity>();
        float damage = Random.Range(_troll.MinDamage, _troll.MaxDamage);

        if(objectThatWasHit)
            objectThatWasHit.Hit(damage);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);


        Debug.DrawRay(transform.position, transform.forward * meleeRange, Color.red);
        Debug.DrawRay(transform.position, (Quaternion.Euler(0, meleeAngle, 0) * transform.forward).normalized * meleeRange, Color.red);
        Debug.DrawRay(transform.position, (Quaternion.Euler(0, meleeAngle, 0) * transform.forward).normalized * meleeRange, Color.red);
    }
}
