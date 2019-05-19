using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitView : View {

	public GameObject coreStart;
	public GameObject coreCentre;

	public int unitIndex;
	public GameObject unit;


	public void clearUnitDetails() {
		this.unit = this.gameObject.transform.FindChild ("Unit Interface").gameObject;
		this.unit.GetComponent<UnitBase> ().unitName = "New Unit";
		this.unit.GetComponent<UnitBase> ().type = UnitBase.UnitType.NONE;
		this.unit.GetComponent<UnitBase> ().portrait = null;
		this.unit.GetComponent<UnitBase> ().stat = new int[8] {0, 0, 0, 0, 0, 0, 0, 0};
		for (int i = 0; i < this.unit.GetComponent<UnitBase> ().equipmentSlots.Length; i++) {
			this.unit.GetComponent<UnitBase> ().equipmentSlots[i].name = "Equipment Slot " + i.ToString();
			this.unit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().equipmentName = "Empty";
			this.unit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().location = EquipmentBase.EquipmentType.NONE;
			this.unit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().description = "";
			this.unit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().validWearer = new UnitBase.UnitType[0];
			this.unit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().stat = new int[0];
		}
	}
/* This is moved over to unitBase. Makes more sense
	public void assignUnitDetails (GameObject unit) {
		if (this.gameObject.GetComponent<UnitBase> () == null) {
			this.gameObject.AddComponent<UnitBase> ();
		}
		this.unit.GetComponent<UnitBase> ().unitName = unit.GetComponent<UnitBase> ().unitName;
		this.unit.GetComponent<UnitBase> ().type = unit.GetComponent<UnitBase> ().type;
		this.unit.GetComponent<UnitBase> ().portrait = unit.GetComponent<UnitBase> ().portrait;
		this.unit.GetComponent<UnitBase> ().stat = unit.GetComponent<UnitBase> ().stat;

		//this.unit.GetComponent<UnitBase> ().equipmentSlots = new GameObject[6];
		this.unit.GetComponent<UnitBase> ().equipmentSlots = new GameObject[unit.GetComponent<UnitBase> ().equipmentSlots.Length];
		//print(unit.GetComponent<UnitBase> ().equipmentSlots.Length);
		//print (this.unit.GetComponent<UnitBase> ().equipmentSlots.Length);

		for (int i = unit.GetComponent<UnitBase> ().equipmentSlots.Length; i < 6; i++) {
			print (i);
			this.unit.transform.GetChild (i).gameObject.name = "Equipment Slot " + i.ToString ();
			this.unit.transform.GetChild (i).gameObject.GetComponent<EquipmentBase> ().equipmentName = "";
			this.unit.transform.GetChild (i).gameObject.GetComponent<EquipmentBase> ().portrait = null;
			this.unit.transform.GetChild (i).gameObject.GetComponent<EquipmentBase> ().description = "";
			this.unit.transform.GetChild (i).gameObject.GetComponent<EquipmentBase> ().location = EquipmentBase.EquipmentType.NONE;
			this.unit.transform.GetChild (i).gameObject.GetComponent<EquipmentBase> ().validWearer = null;
		}

		for (int i = 0; i < unit.GetComponent<UnitBase> ().equipmentSlots.Length; i++) {
			//print (unit.name);
			//print (unit.transform.GetChild (i).name);
			//print (this.unit.name);
			//if (this.unit.GetComponent<UnitBase> ().equipmentSlots[i] == null) {
			//	print ("NULLLLLL");
			//}
			if (i < this.unit.GetComponent<UnitBase> ().equipmentSlots.Length) {
				this.unit.GetComponent<UnitBase> ().equipmentSlots [i] = this.unit.transform.GetChild (i).gameObject;
				this.unit.GetComponent<UnitBase> ().equipmentSlots [i].name = unit.transform.GetChild (i).name;
				this.unit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().equipmentName = unit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().equipmentName;
				this.unit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().portrait = unit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().portrait;
				this.unit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().description = unit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().description;
				this.unit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().location = unit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().location;
				this.unit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().validWearer = unit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().validWearer;
			} 
		}
		this.unit.GetComponent<UnitBase> ().description = unit.GetComponent<UnitBase> ().description;
	}
*/
	//Moved to UnitBase, it makes more sense to have it there
/*	public void assignEquipmentDetails(GameObject equipment, int index) {
		print (this.unit.name + " Parent");
		for (int i = 0; i < this.unit.GetComponent<UnitBase> ().equipmentSlots.Length; i++) {
			print (this.unit.GetComponent<UnitBase> ().equipmentSlots [i].name);
		}
		print (equipment.name + " New");
		this.unit.GetComponent<UnitBase> ().equipmentSlots [index].name = equipment.GetComponent<EquipmentBase> ().equipmentName;
		this.unit.GetComponent<UnitBase> ().equipmentSlots [index].GetComponent<EquipmentBase> ().equipmentName = equipment.GetComponent<EquipmentBase> ().equipmentName;
		this.unit.GetComponent<UnitBase> ().equipmentSlots [index].GetComponent<EquipmentBase> ().description = equipment.GetComponent<EquipmentBase> ().description;
		this.unit.GetComponent<UnitBase> ().equipmentSlots [index].GetComponent<EquipmentBase> ().location = equipment.GetComponent<EquipmentBase> ().location;
		this.unit.GetComponent<UnitBase> ().equipmentSlots [index].GetComponent<EquipmentBase> ().portrait = equipment.GetComponent<EquipmentBase> ().portrait;
		this.unit.GetComponent<UnitBase> ().equipmentSlots [index].GetComponent<EquipmentBase> ().validWearer = equipment.GetComponent<EquipmentBase> ().validWearer;
	//	this.unit.GetComponent<UnitBase> ().equipmentSlots [index].GetComponent<EquipmentBase> ().stat = new int[equipment.GetComponent<EquipmentBase> ().stat.Length];
	//	for (int i = 0; i < equipment.GetComponent<EquipmentBase> ().stat.Length; i++) {
	//		this.unit.GetComponent<UnitBase> ().equipmentSlots [index].GetComponent<EquipmentBase> ().stat [i] = equipment.GetComponent<EquipmentBase> ().stat [i];
	//	}
	//}*/

	public void changeUnitObjectInfo() {
		GameObject unitInterface = this.gameObject.transform.FindChild ("Unit Interface").gameObject;
		print (unitInterface.name + "Interface");
		print (this.unit + " changing into");
		unitInterface.GetComponent<UnitBase> ().assignUnitDetails (this.unit);
		print (this.unit + " changing middle");
		unitInterface.GetComponent<UnitBase> ().assignEquipmentDetails (this.unit);
		print (this.unit + " changing outof");
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
			//EquipmentSlot.name = "ESlot " + i.ToString ();
			if (i < this.unit.GetComponent<UnitBase> ().equipmentSlots.Length) {
				EquipmentSlot.GetComponent<CanvasGroup>().alpha = 1;
				EquipmentSlot.GetComponent<CanvasGroup> ().interactable = true;
				EquipmentSlot.GetComponent<CanvasGroup> ().blocksRaycasts = true;
				EquipmentSlot.transform.FindChild ("Slot").GetComponent<Button> ().onClick.RemoveAllListeners ();
				/*EquipmentSlot.transform.FindChild ("Slot").GetComponent<Button> ().onClick.AddListener (delegate {
					openEquipmentOptions(this.gameObject.transform.FindChild("Options").gameObject);
				});*/
				EquipmentSlot.transform.FindChild("Slot").GetComponent<SlotData>().index = i;
				EquipmentSlot.transform.FindChild("Slot").GetComponent<SlotData> ().type = unit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().location;

				//EquipmentSlot.GetComponent<Image>().sprite = unit.GetComponent<UnitBase>().equipmentSlots[i].GetComponent<EquipmentBase>().portrait;
			} else {
				//Reset this image, it's not being used
				EquipmentSlot.GetComponent<CanvasGroup>().alpha = 0;
				EquipmentSlot.GetComponent<CanvasGroup> ().interactable = false;
				EquipmentSlot.GetComponent<CanvasGroup> ().blocksRaycasts = false;
			}
		}
		addEquipmentButton (this.gameObject.transform.FindChild ("Panel").FindChild ("Canvas").FindChild ("EquipmentSlots").FindChild ("EquipmentSlotsPanel").FindChild ("EquipmentSlot (" + 0 + ")").gameObject);
		addEquipmentButton (this.gameObject.transform.FindChild ("Panel").FindChild ("Canvas").FindChild ("EquipmentSlots").FindChild ("EquipmentSlotsPanel").FindChild ("EquipmentSlot (" + 1 + ")").gameObject);
		addEquipmentButton (this.gameObject.transform.FindChild ("Panel").FindChild ("Canvas").FindChild ("EquipmentSlots").FindChild ("EquipmentSlotsPanel").FindChild ("EquipmentSlot (" + 2 + ")").gameObject);
		addEquipmentButton (this.gameObject.transform.FindChild ("Panel").FindChild ("Canvas").FindChild ("EquipmentSlots").FindChild ("EquipmentSlotsPanel").FindChild ("EquipmentSlot (" + 3 + ")").gameObject);
		addEquipmentButton (this.gameObject.transform.FindChild ("Panel").FindChild ("Canvas").FindChild ("EquipmentSlots").FindChild ("EquipmentSlotsPanel").FindChild ("EquipmentSlot (" + 4 + ")").gameObject);
		addEquipmentButton (this.gameObject.transform.FindChild ("Panel").FindChild ("Canvas").FindChild ("EquipmentSlots").FindChild ("EquipmentSlotsPanel").FindChild ("EquipmentSlot (" + 5 + ")").gameObject);
	}

	public void addEquipmentButton(GameObject EquipmentSlot) {
		EquipmentSlot.transform.FindChild ("Slot").GetComponent<Button> ().onClick.AddListener (delegate {
			//This would have been useful and straightforward with excepton to the closure problem
			//this.gameObject.transform.FindChild("Options").gameObject.GetComponent<OptionsView>().toggleEquipment(this.unit, copyi);
			EquipmentSlot.transform.FindChild ("Slot").gameObject.GetComponent<SlotData>().toggleEquipment(this.unit);
		});
	}

	public void animateDismissal() {
		outward.transform.position = this.gameObject.transform.position;

		inward.transform.position = new Vector3((Screen.width*2), this.gameObject.transform.position.y, this.gameObject.transform.position.z);
		open = true;
		//end = outward.transform;

		resizingx = true;
		resizingy = true;

		toggle ();
	}


	public void animateEnlistment() {
		
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

		if (unit == null) {
			unit = new GameObject ();
			unit.AddComponent<UnitBase> ();
			unit.GetComponent<UnitBase> ().init ();
			unit.transform.SetParent (this.gameObject.transform);
			unit.name = "Unit Interface";
			print ("Did we just reset???");
		} else {
			print ("Did we just clear shit???");
			//clearUnitDetails ();
		}

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

	public void toggleButton(string appear) {
		if (appear == "enlist") {
			print ("enlisting");
			this.transform.GetChild(0).GetChild(0).FindChild ("Enlist").gameObject.GetComponent<CanvasGroup> ().alpha = 1;
			this.transform.GetChild(0).GetChild(0).FindChild ("Enlist").gameObject.GetComponent<CanvasGroup> ().blocksRaycasts = true;
			this.transform.GetChild(0).GetChild(0).FindChild ("Enlist").gameObject.GetComponent<CanvasGroup> ().interactable = true;
			this.transform.GetChild(0).GetChild(0).FindChild ("Modify").gameObject.GetComponent<CanvasGroup> ().alpha = 0;
			this.transform.GetChild(0).GetChild(0).FindChild ("Modify").gameObject.GetComponent<CanvasGroup> ().blocksRaycasts = false;
			this.transform.GetChild(0).GetChild(0).FindChild ("Modify").gameObject.GetComponent<CanvasGroup> ().interactable = false;
		} else if (appear == "modify") {
			print ("modifying");
			this.transform.GetChild(0).GetChild(0).FindChild ("Enlist").gameObject.GetComponent<CanvasGroup> ().alpha = 0;
			this.transform.GetChild(0).GetChild(0).FindChild ("Enlist").gameObject.GetComponent<CanvasGroup> ().blocksRaycasts = false;
			this.transform.GetChild(0).GetChild(0).FindChild ("Enlist").gameObject.GetComponent<CanvasGroup> ().interactable = false;
			this.transform.GetChild(0).GetChild(0).FindChild ("Modify").gameObject.GetComponent<CanvasGroup> ().alpha = 1;
			this.transform.GetChild(0).GetChild(0).FindChild ("Modify").gameObject.GetComponent<CanvasGroup> ().blocksRaycasts = true;
			this.transform.GetChild(0).GetChild(0).FindChild ("Modify").gameObject.GetComponent<CanvasGroup> ().interactable = true;
		}
	}

}