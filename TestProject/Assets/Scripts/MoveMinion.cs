using UnityEngine;
using System.Collections;

public class MoveMinion : MonoBehaviour {

    public Transform GameCharacter;
    public float speed;
	public float AttackSpeed;
    public float Health = 200;
    public float PAttack;
	public float AttackRange;
	public float Armor;
	public int Gold;
	public int Exp;
    // Показатели персонажа.

    Vector3 EndPoint;
    Vector3 PointDestination;

    bool Moving;
    bool NonTarget;
    bool Die;
    bool AttakingOn;

    Vector3 TempNameObject;

    float MoveToRandomPortal;
    public GameObject TargetMark;

    float Timer = 1f;

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
        EndPoint = transform.position;
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
          if (Input.GetMouseButtonDown(1))
            {
                NonTarget = true;
                RaycastHit Hitt;
                Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray1, out Hitt, Mathf.Infinity))
                {
                    if (Hitt.transform.tag != "Enemy")
                    {
                        EndPoint = Hitt.point;
                    }
                    else if (Hitt.transform.tag == "Enemy")
                    {
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
            if (other.transform.tag == "Enemy")
            {
                Target = other.gameObject;
                Debug.Log(Target.transform.tag.ToString());
                NonTarget = false;
                transform.GetComponent<SphereCollider>().enabled = false;
        }
    }
    // Если кто то входит в коллайдер миньон берет его в target.

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
            Debug.Log("sdasd");

        }
    }

    void AttackTarget()
    {
        if (Vector3.Distance(Target.GetComponent<Transform>().position, transform.position) < 1.5f)
        {
            Target.GetComponent<MoveEnemy>().Health -= PAttack*Time.deltaTime;
            GameCharacter.GetComponent<Animation>().Play("attack");
            if (Target.GetComponent<MoveEnemy>().Health < 0)
            {
                NonTarget = true;
                transform.GetComponent<SphereCollider>().enabled = true;
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
        Direction = new Vector3(Direction.x, 0, Direction.z);
        Direction.Normalize();
        transform.LookAt(EndPoint);
        GameCharacter.GetComponent<Animation>().Play("run");
        
        float TargetPosition = Vector3.Distance(transform.position, EndPoint);

        if (TargetPosition > 1.5f)
        {
            transform.Translate(Direction * speed, Space.World);

        }
		else if (NonTarget == true)
        {
            GameCharacter.GetComponent<Animation>().Play("idle");

        }


    }

    IEnumerator DeadTime()
    {
        Die = true;


        GameCharacter.GetComponent<Animation>().Play("die");

        yield return new WaitForSeconds(2f);

        string tempname = gameObject.name;

        int x = LevelScript.Unit.Count;

        int index = LevelScript.Unit.FindIndex(obj => obj.name == tempname);

        LevelScript.Unit.RemoveAt(index);

        Destroy(transform.gameObject);
    }

    IEnumerator Attacking()
    {
        AttakingOn = true;

        GameCharacter.GetComponent<Animation>().Play("attack");

        yield return new WaitForSeconds(0.5f);

        AttakingOn = false;

    }
    // Ждем пару сек ипрежде чем убить обьект находим его индекс в коллецкии Unit и удалем элемент.
}
