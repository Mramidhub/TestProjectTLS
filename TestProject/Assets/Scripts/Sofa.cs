using UnityEngine;
using System.Collections;


public class Sofa : MonoBehaviour {

	public float Health;


	void Start ()
    {
	
	}
	

	void Update ()
    {
		Destruction ();
	}

	void Destruction()
	{
		if (Health <= 0) 
		{
			Destroy (gameObject);
            LevelScript.EndGameLoose = true;
		}
	}
    // При уничтожении дивана - конец игры.
}
