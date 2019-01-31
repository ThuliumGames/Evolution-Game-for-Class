using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {
	
	
	void Strt () {
		Application.LoadLevel("SampleScene");
	}
	
	void BackToMain () {
		Application.LoadLevel("EVO Main Menu");
	}
	
	void Quit () {
		Application.Quit();
	}
}
