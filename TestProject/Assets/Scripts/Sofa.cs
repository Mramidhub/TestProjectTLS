using UnityEngine;
using System.Collections;


public class Sofa : MonoBehaviour {

	public float Health;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		Debug.Log (Health.ToString ());
		Destruction ();

	}

	void Destruction()
	{
		if (Health <= 0) 
		{
			Debug.Log ("Бо-бо");
			Destroy (gameObject);
		}
	}
}
