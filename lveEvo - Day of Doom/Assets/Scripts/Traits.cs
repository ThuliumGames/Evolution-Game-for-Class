using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traits : MonoBehaviour {
	
	//Combined Total of 100
	public float Size;
	public float Speed;
	//Combined Total of 100
	public float Health;
	public float AttackPower;
	//-----
	public bool Male = false;
	
	//Front Ray , Left Ray , Right Ray , Back Ray , Getting Hurt , Hurting Something
	bool[] WaysToInteract = {false, false, false, false, false, false};
	//Between 1 and Amount of Actions
	int[] ActionToExecute = {0, 0, 0, 0, 0, 0};
	
	Animator Anim;
	
	void Start () {
		//Randomize for 1st Generation
		int B = Random.Range(0, 2);
		
		if (B == 1) {
			Male = true;
		}
		
		Speed = Random.Range(0, 100);
		Size = (100-Speed)+Random.Range (-20, 21);
		Speed += Random.Range (-20, 21);
		
		Health = Random.Range(0, 100);
		AttackPower = (100-Health)+Random.Range (-20, 21);
		Health += Random.Range (-20, 21);
		
		Size = Mathf.Clamp (Size, 1, 100);
		Speed = Mathf.Clamp (Speed, 1, 100);
		Health = Mathf.Clamp (Health, 1, 100);
		AttackPower = Mathf.Clamp (AttackPower, 1, 100);
		
		Anim = GetComponent<Animator>();
		Anim.SetFloat("MoveSpeed", Speed);
		transform.localScale = Vector3.one*(Size/20);
	}
	
}
