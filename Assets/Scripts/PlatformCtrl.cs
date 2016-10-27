using UnityEngine;
using System.Collections;

public class PlatformCtrl : MonoBehaviour {

//	private Rigidbody2D playerBody;

	private BoxCollider2D playerCollider;
	private BoxCollider2D platformCollider;
	private PlayerController playerController;

	private GameObject player;

//	private bool isLanded;
	private bool abovePlayer;


	// Use this for initialization
	void Start () {
//		playerBody = GameObject.Find ("Player").GetComponent<
		player = GameObject.Find("Player");
	
		//access colliders
		playerCollider = player.GetComponent<BoxCollider2D>();
		platformCollider = gameObject.GetComponent<BoxCollider2D> ();

		playerController = player.GetComponent<PlayerController> ();

	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (player.transform.position.y < gameObject.transform.position.y) {
			abovePlayer = true;
		} else abovePlayer = false;
		
		
		if (playerController.jumping || abovePlayer )
		{
			Physics2D.IgnoreCollision (playerCollider, platformCollider, true);

		}

		if (!playerController.jumping && !abovePlayer || !playerController.isPouncing && !abovePlayer
			|| !abovePlayer)
		{
			Physics2D.IgnoreCollision (playerCollider, platformCollider, false);
		}
			
	
	}

}
