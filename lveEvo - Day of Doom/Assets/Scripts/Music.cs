using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {
	
	public AudioClip Theme;
	
	public float SwitchTime;
	float t = 0;
	
	void FixedUpdate () {
		if (t > 0) {
			if (!GetComponent<AudioSource>().isPlaying) {
				GetComponent<AudioSource>().Play();
			}
		}
		
		t += Time.fixedDeltaTime;
		
		if (t >= SwitchTime) {
			GetComponent<AudioSource>().clip = Theme;
			GetComponent<AudioSource>().Play();
			enabled = false;
		}
		
	}
}
