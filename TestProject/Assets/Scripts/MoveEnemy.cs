using UnityEngine;
using System.Collections;

public class MoveEnemy : MonoBehaviour {

    Vector3 EndPoint;
    bool Moving;
    float RandomXEndPoint;


	// Use this for initialization
	void Start () {

        Moving = false;
        EndPoint = new Vector3(46f, transform.position.y, 34f);

	}
	
	// Update is called once per frame
	void Update () {
        RandomXEndPoint = Random.Range(-4f, 4f);
        Vector3 Direction = new Vector3(transform.position.x + RandomXEndPoint, EndPoint.y, EndPoint.z);
        
	
	}
}
