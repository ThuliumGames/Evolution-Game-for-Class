using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Traits : MonoBehaviour {
	
	public string[] charNames1 = {"Tug", "Gru", "Bor", "Du", "Gor", "Dom", "Bru", "Cug", "Frub", "Tac", "Boc", "Gub", "Duc", "Ruc", "Brog", "Fu", "Dug", "Gur", "Dub", "Trud", "Kop"};
	public string[] charNames2 = {"tug", "dub", "gug", "du", "dig", "dom", "bom", "rag", "dor", "bug", "den", "tob", "rub", "gud", "cug", "turk", "dog", "tuc", "gur", "", ""};
	
	string ParentsNames = "";
	
	//Combined Total of 100
	public float Size;
	public float Speed;
	//Combined Total of 100
	public float Health;
	public float AttackPower;
	
	Animator Anim;
	
	Animator sideScreen;
	
	string[] names = {"Score", "Food", "Size", "Speed", "MaxHealth", "AttackPower", "Health"};
	Text[] stats;
	Text nameOfChar;
	Text nameOfCharPar;
	
	Text[] Nodes1;
	Text[] Nodes2;
	Text[] Nodes3;
	Text Output;
	
	PlayerBehavior Neu;
	bool CanUpdateLines = true;
	
	void Start () {
		
		Neu = GetComponent<PlayerBehavior>();
		
		name = charNames1[Random.Range(0, charNames1.Length)]+charNames2[Random.Range(0, charNames2.Length)];
		
		nameOfChar = GameObject.Find("CharName").GetComponent<Text>();
		nameOfCharPar = GameObject.Find("CharParName").GetComponent<Text>();
		
		sideScreen = GameObject.Find("WarriorDisplay").GetComponent<Animator>();
		
		System.Array.Resize (ref stats, 7);
		
		for (int i = 0; i < stats.Length; ++i) {
			stats[i] = GameObject.Find(names[i]).GetComponentInChildren<Text>();
		}
		
		Nodes1 = GameObject.Find("Inputs").GetComponentsInChildren<Text>();
		Nodes2 = GameObject.Find("Hidden1").GetComponentsInChildren<Text>();
		Nodes3 = GameObject.Find("Animations").GetComponentsInChildren<Text>();
		Output = GameObject.Find("Output").GetComponentInChildren<Text>();
	}
	
	void Update () {
		
		Anim = GetComponent<Animator>();
		Anim.SetFloat("MoveSpeed", Speed);
		transform.localScale = Vector3.one*(Size/20);
		
		if (Camera.main.GetComponent<God>().following) {
			if (sideScreen.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Hidden") {
				sideScreen.Play("ShowSide");
			}
			
			if (Camera.main.GetComponent<God>().objectToFollow == transform) {
				//Set all Text
				
				stats[0].text = Neu.Score.ToString();
				stats[1].text = Neu.Food.ToString();
				stats[2].text = Size.ToString();
				stats[3].text = Speed.ToString();
				stats[4].text = Health.ToString();
				stats[5].text = AttackPower.ToString();
				stats[6].text = Neu.healthLeft.ToString();
				
				nameOfChar.text = name;
				nameOfCharPar.text = "Son of:\n" + ParentsNames;
				
				GameObject.Find("MoodImage").GetComponent<Image>().sprite = GetComponentInChildren<Mood>().HisMood.sprite;
				
				if (CanUpdateLines) {
					foreach (GameObject Go in GameObject.FindGameObjectsWithTag("Lines")) {
						Destroy (Go);
					}
				}
				
				for (int i = 0; i < Nodes1.Length; ++i) {
					Nodes1[i].GetComponentInChildren<Text>().text = ""+(int)(Neu.InputLayer[i].InputValue);
					for (int x = 0; x < Nodes2.Length; ++x) {
						if (CanUpdateLines) {
							GameObject Temp1 = new GameObject("Line"+x, typeof(RectTransform));
							Temp1.tag = "Lines";
							Temp1.transform.parent = Nodes1[i].transform;
							Temp1.transform.position = Nodes1[i].transform.position;
							Temp1.transform.LookAt(Nodes2[x].transform.position);
							Vector3 Rot = Temp1.transform.eulerAngles;
							Temp1.transform.eulerAngles = new Vector3 (0, 0, -Rot.x);
							Temp1.GetComponent<RectTransform>().pivot = new Vector2 (0, 0);
							Temp1.GetComponent<RectTransform>().sizeDelta = new Vector2 (Vector3.Distance(Nodes1[i].transform.position, Nodes2[x].transform.position), 2);
							Temp1.AddComponent<Image>();
							if (Neu.InputLayer[i].Weights[x] >= 0) {
								Temp1.GetComponent<Image>().color = new Color(0, 1, 0, Mathf.Clamp01(Neu.InputLayer[i].Weights[x]/2));
							} else {
								Temp1.GetComponent<Image>().color = new Color(1, 0, 0, Mathf.Clamp01(-Neu.InputLayer[i].Weights[x]/2));	
							}
							Temp1.transform.parent = GameObject.Find("BG").transform;
						}
					}
				}
					
				for (int i = 0; i < Nodes2.Length; ++i) {
					Nodes2[i].GetComponentInChildren<Text>().text = ""+(int)(Neu.HiddenLayer[i].InputValue*100);
					for (int x = 0; x < Nodes3.Length; ++x) {
						if (CanUpdateLines) {
							GameObject Temp2 = new GameObject("Line"+x, typeof(RectTransform));
							Temp2.tag = "Lines";
							Temp2.transform.parent = Nodes2[i].transform;
							Temp2.transform.position = Nodes2[i].transform.position;
							Temp2.transform.LookAt(Nodes3[x].transform.position);
							Vector3 Rot = Temp2.transform.eulerAngles;
							Temp2.transform.eulerAngles = new Vector3 (0, 0, -Rot.x);
							Temp2.GetComponent<RectTransform>().pivot = new Vector2 (0, 0);
							Temp2.GetComponent<RectTransform>().sizeDelta = new Vector2 (Vector3.Distance(Nodes2[i].transform.position, Nodes3[x].transform.position), 2);
							Temp2.AddComponent<Image>();
							if (Neu.HiddenLayer[i].Weights[x] >= 0) {
								Temp2.GetComponent<Image>().color = new Color(0, 1, 0, Mathf.Clamp01(Neu.HiddenLayer[i].Weights[x]/2));
							} else {
								Temp2.GetComponent<Image>().color = new Color(1, 0, 0, Mathf.Clamp01(-Neu.HiddenLayer[i].Weights[x]/2));	
							}
							Temp2.transform.parent = GameObject.Find("BG").transform;
						}
					}
				}
				
				for (int i = 0; i < Nodes3.Length; ++i) {
					Nodes3[i].GetComponentInChildren<Text>().text = ""+(int)(Neu.OutputLayer[i].InputValue*100);
					if (CanUpdateLines) {
						GameObject Temp3 = new GameObject("Line"+i, typeof(RectTransform));
						Temp3.tag = "Lines";
						Temp3.transform.parent = Nodes3[i].transform;
						Temp3.transform.position = Nodes3[i].transform.position;
						Temp3.transform.LookAt(Output.transform.position);
						Vector3 Rot = Temp3.transform.eulerAngles;
						Temp3.transform.eulerAngles = new Vector3 (0, 0, -Rot.x);
						Temp3.GetComponent<RectTransform>().pivot = new Vector2 (0, 0);
						Temp3.GetComponent<RectTransform>().sizeDelta = new Vector2 (Vector3.Distance(Nodes3[i].transform.position, Output.transform.position), 2);
						Temp3.AddComponent<Image>();
						Temp3.transform.parent = GameObject.Find("BG").transform;
					}
				}
				
				if (Time.frameCount%60 == 0) {
					CanUpdateLines = true;
				} else {
					CanUpdateLines = false;
				}
				
				Output.text = Neu.AnimClipInfo[0].clip.name;
				
			} else {
				CanUpdateLines = true;
			}
			
		} else {
			if (sideScreen.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Shown") {
				sideScreen.Play("HideSide");
			}
			CanUpdateLines = true;
		}
	}
	
	public void CreateNew () {
		//Randomize for 1st Generation
		
		ParentsNames = "You";
		
		Speed = Random.Range(0, 101);
		Size = (100-Speed)+Random.Range (-10, 11);
		Speed += Random.Range (-10, 11);
		
		Health = Random.Range(0, 101);
		AttackPower = (100-Health)+Random.Range (-10, 11);
		Health += Random.Range (-10, 11);
		
		Size = Mathf.Clamp (Size, 10, 50);
		Speed = Mathf.Clamp (Speed, 10, 50);
		Health = Mathf.Clamp (Health, 10, 50);
		AttackPower = Mathf.Clamp (AttackPower, 10, 50);
	}
	
	public void CreateSimilar (Traits T1, Traits T2) {
		//Randomize for allOther Generations
		
		ParentsNames = T1.name + " And " + T2.name;

		int T1Or2 = Random.Range (0, 2);
		
		float R1 = (T1.Neu.Score/10);
		if (Mathf.Abs(R1) < 1) {
			if (R1 >= 0) {
				R1 = 1;
			} else {
				R1 = -1;
			}
		}
		float R2 = (T2.Neu.Score/10);
		if (Mathf.Abs(R2) < 1) {
			if (R2 >= 0) {
				R2 = 1;
			} else {
				R2 = -1;
			}
		}
		
		R1 = 10/R1;
		R2 = 10/R2;
		
		if (T1Or2 == 0) {
			Speed = T1.Speed + Random.Range ((int)-R1, (int)R1+1);
		} else {
			Speed = T2.Speed + Random.Range ((int)-R2, (int)R2+1);
		}
		
		T1Or2 = Random.Range (0, 2);
		
		if (T1Or2 == 0) {
			Size = T1.Size + Random.Range ((int)-R1, (int)R1+1);
		} else {                          
			Size = T2.Size + Random.Range ((int)-R2, (int)R2+1);
		}
		
		T1Or2 = Random.Range (0, 2);
		
		if (T1Or2 == 0) {
			Health = T1.Health + Random.Range ((int)-R1, (int)R1+1);
		} else {                              
			Health = T2.Health + Random.Range ((int)-R2, (int)R2+1);
		}
		
		T1Or2 = Random.Range (0, 2);
		
		if (T1Or2 == 0) {
			AttackPower = T1.AttackPower + Random.Range ((int)-R1, (int)R1+1);
		} else {                                        
			AttackPower = T2.AttackPower + Random.Range ((int)-R2, (int)R2+1);
		}
		
		Size = Mathf.Clamp (Size, 10, 50);
		Speed = Mathf.Clamp (Speed, 10, 50);
		Health = Mathf.Clamp (Health, 10, 50);
		AttackPower = Mathf.Clamp (AttackPower, 10, 50);
	}
}
