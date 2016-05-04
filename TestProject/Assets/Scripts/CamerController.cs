using UnityEngine;
using System.Collections;

public class CamerController : MonoBehaviour {

    public GameObject Player;
    Vector3 Distantion;
    bool FreeCamera;

    // Use this for initialization
    void Start () {
        FreeCamera = false;
        Distantion = transform.position - Player.GetComponent<Transform>().position;
        // Находим дисстанцию на которой находится камера до персонажа.

    }

    void Update()
    {



    }
	
	// Update is called once per frame
	void LateUpdate () {

        if (Input.GetKeyDown("f"))
        {
            if (FreeCamera == false)
            {
                FreeCamera = true;
            }
            else if (FreeCamera == true)
            {
                FreeCamera = false;
            }
        }

        if (FreeCamera == false)
        {
            transform.position = Player.transform.position + Distantion;
            // В конце каждого кадра назначаем новую позицию камеры.
        }
        else if (FreeCamera == true)
        {
            if (Input.GetKey("w"))
            {
                transform.position = new Vector3(transform.position.x + 0.4f, transform.position.y, transform.position.z);

            }
            else if (Input.GetKey("s"))
            {
                transform.position = new Vector3(transform.position.x - 0.4f, transform.position.y, transform.position.z);

            }
            else if (Input.GetKey("a"))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.4f);

            }
            else if (Input.GetKey("d"))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.4f);

            }
        }
	
	}
}
