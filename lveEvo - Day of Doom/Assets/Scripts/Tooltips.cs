using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltips : MonoBehaviour {

	public bool isWarriorDisplay;
	public bool isWarriorDisplayItem;
	public GameObject ClickPerson;
	
	public string Text;
	
	public bool inRange;
	
	void Update () {
		if (name == "ToolTipText") {
			GetComponent<RectTransform>().position = Input.mousePosition + new Vector3 (2, 2, 0);
			bool canReset = true;
			foreach (Tooltips T in GameObject.FindObjectsOfType<Tooltips>()) {
				if (T.inRange) {
					canReset = false;
				}
			}
			if (canReset) {
				GetComponent<Text>().text = "";
			}
		} else {
			if ((Input.mousePosition.x > transform.position.x-(GetComponent<RectTransform>().sizeDelta.x/2)) &&
				(Input.mousePosition.x < transform.position.x+(GetComponent<RectTransform>().sizeDelta.x/2)) &&
				(Input.mousePosition.y > transform.position.y-(GetComponent<RectTransform>().sizeDelta.y/2)) &&
				(Input.mousePosition.y < transform.position.y+(GetComponent<RectTransform>().sizeDelta.y/2))) {
					
					inRange = true;
					if (!isWarriorDisplay) {
						string S = "";
						foreach (char C in Text.ToCharArray()) {
							if (C == '>') {
								S += "\n";
							} else {
								S += C;
							}
						}
						GameObject.Find("ToolTipText").GetComponent<Text>().text = S;
					} else if (!isWarriorDisplayItem) {
						if (Input.GetKeyDown(KeyCode.Mouse0)) {
							Generate.showAllWarriors = !Generate.showAllWarriors;
						}
					} else {
						if (Input.GetKeyDown(KeyCode.Mouse0)) {
							Camera.main.GetComponent<God>().objectToFollow = ClickPerson.transform;
							Camera.main.GetComponent<God>().following = true;
							Generate.showAllWarriors = false;
						}
					}
				} else {
					inRange = false;
				}
		}
	}
}
