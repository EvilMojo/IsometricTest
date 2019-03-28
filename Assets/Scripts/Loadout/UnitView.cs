using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitView : View {

	public GameObject coreStart;
	public GameObject coreCentre;

	public int unitIndex;
	public GameObject unit;

	public void assignUnitDetails (GameObject unit) {
		print (unit.name + " " + unit.GetComponent<UnitBase>().unitName);
		print (this.unit.name + this.unit.transform.parent);
		this.unit.GetComponent<UnitBase> ().unitName = unit.GetComponent<UnitBase> ().unitName;
		this.unit.GetComponent<UnitBase> ().type = unit.GetComponent<UnitBase> ().type;
		this.unit.GetComponent<UnitBase> ().portrait = unit.GetComponent<UnitBase> ().portrait;
		this.unit.GetComponent<UnitBase> ().stat = unit.GetComponent<UnitBase> ().stat;
		this.unit.GetComponent<UnitBase> ().equipmentSlots = unit.GetComponent<UnitBase> ().equipmentSlots;
		this.unit.GetComponent<UnitBase> ().description = unit.GetComponent<UnitBase> ().description;
	}

	public void changeUnitUI() {
		print ("Assigning unit UI");
		this.gameObject.transform.FindChild ("Panel").FindChild ("Canvas").FindChild ("Portrait").gameObject.GetComponent<Image> ().sprite = this.unit.GetComponent<UnitBase> ().portrait;
		this.gameObject.transform.FindChild ("Panel").FindChild ("Canvas").FindChild ("Name").gameObject.GetComponent<Text> ().text = this.unit.GetComponent<UnitBase> ().unitName;

		for (int i = 0; i < this.unit.GetComponent<UnitBase> ().stat.Length; i++) {
			this.gameObject.transform.FindChild ("Panel").FindChild ("Canvas").FindChild ("UnitStats").FindChild ("Panel").FindChild ("Stat (" + i + ")").FindChild("StatNum").gameObject.GetComponent<Text>().text = unit.GetComponent<UnitBase>().stat[i].ToString();
		}

		for (int i = 0; i < 6; i++) {
			GameObject EquipmentSlot = this.gameObject.transform.FindChild ("Panel").FindChild ("Canvas").FindChild ("EquipmentSlots").FindChild ("EquipmentSlotsPanel").FindChild ("EquipmentSlot (" + i + ")").gameObject;
			if (i < this.unit.GetComponent<UnitBase> ().equipmentSlots.Length) {
				EquipmentSlot.GetComponent<CanvasGroup>().alpha = 1;
				EquipmentSlot.GetComponent<CanvasGroup> ().interactable = true;
				EquipmentSlot.GetComponent<CanvasGroup> ().blocksRaycasts = true;
				//EquipmentSlot.GetComponent<Image>().sprite = unit.GetComponent<UnitBase>().equipmentSlots[i].GetComponent<EquipmentBase>().portrait;
			} else {
				//Reset this image, it's not being used
				EquipmentSlot.GetComponent<CanvasGroup>().alpha = 0;
				EquipmentSlot.GetComponent<CanvasGroup> ().interactable = false;
				EquipmentSlot.GetComponent<CanvasGroup> ().blocksRaycasts = false;
			}
		}
	}

	public void dismiss() {
		outward.transform.position = this.gameObject.transform.position;

		inward.transform.position = new Vector3((Screen.width*2), this.gameObject.transform.position.y, this.gameObject.transform.position.z);
		open = true;
		//end = outward.transform;

		resizingx = true;
		resizingy = true;

		toggle ();
	}
	public void enlist() {
		
		inward.transform.position = this.gameObject.transform.position;
		start = inward.transform;

		outward.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y+this.gameObject.GetComponent<RectTransform>().sizeDelta.y/6, this.gameObject.transform.position.z);
		end = outward.transform;

		outward.GetComponent<RectTransform> ().sizeDelta = new Vector2 (100, 100);

		xrate = this.gameObject.GetComponent<RectTransform> ().sizeDelta.x / outward.GetComponent<RectTransform> ().sizeDelta.x;
		yrate = this.gameObject.GetComponent<RectTransform> ().sizeDelta.y / outward.GetComponent<RectTransform> ().sizeDelta.y;


		this.gameObject.transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<CanvasGroup> ().alpha = 0;

		//Debug.Log ("X: " + xrate + " Y: " + yrate);
		rescaleRate = rescaleBase;

		journeyLength = Vector3.Distance (start.position, end.position);
		startTime = Time.time;
		speed = 125.0f;
	}
		
	public void toggleAfterSize() {

		speed = 750.0f;
		inward.transform.position = this.gameObject.transform.position;
		start = inward.transform;

		outward.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y-Screen.height, this.gameObject.transform.position.z);
		end = outward.transform;

		startTime = Time.time;
		journeyLength = Vector3.Distance (start.position, end.position);
		open = false;
		moving = true;

	}

	public override void initiateLocations () {


		unit = new GameObject ();
		unit.AddComponent<UnitBase> ();
		unit.GetComponent<UnitBase>().init ();
		unit.transform.SetParent (this.gameObject.transform);
		unit.name = "Unit Interface";

		open = false;
		moving = false;
		initSpeed = 2000.0f;
		xrate = 0; yrate = 0;
		rescaleBase = 10.0f;
		speed = initSpeed;
		if (coreStart == null) {
			coreStart = new GameObject ("coreStart");
			coreCentre = new GameObject ("coreCentre");
			coreStart.AddComponent<RectTransform> ();
			coreStart.GetComponent<RectTransform> ().sizeDelta = this.gameObject.transform.GetChild(0).GetComponent<RectTransform> ().sizeDelta;
			coreCentre.AddComponent<RectTransform> ();
			coreCentre.GetComponent<RectTransform> ().sizeDelta = this.gameObject.transform.GetChild(0).GetComponent<RectTransform> ().sizeDelta;

		}
		coreStart.transform.position = this.gameObject.transform.position;
		coreStart.transform.position = this.gameObject.transform.position;
	
		if (inward == null)
			inward = new GameObject ("StartScroll");
		if (outward == null) {
			outward = new GameObject ("EndScroll");
			outward.AddComponent<RectTransform> ();
		}

		outward.GetComponent<RectTransform> ().sizeDelta = this.gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
		inward.transform.position = this.gameObject.transform.position;
		outward.transform.position = new Vector3((Screen.width/2)/maxRatio, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
		coreCentre.transform.position = outward.transform.position;
		//this.gameObject.SetActive (false);
		//this.gameObject.transform.position.x + (float)this.gameObject.GetComponent<RectTransform>().rect.width

		start = outward.transform;
		end = inward.transform;
	}

	public override void reset() {

		this.gameObject.transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<CanvasGroup> ().alpha = 1;

		this.gameObject.transform.position = coreStart.transform.position;
		//this.gameObject.GetComponent<RectTransform> ().sizeDelta = coreStart.GetComponent<RectTransform> ().sizeDelta;
		this.gameObject.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta = coreStart.GetComponent<RectTransform> ().sizeDelta;
		open = false;
		resizingx = false;
		resizingy = false;

		initiateLocations ();

	}

	public void slideUpward(RectTransform iconTransform) {
		initiateLocations ();

		inward.transform.position = iconTransform.position;
		start = inward.transform;

		outward.transform.position = coreCentre.transform.position;
		end = coreCentre.transform;

		journeyLength = Vector3.Distance (start.position, end.position);
		startTime = Time.time;

		toggle ();

	}
}