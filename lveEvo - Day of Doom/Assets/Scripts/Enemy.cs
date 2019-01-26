﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	
	float range = 5;
	
	NeuralNetwork[] NN;
	
	float T = 0;
	
	void Update () {
		NN = GameObject.FindObjectsOfType<NeuralNetwork>();
		T += Time.deltaTime;
		
		foreach (NeuralNetwork N in NN) {
			if (T > 2f) {
				if (Vector3.Distance(transform.position, N.transform.position) <= range) {
					
					if (T > 5) {
						if (N.Blocking) {
							N.Score += 5;
						} else {
							N.Score -= 10;
						}
						T = 0;
					}
					
					N.BeingAttacked(T);
				}
			}
		
			if (Vector3.Distance(transform.position, N.transform.position) <= range) {
				if (N.Attacking) {
					N.TimeAttack = 0;
					N.Score += 20;
					Destroy(this.gameObject);
				}
			}
		}
	}
}