using UnityEngine;
using System.Collections;

public class AISpawner : MonoBehaviour {

	public GameObject Bird;
	public float BirdHt, BirdSpawnRate;

	private Transform player;

	// Use this for initialization
	void Start () {

		player = GameObject.Find ("Player").GetComponent<Transform> ();
		InvokeRepeating ("SBird", BirdSpawnRate, BirdSpawnRate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SBird()
	{
		Vector3 BirdPos = new Vector3 (player.transform.position.x + 10, BirdHt);
		Instantiate (Bird, BirdPos, Quaternion.identity);
	}
}
