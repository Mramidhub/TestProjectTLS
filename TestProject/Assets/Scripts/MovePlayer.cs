using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MovePlayer : MonoBehaviour {

    public Button UpIA;
    public Button UpMS;
    public ParticleSystem LvlUpAnim;

    public int Lvl = 1;
    public int ExpUp2lvl;
    public int ExpUp3lvl;
    public float PAtkUp2lvl;
    public float PAtkUp3lvl;
    public float AtkSpdUp2lvl;
    public float AtkSpdUp3lvl;
    public float ArmorUp2lvl;
    public float ArmorUp3lvl;

    bool Upping;

    // Переменные для системы прогресса персонажа.

    public float PAttack = 100f;
    public float speed = 0.2f;
    public float Health = 200f;
	public float AtkSpeed = 400f;
	public float Armor = 100f;
	public int Gold = 0;
	public int Exp = 0;
    // Показатели персонажа.


    public Transform GameCharacter;
    public Text ReburnTIme;
    Vector3 EndPosition;
	bool AddToUnit;
    bool Moving;
    bool Attaking;
    bool Die;
    public bool MinionTargetOn;
    public bool ControlNPC;
    float ReburnTimer;
    public GameObject Target;

    public bool SkillUsed;

    public ParticleSystem MeteorShowerPartSys;
    public float MetShowDamage;
    bool MetHowOn;
    bool StartExplosion;
    bool ReusedOnMS;
    public Text ReusedTimerShowMS;
    public float ReusedTimeMS;
    // Переменные для скилла MeteorShower;.

    public int LvlSkillIA;
    public float IceArrowDamage;
    public float IceArrAtkSlow;
    public float IceArrSpdSlow;
    public float RadiusIA;
    public float Duration;
    Vector3 OldPosition;
    bool CreatArrow;
    bool MoveToRadius;
    bool ArrowDamage;
    bool IceArrOn;
    bool IceArrFly;
    bool ReusedOnIA;
    public Text ReusedTimerShowIA;
    public float ReusedTimeIA;
    public GameObject IceArrow;
    // Переменные для скилла IceArrow.

    public Animation AnimPlayer;


    // Use this for initialization
    void Start () 
	{
        UpIA.enabled = false;
        UpMS.enabled = false;
        LvlUpAnim.GetComponent<ParticleSystem>().enableEmission = false;
        SkillUsed = false;

        StartExplosion = false;
        MetShowDamage = 10;
        MetHowOn = false;
        ReusedTimeMS = 20f;
        ReusedOnMS = false;
        // MetShow.


        ReusedOnIA = false;
        IceArrowDamage = 400f;
        ArrowDamage = false;
        IceArrOn = false;
        IceArrFly = false;
        ReusedTimeIA = 15f;
        // IceArrow.


        Gold = 200;
        Exp = 0;
        MinionTargetOn = false;
        Die = false;
        ReburnTimer = 10f;
    }

	// Update is called once per frame
	void Update ()
    {
        LvlUp();
        HotKeyOn();
        ReusedIceArr();
        ReusedMetShower();

        if (IceArrOn == true)
        {
            IceArrowSkill();
        }
        if (MetHowOn == true)
        {
            MeteorShower();
        }

        Dead();
		if (AddToUnit == false) 
		{
			LevelScript.Unit.Add(gameObject);
			AddToUnit = true;
		}
		if (transform.Find ("TargetMark").GetComponent<Renderer> ().enabled == true) 
		{
			if (MinionTargetOn == false && Die == false) {
				MoveTo ();
			}
			if (MinionTargetOn == true) {
				MoveToTarget ();
			}
			if (Input.GetMouseButtonDown (1)) {
                Target = null; 
                MinionTargetOn = false;
                Attaking = false;
                Moving = true;
                // При каждом нажатии правой кнопкой сбрасываем буллевые атаки и цели, что бы чар не залипал.
                
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit Hit;

				if (Physics.Raycast (ray, out Hit, Mathf.Infinity))
                {
					if (Hit.transform.GetComponent<Rigidbody>().tag!= "Enemy" && Hit.transform.GetComponent<Rigidbody>().tag != "Construction")
                    {
						EndPosition = Hit.point;
                   
					}
                    else if (Hit.transform.GetComponent<Rigidbody>().tag == "Enemy" && Hit.transform.GetComponent<Rigidbody>().tag != "Construction")
                    {
						Target = Hit.transform.gameObject;
                        Target.GetComponent<MoveEnemy>().TargetMark.GetComponent<Renderer>().enabled = true;
						MinionTargetOn = true;
					}
				}
			}
		}
     }


    void LvlUp()
    {
        if (Exp >= ExpUp2lvl && Lvl < 2)
        {
            Lvl += 1;
            PAttack += PAtkUp2lvl;
            AtkSpeed += AtkSpdUp2lvl;
            Armor += ArmorUp2lvl;
            StartCoroutine(LvlUpping());
            Upping = true;
            UpIA.enabled = true;
            UpMS.enabled = true;
        }
        if (Exp >= ExpUp3lvl && Lvl < 3)
        {
            Lvl += 1;
            PAttack += PAtkUp3lvl;
            AtkSpeed += AtkSpdUp3lvl;
            Armor += ArmorUp3lvl;
            StartCoroutine(LvlUpping());
            Upping = true;
            UpIA.enabled = true;
            UpMS.enabled = true;
        }
    }
    // Поднятие характеристик при достижении новго уровня.

    void EnchantIArrow()
    {



    }
    // Улучшение Ледяной стрелы.

    void EnchantMShower()
    {



    }
    // УЛучшение Метеоритного Долждя.


    void MoveToTarget()
    // Движение к target.
    {
        if (MinionTargetOn == true && ControlNPC == false)
        {
            EndPosition = Target.GetComponent<Transform>().position;
            if(IceArrOn == false)
            {
                MoveTo();
                AttackTarget();

            }

        }
    }

    void MoveTo()
    {
        if (Moving && !Attaking)
        {
            Vector3 Direction =  EndPosition- transform.position;
            Direction = new Vector3(Direction.x, 0, Direction.z);
            Direction.Normalize();
            transform.LookAt(EndPosition);
            GameCharacter.GetComponent<Animation>().CrossFade("Run");
            float TargetPosition = Vector3.Distance(transform.position, EndPosition);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction), speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

            if (TargetPosition > 1.2f)
            {
                transform.Translate(Direction * speed * Time.deltaTime, Space.World);
            }
            else if (MinionTargetOn ==false)
            {
                Moving = false;
                GameCharacter.GetComponent<Animation>().CrossFade("idle");
            }

        }
    }
    // Движение к...

    void AttackTarget()
    {
        if (Vector3.Distance(Target.GetComponent<Transform>().position, transform.position) < 1.5f)
        {
            Attaking = true;
            GameCharacter.GetComponent<Animation>().CrossFade("Attack");
            AnimPlayer["Attack"].speed = AtkSpeed;
            float TempPAttack = (PAttack / 100) * (100 - Target.GetComponent<MoveEnemy>().Armor);
            // Высчитываем нанесенный урон, с учетом армора  цели.
            Target.GetComponent<MoveEnemy>().Health -= TempPAttack * Time.deltaTime * AtkSpeed;
            // Наносим урон с учетом скорости атаки.
            if (Target.GetComponent<MoveEnemy>().Health >= 0)
            {
                MinionTargetOn = true;
            }
            else
            {
                Gold += Target.GetComponent<MoveEnemy>().Gold;
                Exp += Target.GetComponent<MoveEnemy>().Exp;
                Attaking = false;
                MinionTargetOn = false;
                GameCharacter.GetComponent<Animation>().CrossFade("idle");
            }
        }



    }

    void Dead()
    {
        if (Health < 0.2f)
        {
            StartCoroutine(DeadTime());

        }
        if (Die == true)
        {
            ReburnTIme.enabled = true;
            ReburnTimer -= Time.deltaTime;
            ReburnTIme.GetComponent<Text>().text = "До воскрешения: " +(int)ReburnTimer; 

        }


    }

    void HotKeyOn()
    {
        if (Input.GetKeyDown("q"))
        {
            IceArrOn = true;
        }
        if (Input.GetKeyDown("w"))
        {
            MetHowOn = true;
            StartExplosion = false;
        }    


    }
    // Проверка на нажатие горячих клавиш.

    public void IceArrowSkillOn()
    {
        if (ReusedOnIA == false)
        {
            IceArrOn = true;
            SkillUsed = true;
        }
        
    }
    // Активация скилла Ледяная стрела по нажатию на кнопку.

     void IceArrowSkill()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IceArrOn = false;
            SkillUsed = false;
            ReusedTimerShowIA.GetComponent<Text>().text = " ";
            return;
        }
        // Отмена скилла по Esc.
        if (IceArrOn == true)
        {
            if (IceArrOn == true)
            {
                ReusedTimerShowIA.GetComponent<Text>().text = " ! ";
            }

            if (Input.GetMouseButton(0) && IceArrFly == false && Target != null && Target.transform.tag == "Enemy")
            {
                if (Vector3.Distance(transform.position, Target.transform.position) <= RadiusIA)
                {
                    Debug.Log(Vector3.Distance(transform.position, Target.transform.position).ToString());
                    CreatArrow = true;
                }
                else
                {
                    Debug.Log("Норм");
                    MoveToRadius = true;
                }
            }

            if (MoveToRadius == true && IceArrFly == false)
            {
                if (Vector3.Distance(transform.position, Target.transform.position) >= RadiusIA)
                {
                    Debug.Log(Vector3.Distance(transform.position, Target.transform.position).ToString());
                    Attaking = false;
                    Moving = true;
                    EndPosition = Target.transform.position;
                    MoveTo();

                }
                else
                {
                    Attaking = true;
                    Moving = false;
                    MoveToRadius = false;
                    CreatArrow = true;
                }


            }

            if (CreatArrow == true && IceArrFly == false)
            {
                IceArrFly = true;
                IceArrow.GetComponent<Renderer>().enabled = true;
                IceArrow.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                OldPosition = transform.position;
                Attaking = false;
                SkillUsed = false;
            }

                if (IceArrFly == true)
                {
                    float TargetPosition = Vector3.Distance(IceArrow.transform.position, EndPosition);
                    Vector3 Direct = EndPosition - IceArrow.transform.position;
                    Direct = new Vector3(Direct.x, 0, Direct.z);
                    Direct.Normalize();
                    transform.LookAt(EndPosition);
                    Moving = true;
                    Attaking = false;
                    if (TargetPosition > 0f)
                    { 
                        IceArrow.transform.Translate(Direct * speed * 3 * Time.deltaTime, Space.World);
                        if (TargetPosition < 1.5f)
                        {
                            ArrowDamage = true;
                            Target.GetComponent<MoveEnemy>().Health -= IceArrowDamage;
                            StartCoroutine(IAEffect(Target));
                        }

                    }
                    if (ArrowDamage == true)
                    {
                        IceArrow.GetComponent<Renderer>().enabled = false;
                        IceArrow.transform.position = OldPosition;
                        IceArrFly = false;
                        ArrowDamage = false;
                        ReusedOnIA = true;
                        TargetPosition = 2f;
                        IceArrOn = false;
                    }
                }
            }
    }
    // Ледняная стрела.

    void ReusedIceArr()
    {
        if (ReusedOnIA == true)
        {
            if (ReusedTimeIA > 0)
            {
                    ReusedTimeIA -= Time.deltaTime;
                    ReusedTimerShowIA.GetComponent<Text>().text = "" + (int)ReusedTimeIA;
            }
            else
            {
                ReusedOnIA = false;
                ReusedTimeIA = 15f;
            }
        }
    }
    // Откат Леяной Стрелы.


    public void MeteorShowerOn()
    {
        if (ReusedOnMS == false)
        {
            SkillUsed = true;
            MetHowOn = true;
            StartExplosion = false;
        }

    }
    // Активация скилла Метеоритный Дождь по нажатию на кнопку.


    void MeteorShower()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SkillUsed = false;
            MetHowOn = false;
            ReusedTimerShowMS.GetComponent<Text>().text = "";
            return;
        }
        // Отмена скилла по Esc.

        if (MetHowOn == true)
        {
            ReusedTimerShowMS.GetComponent<Text>().text = " ! ";
        }

        if (Input.GetMouseButton(0) && ReusedOnMS == false)
        {
            RaycastHit Hit;
            Ray Ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
            SkillUsed = false;
            if (Physics.Raycast(Ray1, out Hit, Mathf.Infinity))
            {
                if (StartExplosion == false)
                {
                    StartCoroutine(Explosion(Hit.point));
                }
                Collider[] HitCollidaers = Physics.OverlapSphere(Hit.transform.position, 5f);
                int i = 0;
                while (i < HitCollidaers.Length)
                {
                    if (HitCollidaers[i].transform.tag == "Enemy")
                    {
                        HitCollidaers[i].GetComponent<MoveEnemy>().Health -= MetShowDamage;
                    }
                    i++;
                }
                if (i == HitCollidaers.Length)
                {
                    MetHowOn = false;
                    ReusedOnMS = true;
                    StartExplosion = false;
                }
            }

        }

    }
    // Метеоритный Дождь.

    void ReusedMetShower()
    {
        if (ReusedOnMS == true)
        {
            if (ReusedTimeMS > 0)
            {
                ReusedTimeMS -= Time.deltaTime;
                ReusedTimerShowMS.GetComponent<Text>().text = "" + (int)ReusedTimeMS;
                
            }
            if(ReusedTimeMS <=0)
            {
                ReusedTimerShowMS.GetComponent<Text>().text = "";
                ReusedOnMS = false;
                ReusedTimeMS = 20f;
            }
        }
    }
    // Откат Метеоритного Дождя.

    IEnumerator IAEffect(GameObject Obj)
    {
        float TempSpd = Obj.GetComponent<MoveEnemy>().speed;
        float TempAtkSpd = Obj.GetComponent<MoveEnemy>().AttackSpeed;

        Obj.GetComponent<MoveEnemy>().speed = Obj.GetComponent<MoveEnemy>().speed - ((Obj.GetComponent<MoveEnemy>().speed / 100) * (IceArrSpdSlow * 100));
        Obj.GetComponent<MoveEnemy>().AttackSpeed = Obj.GetComponent<MoveEnemy>().AttackSpeed - ((Obj.GetComponent<MoveEnemy>().AttackSpeed / 100) * (IceArrAtkSlow * 100));

        yield return new WaitForSeconds(Duration);

        Obj.GetComponent<MoveEnemy>().speed = TempSpd;
        Obj.GetComponent<MoveEnemy>().AttackSpeed = TempAtkSpd;
    }
    // Еффект Ледяной Стреллы

    IEnumerator Explosion(Vector3 obj)
    {
        StartExplosion = true;

        GameObject Explosion = Instantiate(MeteorShowerPartSys, obj, Quaternion.identity) as GameObject;

        yield return new WaitForSeconds(2f);

        Destroy(GameObject.Find("MeteorShower(Clone)"));

        StartExplosion = false;
    }

        IEnumerator DeadTime()
    {
        Die = true;

        GameCharacter.GetComponent<Animation>().Stop(); 

        transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y, 0f); 

        yield return new WaitForSeconds(10f);

        transform.position = new Vector3(52f, 0, 39f); ;

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);


        Health = 1000;

        ReburnTIme.enabled = false;
        ReburnTimer = 10f;

        Die = false;
    }

    IEnumerator LvlUpping()
    {
        LvlUpAnim.GetComponent<ParticleSystem>().enableEmission = true;

        yield return new WaitForSeconds(4f);

        LvlUpAnim.GetComponent<ParticleSystem>().enableEmission = false;

    }

    // Бьем таргет.

}
