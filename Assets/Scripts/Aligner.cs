using UnityEngine;
using System.Collections;

public class Aligner : MonoBehaviour {

	private AlignmentCtrl align;

	public TextBoxManager theTextbox;

	public int GoodStart, GoodEnd;
	public int BadStart, BadEnd;

	public bool Set;

	// Use this for initialization
	void Start () {
		align = FindObjectOfType<AlignmentCtrl> ();
	}
	
	// Update is called once per frame
	void Update () {
		theTextbox = FindObjectOfType<TextBoxManager> ();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player") 
		{
			align.SetAlign ();

				if (!PlayerData.IsKiller) {
					theTextbox.currentLine = GoodStart;
					theTextbox.endAtLine = GoodEnd;
				}

				if (PlayerData.IsKiller) {
					theTextbox.currentLine = BadStart;
					theTextbox.endAtLine = BadEnd;				

				Set = true;
			}
		}
	}
}
