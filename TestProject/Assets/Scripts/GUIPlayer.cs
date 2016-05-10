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

	void Statistic()
	{
		Lvl.text = " " + Player.GetComponent<MovePlayer> ().Lvl;
		PAtk.text = " " +  Player.GetComponent<MovePlayer> ().PAttack;
		AtkSpeed.text = " " + Player.GetComponent<MovePlayer> ().AtkSpeed;
		Speed.text = " " + Player.GetComponent<MovePlayer> ().speed;
		Armor.text = " " + Player.GetComponent<MovePlayer> ().Armor;
		Gold.text = " " + Player.GetComponent<MovePlayer> ().Gold;
		Exp.text = " " + Player.GetComponent<MovePlayer> ().Exp;

	}

}

