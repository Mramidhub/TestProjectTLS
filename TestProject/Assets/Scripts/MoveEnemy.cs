using UnityEngine;
using System.Collections;

public class MoveEnemy : MonoBehaviour {

    public Transform GameCharacter;
    public float speed;
    public float Health = 200;
    public float PAttack;
	float SphereCastTimer = 0.5f;

    Vector3 EndPoint;
    Vector3 PointDestination;

    bool Moving;
    bool AttackOn;
    bool NonTarget;
    bool Ontarget;
    // Если NPC взят в таргет игроком
    bool Die;

    GameObject Target;

	// Use this for initialization
	void Start () {
        Die = false;
        Moving = false;
        NonTarget = true;
        EndPoint = new Vector3(46f, 0, 34f);
        PointDestination = EndPoint;


	}
	
	// Update is called once per frame
	void Update () {

        if (Die == false)
        {
            MoveToTarget();
        }
        if (NonTarget == true)
        {
            MoveTo();
        }
        Dead();
    }

	void FixedUpdate()
	{



	}

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Minion")
        {
            Target = other.gameObject;
            NonTarget = false;
        }

    }

    void Dead()
    {
        if (Health < 0.3f)
        {

            StartCoroutine(DeadTime());
       

        }

    }

    void MoveToTarget()
        // Движение к target.
    {
        if (NonTarget == false)
        {
            EndPoint = Target.GetComponent<Transform>().position;
            MoveTo();
            AttackTarget();

        }
    }

    void AttackTarget()
    {
		if (Vector3.Distance (Target.GetComponent<Transform> ().position, transform.position) < 1.5f)
        {
			GameCharacter.GetComponent<Animation> ().CrossFade ("attack01");
			Target.GetComponent<MoveMinion> ().Health -= PAttack * Time.deltaTime;
			if (Target.GetComponent<MoveMinion> ().Health < 0)
            {
				NonTarget = true;
                EndPoint = PointDestination;
            }

		}
		else
		{
			NonTarget = false;
		}
    }

    void MoveTo()
        // Движение к заданной точке
    {
        Vector3 Direction = EndPoint - transform.position;
        Direction.Normalize();
        transform.LookAt(EndPoint);
        GameCharacter.GetComponent<Animation>().CrossFade("run");
        float TargetPosition = Vector3.Distance(transform.position, EndPoint);

        if (TargetPosition > 1.5f)
        {
            transform.Translate(Direction * speed, Space.World);
        }
		else if(NonTarget == true)
        {
            GameCharacter.GetComponent<Animation>().CrossFade("stand");

        }


    }

    IEnumerator DeadTime()
    {
        Die = true;
        GameCharacter.GetComponent<Animation>().CrossFade("dead");
        
        yield return new WaitForSeconds(2f);

        Destroy(GameCharacter.gameObject);

    }
}
