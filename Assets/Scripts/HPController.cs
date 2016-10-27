using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPController : MonoBehaviour {

	private PlayerController playerController;
	private Image HPBar;
	private Text HPText;
	private Image PounceBar;
	private Text PounceText;

	[SerializeField]
	private float fillAmount;

	private float pounceFA;

	void Awake(){
		
	}

	// Use this for initialization
	void Start () {
		HPBar = GameObject.Find ("Hp Bar").GetComponent<Image> ();
//		HPText = GameObject.Find ("Hp Text").GetComponent<Text>();
		
		PounceBar = GameObject.Find ("Pounce Bar").GetComponent<Image> ();
//		PounceText = GameObject.Find ("Pounce CD").GetComponent<Text> ();
    	
		playerController = GameObject.Find ("Player").GetComponent<PlayerController>();

//		HPText.text = playerController.HPCurrent.ToString () + "/" + playerController.HPMax.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
		fillAmount = Map (playerController.HPCurrent,playerController.HPMax);
		pounceFA = Map (playerController.PounceCD, playerController.PounceCoolDown);

		handleBar ();	
		handlePBar ();
	
	}

	private void handleBar()
	{
		if (fillAmount != HPBar.fillAmount) {	
			
			HPBar.fillAmount = Mathf.Lerp (HPBar.fillAmount, fillAmount, Time.deltaTime * .5f);
//			HPText.text = playerController.HPCurrent.ToString () + "/" + playerController.HPMax.ToString ();
		} 
	}

	private float Map (float Current,float Max)
	{
		return (Current / Max);
	}

	private void handlePBar()
	{
		if (PounceBar.fillAmount != pounceFA) {
			
			PounceBar.fillAmount = Mathf.Lerp (PounceBar.fillAmount, pounceFA, Time.deltaTime * .5f);

			int CoolDown = playerController.PounceCoolDown - playerController.PounceCD;
//			PounceText.text = CoolDown.ToString() + " second(s)";

			if (CoolDown <= 0) {
//				PounceText.text = "Full Energy";
			}
		}
	}

	private string display (float min, float max)
	{
		return min.ToString () + "/" + max.ToString();
	}
}
