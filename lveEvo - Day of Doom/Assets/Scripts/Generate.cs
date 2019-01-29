using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour {
	
	public GameObject WarriorPrefab;
	
	public GameObject EnemyPrefab;
	
	public GameObject[] DestOnGen;
	
	public Animator GateOpen;
	
	NeuralNetwork[] GenePool;
	
	float[] scores;
	
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.E)) {
			
			if (GameObject.FindObjectsOfType<Enemy>().Length < 10) {
			
				//Spawn Enemys
				Array.Resize (ref DestOnGen, 250);
				for (int i = 0; i < 250; ++i) {
					
					if (DestOnGen[i] != null) {
						Destroy (DestOnGen[i]);
					}
					
					DestOnGen[i] = Instantiate (EnemyPrefab, new Vector3 (UnityEngine.Random.Range(-20, 20), 0, UnityEngine.Random.Range(530, 600)), Quaternion.Euler (0, UnityEngine.Random.Range(0, 360), 0));
				}
				
				GateOpen.Play("OpenGate");
			}
		}
		
		bool CantClose = false;
		
		for (int i = 0; i < DestOnGen.Length; ++i) {
			if (DestOnGen[i] != null) {
				if (DestOnGen[i].transform.position.magnitude > 480) {
					CantClose = true;
				}
			}
		}
		
		if (!CantClose && GateOpen.GetCurrentAnimatorClipInfo(0)[0].clip.name == "GateOpened") {
			GateOpen.Play("CloseGate");
		}
		
		
		if (Input.GetKeyDown(KeyCode.Space)) {
				
			//Spawn Warriors
			Array.Resize (ref GenePool, 0);
			
			if (GameObject.FindObjectsOfType<NeuralNetwork>().Length != 0) {
				
				foreach (NeuralNetwork NN in GameObject.FindObjectsOfType<NeuralNetwork>()) {
					for (int i = 0; i < NN.Health+1; ++i) {
						Array.Resize (ref GenePool, GenePool.Length+1);
						GenePool[GenePool.Length-1] = NN;
					}
				}
				
				for (int i = 0; i < 100; ++i) {
					GameObject G = Instantiate (WarriorPrefab, new Vector3 (UnityEngine.Random.Range(-250, 250), 0, UnityEngine.Random.Range(-250, 250)), Quaternion.Euler (0, UnityEngine.Random.Range(0, 360), 0));
					G.GetComponent<NeuralNetwork>().GenerateSimilar (GenePool[UnityEngine.Random.Range(0, GenePool.Length)], GenePool[UnityEngine.Random.Range(0, GenePool.Length)]);
				}
				
				for (int i = 0; i < GenePool.Length; ++i) {
					if (GenePool[i] != null) {
						Destroy(GenePool[i].gameObject);
					}
				}
				
			} else {
				
				for (int i = 0; i < 100; ++i) {
					GameObject G = Instantiate (WarriorPrefab, new Vector3 (UnityEngine.Random.Range(-250, 250), 0, UnityEngine.Random.Range(-250, 250)), Quaternion.Euler (0, UnityEngine.Random.Range(0, 360), 0));
					G.GetComponent<NeuralNetwork>().OnCreation ();
				}
			}
		}
	}
}
