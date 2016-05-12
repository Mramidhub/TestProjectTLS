using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RespawnWarrior : MonoBehaviour {

    public GameObject Warrior;
    public Canvas GUIBarracks;
    public Text OrderWarrior;
    int Order;
    int BarrackLvl;
    float TimeTraining;
    bool Training;

    float RespawnTimeWarrior;
    Vector3 RandomRespawnPosition;


    void Start ()
    {
        TimeTraining = 4f;
    }
	

	void Update ()
    {
        Respawn();
    }

    void ProgresBarack()
    {



    }
    
    void Respawn()
    {
        OrderWarrior.text = Order.ToString();

        if (Order > 0 && Training == false)
        {
            StartCoroutine(TrainigUnit());
        }

    }

    public void ClicOrder()
    {
        Order += 1;
    }

    // При нажатии на кнопку, пополняеться счетсчик заказа.
    IEnumerator TrainigUnit()
    {
        Training = true;

        yield return new WaitForSeconds(TimeTraining);

        RandomRespawnPosition = new Vector3(58 + Random.Range(-3f, 3f), 0, 45 + Random.Range(0f, 3f));
        GameObject warrior = Instantiate(Warrior, RandomRespawnPosition, Quaternion.identity) as GameObject;

        Training = false;
        if (Order > 1)
        {
            Order -= 1;
        }
        else if (Order == 1)
        {
            Order = 0;
        }

    }
    // Тренировка миньона.
}
