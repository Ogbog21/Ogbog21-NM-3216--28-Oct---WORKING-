using UnityEngine;
using System.Collections;

public class DestroyOther : MonoBehaviour {

	private PlayerController playerController;

	void Awake()
	{
		playerController = GameObject.Find ("Player").GetComponent<PlayerController> ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.CompareTag("Enemies"))
		{			
			float chance = .5f;

			if (playerController.isPouncing) 
			{
				chance = 1;
			}

			playerController.IncHP (chance,1);
			playerController.CountDeath ();

			Destroy (other.gameObject);
			Destroy (gameObject);


		}

	}
		
}
