using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : MonoBehaviour {
	
	public static float Health = 1000;
	bool canGetHit = true;
	bool canThisGetHit = true;
	public RectTransform HealthBar;
	public bool almostAttacking = false;
	public bool attacking = false;
	
	public PlayerBehavior NN;
	
	void Update () {
		if (GameObject.FindObjectsOfType<PlayerBehavior>().Length == 1) {
		
			NN = GameObject.FindObjectOfType<PlayerBehavior>();
			
			if (Health <= 0) {
				Application.LoadLevel("Win");
			}
			
			HealthBar.sizeDelta = new Vector2 (Health, 10);
			
			if (GetComponentInParent<Rigidbody>().transform.position.y < 138) {
				GetComponentInParent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
			}		
		} else if (GameObject.FindObjectsOfType<PlayerBehavior>().Length == 0) {
			Application.LoadLevel("Lose");
		}
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.gameObject == NN.gameObject && attacking && canGetHit) {
			if (!NN.Blocking) {
				NN.healthLeft -= 10;
			} else {
				NN.healthLeft -= 5;
			}
			canGetHit = false;
		}
	}
	
	void OnTriggerStay (Collider other) {
		if (other.gameObject == NN.gameObject) {
			if (NN.Attacking && canThisGetHit) {
				canThisGetHit = false;
				Health -= NN.GetComponent<Traits>().AttackPower;
			}
		}
	}
	
	void OnTriggerExit () {
		canGetHit = true;
		canThisGetHit = true;
	}
}
