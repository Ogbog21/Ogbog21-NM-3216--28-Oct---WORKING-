using UnityEngine;
using System.Collections;

public class DogCtrl : MonoBehaviour {

	public float speedX, JumpspeedY, JumpDelay, WalkRange;
	public GameObject self;

	private Rigidbody2D rb;
	private bool facingRight;

	private float speed;
	private float startingX;

	private Vector3 startingPos;


	// Use this for initialization
	void Start () {

		rb = gameObject.GetComponent<Rigidbody2D> ();
		rb.velocity = new Vector2 (speedX, 0);
		startingX = transform.position.x;
		InvokeRepeating ("CheckTurn",2,2);
//		InvokeRepeating ("Turn", 5, Random.Range (1, 5));

		startingPos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		MoveAI ();

	}

	void MoveAI()
	{
		if (facingRight) {
		
			speed = speedX;
		}

		if (!facingRight)
		{
			speed = -speedX;
		}

		rb.velocity = new Vector2 (speed, rb.velocity.y);


	}

	void OnCollisionEnter2D (Collision2D other)
	{
		
		if (other.gameObject.CompareTag ("Player") || other.gameObject.CompareTag ("Wall") || other.gameObject.CompareTag ("Platform")) {
			Turn ();
		} 
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if(other.CompareTag("Wall"))
		{
			Turn ();
		}

		if (other.CompareTag ("Death")) {
			Instantiate (self, startingPos, Quaternion.identity);
			Destroy (gameObject);
		}
	}


	void Turn()
	{
		Vector3 temp = transform.localScale;
		temp.x *= -1;
		transform.localScale = temp;
		facingRight = !facingRight;
	}

	void CheckTurn()
	{
		if (transform.position.x < startingX - WalkRange || transform.position.x > startingX + WalkRange) {
			Turn ();
		}
	}
}
