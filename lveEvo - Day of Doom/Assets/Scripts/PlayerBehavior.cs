using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Closest Enemy Local X : Closest Enemy Local Z : Closest Enemy Distance : Closest Food Local X : Closest Food Local Z : Closest Food Distance : Left Wall Distance: Right Wall Distance: Front Wall Distance : Enemy is Attacking//
//0,1,2 - Time Without Enemy : 3,4,5 - Time Without Food : 6,7,8,9 - 1

public class PlayerBehavior : NNs {
	
	public int Food;
	[HideInInspector]
	public int healthLeft;
	
	public int Score;
	[HideInInspector]
	public int ScorePrev;
	
	public Animator Anim;
	public GameObject Death;
	[HideInInspector]
	public AnimatorClipInfo[] AnimClipInfo;
	[HideInInspector]
	public string[] AnimNames = {"Idle", "Walk", "TurnLeft", "TurnRight", "SwingSword", "Block"};
	
	
	
	[HideInInspector]
	public float TimeAttack;
	[HideInInspector]
	public float TimeIdle;
	[HideInInspector]
	public float TimeFood;
	
	[HideInInspector]
	public bool Attacking = false;
	[HideInInspector]
	public bool Blocking = false;
	
	public LayerMask LM;
	
	float Ti;
	float Tm;
	float Tf;
	float Ta;
	float Td;
	float dist;
	Transform closestObject;
	
	public void OnCreation () {
		
		GetComponent<Traits>().CreateNew();
		
		foreach (Layer I in OutputLayer) {
			Array.Resize (ref I.Weights, 0);
			I.Bias = UnityEngine.Random.Range(0.0f, 1.0f);
		}
		
		foreach (Layer I in HiddenLayer) {
			Array.Resize (ref I.Weights, 6);
			for (int i = 0; i < I.Weights.Length; ++i) {
				I.Weights[i] = UnityEngine.Random.Range(-1.0f, 1.0f);
			}
			I.Bias = UnityEngine.Random.Range(0.0f, 1.0f);
		}
		
		foreach (Layer I in InputLayer) {
			Array.Resize (ref I.Weights, 8);
			for (int i = 0; i < I.Weights.Length; ++i) {
				I.Weights[i] = UnityEngine.Random.Range(-1.0f, 1.0f);
			}
			I.Bias = UnityEngine.Random.Range(0.0f, 1.0f);
		}
	}
	
	public void GenerateSimilar (PlayerBehavior N1, PlayerBehavior N2) {
		
		PlayerBehavior[] Ns = {N1, N2};
		
		int x = 0;
		
		GetComponent<Traits>().CreateSimilar(N1.GetComponent<Traits>(), N2.GetComponent<Traits>());
		
		foreach (Layer I in OutputLayer) {
			
			int T = UnityEngine.Random.Range(0, 2);
			
			Array.Resize (ref I.Weights, 0);
			I.Bias = Ns[T].OutputLayer[x].Bias;
			I.Bias += UnityEngine.Random.Range(-0.01f/Mathf.Clamp(Ns[T].Score, 1, 100), 0.01f/Mathf.Clamp(Ns[T].Score, 1, 100));
			I.Bias = Mathf.Clamp(I.Bias, 0.0f, 1.0f);
			++x;
		}
		x = 0;
		foreach (Layer I in HiddenLayer) {
			
			int T = UnityEngine.Random.Range(0, 2);
			
			Array.Resize (ref I.Weights, 6);
			for (int i = 0; i < I.Weights.Length; ++i) {
				I.Weights[i] = Ns[T].HiddenLayer[x].Weights[i];
				I.Weights[i] += UnityEngine.Random.Range(-0.01f/Mathf.Clamp(Ns[T].Score, 1, 100), 0.01f/Mathf.Clamp(Ns[T].Score, 1, 100));
				I.Weights[i] = Mathf.Clamp(I.Weights[i], -1.0f, 1.0f);
			}
			I.Bias = Ns[T].HiddenLayer[x].Bias;
			I.Bias += UnityEngine.Random.Range(-0.01f/Mathf.Clamp(Ns[T].Score, 1, 100), 0.01f/Mathf.Clamp(Ns[T].Score, 1, 100));
			I.Bias = Mathf.Clamp(I.Bias, 0.0f, 1.0f);
			++x;
		}
		x = 0;
		foreach (Layer I in InputLayer) {
			
			int T = UnityEngine.Random.Range(0, 2);
			
			Array.Resize (ref I.Weights, 8);
			for (int i = 0; i < I.Weights.Length; ++i) {
				I.Weights[i] = Ns[T].InputLayer[x].Weights[i];
				I.Weights[i] += UnityEngine.Random.Range(-0.01f/Mathf.Clamp(Ns[T].Score, 1, 100), 0.01f/Mathf.Clamp(Ns[T].Score, 1, 100));
				I.Weights[i] = Mathf.Clamp(I.Weights[i], -1.0f, 1.0f);
			}
			I.Bias = Ns[T].InputLayer[x].Bias;
			I.Bias += UnityEngine.Random.Range(-0.01f/Mathf.Clamp(Ns[T].Score, 1, 100), 0.01f/Mathf.Clamp(Ns[T].Score, 1, 100));
			I.Bias = Mathf.Clamp(I.Bias, 0.0f, 1.0f);
			++x;
		}
	}
	
