using UnityEngine;
using System.Collections;

public class CrosshairControl : MonoBehaviour {

	private GameObject player;

	private Rigidbody2D rb;

	[SerializeField]
	private SpriteRenderer colour;

	private float clampedX,clampedY;
	private float xtemp, ytemp;

	[SerializeField]	
	private float speed,smultiplier;

	[SerializeField]
	private float lerpval;

	public float Counter,lagTime;

	[SerializeField]
	private float counterMax;

	[SerializeField]
	private Color crosshairFade, crosshairOpaque, crosshairColour;

	private SceneFader sceneFader;

	private bool isStarted;

	private PlayerController playerController;

	void Awake(){
		colour = gameObject.GetComponent<SpriteRenderer> ();		
	}

	private AudioSource source;

	// Use this for initialization
	void Start () {
		
		counterMax = Counter;

		sceneFader = GameObject.Find ("Cover").GetComponent<SceneFader> ();

		player = GameObject.Find ("Player");	
		playerController = player.GetComponent<PlayerController> ();

		rb = gameObject.GetComponent<Rigidbody2D> ();
		source = gameObject.GetComponent<AudioSource> ();


		crosshairColour = colour.color;
		crosshairFade = new Color (Color.white.r, Color.white.g, Color.white.b, 0f);
		crosshairOpaque = new Color (crosshairColour.r, crosshairColour.g, crosshairColour.b, 1f);
		colour.color = crosshairFade;

		isStarted = false;

		source.pitch = 1.2f;
	}
	
	// Update is called once per frame
	void Update () {
		checkCount ();

		lerpval = ((counterMax-Counter) / counterMax);
		colour.color = Color.Lerp (crosshairFade,crosshairOpaque,lerpval);

//		if (!sceneFader.IsFaded) 
//		{
//			Destroy (gameObject);
//		}
	}

	void FixedUpdate()
	{
		clampedX = Mathf.Clamp (transform.position.x, player.transform.position.x - 7.5f, player.transform.position.x + 7.5f);
		clampedY = Mathf.Clamp (transform.position.y, player.transform.position.y - 2.5f, player.transform.position.y + 2.5f);
		transform.position = new Vector3 (clampedX, clampedY);

		if (sceneFader.IsFaded && !	isStarted) {
			isStarted = true;
			Recalibrate ();
		}

		
		playerController.CrosshairCounter = Counter;

	}


	void Recalibrate()
	{
		if (gameObject.transform.position.x > player.transform.position.x && rb.velocity.x >0 ||
			gameObject.transform.position.x < player.transform.position.x && rb.velocity.x <0) 
		{
			float checkX = gameObject.transform.position.x - player.transform.position.x;
			if (Mathf.Abs (checkX) > 3 ) {
				float tempx = rb.velocity.x;
				tempx *= -1;
				rb.velocity = new Vector2 (tempx, rb.velocity.y);
			}
		}

		//Determine Direction
		xtemp = player.transform.position.x - gameObject.transform.position.x;
		ytemp = player.transform.position.y - gameObject.transform.position.y;

		xtemp = Mathf.Clamp (xtemp, -1f, 1f);
		ytemp = Mathf.Clamp (ytemp, -1f, 1f);

		Vector2 calForce = new Vector2 (xtemp,ytemp) ;		

		if (Counter > 4) {
			float ranForce = smultiplier * .2f;

			//Adds randomforce
			rb.AddForce(new Vector2
				(Random.Range(ranForce * -1 ,ranForce),Random.Range(ranForce * -1 ,ranForce)));
		}

		if (Counter > 0) {
			rb.AddForce (calForce * smultiplier);

		}

		if (Counter == 0) {
			float ranForce = smultiplier * .1f;

			rb.AddForce(calForce);
			rb.AddForce(new Vector2
				(Random.Range(ranForce * -1 ,ranForce),Random.Range(ranForce * -1 ,ranForce)));
		}

		if (Counter > 0) {
			Counter -= 1;
		}

		Invoke("Recalibrate", lagTime);

	}

	void checkCount()
	{
		if (Counter >= 5) {
			lagTime = 4;
			smultiplier = speed;
			source.pitch = 1.2f;
		}

		if (Counter <= 3 && Counter > 1) {
			lagTime = 2;
			float tempspeed = speed * 2;
			smultiplier = tempspeed;
			source.pitch = 1.5f;
		}

		if (Counter <= 1 && Counter >=0) {
			lagTime = 0;	
			float tempspeed = speed * 3;
			smultiplier = tempspeed;
			source.pitch = 2;				
		}
	}


}
