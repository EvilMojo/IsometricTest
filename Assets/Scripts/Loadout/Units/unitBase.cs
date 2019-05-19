using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour {


	public enum UnitType
	{
		NONE,
		DRONE,
		SENTINEL,
		PRISM,
		MACARBIS,
		SEEKER
	}

	public string unitName;
	public int unitIndex;
	public UnitType type;
	public Sprite portrait;
	public int[] stat;
	public GameObject[] equipmentSlots;
	public string description;

	// Use this for initialization
	void Start () {
	}

	public void init() {

		unitName = "Unnamed Unit";
		name = unitName;

		//portrait = new Sprite ((int)GameObject.Find("UnitPicker").transform.GetChild(0).GetChild(0).FindChild("Portrait").GetComponent<RectTransform>().sizeDelta.x, (int)GameObject.Find("UnitPicker").transform.GetChild(0).GetChild(0).FindChild("Portrait").GetComponent<RectTransform>().sizeDelta.y);

		portrait = new Sprite ();

		stat = new int[8];
		for (int i = 0; i < 8; i++) {
			stat [i] = 0;
		}

		equipmentSlots = new GameObject[6];
		for (int i = 0; i < 6; i++) {
			equipmentSlots[i] = new GameObject();
			equipmentSlots [i].name = ("Equipment Slot " + i.ToString());
			equipmentSlots [i].AddComponent<EquipmentBase> ();
			equipmentSlots [i].transform.SetParent (this.gameObject.transform);
		}

		description = "Basic Unit";
	}

	public void initEquipment() {
		if (equipmentSlots == null) {
			equipmentSlots = new GameObject[6];
			for (int i = 0; i < 6; i++) {
				equipmentSlots[i] = new GameObject();
				equipmentSlots [i].name = ("Equipment Slot " + i.ToString());
				equipmentSlots [i].AddComponent<EquipmentBase> ();
				equipmentSlots [i].transform.SetParent (this.gameObject.transform);
			}
		}
	}

	public void assignUnitDetails (GameObject unit) {
		unit.GetComponent<UnitBase> ().printInfo ();
		this.unitName = unit.GetComponent<UnitBase> ().unitName;
		this.type = unit.GetComponent<UnitBase> ().type;
		this.portrait = unit.GetComponent<UnitBase> ().portrait;
		this.stat = unit.GetComponent<UnitBase> ().stat;
		this.description = unit.GetComponent<UnitBase> ().description;

		//this.unit.GetComponent<UnitBase> ().equipmentSlots = new GameObject[6];
		//this.equipmentSlots = new GameObject[unit.GetComponent<UnitBase> ().equipmentSlots.Length];
		//print(unit.GetComponent<UnitBase> ().equipmentSlots.Length);
		//print (this.unit.GetComponent<UnitBase> ().equipmentSlots.Length);

		/*for (int i = unit.GetComponent<UnitBase> ().equipmentSlots.Length; i < 6; i++) {
			print (i);
			this.name = "Equipment Slot " + i.ToString ();
			this.unit.transform.GetChild (i).gameObject.GetComponent<EquipmentBase> ().equipmentName = "";
			this.unit.transform.GetChild (i).gameObject.GetComponent<EquipmentBase> ().portrait = null;
			this.unit.transform.GetChild (i).gameObject.GetComponent<EquipmentBase> ().description = "";
			this.unit.transform.GetChild (i).gameObject.GetComponent<EquipmentBase> ().location = EquipmentBase.EquipmentType.NONE;
			this.unit.transform.GetChild (i).gameObject.GetComponent<EquipmentBase> ().validWearer = null;
		}*/
		/*
		if (this.equipmentSlots == null) {
			this.equipmentSlots = new GameObject[]
		}*/

		//for (int i = 0; i < unit.GetComponent<UnitBase> ().equipmentSlots.Length; i++) {
			//print (unit.name);
			//print (unit.transform.GetChild (i).name);
			//print (this.unit.name);
			//if (this.unit.GetComponent<UnitBase> ().equipmentSlots[i] == null) {
			//	print ("NULLLLLL");
			//}

			/*print (i);
			print (unit);
			print (unit.GetComponent<UnitBase>().equipmentSlots.Length);
			//print (this.equipmentSlots[i].name);
			print (unit.GetComponent<UnitBase> ().equipmentSlots [i]);
			print (unit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ());
			print (unit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().equipmentName);
			print ("--");
			print (this.gameObject);
			print (this.equipmentSlots.Length);
			print (equipmentSlots [i]);
			print ("??");
			print (equipmentSlots [i].GetComponent<EquipmentBase> ());
			print (equipmentSlots [i].GetComponent<EquipmentBase> ().equipmentName);
			print ("!!");

			if (i < this.equipmentSlots.Length) {
				equipmentSlots[i].GetComponent<EquipmentBase>().assignEquipmentDetails (unit.GetComponent<UnitBase> ().equipmentSlots [i]);
			} */
		//}
	}

	public void assignEquipmentTemplates(GameObject unitTemplate) {
		//this.unit.GetComponent<UnitBase> ().equipmentSlots = new GameObject[6];
		//equipmentSlots = new GameObject[unitTemplate.GetComponent<UnitBase> ().equipmentSlots.Length];
		//print(unit.GetComponent<UnitBase> ().equipmentSlots.Length);
		//print (this.unit.GetComponent<UnitBase> ().equipmentSlots.Length);

		/*for (int i = unit.GetComponent<UnitBase> ().equipmentSlots.Length; i < 6; i++) {
			print (i);
			this.unit.transform.GetChild (i).gameObject.name = "Equipment Slot " + i.ToString ();
			this.unit.transform.GetChild (i).gameObject.GetComponent<EquipmentBase> ().equipmentName = "";
			this.unit.transform.GetChild (i).gameObject.GetComponent<EquipmentBase> ().portrait = null;
			this.unit.transform.GetChild (i).gameObject.GetComponent<EquipmentBase> ().description = "";
			this.unit.transform.GetChild (i).gameObject.GetComponent<EquipmentBase> ().location = EquipmentBase.EquipmentType.NONE;
			this.unit.transform.GetChild (i).gameObject.GetComponent<EquipmentBase> ().validWearer = null;
		}*/
		for (int i = 0; i < equipmentSlots.Length; i++) {
			//print (unit.name);
			//print (unit.transform.GetChild (i).name);
			//print (this.unit.name);
			//if (this.unit.GetComponent<UnitBase> ().equipmentSlots[i] == null) {
			//	print ("NULLLLLL");
			//}
			if (i < equipmentSlots.Length && i < unitTemplate.GetComponent<UnitBase> ().equipmentSlots.Length) {
				//equipmentSlots [i] = new GameObject ();
				//unitTemplate.transform.GetChild (i).gameObject;

				equipmentSlots [i].name = unitTemplate.transform.GetChild (i).name;
				equipmentSlots [i].GetComponent<EquipmentBase> ().equipmentName = unitTemplate.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().equipmentName;
				equipmentSlots [i].GetComponent<EquipmentBase> ().portrait = unitTemplate.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().portrait;
				equipmentSlots [i].GetComponent<EquipmentBase> ().description = unitTemplate.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().description;
				equipmentSlots [i].GetComponent<EquipmentBase> ().location = unitTemplate.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().location;
				equipmentSlots [i].GetComponent<EquipmentBase> ().validWearer = unitTemplate.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().validWearer;
			} 
		}
	}

	public void assignEquipmentDetails(GameObject fromUnit) {
		//print ("makiing templates from " + fromUnit.name);
		for (int i = 0; i < fromUnit.GetComponent<UnitBase> ().equipmentSlots.Length; i++) {
			this.equipmentSlots [i].GetComponent<EquipmentBase> ().assignEquipmentDetails (fromUnit.GetComponent<UnitBase> ().equipmentSlots [i]);
		}
	}

	public void printInfo() {
		print ("Unit Profile for: " + gameObject.name);
		print (unitName);
		print (type);
		//print (portrait);
		string line = "";
		foreach (int i in stat) {
			line += i.ToString () + ", ";
		}
		print (line);
		//public GameObject[] equipmentSlots;
		foreach (GameObject equipment in equipmentSlots) {
			print (equipment.GetComponent<EquipmentBase> ().equipmentName);
		}
		//public string description;
	}
}
