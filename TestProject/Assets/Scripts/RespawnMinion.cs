using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RespawnMinion : MonoBehaviour {

    public GameObject Unit;
    public GameObject Player;
    public Canvas GUIConstruction;
    public Text OrderWarrior;
    public Text SystemMsg;
    public Sprite UnitImage;
    public int PriceUnit;
    int Order;
    int BarrackLvl;
    float TimeTraining;
    bool Training;

    float RespawnTimeWarrior;
    Vector3 RandomRespawnPosition;


    void Start ()
    {
        TimeTraining = 2f;
        PriceUnit = 50;
    }
	

	void Update ()
    {
        OrderWarrior.text = Order.ToString();

        if (LevelScript.StartGame == true)
        {
            Respawn();
        }
    }

    void ProgresBarack()
    {



    }
    
    void Respawn()
    {
        
        if (Order > 0 && Training == false)
        {
            StartCoroutine(TrainigUnit());
        }

    }

    public void ClicOrder()
    {
        if (Player.GetComponent<MovePlayer>().Gold >= PriceUnit)
        {
            Order += 1;
            Player.GetComponent<MovePlayer>().Gold -= PriceUnit;
        }
        else
        {
            SystemMsg.text = "У вас не хватает золота";

        }
    }

    // При нажатии на кнопку, пополняеться счетсчик заказа.
    IEnumerator TrainigUnit()
    {
        Training = true;

        yield return new WaitForSeconds(TimeTraining);

        if (Unit.transform.name == "Warrior")
        {
            RandomRespawnPosition = new Vector3(42f + Random.Range(-3f, 3f), 0, 42f + Random.Range(0f, 3f));
        }
        if (Unit.transform.name == "Necromancer")
        {
            RandomRespawnPosition = new Vector3(56f + Random.Range(-3f, 3f), 0, 45f + Random.Range(0f, 3f));
        }

        GameObject Minion = Instantiate(Unit, RandomRespawnPosition, Quaternion.identity) as GameObject;

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
