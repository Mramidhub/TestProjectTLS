using UnityEngine;
using System.Collections;

public class RespawnEnemy : MonoBehaviour {

    public GameObject GreenGoblin;
	bool RespawnOn;
	bool StartGame;
	bool GroupRespawnOn;
    // Блок переменных врагов.

    float RespawnTimeGoblin;
    Vector3 RandomRespawnPosition;

    // Use this for initialization
    void Start () {
        RespawnTimeGoblin = 2f;
		RespawnOn = false;
		StartGame = false;
		GroupRespawnOn = false;

		StartCoroutine (StartRespawn ());
    }
	
	// Update is called once per frame
	void Update () {
		if (StartGame == true && GroupRespawnOn == false) 
		{
			StartCoroutine (GroupSpawn ());
		}

		if(RespawnOn == true)
		{
       	 	RandomRespawnPosition = new Vector3(Random.Range(-1f, 1f), transform.position.y, Random.Range(-1f, 1f));
       		RespawnTimeGoblin -= Time.deltaTime;

        	if (RespawnTimeGoblin < 0f)
      		  {

           		 GameObject Goblin = Instantiate(GreenGoblin, transform.position + RandomRespawnPosition, Quaternion.identity) as GameObject;
           		 RespawnTimeGoblin = 2f;
       		 }
        // Таймер респавна зеленых гоблинов.
		}
	
	}
	IEnumerator StartRespawn()
	{

		yield return new WaitForSeconds (30f);

		StartGame = true;

	}

	// После запуска игры ждем 30 сек , прежде чем мобы начнут респавниться. 


	IEnumerator GroupSpawn()
	{
		GroupRespawnOn = true;
		RespawnOn = true;

		yield return new WaitForSeconds (6f);

		RespawnOn = false;

		yield return new WaitForSeconds (6f);

		GroupRespawnOn = false;
	}
	// Респавн гоблинов  группами по три раз в 6 секунд.

}
