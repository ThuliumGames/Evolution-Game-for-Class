using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	
	public bool DontKill;
	
	public Animator Anim;
	public GameObject Death;
	
	public float Health = 50;
	bool canGetHit = true;
	
	float range = 10;
	
	public static PlayerBehavior[] NN;
	public static GameObject[] Ens;
	public static GameObject[] Food;
	
	float T = 0;
	float CD = 0;
	public int C = 0;
	
	bool isClose = false;
	
	void Update () {
		
		if (name == "StayForever") {
			NN = GameObject.FindObjectsOfType<PlayerBehavior>();
			Ens = GameObject.FindGameObjectsWithTag("Enemy");
			Food = GameObject.FindGameObjectsWithTag("Food");
		} else if (NN != null) {
			
			float dist = 100000;
			isClose = false;
			
			bool isBait = false;
			GameObject B = null;
			if (GameObject.FindGameObjectWithTag("Bait") == null) {
				for (int i = 0; i < NN.Length; ++i) {
					
					float thisDist = Vector3.Distance(transform.position, NN[i].transform.position);
					
					if (thisDist <= dist) {
						dist = thisDist;
						C = i;
					}
					
					if (thisDist <= range) {
						isClose = true;
					}
				}
			} else {
				B = GameObject.FindGameObjectWithTag("Bait");
				isBait = true;
				float thisDist = Vector3.Distance(transform.position, B.transform.position);
				
				if (thisDist <= range) {
					isClose = true;
				}
			}
			
			if (!isClose) {
				Anim.SetBool("Move", true);
				Anim.SetBool("Swing", false);
			} else {
				
				Anim.SetBool("Move", false);
				
				T += Time.deltaTime;
				
				if (T > 2) {
						
					Anim.SetBool("Swing", true);
					
					if (T > 5) {
						
						if (!isBait) {
							if (NN[C].Blocking) {
								NN[C].Score += 1;
							} else {
								NN[C].Score -= 10;
								--NN[C].healthLeft;
							}
						} else {
							Destroy(B);
						}
						T = 0;
						Anim.SetBool("Swing", false);
					}
					
					NN[C].BeingAttacked(T);
				}
					
				if (!isBait) {
					
					if (NN[C].Attacking && canGetHit) {
							CD = 0;
							NN[C].TimeAttack = 0;
							NN[C].Score += 5;
							Health -= NN[C].GetComponent<Traits>().AttackPower;
							canGetHit = false;
							if (Health <= 0) {
								NN[C].Score += 15;
								Destroy(this.gameObject);
							}
					} else {
						if (!NN[C].Attacking) {
							canGetHit = true;
						} else {
							CD += Time.deltaTime;
							if (CD > 1.5f) {
								canGetHit = true;
							}
						}
					}
					
				}
			}
			
			if (!isBait) {
				transform.LookAt(new Vector3 (NN[C].transform.position.x, transform.position.y, NN[C].transform.position.z));
			} else {
				transform.LookAt(new Vector3 (B.transform.position.x, transform.position.y, B.transform.position.z));
			}
			
			if (GameObject.FindObjectOfType<BossAI>() != null) {
				if (!DontKill) {
					Destroy(this.gameObject);
				}
			}
			
		}
	}
	
	void OnCollisionEnter (Collision other) {
		if (other.collider.tag == "Lightning") {
			Destroy (this.gameObject);
		}
	}
	
	void OnDestroy() {
		Instantiate(Death, transform.position+Vector3.up, Quaternion.identity);
	}
}
