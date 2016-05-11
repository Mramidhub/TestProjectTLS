using UnityEngine;
using System.Collections;

public class MoveEnemy : MonoBehaviour {

    public Transform GameCharacter;
    public float speed;
	public float AttackSpeed;
    public float Health;
    public float PAttack;
	public float AttackRange;
	public float Armor;
	public int Gold;
	public int Exp;
    // Показатели персонажа.

    float SphereCastTimer = 0.5f;

    Vector3 EndPoint;
    Vector3 PointDestination;

    bool Moving;
    bool AttackOn;
    bool NonTarget;
    bool Ontarget;
    bool Die;

    GameObject Target;
    public GameObject TargetMark;


	void Start ()
    {
        Die = false;
        Moving = false;
        NonTarget = true;
        EndPoint = new Vector3(49f, 0, 58f);
        PointDestination = EndPoint;
	}


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

    void OnTriggerEnter(Collider other)
    {
		if (other.transform.tag == "Minion" || other.transform.tag == "DevelopersSofa" || other.transform.tag == "Player")
        {
            Target = other.gameObject;
            NonTarget = false;
        }

    }
    // Если кто-то зашел в коллайдет с тригером моб берет его в Target.

    void Dead()
    {
        if (Health < 0.3f)
        {

            StartCoroutine(DeadTime());
       

        }

    }
	// Смерть.

    void MoveToTarget()
    {
        if (NonTarget == false && LevelScript.EndGameLoose == false)
        {
            EndPoint = Target.GetComponent<Transform>().position;
            MoveTo();
            AttackTarget();

        }
    }
	// Движение к Target.

    void AttackTarget()
    {
        if (Vector3.Distance(Target.GetComponent<Transform>().position, transform.position) < 1.5f)
        {
            GameCharacter.GetComponent<Animation>().CrossFade("attack01");
            if (Target.tag == "Minion" ||  Target.tag == "Player")
            {
                Target.GetComponent<MoveMinion>().Health -= PAttack * Time.deltaTime;
            }
            else if (Target.tag == "DevelopersSofa")
            {
                Target.GetComponent<Sofa>().Health -= PAttack * Time.deltaTime;
            }
            
            if (Target.tag == "Minion" || Target.tag == "Player" && Target.GetComponent<MoveMinion>().Health <= 0)
            {
                NonTarget = true;
                EndPoint = PointDestination;
            }
            if (Target.tag == "DevelopersSofa" && Target.GetComponent<Sofa>().Health <= 0)
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
	// Атака Target.

    void MoveTo()
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
    // Движение к заданной точке.

    IEnumerator DeadTime()
    {
        Die = true;
        GameCharacter.GetComponent<Animation>().CrossFade("dead");
        
        yield return new WaitForSeconds(2f);

        Destroy(GameCharacter.gameObject);

    }
    // Моб падает и через несколько секунд GO уничтожаеться.
}
