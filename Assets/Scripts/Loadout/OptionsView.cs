using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsView : View {

	public List<GameObject> choiceUIObject;

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

		this.gameObject.transform.FindChild ("Panel").transform.FindChild ("UnitOptions").GetComponent<CanvasGroup> ().alpha = 1;
		this.gameObject.transform.FindChild ("Panel").transform.FindChild ("UnitOptions").GetComponent<CanvasGroup> ().interactable = true;
		this.gameObject.transform.FindChild ("Panel").transform.FindChild ("UnitOptions").GetComponent<CanvasGroup> ().blocksRaycasts = true;

		this.gameObject.transform.FindChild ("Panel").transform.FindChild ("EquipmentOptions").GetComponent<CanvasGroup> ().alpha = 0;
		this.gameObject.transform.FindChild ("Panel").transform.FindChild ("EquipmentOptions").GetComponent<CanvasGroup> ().interactable = false;
		this.gameObject.transform.FindChild ("Panel").transform.FindChild ("EquipmentOptions").GetComponent<CanvasGroup> ().blocksRaycasts = false;

		print (this.gameObject.transform.FindChild("Panel").FindChild ("UnitOptions").FindChild ("Viewport").FindChild ("Content").childCount);
		if (this.gameObject.transform.FindChild ("Panel").FindChild("UnitOptions").FindChild ("Viewport").FindChild ("Content").childCount == 0) {
			choiceUIObject = new List<GameObject> ();
			//print (GameObject.Find ("PlayerSetup").GetComponent<PlayerSetup> ().unitList);
			int iterator = 0;

			foreach (GameObject unitTemplate in GameObject.Find("PlayerSetup").GetComponent<Human>().unitList) {

				GameObject unitItem = Instantiate (Resources.Load ("UIPrefabs/UnitItem", typeof(GameObject)) as GameObject);

				unitItem.transform.FindChild ("Portrait").gameObject.GetComponent<Image> ().sprite = unitTemplate.GetComponent<UnitBase> ().portrait;
				unitItem.transform.FindChild ("Name").gameObject.GetComponent<Text> ().text = unitTemplate.GetComponent<UnitBase> ().unitName;
				unitItem.transform.FindChild ("Description").FindChild ("Text").gameObject.GetComponent<Text> ().text = unitTemplate.GetComponent<UnitBase> ().description;

				for (int i = 0; i < 8; i++) {
					//unitItem.transform.FindChild("Stats").FindChild("Stat ("+i.ToString()+")").FindChild("StatIcon") == REQUIRE IMAGES FIRST;
					unitItem.transform.FindChild ("Stats").FindChild ("Stat (" + i.ToString () + ")").FindChild ("StatNum").gameObject.GetComponent<Text> ().text = unitTemplate.GetComponent<UnitBase> ().stat [i].ToString ();
				}

				unitItem.transform.SetParent (GameObject.Find ("ArmyPicker").transform.FindChild ("UnitPicker").FindChild ("Options").FindChild ("Panel").FindChild ("UnitOptions").FindChild ("Viewport").FindChild ("Content"));
				unitItem.transform.localScale = new Vector3 (1, 1, 1);
				unitItem.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 1);
				unitItem.GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 1);
				unitItem.GetComponent<RectTransform> ().pivot = new Vector2 (0, 0);
				unitItem.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, -(unitItem.GetComponent<RectTransform> ().sizeDelta.y * (iterator + 1)));
				unitItem.GetComponent<RectTransform> ().sizeDelta = new Vector2 (400, 90);

				unitItem.GetComponent<Button> ().onClick.AddListener (toggle);
				iterator++;
				//Need to make it so that each unit template is presented in the unit List view as a separate option
				//GameObject choice = Instantiate(
			}
			GameObject.Find ("ArmyPicker").transform.FindChild ("UnitPicker").FindChild ("Options").FindChild ("Panel").FindChild ("UnitOptions").FindChild ("Viewport").FindChild ("Content").gameObject.GetComponent<RectTransform> ().sizeDelta 
			= new Vector2 (GameObject.Find ("ArmyPicker").transform.FindChild ("UnitPicker").FindChild ("Options").FindChild ("Panel").FindChild ("UnitOptions").FindChild ("Viewport").FindChild ("Content").gameObject.GetComponent<RectTransform> ().sizeDelta.x, 
				15 * iterator);
			infoToggle ();
			infoToggle ();
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
			if (item.FindChild ("Description").GetComponent<CanvasGroup> ().alpha == 1) {
				item.FindChild ("Description").GetComponent<CanvasGroup> ().alpha = 0;
				item.FindChild ("Stats").GetComponent<CanvasGroup> ().alpha = 1;
			} else {
				item.FindChild ("Description").GetComponent<CanvasGroup> ().alpha = 1;
				item.FindChild ("Stats").GetComponent<CanvasGroup> ().alpha = 0;
			}
		}
	}
}
