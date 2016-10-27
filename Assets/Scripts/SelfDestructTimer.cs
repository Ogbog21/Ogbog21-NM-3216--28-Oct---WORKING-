using UnityEngine;
using System.Collections;

public class SelfDestructTimer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("SelfDestruct", 20);
	
	}


	void SelfDestruct ()
	{
		Destroy (gameObject);
	}

}
