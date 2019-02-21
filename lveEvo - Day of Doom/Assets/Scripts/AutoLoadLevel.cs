using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLoadLevel : MonoBehaviour {
	
	public float ST = 9;
	
	public string LevelName;
	
	float t = 0;
	
	void Update () {
		t += Time.deltaTime;
		if (t >= ST) {
			Application.LoadLevel(LevelName);
		}
	}
}
