using UnityEngine;
using System.Collections;

public class rotator : MonoBehaviour {

	public float RotateSpeed;

	void Start()
	{
		if (RotateSpeed == 0) 
		{
			RotateSpeed = 1;
		}
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate  (new Vector3 (0, 0, 45) * Time.deltaTime * RotateSpeed);
	}
}
