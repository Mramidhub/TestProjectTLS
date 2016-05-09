using UnityEngine;
using System.Collections;


public class MovePlayer : MonoBehaviour {

    public float PAttack = 100f;
    public float speed = 0.2f;
    public float Health = 200f;


    public Transform GameCharacter;
    Vector3 EndPosition;
    bool Moving;
    bool Attaking;
    public bool MinionTargetOn;
    GameObject Target;


    // Use this for initialization
    void Start () {

        MinionTargetOn = false;
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (MinionTargetOn == false)
        {
            MoveTo();
        }
        MoveToTarget();
        if (Input.GetMouseButtonDown(1))
        {
            Moving = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit Hit;

            if (Physics.Raycast(ray, out Hit, Mathf.Infinity))
            {
                if (Hit.transform.tag != "Enemy")
                {
                    Debug.Log("1");
                    EndPosition = Hit.point;
                   
                }
                else if (Hit.transform.tag == "Enemy")
                {
                    Target = Hit.transform.gameObject;
                    MinionTargetOn = true;
                    Debug.Log(Target.ToString());
                }

            }


        }

        
     }

    void MoveToTarget()
    // Движение к target.
    {
        if (MinionTargetOn == true)
        {
            EndPosition = Target.GetComponent<Transform>().position;
            MoveTo();
            AttackTarget();

        }
    }

    void MoveTo()
    {

        if (Moving && !Attaking)
        {
            Debug.Log("3");
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
                transform.Translate(Direction * speed, Space.World);
                Debug.Log("4");
            }
            else if (MinionTargetOn ==false)
            {
                Moving = false;
                GameCharacter.GetComponent<Animation>().CrossFade("idle");
                Debug.Log("5");
            }

        }
    }

    void AttackTarget()
    {
        if (Vector3.Distance(Target.GetComponent<Transform>().position, transform.position) < 1.5f)
        {
            Attaking = true;
            Debug.Log("Attack!");
            GameCharacter.GetComponent<Animation>().CrossFade("Attack");
            Target.GetComponent<MoveEnemy>().Health -= PAttack * Time.deltaTime;
            if (Target.GetComponent<MoveEnemy>().Health > 0)
            {
                MinionTargetOn = true;
            }
            else
            {
                MinionTargetOn = false;
            }
        }

    }

}
