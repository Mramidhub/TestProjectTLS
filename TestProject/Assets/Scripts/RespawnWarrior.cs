using UnityEngine;
using System.Collections;

public class RespawnWarrior : MonoBehaviour {

    public GameObject Warrior;
    public Canvas GUIBarracks;

    float RespawnTimeWarrior;
    Vector3 RandomRespawnPosition;


    void Start ()
    {

	}
	

	void Update ()
    {


    }

    void RespawnUnit()
    {
        RandomRespawnPosition = new Vector3(47 + Random.Range(-3f, 3f), 0, 32 + Random.Range(0f, 3f));

        if (RespawnTimeWarrior < 0f)
        {
            GameObject warrior = Instantiate(Warrior, RandomRespawnPosition, Quaternion.identity) as GameObject;
        }

    }
}
