using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour 
{
	public Transform player;
	public float MaxY;

	Vector3 offset;
//	private float camY;

	void Start()
	{
		offset = transform.position - player.position;	
//		camY = 1.51f;		

		if (MaxY == 0) 
		{
			MaxY = 6;
		}
	}
		
	void LateUpdate () 	
	{	
		Vector3 temp = new Vector3 (player.position.x, player.position.y + offset.y + 1, transform.position.z);
		temp.y = Mathf.Clamp (temp.y, -1.17f , MaxY);
		transform.position = temp;
	}
}
