using UnityEngine;
using System.Collections;

public class enemiesCtrl : MonoBehaviour {
	public float MovementSpeed, WalkRange, JumpspeedY;
	public int HP;
	public GameObject Tombstone;
	public GameObject P1, P2, P3, P4;

	private Rigidbody2D rb2d;
	private int count;
	private float startingX;
	private bool FacingRight, onGround;
	private Vector2 MS;
	private int CurrHP;
	int countTA = 0;


	// Use this for initialization
	void Start () 
	{	
		rb2d = GetComponent<Rigidbody2D> ();
		startingX = transform.position.x;
		MS = new Vector2(MovementSpeed *- 1,0);
		rb2d.velocity = MS;
		FacingRight = false;
		onGround = false;
//		PlayerMan = Cat.GetComponent<PlayerManager> ();
		CurrHP = HP;
		InvokeRepeating ("CancelSD", 20, 20f);
	}

	void Update()
	{
		if (transform.position.x < startingX - WalkRange || transform.position.x > startingX + WalkRange) 
		{
			TurnAround ();
		}

		if (onGround) 
		{
			rb2d.AddForce (new Vector2 (rb2d.velocity.x, JumpspeedY));
			onGround = false;
		}

//		if(transform.position.x <-21 || transform.position.x > 130)
//		{
//			SelfDestruct ();
//			PlayerMan.SpawnREnemy ();
//		}

		BotMove();
		CheckHP ();

	}

	void OnCollisionEnter2D (Collision2D other)
	{
		
		if (other.gameObject.CompareTag ("GROUND")) 
		{
			onGround = true;
		}

		if (other.gameObject.CompareTag ("Wall") || other.gameObject.CompareTag ("Crate") || other.gameObject.CompareTag("Player")) 
		{
			TurnAround ();
		}

		if (other.gameObject.CompareTag ("Enemies")) 
		{
			TurnAround ();
		}

//		if (other.gameObject.CompareTag("Player") )
//		{
//			if (other.relativeVelocity.y > 0) 
//			{
//				TakeDamage ();
//			}
//		}

	
	}

	void TurnAround()
	{
		//flip
		Vector3 temp = transform.localScale;
		temp.x = -temp.x;
		transform.localScale = temp;
		FacingRight = !FacingRight;


		countTA = countTA + 1;
		if (countTA > 10) 
		{
			SelfDestruct ();
		}

	}

	void TakeDamage()
	{
		CurrHP = CurrHP - 1;
		if(CurrHP <= 0)
		{							
			//Destroy (gameObject);
			//PlayerMan.EnemyDown ();
			Instantiate (Tombstone, transform.position, Quaternion.identity);
		}
	}

	void BotMove()
	{
		if(FacingRight) 
		{
			rb2d.velocity = new Vector2 (- MS.x,rb2d.velocity.y);
		} 

		if (!FacingRight) 
		{
			rb2d.velocity = new Vector2 (MS.x,rb2d.velocity.y);
		}
	}
		
	void CheckHP()
	{
		P1.SetActive (true);
		P2.SetActive (true);
		P3.SetActive (true);
		P4.SetActive (true);

		if (CurrHP <= 3) 
		{
			P4.SetActive (false);
		}

		if (CurrHP <= 2) 
		{
			P3.SetActive (false);
		}	

		if (CurrHP <= 1) 
		{
			P2.SetActive (false);
		}

		if (CurrHP <= 0) 
		{
			P1.SetActive (false);
		}
	}

	void SelfDestruct()
	{
		Destroy (gameObject);
	}

	void CancelSD ()
	{
		countTA = 0;
	}
}
