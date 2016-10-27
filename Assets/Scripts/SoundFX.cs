using UnityEngine;
using System.Collections;

public class SoundFX : MonoBehaviour {

	public AudioClip Ding;

	private AudioSource source;

	// Use this for initialization
	void Start () {
		source = gameObject.GetComponent<AudioSource> ();
	
	}


	public void PlaySFX(AudioClip SFX, float pitch, float vol)
	{
		source.volume = vol;
		source.pitch = pitch;
		source.PlayOneShot (SFX);
	}
}
