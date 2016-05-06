using UnityEngine;
using System.Collections;

public class RespawnMinion : MonoBehaviour {

    public GameObject Warrior;

    float RespawnTimeWarrior;
    Vector3 RandomRespawnPosition;

    // Use this for initialization
    void Start () {

        RespawnTimeWarrior = 2.5f;

	}
	
	// Update is called once per frame
	void Update () {

        RandomRespawnPosition = new Vector3(47 + Random.Range(-3f, 3f), transform.position.y, 32 + Random.Range(0f, 3f));
        RespawnTimeWarrior -= Time.deltaTime;

        if (RespawnTimeWarrior < 0f)
        {

            GameObject Goblin = Instantiate(Warrior, RandomRespawnPosition, Quaternion.identity) as GameObject;
            RespawnTimeWarrior = 2.5f;
        }

    }
}
