using UnityEngine;
using System.Collections;

public class RespawnEnemy : MonoBehaviour {

    public GameObject RangeGoblin;
    public GameObject Demon;
    public GameObject Cyclop;
	bool RespawnOn;
	bool StartGame;
	bool GroupRespawnOn;
    bool FirstRespawn;

    public static float RespTimeRGoblin = 10f;
    public static float RespTimeDemon = 10f;
    public static float RespTimeCyclop = 40f;


    float TempRespTimeRGoblin;
    float TempRespTimeDemon;
    float TempRespTimeCyclop;

    Vector3 RandomRespawnPosition;


    void Start () {
		RespawnOn = false;
		StartGame = false;
		GroupRespawnOn = false;
        FirstRespawn = false;

        TempRespTimeCyclop = RespTimeCyclop;
        TempRespTimeDemon = RespTimeDemon;
        TempRespTimeRGoblin = RespTimeRGoblin;
    }
	
	// Update is called once per frame
	void Update () {
		if (LevelScript.StartGame == true && GroupRespawnOn == false) 
		{
			StartCoroutine (GroupSpawn ());
		}

        if (FirstRespawn == false)
        {
            TempRespTimeCyclop =20;
            TempRespTimeDemon = 10;
            TempRespTimeRGoblin = 10;
        }

        if (RespawnOn == true)
		{
            if (FirstRespawn == true)
            {
                TempRespTimeCyclop -= Time.deltaTime;
                TempRespTimeDemon -= Time.deltaTime;
                TempRespTimeRGoblin -= Time.deltaTime;
            }

            RandomRespawnPosition = new Vector3(Random.Range(-1f, 1f), transform.position.y, Random.Range(-1f, 1f));

            if (TempRespTimeCyclop < 0f)
            {
                GameObject EnemyCyclop = Instantiate(Cyclop, transform.position + RandomRespawnPosition, Quaternion.identity) as GameObject;
                TempRespTimeCyclop = RespTimeCyclop;
            }
            if (TempRespTimeDemon < 0f)
            {
                GameObject EnemyDemon = Instantiate(Demon, transform.position + RandomRespawnPosition, Quaternion.identity) as GameObject;
                TempRespTimeDemon = RespTimeDemon;
            }
            if (TempRespTimeRGoblin < 0f)
            {
                GameObject EnemyDemon = Instantiate(RangeGoblin, transform.position + RandomRespawnPosition, Quaternion.identity) as GameObject;
                TempRespTimeRGoblin = RespTimeRGoblin;
            }
            FirstRespawn = true;
        // Таймер респавна зеленых гоблинов.
		}
	
	}

	// После запуска игры ждем 30 сек , прежде чем мобы начнут респавниться. 


	IEnumerator GroupSpawn()
	{
		GroupRespawnOn = true;
		RespawnOn = true;
		yield return new WaitForSeconds (LevelScript.WaveTime);
        RespawnOn = false;
        GroupRespawnOn = false;
	}
    // Респавн гоблинов  группами по три раз в 6 секунд.

}
