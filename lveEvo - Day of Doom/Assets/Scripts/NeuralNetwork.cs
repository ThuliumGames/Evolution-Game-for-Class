using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Closest Enemy Local X : Closest Enemy Local Z : Closest Enemy Distance : Closest Food Local X : Closest Food Local Z : Closest Food Distance : Left Wall Distance: Right Wall Distance: Front Wall Distance : Enemy is Attacking//
[System.Serializable]
public class Inputs {
	[HideInInspector]
	public float inputValue;
	//[HideInInspector]
	public float[] weights;
}

public class NeuralNetwork : MonoBehaviour {
	
	public int Health = 20;
	
	public int Score = 20;
	
	public Inputs[] inputs;
	
	public Animator Anim;
	AnimatorClipInfo[] AnimClipInfo;
	public string[] AnimNames = {"Idle", "Walk", "TurnLeft", "TurnRight", "SwingSword", "Block"};
	
	float dist = 1000000;
	Transform closestObject = null;
	float[] allValues = {0, 0, 0, 0, 0, 0};
	float[] max = {0, 0};
	
	public bool Attacking = false;
	public bool Blocking = false;
	
	public void OnCreation () {
		
		foreach (Inputs I in inputs) {
			Array.Resize (ref I.weights, 6);
			float Total = 0;
			int StartNum = UnityEngine.Random.Range (0, I.weights.Length);
			for (int i = 0; i < I.weights.Length; ++i) {
				if (i == 0) {
					I.weights[StartNum] = UnityEngine.Random.Range(0, 10-Total);
				} else {
					if (i <= StartNum) {
						I.weights[i-1] = UnityEngine.Random.Range(0, 10-Total);
					} else {
						I.weights[i] = UnityEngine.Random.Range(0, 10-Total);
					}
				}
				Total += I.weights[i];
			}
		}
	}
	
	public void GenerateSimilar (NeuralNetwork N1, NeuralNetwork N2) {
		int x = 0;
		foreach (Inputs I in inputs) {
			Array.Resize (ref I.weights, 6);
			for (int i = 0; i < I.weights.Length; ++i) {
				
				bool ChoseN1 = true;
				
				int a = UnityEngine.Random.Range (0, 100);
				
				int z = 0;
				
				if (N1.Score < N2.Score) {
					
					z = N1.Score/N2.Score;
					
					if (a < z) {
						I.weights[i] = N1.inputs[x].weights[i];
					} else {
						I.weights[i] = N2.inputs[x].weights[i];
						ChoseN1 = false;
					}
				} else {
					z = N2.Score/N1.Score;
					if (a < z) {
						I.weights[i] = N2.inputs[x].weights[i];
						ChoseN1 = false;
					} else {
						I.weights[i] = N1.inputs[x].weights[i];
					}
				}
				
				if (ChoseN1) {
					if (N1.Score >= 500) {
						I.weights[i] += UnityEngine.Random.Range(-0.001f, 0.001f);
					} else {
						I.weights[i] += UnityEngine.Random.Range(-0.001f*(500-N1.Score), 0.001f*(500-N1.Score));
					}
				} else {
					if (N2.Score >= 500) {
						I.weights[i] += UnityEngine.Random.Range(-0.001f, 0.001f);
					} else {
						I.weights[i] += UnityEngine.Random.Range(-0.001f*(500-N2.Score), 0.001f*(500-N2.Score));
					}
				}
			}
			++x;
		}
	}
	
	void Update () {
		
		//Test if Attacking or Blocking
		AnimClipInfo = Anim.GetCurrentAnimatorClipInfo(0);
		
		if (AnimClipInfo[0].clip.name == "SwingSword") {
			Attacking = true;
		} else {
			Attacking = false;
		}
		
		if (AnimClipInfo[0].clip.name == "Block") {
			Blocking = true;
		} else {
			Blocking = false;
		}
		
		int OutputValue = 0;
		
		//Get Closest Enemy
		GetCloseAndOutput("Enemy", 0, 1, 2);
		
		//Get Closest Food
		GetCloseAndOutput("Food", 3, 4, 5);
		
		//Walls
		RaycastHit hit;
		Physics.Raycast (transform.position, -transform.right, out hit, 5000);
		inputs[6].inputValue = hit.distance;
		Physics.Raycast (transform.position, transform.right, out hit, 5000);
		inputs[7].inputValue = hit.distance;
		Physics.Raycast (transform.position, transform.forward, out hit, 5000);
		inputs[8].inputValue = hit.distance;
		
		//CalculateValue
		
		for (int i = 0; i < allValues.Length; ++i) {
			allValues[i] = CalculateValue(i);
			
			if (allValues[i] > max[0] || i == 0) {
				max[0] = allValues[i];
				max[1] = i;
			}
		}
		
		OutputValue = (int)max[1];
		
		//Play Anim
		Anim.Play(AnimNames[OutputValue]);
		
		//Reset Numbers
		for (int i = 0; i < inputs.Length; ++i) {
			inputs[i].inputValue = 0;
		}
	}
	
	//--------------------//
	
	float CalculateValue (int x) {
		
		float retVal = 0;
		
		for (int i = 0; i < inputs.Length; ++i) {
			retVal += inputs[i].inputValue * inputs[i].weights[x];
		}
		
		return (retVal);
	}
	
	//--------------------//
	
	void GetCloseAndOutput (string TagName, int F1, int F2, int F3) {
		dist = 5000;
		closestObject = null;
		
		//Get Closest Food
		foreach (GameObject T in GameObject.FindGameObjectsWithTag(TagName)) {
			
			float D = Vector3.Distance (transform.position, T.transform.position);
			
			if (D < dist) {
				dist = D;
				closestObject = T.transform;
			}
		}
		
		//Determine Input Values
		if (closestObject != null) {
			inputs[F1].inputValue = transform.InverseTransformPoint(closestObject.position).x;
			inputs[F2].inputValue = transform.InverseTransformPoint(closestObject.position).z;
			inputs[F3].inputValue = dist;
		}
	}
	
	void OnCollisionEnter (Collision other) {
		if (other.collider.tag == "Food") {
			Health += 5;
			Destroy (other.collider.gameObject);
		}
	}
	
	public void BeingAttacked (float N) {
		inputs[9].inputValue = N;
	}
}
