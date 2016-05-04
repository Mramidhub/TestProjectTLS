using UnityEngine;
using System.Collections;

public class LevelScript : MonoBehaviour {

    public GameObject Target;
    Vector3 TargetPosition;



	void Start () {
	
	}
	

	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.tag == "Enemy" || hit.transform.tag == "Minion" || hit.transform.tag == "Player")
                {
                    TargetPosition = new Vector3(hit.transform.position.x, 0.1f, hit.transform.position.z);
                    GameObject ChosenTarget = new GameObject();
                    ChosenTarget = Instantiate(Target, TargetPosition, Quaternion.identity) as GameObject;
                    ChosenTarget.transform.parent = hit.transform;
                    ChosenTarget.transform.Rotate(90f, 0f, 0f);

                }

            }

        }
        // Создаем луч. Проверяем по тегу обьекта в который попал луч и если он соответствует перечисленным создаем target под объектом. 
        // Делаем target дочерним к объекту.


	}
}
