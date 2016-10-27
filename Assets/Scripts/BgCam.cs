using UnityEngine;
using System.Collections;

public class BgCam : MonoBehaviour {

	private PlayerController player; 

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController> ();
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		Vector3 temp =	new Vector3 ( transform.position.x,(.2f * player.transform.position.y) + 121 ,transform.position.z);
		temp.y = Mathf.Clamp (temp.y, 121, 125);
		transform.position = temp;	
	}
}
