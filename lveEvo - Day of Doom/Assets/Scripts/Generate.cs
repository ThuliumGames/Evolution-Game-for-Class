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
	
	public GameObject sideScreen;
	
	void Start () {
		sideScreen.SetActive(false);
	}
	
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.Space)) {
			
			sideScreen.SetActive(true);
			
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
			Array.Resize (ref DestOnGen, 250);
			bool food = false;
			for (int i = 0; i < 250; ++i) {
				
				if (DestOnGen[i] != null) {
					Destroy (DestOnGen[i]);
				}
				
				if (food) {
					DestOnGen[i] = Instantiate (FoodPrefab, new Vector3 (UnityEngine.Random.Range(-250, 250), 0, UnityEngine.Random.Range(-250, 250)), Quaternion.Euler (0, UnityEngine.Random.Range(0, 360), 0));
				} else {
					DestOnGen[i] = Instantiate (EnemyPrefab, new Vector3 (UnityEngine.Random.Range(-250, 250), 0, UnityEngine.Random.Range(-250, 250)), Quaternion.Euler (0, UnityEngine.Random.Range(0, 360), 0));
				}
				
				food = !food;
			}
			
			if (CanRegen >= 2) {
				
				//Spawn Warriors
				Array.Resize (ref GenePool, 0);
				
				if (GameObject.FindObjectsOfType<NeuralNetwork>().Length != 0) {
					int a = 0;
					foreach (NeuralNetwork NN in GameObject.FindObjectsOfType<NeuralNetwork>()) {
						if (NN.Health >= 20) {
							Array.Resize (ref GenePool, GenePool.Length+1);
							GenePool[GenePool.Length-1] = NN;
						} else {
							Destroy(NN.gameObject);
						}
						++a;
					}
					
					for (int i = 0; i < 100; ++i) {
						GameObject G = Instantiate (WarriorPrefab, new Vector3 (UnityEngine.Random.Range(-250, 250), 5, UnityEngine.Random.Range(-250, 250)), Quaternion.Euler (0, UnityEngine.Random.Range(0, 360), 0));
						G.GetComponent<NeuralNetwork>().GenerateSimilar (GenePool[UnityEngine.Random.Range(0, GenePool.Length)], GenePool[UnityEngine.Random.Range(0, GenePool.Length)]);
					}
					
					for (int i = 0; i < GenePool.Length; ++i) {
						Destroy(GenePool[i].gameObject);
					}
					
				} else {
					
					for (int i = 0; i < 100; ++i) {
						GameObject G = Instantiate (WarriorPrefab, new Vector3 (UnityEngine.Random.Range(-250, 250), 5, UnityEngine.Random.Range(-250, 250)), Quaternion.Euler (0, UnityEngine.Random.Range(0, 360), 0));
						G.GetComponent<NeuralNetwork>().OnCreation ();
					}
				}
			}
			}
	}
}
