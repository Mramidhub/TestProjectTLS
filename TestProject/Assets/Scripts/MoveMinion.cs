using UnityEngine;
using System.Collections;

public class MoveMinion : MonoBehaviour {

    public Transform GameCharacter;
    public float speed;
    public float Health = 200;
    public float PAttack;

    Vector3 EndPoint;
    Vector3 PointDestination;

    bool Moving;
    bool AttackOn;
    bool NonTarget;
    bool Die;


    float MoveToRandomPortal;

    GameObject Target;
    // Use this for initialization
    void Start () {

        LevelScript.Unit.Add(gameObject);
        // Добавляем gameObject в коллекицю unit в LevelScript. Для того, что бы его омжно было исопльзовать в выделении рамкой.
        Die = false;
        Moving = false;
        NonTarget = true;
        RandomEndPoint();

    }
	
	// Update is called once per frame
	void Update ()
    {

        if (Die == false)
        {
            MoveToTarget();


        }
        if (NonTarget == true)
        {
            MoveTo();
        }
        Dead();
        DestroyOnTarget();

    }

	void FixedUpdate()
	{


	}

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            Target = other.gameObject;
            NonTarget = false;
        }

    }
    // Если кто то входит в коллайдер миньон берет его в target.

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
        PointDestination = EndPoint;

    }

    void Dead()
    {
        if (Health < 0)
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
        if (Vector3.Distance(Target.GetComponent<Transform>().position, transform.position) < 1.5f)
        {
            GameCharacter.GetComponent<Animation>().CrossFade("attack");
            Target.GetComponent<MoveEnemy>().Health -= PAttack*Time.deltaTime;
			if (Target.GetComponent<MoveEnemy>().Health < 0)
            {
                NonTarget = true;
                EndPoint = PointDestination;
            }
			else
			{
				NonTarget = false;
			}
        }
    }
    // Атака target.

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
		else if (NonTarget == true)
        {
            GameCharacter.GetComponent<Animation>().CrossFade("idle");

        }


    }

    public void DestroyOnTarget()
    {
        if (LevelScript.OnTargetNPC == false )
        {
            if (transform.Find("Target(Clone)").gameObject)
            {
              //  Destroy(transform.Find("Target(Clone)").gameObject);
            }
            
        }
    }
    // Если попали в пустоту, уничтожаем дочерний обьект таргет-метки, если он есть.

    IEnumerator DeadTime()
    {
        Die = true;

        GameCharacter.GetComponent<Animation>().CrossFade("die");

        yield return new WaitForSeconds(2f);

        Destroy(GameCharacter.gameObject);

    }
}
