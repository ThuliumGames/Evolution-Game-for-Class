using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class God : MonoBehaviour {
	
	float AngX = 35;
	float AngY;
	
	public float speed;
	public float angSpeed;
	
	public GameObject FoodObject;
	public GameObject ToxicObject;
	public GameObject BaitObject;
	public GameObject Lightning;
	public LayerMask LM;
	
	public bool following;
	public Transform objectToFollow;
	float height;
	float ThrowPower = 0;
	float ThrowPower2 = 0;
	float ThrowPower3 = 0;
	int FoodLeft = 100;
	public Text FoodText;
	
	void Update () {
		
		//Cam Control
		if (!following) {
			
			objectToFollow = null;
			
			if (Input.GetKey(KeyCode.Mouse2)) {
				transform.Translate (-Input.GetAxis("Mouse X")*speed*Time.deltaTime, -Input.GetAxis("Mouse Y")*speed*Time.deltaTime,  0);
			} else if (Input.GetKey(KeyCode.Mouse1)) {
				AngX += -Input.GetAxis("Mouse Y")*angSpeed*Time.deltaTime;
				AngY += Input.GetAxis("Mouse X")*angSpeed*Time.deltaTime;
			}
			Vector3 Temp = transform.eulerAngles;
			transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
				transform.Translate(Input.GetAxis("Horizontal")*speed*3*Time.deltaTime, 0,  Input.GetAxis("Vertical")*speed*3*Time.deltaTime);
			} else {
				transform.Translate(Input.GetAxis("Horizontal")*speed*Time.deltaTime, 0,  Input.GetAxis("Vertical")*speed*Time.deltaTime);
			}
			transform.eulerAngles = Temp;
			transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel")*speed*20*Time.deltaTime);
		} else {
			
			//followWarrior
			
			if (objectToFollow != null) {
				height += -Input.GetAxis("Mouse ScrollWheel")*speed*10*Time.deltaTime;
				height = Mathf.Clamp(height, 15, 50);
				transform.position = Vector3.Lerp(transform.position, objectToFollow.position + (Vector3.up*(objectToFollow.GetComponent<Traits>().Size/20)*height), 10*Time.deltaTime);
				AngX = Mathf.Lerp(AngX, 90, 10*Time.deltaTime);
				
				if (Input.GetKey(KeyCode.Mouse2) || Input.GetKey(KeyCode.Mouse1) || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
					if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
						following = false;
					}
				}
			} else {
				following = false;
			}
		}
		
		AngX = Mathf.Clamp (AngX, -90, 90);
		
		transform.eulerAngles = new Vector3 (AngX, AngY, 0);
		
		float MaxX = 290*Mathf.Cos(Mathf.Asin(transform.position.z/290));
		float MaxZ = 290*Mathf.Cos(Mathf.Asin(transform.position.x/290));
		
		transform.position = new Vector3 (Mathf.Clamp(transform.position.x, -MaxX, MaxX), Mathf.Clamp(transform.position.y, 0.1f, 200), Mathf.Clamp(transform.position.z, -MaxZ, MaxZ));
		
		//Food
		if (GameObject.FindObjectOfType<PlayerBehavior>() != null) {
			if (Input.GetKey(KeyCode.F)) {
				ThrowPower += Time.deltaTime*50;
			}
			if (Input.GetKeyUp(KeyCode.F)) {
				
				ThrowPower = Mathf.Clamp (ThrowPower, 1, 750);
				if (FoodLeft > 0) {
					if (GameObject.FindObjectOfType<BossAI>() != null) {
						--FoodLeft;
					}
					
					GameObject G = Instantiate (FoodObject, transform.position + ((transform.right*((Input.mousePosition.x-(Screen.width/2))/Screen.width))*((2.0f/9)*16)) + ((transform.up*((Input.mousePosition.y-(Screen.height/2))/Screen.height))*2) + (transform.forward*2), transform.rotation);
					
					if (objectToFollow != null) {
						G.GetComponent<Rigidbody>().isKinematic = true;
						G.AddComponent<FoodAutoEat>();
						G.GetComponent<FoodAutoEat>().goTo = objectToFollow;
					} else {
						G.transform.LookAt (transform.position);
						G.transform.Rotate (0, 180, 0);
						G.GetComponent<Rigidbody>().AddRelativeForce (0, 0, 750*ThrowPower);
					}
				}
				ThrowPower = 0;
			}
			if (Input.GetKey(KeyCode.T)) {
				ThrowPower3 += Time.deltaTime*50;
			}
			if (Input.GetKeyUp(KeyCode.T) && GameObject.FindObjectOfType<BossAI>() == null) {
				
				ThrowPower3 = Mathf.Clamp (ThrowPower3, 1, 750);
					
				GameObject G = Instantiate (ToxicObject, transform.position + ((transform.right*((Input.mousePosition.x-(Screen.width/2))/Screen.width))*((7.0f/9)*16)) + ((transform.up*((Input.mousePosition.y-(Screen.height/2))/Screen.height))*7) + (transform.forward*7), transform.rotation);
				
				if (objectToFollow != null) {
					G.GetComponent<Rigidbody>().isKinematic = true;
					G.AddComponent<FoodAutoEat>();
					G.GetComponent<FoodAutoEat>().goTo = objectToFollow;
				} else {
					G.transform.LookAt (transform.position);
					G.transform.Rotate (0, 180, 0);
					G.GetComponent<Rigidbody>().AddRelativeForce (0, 0, 750*ThrowPower3*10);
				}
				ThrowPower3 = 0;
			}
			
			if (Input.GetKey(KeyCode.B)) {
				ThrowPower2 += Time.deltaTime*50;
			}
			if (Input.GetKeyUp(KeyCode.B) && GameObject.FindObjectOfType<BossAI>() == null) {
				
				if (GameObject.FindGameObjectWithTag("Bait") != null) {
					Destroy(GameObject.FindGameObjectWithTag("Bait"));
				}
				
				ThrowPower2 = Mathf.Clamp (ThrowPower2, 1, 750);
					
				GameObject G = Instantiate (BaitObject, transform.position + ((transform.right*((Input.mousePosition.x-(Screen.width/2))/Screen.width))*((7.0f/9)*16)) + ((transform.up*((Input.mousePosition.y-(Screen.height/2))/Screen.height))*7) + (transform.forward*7), transform.rotation);

				G.transform.LookAt (transform.position);
				G.transform.Rotate (0, 180, 0);
				G.GetComponent<Rigidbody>().AddRelativeForce (0, 0, 750*ThrowPower2*10);
				
				ThrowPower2 = 0;
			}
		}
		
		if (GameObject.FindObjectOfType<BossAI>() != null) {
			if (GameObject.FindGameObjectWithTag("Bait") != null) {
				Destroy(GameObject.FindGameObjectWithTag("Bait"));
			}
			FoodText.color = Color.white;
			FoodText.text = "\n\nFood Left\n<Size=50>"+FoodLeft+"</Size>";
		} else {
		
			if (GameObject.FindObjectOfType<PlayerBehavior>() != null) {
				if (Input.GetKeyDown(KeyCode.L)) {
					
					RaycastHit theObject;
					
					if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out theObject, Mathf.Infinity, LM)) {
						Instantiate (Lightning, new Vector3 (theObject.point.x, 0, theObject.point.z), Quaternion.identity);
					}
				}
			}
		}
	}
	
	void MakeAnimate0 () {
		if (following) {
			objectToFollow.GetComponent<PlayerBehavior>().MakeAnimate(0);
		}
	}
	void MakeAnimate1 () {
		if (following) {
			objectToFollow.GetComponent<PlayerBehavior>().MakeAnimate(1);
		}
	}
	void MakeAnimate2 () {
		if (following) {
			objectToFollow.GetComponent<PlayerBehavior>().MakeAnimate(2);
		}
	}
	void MakeAnimate3 () {
		if (following) {
			objectToFollow.GetComponent<PlayerBehavior>().MakeAnimate(3);
		}
	}
	void MakeAnimate4 () {
		if (following) {
			objectToFollow.GetComponent<PlayerBehavior>().MakeAnimate(4);
		}
	}
	void MakeAnimate5 () {
		if (following) {
			objectToFollow.GetComponent<PlayerBehavior>().MakeAnimate(5);
		}
	}
}
