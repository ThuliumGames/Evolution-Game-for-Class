using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generate : MonoBehaviour {
	
	public GameObject WarriorPrefab;
	public GameObject GodRayPrefab;
	
	public GameObject EnemyPrefab;
	
	public GameObject[] DestOnGen;
	
	public Animator GateOpen;
	
	public Image ControlsImage;
	public Sprite[] ControlsSprite;
	public Image ControlsForCamera;
	public static bool showAllWarriors;
	public Text totNum;
	public GameObject ParentForWar;
	public GameObject ScrollBar;
	public GameObject WarDisps;
	public GameObject[] allWarDisps;
	
	PlayerBehavior[] GenePool;
	
	float[] scores;
	
	bool ReadyToFight = false;
	
	public Vector4[] RangesX;
	
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.LoadLevel("Evo Main Menu");
		}
		
		if (GameObject.FindObjectOfType<BossAI>() != null) {
			if (!ReadyToFight) {
				PlayerBehavior Keep = null;
				float MaxScore = -100000;
				foreach (PlayerBehavior NN in GameObject.FindObjectsOfType<PlayerBehavior>()) {
					if (NN.Score > MaxScore) {
						Keep = NN;
						MaxScore = NN.Score;
					}
				}
				foreach (PlayerBehavior NN in GameObject.FindObjectsOfType<PlayerBehavior>()) {
					if (NN != Keep) {
						Destroy(NN.gameObject);
					}
				}
				ReadyToFight = true;
				totNum.text = Keep.name;
				
				ScrollBar.SetActive(false);
				for (int i = 0; i < allWarDisps.Length; ++i) {
					if (allWarDisps[i] != null) {
						Destroy(allWarDisps[i]);
					}
				}
				Array.Resize(ref allWarDisps, 0);
			}
			
			ControlsImage.sprite = ControlsSprite[3];
			
		} else {
		
			int CanRepro = 0;
			bool areAny = false;
			
			foreach (PlayerBehavior NN in GameObject.FindObjectsOfType<PlayerBehavior>()) {
				areAny = true;
				++CanRepro;
			}
			
			totNum.text = CanRepro + " / 200";
			
			if (Camera.main.GetComponent<God>().following) {
				showAllWarriors = false;
			}
			
			if (allWarDisps.Length <= 6) {
				ScrollBar.GetComponent<Scrollbar>().value = 0;
			}
			ParentForWar.transform.localPosition = new Vector3 (0, 250 + ((allWarDisps.Length-6)*65*ScrollBar.GetComponent<Scrollbar>().value), 0);
			
			if (showAllWarriors && allWarDisps.Length != CanRepro) {
				ScrollBar.SetActive(true);
				if (allWarDisps.Length != CanRepro) {
					for (int i = 0; i < allWarDisps.Length; ++i) {
						if (allWarDisps[i] != null) {
							Destroy(allWarDisps[i]);
						}
					}
				}
				Array.Resize(ref allWarDisps, CanRepro);
				PlayerBehavior[] N = GameObject.FindObjectsOfType<PlayerBehavior>();
				
				//joshpsawyer: https://answers.unity.com/questions/695863/sort-an-array-of-classes-by-a-variable-value.html
				Array.Sort(N, delegate(PlayerBehavior a, PlayerBehavior b) { return b.Score.CompareTo(a.Score); });
				//
				
				for (int i = 0; i < CanRepro; ++i) {
					allWarDisps[i] = Instantiate (WarDisps, Vector3.zero, Quaternion.Euler(Vector3.zero));
					allWarDisps[i].transform.parent = ParentForWar.transform;
					allWarDisps[i].transform.localPosition = new Vector3 (0, (-65*(i))-85, 0);
					allWarDisps[i].GetComponentInChildren<Text>().text = N[i].name + "\nScore: " + N[i].Score;
					allWarDisps[i].GetComponent<Tooltips>().ClickPerson = N[i].gameObject;
				}
			} else if (!showAllWarriors) {
				ScrollBar.SetActive(false);
				for (int i = 0; i < allWarDisps.Length; ++i) {
					if (allWarDisps[i] != null) {
						Destroy(allWarDisps[i]);
					}
				}
				Array.Resize(ref allWarDisps, 0);
			}
			
			if (!areAny) {
				ControlsImage.sprite = ControlsSprite[0];
			} else {
				if (CanRepro == 200) {
					ControlsImage.sprite = ControlsSprite[2];
				} else {
					ControlsImage.sprite = ControlsSprite[1];
				}
			}
			
			if (Input.GetKeyDown(KeyCode.E)) {
				
				if (GameObject.FindObjectOfType<PlayerBehavior>() != null) {
				
					//Spawn Enemys
					Array.Resize (ref DestOnGen, 50);
					for (int i = 0; i < 50; ++i) {
						if (DestOnGen[i] == null) {
							int R = UnityEngine.Random.Range(0, RangesX.Length);
							
							DestOnGen[i] = Instantiate (EnemyPrefab, new Vector3 (UnityEngine.Random.Range(RangesX[R].x, RangesX[R].y), 0, UnityEngine.Random.Range(RangesX[R].z, RangesX[R].w)), Quaternion.Euler (0, UnityEngine.Random.Range(0, 360), 0));
							float S = UnityEngine.Random.Range(1.0f, 2.0f);
							DestOnGen[i].transform.localScale = Vector3.one*S;
						}
					}
					
					GateOpen.Play("OpenGate");
				}
			}
			
			bool CantClose = false;
			
			for (int i = 0; i < DestOnGen.Length; ++i) {
				if (DestOnGen[i] != null) {
					if (DestOnGen[i].transform.position.magnitude > 300) {
						CantClose = true;
					}
				}
			}
			
			if (!CantClose && GateOpen.GetCurrentAnimatorClipInfo(0)[0].clip.name == "GateOpened") {
				GateOpen.Play("CloseGate");
			}
			
			
			if (Input.GetKeyDown(KeyCode.Space)) {
					
				//Spawn Warriors
				Array.Resize (ref GenePool, 0);
				
				if (GameObject.FindObjectsOfType<PlayerBehavior>().Length != 0 && CanRepro < 200 && (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))) {
					
					foreach (PlayerBehavior NN in GameObject.FindObjectsOfType<PlayerBehavior>()) {
						for (int i = 0; i < NN.Food+1; ++i) {
							Array.Resize (ref GenePool, GenePool.Length+1);
							GenePool[GenePool.Length-1] = NN;
						}
					}
					
					GameObject G = Instantiate (WarriorPrefab, new Vector3 (UnityEngine.Random.Range(-200, 200), 0, UnityEngine.Random.Range(-200, 200)), Quaternion.Euler (0, UnityEngine.Random.Range(0, 360), 0));
					Instantiate (GodRayPrefab, G.transform.position, Quaternion.Euler (-90, 0, 0));
					G.GetComponent<PlayerBehavior>().GenerateSimilar (GenePool[UnityEngine.Random.Range(0, GenePool.Length)], GenePool[UnityEngine.Random.Range(0, GenePool.Length)]);
					
				} else if ((GameObject.FindObjectsOfType<PlayerBehavior>().Length == 0) || (GameObject.FindObjectsOfType<PlayerBehavior>().Length != 0 && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))) {
					
					if (GameObject.FindObjectsOfType<PlayerBehavior>().Length != 0) {
						foreach (PlayerBehavior NN in GameObject.FindObjectsOfType<PlayerBehavior>()) {
							Destroy(NN.gameObject);
						}
					}
					
					for (int i = 0; i < 100; ++i) {
						ControlsForCamera.enabled = false;
						GameObject G = Instantiate (WarriorPrefab, new Vector3 (UnityEngine.Random.Range(-200, 200), 0, UnityEngine.Random.Range(-200, 200)), Quaternion.Euler (0, UnityEngine.Random.Range(0, 360), 0));
						G.GetComponent<PlayerBehavior>().OnCreation ();
					}
				}
			}
		}
	}
}
