using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour {
	
	public GameObject WarriorPrefab;
	
	public GameObject EnemyPrefab;
	public GameObject FoodPrefab;
	
	public GameObject[] DestOnGen;
	
	NeuralNetwork[] GenePool;
	
	float[] scores;
	
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.Space)) {
			
			Array.Resize (ref scores, GameObject.FindObjectsOfType<NeuralNetwork>().Length);
			
			int CanRegen = 0;
			
			if (GameObject.FindObjectsOfType<NeuralNetwork>().Length != 0) {
				int x = 0;
				foreach (NeuralNetwork NN in GameObject.FindObjectsOfType<NeuralNetwork>()) {
					if (NN.Health >= 20) {
						++CanRegen;
					}
					scores[x] = NN.Score;
					++x;
				}
			} else {
				CanRegen = 100;
			}
			
			//Spawn Food and Enemys
			Array.Resize (ref DestOnGen, 100);
			bool food = false;
			for (int i = 0; i < 100; ++i) {
				
				if (DestOnGen[i] != null) {
					Destroy (DestOnGen[i]);
				}
				
				if (food) {
					DestOnGen[i] = Instantiate (FoodPrefab, new Vector3 (UnityEngine.Random.Range(-100, 100), 0, UnityEngine.Random.Range(-100, 100)), Quaternion.identity);
				} else {
					DestOnGen[i] = Instantiate (EnemyPrefab, new Vector3 (UnityEngine.Random.Range(-100, 100), 0, UnityEngine.Random.Range(-100, 100)), Quaternion.identity);
				}
				
				food = !food;
			}
			
			if (CanRegen >= 2) {
				
				//Spawn Warriors
				Array.Resize (ref GenePool, 0);
				
				if (GameObject.FindObjectsOfType<NeuralNetwork>().Length != 0) {
					int a = 0;
					foreach (NeuralNetwork NN in GameObject.FindObjectsOfType<NeuralNetwork>()) {
						if (NN.Health >= 20 && scores[a]/Mathf.Max(scores) > 0.5f) {
							Array.Resize (ref GenePool, GenePool.Length+1);
							GenePool[GenePool.Length-1] = NN;
						} else {
							Destroy(NN.gameObject);
						}
						++a;
					}
					
					for (int i = 0; i < 100; ++i) {
						GameObject G = Instantiate (WarriorPrefab, new Vector3 (UnityEngine.Random.Range(-50, 50), 1, UnityEngine.Random.Range(-50, 50)), Quaternion.identity);
						G.GetComponent<NeuralNetwork>().GenerateSimilar (GenePool[UnityEngine.Random.Range(0, GenePool.Length)], GenePool[UnityEngine.Random.Range(0, GenePool.Length)]);
					}
					
					for (int i = 0; i < GenePool.Length; ++i) {
						Destroy(GenePool[i].gameObject);
					}
					
				} else {
					
					for (int i = 0; i < 100; ++i) {
						GameObject G = Instantiate (WarriorPrefab, new Vector3 (UnityEngine.Random.Range(-50, 50), 1, UnityEngine.Random.Range(-50, 50)), Quaternion.identity);
						G.GetComponent<NeuralNetwork>().OnCreation ();
					}
				}
			}
		}
	}
}