	void Start () {
		healthLeft = (int)GetComponent<Traits>().Health;
	}
	
	void Update () {
		
		if (TimeFood >= 40) {
			Tf += Time.deltaTime;
			if (Tf >= 1) {
				--Score;
				Tf = 0;
			}
		}
		
		if (TimeAttack >= 40) {
			Ta += Time.deltaTime;
			if (Ta >= 1) {
				--Score;
				Ta = 0;
			}
		}
		
		if (TimeIdle >= 20) {
			Td += Time.deltaTime;
			if (Td >= 1) {
				--Score;
				Td = 0;
			}
		}
		
		//Reset Numbers
		for (int i = 0; i < InputLayer.Length; ++i) {
			InputLayer[i].InputValue = 0;
		}
		
		healthLeft = (int)Mathf.Clamp (healthLeft, -1, GetComponent<Traits>().Health);
		
		if (healthLeft <= 0) {
			Destroy(gameObject);
		}
		
		TimeAttack += Time.deltaTime;
		TimeFood += Time.deltaTime;
		
		//Test if Attacking or Blocking
		AnimClipInfo = Anim.GetCurrentAnimatorClipInfo(0);
		
		if (AnimClipInfo[0].clip.name == "Idle" || AnimClipInfo[0].clip.name == "TurnLeft" || AnimClipInfo[0].clip.name == "TurnRight") {
			TimeIdle += Time.deltaTime;
		} else {
			TimeIdle = 0;
		}
		
		bool isClose = false;
		if (GameObject.FindObjectOfType<BossAI>() != null) {
			isClose = true;
		} else {
			foreach (GameObject E in Enemy.Ens) {
				if (Enemy.NN[E.GetComponent<Enemy>().C] == this) {
					isClose = true;
				}
			}
		}
		
		if (AnimClipInfo[0].clip.name == "SwingSword") {
			Attacking = true;
			Ti += Time.deltaTime;
			if (!isClose && Ti >= 1) {
				--Score;
				Ti = 0;
			}
		} else {
			Attacking = false;
			Ti = 0;
		}
		
		if (AnimClipInfo[0].clip.name == "Block") {
			Blocking = true;
			Tm += Time.deltaTime;
			if (!isClose && Tm >= 1) {
				--Score;
				Tm = 0;
			}
		} else {
			Blocking = false;
			Tm = 0;
		}
		
		//Get Closest Enemy
		GetCloseAndOutput(Enemy.Ens, 0, 1, 2);
		
		//Get Closest Food
		GetCloseAndOutput(Enemy.Food, 3, 4, 5);
		
		//Walls
		RaycastHit hit;
		Physics.Raycast (transform.position, -transform.right, out hit, 5000);
		InputLayer[6].InputValue = hit.distance;
		Physics.Raycast (transform.position, transform.right, out hit, 5000);
		InputLayer[7].InputValue = hit.distance;
		Physics.Raycast (transform.position, transform.forward, out hit, 5000);
		InputLayer[8].InputValue = hit.distance;
		
		//Play Anim
		Anim.Play(AnimNames[(int)CalculateAnim()]);
		
		//Camera Follow This
		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			RaycastHit theObject;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out theObject, Mathf.Infinity, LM)) {
				if (theObject.collider.gameObject == gameObject) {
					Camera.main.GetComponent<God>().following = true;
					Camera.main.GetComponent<God>().objectToFollow = transform;
				}
			}
		}
		
		ScorePrev = Score;
	}
	
	//--------------------//
	
	void GetCloseAndOutput (GameObject[] G, int F1, int F2, int F3) {
		dist = 5000;
		
		//Get Closest Food
		for (int i = 0; i < G.Length; ++i) {
			
			float D = Vector3.Distance (transform.position, G[i].transform.position);
			
			if (D < dist) {
				dist = D;
				closestObject = G[i].transform;
			}
		}
		
		//Determine Input Values
		if (closestObject != null) {
			InputLayer[F1].InputValue = transform.InverseTransformPoint(closestObject.position).x;
			InputLayer[F2].InputValue = transform.InverseTransformPoint(closestObject.position).z;
			InputLayer[F3].InputValue = 600-dist;
		}
	}
	
	void OnCollisionEnter (Collision other) {
		if (other.collider.tag == "Food") {
			TimeFood = 0;
			Food += 1;
			++healthLeft;
			Score += 1;
			Destroy (other.collider.gameObject);
		}
		if (other.collider.tag == "Meat") {
			TimeFood = 0;
			Food += 5;
			healthLeft += 5;
			Score += 1;
			Destroy (other.collider.gameObject);
		}
		if (other.collider.tag == "Toxic") {
			Score -= 10;
			GenerateSimilar(this, this);
			Destroy (other.collider.gameObject);
		}
		if (other.collider.tag == "Lightning") {
			Destroy (this.gameObject);
		}
	}
	
	public void BeingAttacked (float N) {
		InputLayer[9].InputValue = N;
	}
	
	void OnDestroy() {
		Instantiate(Death, transform.position, Quaternion.identity);
	}
	
	public void MakeAnimate (int Output) {
		float[] CurrentOutputs = {};
		float[] WantedOutputs = {};
		float[] WantedInputs = {};
		Array.Resize(ref CurrentOutputs, OutputLayer.Length);
		Array.Resize(ref WantedOutputs, OutputLayer.Length);
		Array.Resize(ref WantedInputs, HiddenLayer.Length);
		
		for (int i = 0; i < OutputLayer.Length; ++i) {
			CurrentOutputs[i] = OutputLayer[i].InputValue;
			if (i == Output) {
				WantedOutputs[i] = 1;
			} else {
				WantedOutputs[i] = 0;
			}
		}
		
		for (int i = 0; i < HiddenLayer.Length; ++i) {
			
			if (i == Output) {
				HiddenLayer[i].Bias += 0.05f;
			} else {
				HiddenLayer[i].Bias -= 0.05f;
			}
			
			for (int x = 0; x < HiddenLayer[i].Weights.Length; ++x) {
				
				HiddenLayer[i].Weights[x] += (WantedOutputs[x]-CurrentOutputs[x])/10;
				
				HiddenLayer[i].Weights[x] = Mathf.Clamp(HiddenLayer[i].Weights[x], -1.0f, 1.0f);
				HiddenLayer[i].Bias = Mathf.Clamp(HiddenLayer[i].Bias, 0.0f, 1.0f);
				
				if (HiddenLayer[x].InputValue > 50) {
					if (InputLayer[i].InputValue >= 0) {
						WantedInputs[i] = 1;
					} else {
						WantedInputs[i] = -1;
					}
				} else {
					WantedInputs[i] = 0;
				}
			}
		}
		
		for (int i = 0; i < InputLayer.Length; ++i) {
			for (int x = 0; x < InputLayer[i].Weights.Length; ++x) {
				
				InputLayer[i].Weights[x] += (WantedInputs[x]-HiddenLayer[x].InputValue)/10;
				
				InputLayer[i].Weights[x] = Mathf.Clamp(InputLayer[i].Weights[x], -1.0f, 1.0f);
				InputLayer[i].Bias = Mathf.Clamp(InputLayer[i].Bias, 0.0f, 1.0f);
			}
		}
	}
}
