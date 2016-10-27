using UnityEngine;
using System.Collections;

public class WolfAbilities : MonoBehaviour {

	//public GameObject Player;

	private PlayerController playerController;

	// Use this for initialization
	void Start () {
		playerController = GameObject.Find("Player").GetComponent<PlayerController> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (!playerController.isPouncing && !playerController.Attacking) 
		{		
			Destroy (gameObject);
		}

		if (playerController.Attacking) 
		{
			Destroy (gameObject, .5f);
		}
	
	}
}
