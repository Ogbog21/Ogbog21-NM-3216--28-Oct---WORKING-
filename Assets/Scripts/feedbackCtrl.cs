using UnityEngine;
using System.Collections;

public class feedbackCtrl : MonoBehaviour {

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();
		rb.velocity = new Vector2 (0, 2);
		Destroy (gameObject, 2);
	}	

}
