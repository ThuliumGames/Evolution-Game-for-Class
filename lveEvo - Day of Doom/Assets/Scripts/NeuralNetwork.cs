using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Closest Enemy Local X : Closest Enemy Local Z : Closest Enemy Distance : Closest Food Local X : Closest Food Local Z : Closest Food Distance : Left Wall Distance: Right Wall Distance: Front Wall Distance : Enemy is Attacking//
//0,1,2 - Time Without Enemy : 3,4,5 - Time Without Food : 6,7,8,9 - 1
[System.Serializable]
public class Inputs {
	[HideInInspector]
	public float inputValue;
	
	public float[] weights;
	
	public float weight;
}

public class NeuralNetwork : MonoBehaviour {
	
	public int Health;
	public int healthLeft;
	
	public int Score;
	
	public Inputs[] inputs;
	
	public Animator Anim;
	[HideInInspector]
	public AnimatorClipInfo[] AnimClipInfo;
	public string[] AnimNames = {"Idle", "Walk", "TurnLeft", "TurnRight", "SwingSword", "Block"};
	public float[] WeightWeights = {0.9f, 1, 0.95f, 0.95f, 1.05f, 1.05f};
	
	float dist = 1000000;
	Transform closestObject = null;
	[HideInInspector]
	public float[] allValues = {0, 0, 0, 0, 0, 0};
	float[] max = {0, 0};
	[HideInInspector]
	public float TimeAttack;
	[HideInInspector]
	public float TimeFood;
	
	public bool Attacking = false;
	public bool Blocking = false;
	
	public void OnCreation () {
		
		GetComponent<Traits>().CreateNew();
		
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
		
		GetComponent<Traits>().CreateSimilar(N1.GetComponent<Traits>(), N2.GetComponent<Traits>());
		
		int x = 0;
		foreach (Inputs I in inputs) {
			Array.Resize (ref I.weights, 6);
			for (int i = 0; i < I.weights.Length; ++i) {
				
				bool ChoseN1 = true;
				
				int a = UnityEngine.Random.Range (0, 100);
				
				int z = 0;
				
				if (N1.Score < N2.Score) {
					
					z = (N1.Score+1)/(N2.Score+1);
					
					if (a < z) {
						I.weights[i] = N1.inputs[x].weights[i];
					} else {
						I.weights[i] = N2.inputs[x].weights[i];
						ChoseN1 = false;
					}
				} else {
					z = (N2.Score+1)/(N1.Score+1);
					if (a < z) {
						I.weights[i] = N2.inputs[x].weights[i];
						ChoseN1 = false;
					} else {
						I.weights[i] = N1.inputs[x].weights[i];
					}
				}
				
				if (ChoseN1) {
					if (N1.Score >= 50) {
						I.weights[i] += UnityEngine.Random.Range(-0.001f, 0.001f);
					} else {
						I.weights[i] += UnityEngine.Random.Range(-0.001f*(50-N1.Score), 0.001f*(50-N1.Score));
					}
				} else {
					if (N2.Score >= 50) {
						I.weights[i] += UnityEngine.Random.Range(-0.001f, 0.001f);
					} else {
						I.weights[i] += UnityEngine.Random.Range(-0.001f*(50-N2.Score), 0.001f*(50-N2.Score));
					}
				}
			}
			++x;
		}
	}
	
	void Start () {
		healthLeft = (int)GetComponent<Traits>().Health;
	}
	
	void Update () {
		
		//Reset Numbers
		for (int i = 0; i < inputs.Length; ++i) {
			inputs[i].inputValue = 0;
		}
		
		healthLeft = (int)Mathf.Clamp (healthLeft, -1, GetComponent<Traits>().Health);
		
		if (healthLeft <= 0) {
			Destroy(gameObject);
		}
		
		TimeAttack += Time.deltaTime;
		TimeFood += Time.deltaTime;
		
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
		
		//CalculateWeight
		inputs[0].weight = 1+TimeAttack/100;
		inputs[1].weight = 1+TimeAttack/100;
		inputs[2].weight = 1+TimeAttack/100;
		inputs[3].weight = 1+TimeFood/500;
		inputs[4].weight = 1+TimeFood/500;
		inputs[5].weight = 1+TimeFood/500;
		inputs[6].weight = 1;
		inputs[7].weight = 1;
		inputs[8].weight = 1;
		
		
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
			allValues[i] = CalculateValue(i)*WeightWeights[i];
			
			if (allValues[i] > max[0] || i == 0) {
				max[0] = allValues[i];
				max[1] = i;
			}
		}
		
		OutputValue = (int)max[1];
		
		//Play Anim
		Anim.Play(AnimNames[OutputValue]);
		
		//Camera Follow This
		
		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			RaycastHit theObject;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out theObject, Mathf.Infinity)) {
				if (theObject.collider.gameObject == gameObject) {
					Camera.main.GetComponent<God>().following = true;
					Camera.main.GetComponent<God>().objectToFollow = transform;
				}
			}
		}
		
	}
	
	//--------------------//
	
	float CalculateValue (int x) {
		
		float retVal = 0;
		
		for (int i = 0; i < inputs.Length; ++i) {
			retVal += inputs[i].inputValue * inputs[i].weight * inputs[i].weights[x];
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
			TimeFood = 0;
			Health += 5;
			++healthLeft;
			Score += 1;
			Destroy (other.collider.gameObject);
		}
	}
	
	public void BeingAttacked (float N) {
		inputs[9].inputValue = N;
	}
}
