using UnityEngine;
using System.Collections;

public class MoveMinion : MonoBehaviour {

    public Transform GameCharacter;
    public float speed;
    public float Health = 200;
    public float PAttack;
    // Показатели персонажа.

    Vector3 EndPoint;
    Vector3 PointDestination;

    bool Moving;
    bool NonTarget;
    bool Die;
    bool AutoMove;

    Vector3 TempNameObject;

    float MoveToRandomPortal;
    public GameObject TargetMark;

    GameObject Target;
    // Use this for initialization
    void Start () {

        TempNameObject = transform.position;
        gameObject.name = TempNameObject.ToString();
        LevelScript.Unit.Add(gameObject);

        // Добавляем gameObject в коллекицю unit в LevelScript. Для того, что бы его омжно было исопльзовать в выделении рамкой.
        Die = false;
        Moving = false;
        NonTarget = true;
        AutoMove = true;
        RandomEndPoint();

    }
	
	// Update is called once per frame
	void Update ()
    {
        ControlTarget();
        Dead();

        if (Die == false)
        {
            MoveToTarget();


        }
        if (NonTarget == true)
        {
            MoveTo();
        }
        
    }

    void ControlTarget()
    {
        if (TargetMark.GetComponent<Renderer>().enabled == true)
        {
            AutoMove = false;

            if (Input.GetMouseButtonDown(1))
            {
                NonTarget = true;
                Debug.Log("5");
                RaycastHit Hitt;
                Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray1, out Hitt, Mathf.Infinity))
                {
                    if (Hitt.transform.tag != "Enemy")
                    {
                        Debug.Log("6");
                        EndPoint = Hitt.point;
                    }
                    else if (Hitt.transform.tag == "Enemy")
                    {
                        Debug.Log("3");
                        NonTarget = false;
                        Target = Hitt.transform.gameObject;
                        Debug.Log(Target.ToString());
                    }


                }

            }   
        }

    }

    void FixedUpdate()
	{


	}

    void OnTriggerEnter(Collider other)
    {
        if (AutoMove == true)
        {
            if (other.transform.tag == "Enemy")
            {
                Target = other.gameObject;
                NonTarget = false;
            }
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
       // Debug.Log("Attack!!!");
        if (Vector3.Distance(Target.GetComponent<Transform>().position, transform.position) < 1.5f)
        {
            GameCharacter.GetComponent<Animation>().CrossFade("attack");
            Target.GetComponent<MoveEnemy>().Health -= PAttack*Time.deltaTime;
			if (Target.GetComponent<MoveEnemy>().Health < 0)
            {
                Debug.Log("1");
                NonTarget = true;
                EndPoint = PointDestination;
            }
			else
			{
               // Debug.Log("2");
                NonTarget = false;
			}
        }
    }
    // Атака target.

    void MoveTo()
    // Движение к заданной точке.
    {
        Vector3 Direction = EndPoint - transform.position;
        Direction = new Vector3(Direction.x, 0, Direction.z);
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

    IEnumerator DeadTime()
    {
        Die = true;


        GameCharacter.GetComponent<Animation>().CrossFade("die");

        yield return new WaitForSeconds(2f);

        string tempname = gameObject.name;

        int x = LevelScript.Unit.Count;

        int index = LevelScript.Unit.FindIndex(obj => obj.name == tempname);

        LevelScript.Unit.RemoveAt(index);

        Destroy(transform.gameObject);
    }
    // Ждем пару сек ипрежде чем убить обьект находим его индекс в коллецкии Unit и удалем элемент.
}
