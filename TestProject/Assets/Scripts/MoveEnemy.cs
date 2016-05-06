﻿using UnityEngine;
using System.Collections;

public class MoveEnemy : MonoBehaviour {

    public Transform GameCharacter;
    public float speed;
    public float Health = 200;
    public float PAttack;
	float SphereCastTimer = 0.5f;

    Vector3 EndPoint;

    bool Moving;
    bool AttackOn;
    bool NonTarget;
    bool Live;
    public bool IDead;

    GameObject Target;





	// Use this for initialization
	void Start () {
        Moving = false;
        NonTarget = true;
        Live = true;
        IDead = false;
        EndPoint = new Vector3(46f, 0, 34f);


	}
	
	// Update is called once per frame
	void Update () {

		NonTargetMove();
        MoveToTarget();
        Dead();
    }

	void FixedUpdate()
	{



	}

    void Dead()
    {
        if (Health < 0.3f)
        {
            Live = false;
            IDead = true;
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
		if (Vector3.Distance (Target.GetComponent<Transform> ().position, transform.position) < 1.5f) {
			GameCharacter.GetComponent<Animation> ().CrossFade ("attack01");
			Target.GetComponent<MoveMinion> ().Health -= PAttack * Time.deltaTime;
			if (Target.GetComponent<MoveMinion> ().Health < 0) {
				NonTarget = true;
			}

		}
		else
		{
			NonTarget = false;
		}
    }


    void NonTargetMove()
        // Движение Enemy без цели
    {
		if (NonTarget == true) {
			float random = 1f;
			Vector3 RandomSide;
			random -= Time.deltaTime;

			if (random < 0.5) {
				RandomSide = transform.forward;
				random = 1f;
			}
			else
			{
				RandomSide = transform.forward * -1;
				random = 1f;
			}
			MoveTo ();
			SphereCastTimer -= Time.deltaTime;

			if (SphereCastTimer < 0) 
			{
				RaycastHit hit;
				if (Physics.SphereCast (transform.position, 5f, RandomSide, out hit, 5f))
            // С помощью СферКаста ищем ближайший тег миньона, либо игрока, либо ворот
					// берем выбранный обьект в таргет
				{     
					if (hit.transform.tag == "Player" || hit.transform.tag == "Minion" || hit.transform.tag == "Gate") {
						Target = hit.transform.gameObject;
						NonTarget = false;

                    
					} 
					else
					{
						SphereCastTimer = 0.5f;

					}
				}
			}
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
        GameCharacter.GetComponent<Animation>().CrossFade("dead");
        
        yield return new WaitForSeconds(3f);

        Destroy(GameCharacter.gameObject);

    }
}
