using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

	public Text lives;

	[SerializeField]
	private PlayerController playerController;

	
	// Update is called once per frame
	void Update () {

		playerController = FindObjectOfType<PlayerController> ();
		lives.text = "Lives :" + playerController.Lives.ToString();	
	}

	public void LoadScene(string SceneName)
	{
//		PlayerData.IsKiller = false;
//		PlayerData.AlignSet = false;
		SceneManager.LoadScene(SceneName);
	}


}
