using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsView : View {

	public override void initiateLocations () {
		open = false;
		moving = false;
		initSpeed = 2000.0f;
		speed = initSpeed;
		inward = new GameObject ("StartScroll");
		outward = new GameObject ("EndScroll");
		inward.transform.position = this.gameObject.transform.position;
		outward.transform.position = new Vector3(this.gameObject.GetComponent<RectTransform>().sizeDelta.x/2-10, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
		this.gameObject.GetComponent<CanvasGroup> ().alpha = 0;
		this.gameObject.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		this.gameObject.GetComponent<CanvasGroup> ().interactable = false;
		inward.transform.parent = GameObject.Find ("UnitPicker").transform;
		outward.transform.parent = GameObject.Find ("UnitPicker").transform;

		Transform collection;

		for (int i = 2; i <= 3; i++) {
			foreach (Transform item in this.gameObject.transform.GetChild (0).GetChild (i).GetChild (0).GetChild (0)) {
					item.GetChild (2).GetComponent<CanvasGroup> ().alpha = 1;
					item.GetChild (3).GetComponent<CanvasGroup> ().alpha = 0;
			}
		}
		//this.gameObject.SetActive (false);
		//this.gameObject.transform.position.x + (float)this.gameObject.GetComponent<RectTransform>().rect.width
	}

	public void toggleUnit() {

		//this.gameObject.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		//this.gameObject.GetComponent<CanvasGroup> ().interactable = true;

		this.gameObject.transform.GetChild (0).transform.GetChild (2).GetComponent<CanvasGroup> ().alpha = 1;
		this.gameObject.transform.GetChild (0).transform.GetChild (2).GetComponent<CanvasGroup> ().interactable = true;
		this.gameObject.transform.GetChild (0).transform.GetChild (2).GetComponent<CanvasGroup> ().blocksRaycasts = true;

		this.gameObject.transform.GetChild (0).transform.GetChild (3).GetComponent<CanvasGroup> ().alpha = 0;
		this.gameObject.transform.GetChild (0).transform.GetChild (3).GetComponent<CanvasGroup> ().interactable = false;
		this.gameObject.transform.GetChild (0).transform.GetChild (3).GetComponent<CanvasGroup> ().blocksRaycasts = false;

		//print (GameObject.Find ("PlayerSetup").GetComponent<PlayerSetup> ().unitList);
		foreach (GameObject unitType in GameObject.Find("PlayerSetup").GetComponent<Human>().unitList) {
			print (unitType.GetComponent<UnitBase>().unitName);

			//Need to make it so that each unit template is presented in the unit List view as a separate option
		}

		toggle ();
	}

	public void toggleEquipment() {

		//this.gameObject.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		//this.gameObject.GetComponent<CanvasGroup> ().interactable = true;

		this.gameObject.transform.GetChild (0).transform.GetChild (2).GetComponent<CanvasGroup> ().alpha = 0;
		this.gameObject.transform.GetChild (0).transform.GetChild (2).GetComponent<CanvasGroup> ().interactable = false;
		this.gameObject.transform.GetChild (0).transform.GetChild (2).GetComponent<CanvasGroup> ().blocksRaycasts = false;

		this.gameObject.transform.GetChild (0).transform.GetChild (3).GetComponent<CanvasGroup> ().alpha = 1;
		this.gameObject.transform.GetChild (0).transform.GetChild (3).GetComponent<CanvasGroup> ().interactable = true;
		this.gameObject.transform.GetChild (0).transform.GetChild (3).GetComponent<CanvasGroup> ().blocksRaycasts = true;

		toggle ();
	}

	public void infoToggle() {

		Transform collection;

		if (this.gameObject.transform.GetChild (0).transform.GetChild (2).GetComponent<CanvasGroup> ().alpha == 1) {
			collection = this.gameObject.transform.GetChild (0).GetChild (2).GetChild (0).GetChild (0);
		} else {
			collection = this.gameObject.transform.GetChild (0).GetChild (3).GetChild (0).GetChild (0);
		}

		foreach (Transform item in collection) {
			if (item.GetChild (2).GetComponent<CanvasGroup> ().alpha == 1) {
				item.GetChild (2).GetComponent<CanvasGroup> ().alpha = 0;
				item.GetChild (3).GetComponent<CanvasGroup> ().alpha = 1;
			} else {
				item.GetChild (2).GetComponent<CanvasGroup> ().alpha = 1;
				item.GetChild (3).GetComponent<CanvasGroup> ().alpha = 0;
			}
		}
	}
}
