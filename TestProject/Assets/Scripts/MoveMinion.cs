using UnityEngine;
using System.Collections;

public class MoveMinion : MonoBehaviour {

    public Transform GameCharacter;
    public float speed;
    public float AttackSpeed = 5f;
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
    bool ReturnToSofa;

    Vector3 TempNameObject;

    float MoveToRandomPortal;
    public GameObject TargetMark;

    float Timer = 1f;

    GameObject Target;

    public Animation AnimMinion;
    // Use this for initialization
    void Start () {




        TempNameObject = transform.position;
        gameObject.name = TempNameObject.ToString();
        LevelScript.Unit.Add(gameObject);

        // Добавляем gameObject в коллекицю unit в LevelScript. Для того, что бы его омжно было исопльзовать в выделении рамкой.
        Die = false;
        Moving = false;
        NonTarget = true;
        ReturnToSofa = false;
        EndPoint = transform.position;
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
            NoEnemy();
        }

        EnemyTargetMarkOn();
        ControlTarget();
        Dead();

    }

    void EnemyTargetMarkOn()
    {
        if (Target != null)
        {
            if (transform.Find("TargetMark").GetComponent<Renderer>().enabled == true)
            {
                Target.transform.Find("TargetMark").GetComponent<Renderer>().enabled = true;
            }
            else
            {
                Target.transform.Find("TargetMark").GetComponent<Renderer>().enabled = false;
            }
        }
    }    
    // Включение метки взятой в таргет цели, если выбран миьон. 

    void ControlTarget()
    {
        if (TargetMark.GetComponent<Renderer>().enabled == true)
        {
          if (Input.GetMouseButtonDown(1))
            {
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
                    }


                }

            }   
        }

    }
    // Контролб мобов игроком.

    void OnTriggerEnter(Collider other)
    {
       if (other.transform.tag == "Enemy" && transform.GetComponent<SphereCollider>().enabled == true)
       {
            ReturnToSofa = false;
            Target = other.gameObject;
            NonTarget = false;
            transform.GetComponent<SphereCollider>().enabled = false;
            transform.GetComponent<SphereCollider>().isTrigger = false;
        }
    }
    // Если кто то входит в коллайдер миньон берет его в target.

    void Dead()
    {
        if (Health < 0.2f)
        {
            StartCoroutine(DeadTime());

        }

    }
    // Смерть.

    void MoveToTarget()
    // Движение к target.
    {
        if (NonTarget == false)
        {
            if (Target != null)
            {
                EndPoint = Target.GetComponent<Transform>().position;
            }
            MoveTo();
            if (Target != null)
            {
                AttackTarget();
            }

        }
    }

    void AttackTarget()
    {
        if (Vector3.Distance(Target.GetComponent<Transform>().position, transform.position) < AttackRange)
        {
            Moving = false;
            GameCharacter.GetComponent<Animation>().CrossFade("attack");
            AnimMinion["attack"].speed = AttackSpeed;

            if (Target.tag == "Enemy")
            {
                 float TempPAttack = (PAttack / 100) * (100 - Target.GetComponent<MoveEnemy>().Armor);
                 // Высчитываем нанесенный урон, с учетом армора  цели.
                 Target.GetComponent<MoveEnemy>().Health -= TempPAttack * Time.deltaTime * AttackSpeed;
                 // Наносим урон с учетом скорости атаки.
            }
            if (Target.tag == "Enemy" && Target.GetComponent<MoveEnemy>().Health <= 0)
            {
                NonTarget = true;
                EndPoint = transform.position;
                transform.GetComponent<SphereCollider>().enabled = true;
                transform.GetComponent<SphereCollider>().isTrigger = true;
            }
        }
        else
        {
            NonTarget = false;
        }
    }
    // Атака target.

    void MoveTo()
    {
        if (AttakingOn == false)
        {
            Vector3 Direction = EndPoint - transform.position;
            Direction = new Vector3(Direction.x, 0, Direction.z);
            Direction.Normalize();
            transform.LookAt(EndPoint);
            GameCharacter.GetComponent<Animation>().CrossFade("run");
            float TargetPosition = Vector3.Distance(transform.position, EndPoint);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction), speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
            if (TargetPosition > 1.5f)
            {
                 transform.Translate(Direction * speed * Time.deltaTime, Space.World);
            }
            else if (NonTarget == true)
            {
                GameCharacter.GetComponent<Animation>().CrossFade("idle");

            }
        }
    }
    // Движение к заданной точке.

    void NoEnemy()
    {
        if (LevelScript.EnemyOnMap == false && LevelScript.StartGame == true)
        {
            EndPoint = new Vector3(52f, 0 ,36f);
            if (transform.position.z < 38f)
            {
                EndPoint = transform.position;
                ReturnToSofa = true;
            }
        }
        if (LevelScript.EnemyOnMap == true && ReturnToSofa == true)
        {
            EndPoint = new Vector3(47f, 0, 62f);
            if (transform.position.z > 61f)
            {
                EndPoint = transform.position;
                ReturnToSofa = false;
            }

        }

    }
    // Пhи отсутствии врагов при активной таргет-метке мимньоны возвращаются к фонтану. ПРи их появлении возвращаются к дивнау. 
    // При появлении вргаов на карте все миньоны возвращаються к дивану.
    // При вхождении в тригер коллайдер врага, переменная возвращения к дивану устанавлияваеться на false.(смотреть OnTriggerEnter)


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

    IEnumerator Attacking()
    {
        AttakingOn = true;

        GameCharacter.GetComponent<Animation>().CrossFade("attack");

        yield return new WaitForSeconds(0.5f);

        AttakingOn = false;

    }
    // Ждем пару сек ипрежде чем удалить обьект находим его индекс в коллецкии Unit и удалем элемент.
}
