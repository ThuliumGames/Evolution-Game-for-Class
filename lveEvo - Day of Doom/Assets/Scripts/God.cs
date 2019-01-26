using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God : MonoBehaviour {
	
	float AngX;
	float AngY;
	
	public float speed;
	public float angSpeed;
	
	public GameObject FoodObject;
	
	void Update () {
		
		//Cam Control
		if (Input.GetKey(KeyCode.Mouse2)) {
			transform.Translate (-Input.GetAxis("Mouse X")*speed*Time.deltaTime, -Input.GetAxis("Mouse Y")*speed*Time.deltaTime,  0);
		} else if (Input.GetKey(KeyCode.Mouse1)) {
			AngX += -Input.GetAxis("Mouse Y")*angSpeed*Time.deltaTime;
			AngY += Input.GetAxis("Mouse X")*angSpeed*Time.deltaTime;
		}
		transform.eulerAngles = new Vector3 (AngX, AngY, 0);
		transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel")*speed*10*Time.deltaTime);
		
		//Food
		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			GameObject G = Instantiate (FoodObject, transform.position + ((transform.right*((Input.mousePosition.x-(Screen.width/2))/Screen.width))*2) + ((transform.up*((Input.mousePosition.y-(Screen.height/2))/Screen.height))*2) + (transform.forward*2), transform.rotation);
			G.transform.LookAt (transform.position);
			G.transform.Rotate (0, 180, 0);
			G.GetComponent<Rigidbody>().AddRelativeForce (0, 0, 5000);
		}
	}
}
