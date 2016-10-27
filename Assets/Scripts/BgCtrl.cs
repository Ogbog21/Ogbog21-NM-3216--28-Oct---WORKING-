using UnityEngine;
using System.Collections;

public class BgCtrl : MonoBehaviour {

	private Renderer rend;

	public float speed;

	private Vector3 playerOffset;
	private Vector3 origin;

	private GameObject player;
	// Use this for initialization
	void Start () {
		rend = gameObject.GetComponent<Renderer>();
		player = GameObject.Find ("Player");

		origin = player.transform.position;
	
	}

	// Update is called once per frame
	void Update () {

		playerOffset = player.transform.position - origin;
		
		rend.material.mainTextureOffset = new Vector2(playerOffset.x * speed,0);
	}
}
