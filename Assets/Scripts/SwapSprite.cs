using UnityEngine;
using System.Collections;

public class SwapSprite : MonoBehaviour {

	public Sprite NewSprite;

	private SpriteRenderer spriteRenderer;

	private Vector3 scale;

	// Use this for initialization
	void Start () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		scale = transform.localScale;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.name == "Player") 
		{
			spriteRenderer.sprite = NewSprite;
			transform.localScale = scale;
		}
	}
		
}
