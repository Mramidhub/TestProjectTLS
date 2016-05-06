using UnityEngine;
using System.Collections;

public class RespawnEnemy : MonoBehaviour {

    public GameObject GreenGoblin;
    // Блок переменных врагов.

    float RespawnTimeGoblin;
    Vector3 RandomRespawnPosition;

    // Use this for initialization
    void Start () {
        RespawnTimeGoblin = 4f;
    }
	
	// Update is called once per frame
	void Update () {
        RandomRespawnPosition = new Vector3(Random.Range(-1f, 1f), transform.position.y, Random.Range(-1f, 1f));
        RespawnTimeGoblin -= Time.deltaTime;

        if (RespawnTimeGoblin < 0f)
        {

            GameObject Goblin = Instantiate(GreenGoblin, transform.position + RandomRespawnPosition, Quaternion.identity) as GameObject;
            RespawnTimeGoblin = 4f;
        }
        // Таймер респавна зеленых гоблинов.

	
	}
}
