using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	
	public float Health = 25;
	bool canGetHit = true;
	
	float range = 10;
	
	public static NeuralNetwork[] NN;
	
	float T = 0;
	
	void Update () {
		
		if (name == "StayForever") {
			NN = GameObject.FindObjectsOfType<NeuralNetwork>();
		} else {
			
			T += Time.deltaTime;
			
			for (int i = 0; i < NN.Length; ++i) {
				
				if (Vector3.Distance(transform.position, NN[i].transform.position) <= range) {
				
					if (T > 2f) {
						if (T > 5) {
							if (NN[i].Blocking) {
								NN[i].Score += 5;
							} else {
								NN[i].Score -= 10;
								--NN[i].healthLeft;
							}
							T = 0;
						}
						
						NN[i].BeingAttacked(T);
					}
					
					if (NN[i].Attacking) {
						if (canGetHit) {
							NN[i].TimeAttack = 0;
							NN[i].Score += 10;
							Health -= NN[i].GetComponent<Traits>().AttackPower;
							canGetHit = false;
							if (Health <= 0) {
								NN[i].Score += 20;
								Destroy(this.gameObject);
							}
						}
					} else {
						canGetHit = true;
					}
				}
			}
		}
	}
}
