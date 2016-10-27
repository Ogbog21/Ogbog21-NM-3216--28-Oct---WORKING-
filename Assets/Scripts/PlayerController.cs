using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {


	public float speedX;
	public float jumpspeedY;
	public Vector2 KnockbackForce;
	public Vector2 PounceForce;	

	public bool isPouncing, Attacking;
	public bool Invulnerable;
	public bool isHurt, jumping, facingRight;
	public bool isAlive;
	public bool CanMove;
	public bool IsKiller;

	//for testing only (makes player invulnerable)
	public bool isTest;

	public GameObject Pouncer, Slash;
	public GameObject LeftBarrier, RightBarrier;
	public GameObject Crosshair;
	public GameObject HpLogo;
	public GameObject TeleIcon;

	public int HPCurrent;
	public int HPMax;
	public int PounceCD, PounceCoolDown;
	public int Lives;

	private int hPCurrent;

	private float speed;

	public float CrosshairCounter;

	[SerializeField]
	private float TeleportDistance;
	
	[SerializeField]
	private float TeleportTime;
	
	public Text UI;

	public AudioClip Ouch,Boom;

	private Rigidbody2D rb;
	private Animator anim;
	private AudioSource source;
	private SceneFader sceneFader;
	private Vector3 respawnPosition;

	private Transform PouncePos;
	private Transform crosshairLoc;
	private Transform feedbackLoc;

	private Color cFull;


	// Use this for initialization
	void Start () {

		rb = gameObject.GetComponent<Rigidbody2D> ();
		anim = gameObject.GetComponent<Animator> ();
		wolfSprite = gameObject.GetComponent<SpriteRenderer> ();
		sceneFader = GameObject.Find ("Cover").GetComponent<SceneFader> ();	
		source = gameObject.GetComponent<AudioSource>();
		cFull = wolfSprite.color;

		isHurt = false;
		jumping = false;
		facingRight = true;
		isPouncing = false;
		Invulnerable = false;
		isAlive = true;
		isTest = false;

		hPCurrent = HPMax;
		UI.text = "";

		//find Locations
		PouncePos = transform.FindChild ("pounceLoc");
		crosshairLoc = transform.FindChild ("crosshairLoc");
		feedbackLoc = transform.FindChild ("feedback");

		InvokeRepeating ("PounceTimer", 1, 1);
		PounceCD = PounceCoolDown;
		PlayerPrefs.SetInt ("Cutscene", 0);

		respawnPosition = gameObject.transform.position;

		TeleIcon.SetActive (false);

		if (PlayerData.AlignSet) 
		{
			IsKiller = PlayerData.IsKiller;
		}


		//only proceed to chance alignment if alignment has already been set.

	}
	
	// Update is called once per frame
	void Update () 
	{


		// On Death
		if (hPCurrent <= 0) {
			isAlive = false;
		}

	}

	void FixedUpdate()
	{
		if (!CanMove) 
		{
			return;
		}

		// Player Movement
		if (isAlive && sceneFader.IsFaded) 
		{
			CheckKeyboard ();		
			MovePlayer (speed);
			flip ();
			TeleportAnim ();
		}

		if (!isAlive && Lives > 0)
		{
			if (Input.GetKeyDown (KeyCode.R)) 
			{
				Respawn ();
			}
		}

		if (wolfSprite.color.a > .9f) 
		{
			makeVulnerable ();
		}
			
	}

	void LateUpdate()
	{
		if (hPCurrent != HPCurrent) {
			hPCurrent = Mathf.Clamp (hPCurrent, 0, HPMax);
			HPCurrent = hPCurrent;
		}
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.CompareTag ("GROUND")) {
			isHurt = false;
			anim.SetInteger ("State", 0);

		}

		if(other.gameObject.CompareTag("Enemies")){
			takeDamage (1);

		}

		if (other.gameObject.CompareTag ("Platform")) {
			isHurt = false;
			anim.SetInteger ("State", 0);


		}

	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if(other.gameObject.CompareTag("Enemies")){
			takeDamage (1);
		}

		if (other.gameObject.CompareTag("Death"))
		{
			source.PlayOneShot (Ouch);
			isAlive = false;
			sceneFader.IsFaded = false;
		}

		if (other.gameObject.CompareTag ("Crosshair")) {
			
			if (CrosshairCounter <= 2) {

				Invulnerable = false;
				takeDamage (2);
				Destroy (other.gameObject);
				Invoke ("spawnCrosshair", 3);
				source.PlayOneShot (Boom);
			}

		}

		if (other.gameObject.CompareTag("Obstacle"))
		{
			takeDamage (1);
		}

		if(other.gameObject.CompareTag("Checkpoint"))
		{
			respawnPosition = other.transform.position;
		}

		if (other.gameObject.CompareTag("Knockback"))
		{
			takeDamage (0);
		}

	}

 
	void MovePlayer(float playerSpeed)
	{
		//code for player movement

		if(playerSpeed < 0 && ! jumping  || playerSpeed > 0 && !jumping)
		{
			if (isHurt == false)
			{
				anim.SetInteger("State",1);
			}
		}

		if(playerSpeed == 0 && !jumping && !isPouncing)
		{
			if (isHurt == false) 
			{
				anim.SetInteger ("State", 0);
			}
		}

		//Default movement
		if (!isPouncing &&!isHurt) {
			rb.velocity = new Vector3(playerSpeed, rb.velocity.y,0);

		}

		//Allows Addforce on Damage
		rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y,0);

		Vector3 tempVelocity = rb.velocity;
		tempVelocity.y = Mathf.Clamp (rb.velocity.y, -8	, 8);
		rb.velocity = tempVelocity;
	}

	void CheckKeyboard(){


		speed = Input.GetAxis ("Horizontal") * speedX;

		//Jump movement
		if(Input.GetKeyDown(KeyCode.UpArrow) && !jumping)
		{
			Jump ();
			jumping = true;
		}

		//fall faster
		if (Input.GetKeyDown (KeyCode.DownArrow)) 
		{
			rb.AddForce (new Vector2 (0, jumpspeedY * -.6f));
		}

		if (IsKiller && PlayerData.AlignSet || !PlayerData.AlignSet) {

			//pounce
			if (Input.GetKeyDown (KeyCode.V) && !isPouncing && !jumping) {
				Pounce ();
			}

			//Attack
			if (Input.GetKeyDown (KeyCode.C) && !isPouncing && !Attacking) {
				Attack ();
			}

		}


		if (!IsKiller && PlayerData.AlignSet || !PlayerData.AlignSet) 
		{
			
			if (Input.GetKeyDown (KeyCode.X) && PounceCD >= PounceCoolDown) 
			{
				TeleIcon.SetActive (true);
			}

			if(Input.GetKeyUp(KeyCode.X) && PounceCD >= PounceCoolDown)
			{
				Teleport ();
				TeleIcon.SetActive (false);
			}

			if (Input.GetKeyDown (KeyCode.Z)) 
			{
				Dodge ();
			}
		}
	}

	void Jump(){

		rb.AddForce (new Vector2 (0,jumpspeedY));

	}


	void Pounce(){

		if (PounceCD >= PounceCoolDown) {
			isPouncing = true;
			jumping = true;
			anim.SetInteger ("State", 2);
			PounceCD = 0;

			if (facingRight) {
				rb.AddForce (PounceForce);
			}

			if (!facingRight) {
				rb.AddForce (new Vector2(- PounceForce.x , PounceForce.y));			
			}



			Instantiate (Pouncer, PouncePos.position, Quaternion.identity, gameObject.transform);
		}
	}

	void flip () 
	{
		if (speed > 0 && !facingRight || speed < 0 && facingRight) 
		{
			facingRight = !facingRight;

			Vector3 temp = transform.localScale;
			temp.x *= -1;
			transform.localScale = temp;
		}
	}

	void Attack()
	{
		if (PounceCD > 1) {
			
			Instantiate (Slash, PouncePos.position, Quaternion.identity);
			Attacking = true;

			PounceCD -= 2;

			Invoke ("AttackReset", .5f);
//			anim.SetInteger ("State", 1);
		}

	}

	public void AttackReset()
	{
		Attacking = false;
		anim.SetInteger ("State", 0);
	}

	void PounceTimer()
	{
		if (PounceCD < PounceCoolDown) {
			PounceCD += 1;
		}
	}

	void takeDamage(int Damage)
	{
		if (isTest) {
			return;
		}

		if (sceneFader.IsFaded && !Invulnerable) {

			rb.velocity = new Vector2 (0, rb.velocity.y);
			hPCurrent -= Damage;
			rb.AddForce (KnockbackForce);
			source.PlayOneShot (Ouch);
			isHurt = true;
			Invoke ("Unhurt", .5f);

			Invulnerability ();

			makeFaded ();

		}
	}


	public void IncHP(float chance, int HP)
	{
		chance = 1 - chance;
		if (Random.value >= chance) 
		{			
			if (hPCurrent < HPMax) {

				Instantiate (HpLogo, feedbackLoc.position, Quaternion.identity);

				int hpToHeal = HPMax - hPCurrent;

				if (hpToHeal >= 2 && HP>hpToHeal) 
				{
					Instantiate (HpLogo, new Vector2(feedbackLoc.position.x,feedbackLoc.position.y + 1), Quaternion.identity);
				}

				if ( hpToHeal >= 3 && HP> hpToHeal) 
				{
					Instantiate (HpLogo, new Vector2(feedbackLoc.position.x,feedbackLoc.position.y + 2), Quaternion.identity);
				}

				hPCurrent += HP;
			}
		}
	}

	[SerializeField]
	private SpriteRenderer wolfSprite;
	private bool isTeleporting = false;


	void Teleport()
	{
		Vector3 temp = gameObject.transform.position;

		if (facingRight) {
			temp.x += TeleportDistance;
		}

		if (!facingRight) {
			temp.x -= TeleportDistance;
		}

		if (RightBarrier != null && LeftBarrier != null) 
		{

			if (temp.x < RightBarrier.transform.position.x && temp.x > LeftBarrier.transform.position.y) {
				gameObject.transform.position = temp;
				PounceCD = 0;


				makeFaded ();
				Invulnerability ();


			}
		} else gameObject.transform.position = temp;
		PounceCD = 0;


		makeFaded ();
		Invulnerability ();

	}

	void Dodge()
	{
		if (PounceCD >= 2) {
			makeFaded ();
			Invulnerability ();
			PounceCD -= 2;
			Physics2D.IgnoreLayerCollision (10, 11, true);
			Invoke ("makeVulnerable", 3);
		}
	}

	void TeleportAnim()
	{
		if (isTeleporting) 
		{
			cFull.a = 1;	
			wolfSprite.color = Color.Lerp (wolfSprite.color,cFull , Time.deltaTime * .5f);
			if (wolfSprite.color.a >= 1) {
				isTeleporting = false;
			}
		}


	}

	void makeFaded()
	{
		Color ctemp = wolfSprite.color;
		ctemp.a = .5f;
		wolfSprite.color = ctemp; 
		isTeleporting = true;
	}

	void Respawn()
	{
		transform.position = respawnPosition;
		hPCurrent = HPMax;
		makeFaded ();
		Invulnerability ();

		isAlive = true;
		sceneFader.IsFaded = true;
		Lives -= 1;
	}


	void Invulnerability ()
	{
		Invulnerable = true;
	}

	void makeVulnerable()
	{
		Invulnerable = false;
		Physics2D.IgnoreLayerCollision (10, 11	, false);
		wolfSprite.color = cFull;
	}

	void spawnCrosshair()
	{
		Instantiate (Crosshair,crosshairLoc.position,Quaternion.identity);
	}

	void Unhurt()
	{
		isHurt = false;
	}


	public void CountDeath ()
	{
		PlayerData.KillCount += 1;
	}


	public void ResetJump()
	{
		jumping = false;
		isPouncing = false;
	}

	public void MovementFreeze()
	{
		rb.velocity = new Vector2 (0, 0);
	}

}
