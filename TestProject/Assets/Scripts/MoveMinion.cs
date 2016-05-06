﻿using UnityEngine;
using System.Collections;

public class MoveMinion : MonoBehaviour {

    public Transform GameCharacter;
    public float speed;
    public float Health;
    public float PAttack;

    Vector3 EndPoint;

    bool Moving;
    bool AttackOn;
    bool NonTarget;
    bool Live;
    public bool IDead;

    float MoveToRandomPortal;

    GameObject Target;
    // Use this for initialization
    void Start () {

        Moving = false;
        NonTarget = true;
        Live = true;
        IDead = false;
        RandomEndPoint();

    }
	
	// Update is called once per frame
	void Update () {

        NonTargetMove();
        MoveToTarget();
        Dead();

    }

    void RandomEndPoint()
    {
        MoveToRandomPortal = Random.Range(0f, 3f);
        if (MoveToRandomPortal < 1f)
        {
            EndPoint = new Vector3(84f, 0, 94f);
        }
        else if (MoveToRandomPortal < 2f)
        {
            EndPoint = new Vector3(50f, 0, 94f);
        }
        else if (MoveToRandomPortal < 3f)
        {
            EndPoint = new Vector3(14f, 0, 94f);
        }

    }

    void Dead()
    {
        if (Health < 0)
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
        if (Vector3.Distance(Target.GetComponent<Transform>().position, transform.position) < 4f)
        {
            GameCharacter.GetComponent<Animation>().CrossFade("attack");
            Target.GetComponent<MoveEnemy>().Health -= PAttack*Time.deltaTime;
            if (Target.GetComponent<MoveEnemy>().IDead == true)
            {
                NonTarget = true;   
            }

        }
    }


    void NonTargetMove()
    // Движение Enemy без цели
    {
        if (NonTarget == true)
        {
            MoveTo();
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 5f, transform.forward, out hit, 5f))
            // С помощью СферКаста ищем ближайший тег енеми, либо портала.
            // Берем выбранный обьект в таргет.
            {
                if (hit.transform.tag == "Enemy" || hit.transform.tag == "Portal")
                {
                    Target = hit.transform.gameObject;
                    NonTarget = false;
                }
            }
        }


    }

    void MoveTo()
    // Движение к заданной точке.
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
        else
        {
            GameCharacter.GetComponent<Animation>().CrossFade("idle");

        }


    }

    IEnumerator DeadTime()
    {
        GameCharacter.GetComponent<Animation>().CrossFade("die");

        yield return new WaitForSeconds(2f);

        Destroy(this.gameObject);

    }
}
