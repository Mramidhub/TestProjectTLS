using UnityEngine;
using System.Collections;

public class CamerController : MonoBehaviour {

    public GameObject Player;
    public float CameraSpeed;
    Vector3 Distantion;
    bool FreeCamera;


    void Start () {
        FreeCamera = false;
        Distantion = transform.position - Player.GetComponent<Transform>().position;
        CameraSpeed = 0.3f;
        // Находим дисстанцию на которой находится камера до персонажа.

    }

    void Update()
    {



    }
	

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
        // Кнопкой F переключаем способ поведения камеры.

        if (FreeCamera == false)
        {
            transform.position = Player.transform.position + Distantion;
            // В конце каждого кадра назначаем новую позицию камеры.
        }
        // Камера следует за игроком.
        else if (FreeCamera == true)
        {
                if (Input.mousePosition.x <= 0 && transform.position.z < 95f)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - CameraSpeed);

                }
                else if (Input.mousePosition.x >= Screen.width && transform.position.z > 4f)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + CameraSpeed);

                }
                else if (Input.mousePosition.y <= 0 && transform.position.x > 0f)
                {
                    transform.position = new Vector3(transform.position.x + CameraSpeed, transform.position.y, transform.position.z);

                }
                else if (Input.mousePosition.y >= Screen.height && transform.position.x < 76f)
                {
                    transform.position = new Vector3(transform.position.x - CameraSpeed, transform.position.y, transform.position.z);

                }

        }
        // Управление камерой с посощью позиции мышки на ее границе.
	
	}
}
