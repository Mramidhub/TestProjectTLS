using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIPlayer : MonoBehaviour {

	public GameObject Player;
	public Canvas GUIP;
	public Text Lvl;
	public Text PAtk;
	public Text AtkSpeed;
	public Text Speed;
	public Text Armor;
	public Text Gold;
	public Text Exp;
    public Text HP;

	// Use this for initialization
	void Start () {
		
		GUIP.enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {
	
		GUIOn ();
		Statistic ();

	}


	void GUIOn()
	{
		if (Player.transform.Find ("TargetMark").GetComponent<Renderer> ().enabled == true) {

			GUIP.enabled = true;

		} 
		else 
		{

			GUIP.enabled = false;

		}


	}
    // Активация, дезактивация GUI игрока.
	void Statistic()
	{
		Lvl.GetComponent<Text>().text = " " + Player.GetComponent<MovePlayer> ().Lvl;
		PAtk.GetComponent<Text>().text = " " +  Player.GetComponent<MovePlayer> ().PAttack;
		AtkSpeed.GetComponent<Text>().text = " " + Player.GetComponent<MovePlayer> ().AtkSpeed;
		Speed.GetComponent<Text>().text = " " + Player.GetComponent<MovePlayer> ().speed;
		Armor.GetComponent<Text>().text = " " + Player.GetComponent<MovePlayer> ().Armor;
		Gold.GetComponent<Text>().text = " " + Player.GetComponent<MovePlayer> ().Gold;
		Exp.GetComponent<Text>().text = " " + Player.GetComponent<MovePlayer> ().Exp;
        HP.GetComponent<Text>().text = "HP: " + (int)Player.GetComponent<MovePlayer>().Health;
	}
    // Вывод статистики на экран.
}

