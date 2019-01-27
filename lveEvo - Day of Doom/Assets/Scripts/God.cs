using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God : MonoBehaviour {
	
	float AngX;
	float AngY;
	
	public float speed;
	public float angSpeed;
	
	public GameObject FoodObject;
	
	public bool following;
	public Transform objectToFollow;
	float height;
	
	void Update () {
		
		//Cam Control
		if (!following) {
			if (Input.GetKey(KeyCode.Mouse2)) {
				transform.Translate (-Input.GetAxis("Mouse X")*speed*Time.deltaTime, -Input.GetAxis("Mouse Y")*speed*Time.deltaTime,  0);
			} else if (Input.GetKey(KeyCode.Mouse1)) {
				AngX += -Input.GetAxis("Mouse Y")*angSpeed*Time.deltaTime;
				AngY += Input.GetAxis("Mouse X")*angSpeed*Time.deltaTime;
			}
			transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel")*speed*10*Time.deltaTime);
		} else {
			
			//followWarrior
			
			if (objectToFollow != null) {
				height += -Input.GetAxis("Mouse ScrollWheel")*speed*10*Time.deltaTime;
				height = Mathf.Clamp(height, 15, 50);
				transform.position = Vector3.Lerp(transform.position, objectToFollow.position + (Vector3.up*(objectToFollow.GetComponent<Traits>().Size/20)*height), 10*Time.deltaTime);
				AngX = Mathf.Lerp(AngX, 90, 10*Time.deltaTime);
				
				if (Input.GetKey(KeyCode.Mouse2) || Input.GetKey(KeyCode.Mouse1)) {
					if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0) {
						following = false;
					}
				}
			} else {
				following = false;
			}
		}
		
		transform.eulerAngles = new Vector3 (AngX, AngY, 0);
		
		//Food
		if (Input.GetKeyDown(KeyCode.F)) {
			GameObject G = Instantiate (FoodObject, transform.position + ((transform.right*((Input.mousePosition.x-(Screen.width/2))/Screen.width))*2) + ((transform.up*((Input.mousePosition.y-(Screen.height/2))/Screen.height))*2) + (transform.forward*2), transform.rotation);
			G.transform.LookAt (transform.position);
			G.transform.Rotate (0, 180, 0);
			G.GetComponent<Rigidbody>().AddRelativeForce (0, 0, 5000);
		}
	}
}
