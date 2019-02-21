using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeetOnTrigger : MonoBehaviour {
	
	void OnTriggerEnter (Collider other) {
		GetComponentInChildren<AudioSource>().Play();
		Destroy(other.gameObject, 3);
	}
}
