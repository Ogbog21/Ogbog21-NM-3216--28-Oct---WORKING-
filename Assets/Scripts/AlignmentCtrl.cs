using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AlignmentCtrl : MonoBehaviour {
	
	public TextAsset WhiteText, BlackText;

	private TextBoxManager textBoxManager;

	private Animator anim;

	[SerializeField]
	private float alignCost;

	void Start()
	{
		anim = GameObject.Find ("Player").GetComponent<Animator> ();
		textBoxManager = FindObjectOfType<TextBoxManager> ();

	}

	void Update ()
	{	

		if (!PlayerData.AlignSet) {
			return;
		}

		if (PlayerData.Scene > 1) 
		{
			if (PlayerData.IsKiller) {
				textBoxManager.ReloadScript (BlackText, false);
				textBoxManager.FixEndLine ();
				anim.SetBool ("Killer", true);
			
			}	
			
			if (!PlayerData.IsKiller) {
				textBoxManager.ReloadScript (WhiteText, false);
				textBoxManager.FixEndLine ();
				anim.SetBool ("Good", true);
			}
		}

	}
	

	public void SetAlign()
	{
		if (PlayerData.KillCount > alignCost) 
		{
			PlayerData.IsKiller = true;
		}

		PlayerData.AlignSet = true;
	}

}
