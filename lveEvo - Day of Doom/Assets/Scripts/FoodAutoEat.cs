using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodAutoEat : MonoBehaviour {
	
	public Transform goTo;
	
	public float speed;
	
	void Update () {
		
		if (goTo != null) {
			transform.LookAt (goTo.position);
			speed += 9.81f*(Time.deltaTime/2);
			transform.Translate (0, 0, speed);
		} else {
			GetComponent<Rigidbody>().isKinematic = false;
		}
	}
}
