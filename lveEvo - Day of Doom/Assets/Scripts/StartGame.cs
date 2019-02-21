using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {
	
	
	void Strt () {
		Application.LoadLevel("SampleScene");
	}
	
	void Tot () {
		Application.LoadLevel("Totorial");
	}
	
	void BackToMain () {
		Application.LoadLevel("EVO Main Menu");
	}
	
	void Quit () {
		Application.Quit();
	}
	
	void Cred () {
		Application.LoadLevel("Credits");
	}
}
