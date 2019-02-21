using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour {
	
	public PlayerBehavior NN;
	public float Range;
	public Animator Anim;
	bool WasInRange;
	bool BackToStart = false;
	bool doNew;
	
	void Start () {
		GameObject.Find("WhatDo").SetActive(false);
	}
	
	void Update () {
		
		AnimatorClipInfo[] AnimClipInfo = Anim.GetCurrentAnimatorClipInfo(0);
		
		if (GameObject.FindObjectsOfType<PlayerBehavior>().Length == 1) {
		
			NN = GameObject.FindObjectOfType<PlayerBehavior>();
			
			if (Vector3.Distance (transform.position, NN.transform.position) < Range && !doNew) {
				if (AnimClipInfo[0].clip.name == "BossIdle" || AnimClipInfo[0].clip.name == "BossRun") {
					int R = Random.Range(1, 4);
					Anim.Play("BossAttack"+R);
					doNew = true;
				}
				
			} else {
				
				if (AnimClipInfo[0].clip.name == "BossIdle") {
					if (transform.position.y < 138) {
						Anim.Play("BossRun");
					}
				}
			}
			
			transform.LookAt (new Vector3 (NN.transform.position.x, transform.position.y, NN.transform.position.z));
			
			if (doNew && (AnimClipInfo[0].clip.name == "BossIdle" || AnimClipInfo[0].clip.name == "BossRun")) {
				transform.Rotate (0, 180, 0);
				if (Vector3.Distance (transform.position, NN.transform.position) > Range*4) {
					doNew = false;
				}
			}
		}
	}
}
