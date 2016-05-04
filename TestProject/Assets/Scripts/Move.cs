using UnityEngine;
using System.Collections;


public class Move : MonoBehaviour {

    public Transform GameCharacter;
    public float speed;
    Vector3 EndPosition;
    bool moving;

	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetMouseButtonDown(1))
        {
            moving = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit EndPoint;

            if (Physics.Raycast(ray, out EndPoint, Mathf.Infinity))
            {
                EndPosition = EndPoint.point;
            }

        }

        if (moving)
        {
            Vector3 Direction = EndPosition - transform.position;
            Direction.Normalize();
            transform.LookAt(EndPosition);
            GameCharacter.GetComponent<Animation>().CrossFade("Run");
            float TargetPosition = Vector3.Distance(transform.position, EndPosition);
            if (TargetPosition > 0.3f)
            {
                transform.Translate(Direction * speed, Space.World);
            }
            else
            {
                moving = false;
                GameCharacter.GetComponent<Animation>().CrossFade("idle");
            }
        }
     }
        
}
