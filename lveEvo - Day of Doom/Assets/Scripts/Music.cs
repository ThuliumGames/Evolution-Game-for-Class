using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {
	
	public AudioClip Theme;
	
	void Update () {
		
		if (!GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().clip = Theme;
			GetComponent<AudioSource>().Play();
			GetComponent<AudioSource>().loop = true;
			enabled = false;
		}
		
	}
}
