using UnityEngine;
using System.Collections;

public class MoveEnemy : MonoBehaviour {

    public Transform GameCharacter;
    public float speed;
    Vector3 EndPoint;
    bool Moving;



	// Use this for initialization
	void Start () {

        Moving = false;
        EndPoint = new Vector3(46f, 0, 34f);

	}
	
	// Update is called once per frame
	void Update () {

        Vector3 Direction = EndPoint - transform.position;
        Direction.Normalize();
        transform.LookAt(EndPoint);
        GameCharacter.GetComponent<Animation>().CrossFade("run");
        float TargetPosition = Vector3.Distance(transform.position, EndPoint);

        if (TargetPosition > 0.3f)
        {
            transform.Translate(Direction * speed, Space.World);
        }
        else
        {
            GameCharacter.GetComponent<Animation>().CrossFade("stand");

        }

    }
}
