using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Human : MonoBehaviour {
	
	//Store available units and equipment as GameObjects

	public GameObject[] unitList;
	public GameObject[] equipmentList;
	public string dataPath = "Assets/Resources/GameData/";
	public string unitsFile = "humanUnits.txt";
	public string equipmentFile = "humanEquipments.txt";
	public const int unitCount = 5;
	public const int equipmentCount = 5;


	// Use this for initialization
	void Start () {
		//loadEquipment();
		//loadUnits();
	}

	public GameObject[] loadUnits(){

		/*public string unitName;
		public UnitType type;
		public Texture2D portrait;
		public int[] stat;				* 10
		public EquipmentBase[] equipment; * 6
		public string description;*/

		unitList = new GameObject[5];

		List<string> unitdata = readFile (dataPath + unitsFile);

		int iterator = 0;

		for (int i = 0; i < unitList.Length; i++) {

			unitList[i] = new GameObject();

			unitList [i].gameObject.transform.SetParent (this.gameObject.transform);

			unitList[i].AddComponent<UnitBase>();

			unitList[i].GetComponent<UnitBase>().unitName = unitdata [iterator];
			unitList [i].GetComponent<UnitBase> ().gameObject.name = unitdata [iterator++] + "Template";

			if (unitdata [iterator] == "DRONE") {
				unitList [i].GetComponent<UnitBase> ().type = UnitBase.UnitType.DRONE;
			} else if (unitdata [iterator] == "SENTINEL") {
				unitList [i].GetComponent<UnitBase> ().type = UnitBase.UnitType.SENTINEL;
			} else if (unitdata [iterator] == "PRISM") {
				unitList [i].GetComponent<UnitBase> ().type = UnitBase.UnitType.PRISM;
			} else if (unitdata [iterator] == "MACARBIS") {
				unitList [i].GetComponent<UnitBase> ().type = UnitBase.UnitType.MACARBIS;
			} else if (unitdata [iterator] == "SEEKER") {
				unitList [i].GetComponent<UnitBase> ().type = UnitBase.UnitType.SEEKER;
			}
			iterator++;

			unitList[i].GetComponent<UnitBase>().portrait = Resources.Load(("Images/Factions/Human/Units/" + (unitdata [iterator++])), typeof (Sprite)) as Sprite;

			string[] ints = unitdata [iterator++].Split (' ');

			if (unitList [i].GetComponent<UnitBase> ().stat == null) {
				unitList [i].GetComponent<UnitBase> ().stat = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
			} 
			for (int s = 0; s < ints.Length; s++) {
				unitList [i].GetComponent<UnitBase> ().stat [s] = int.Parse (ints [s]);
			}

			string[] equipments = unitdata [iterator++].Split (' ');
			if (unitList [i].GetComponent<UnitBase> ().equipmentSlots == null) {
				unitList [i].GetComponent<UnitBase> ().equipmentSlots = new GameObject[equipments.Length];
			}
			for (int e = 0; e < equipments.Length; e++) {
				unitList [i].GetComponent<UnitBase> ().equipmentSlots [e] = new GameObject ();
				unitList [i].GetComponent<UnitBase> ().equipmentSlots [e].transform.SetParent (unitList[i].transform);
				unitList [i].GetComponent<UnitBase> ().equipmentSlots [e].name = equipments [e];
				unitList [i].GetComponent<UnitBase> ().equipmentSlots [e].AddComponent<EquipmentBase> ();
				switch (equipments [e]) {
				case "HEAD": 
					unitList [i].GetComponent<UnitBase> ().equipmentSlots [e].GetComponent<EquipmentBase> ().location = EquipmentBase.EquipmentType.HEAD;
					break;
				case "OPTIC": 
					unitList [i].GetComponent<UnitBase> ().equipmentSlots [e].GetComponent<EquipmentBase> ().location = EquipmentBase.EquipmentType.OPTIC;
					break;
				case "BODY": 
					unitList [i].GetComponent<UnitBase> ().equipmentSlots [e].GetComponent<EquipmentBase> ().location = EquipmentBase.EquipmentType.BODY;
					break;
				case "WEAPON": 
					unitList [i].GetComponent<UnitBase> ().equipmentSlots [e].GetComponent<EquipmentBase> ().location = EquipmentBase.EquipmentType.WEAPON;
					break;
				case "CARRY": 
					unitList [i].GetComponent<UnitBase> ().equipmentSlots [e].GetComponent<EquipmentBase> ().location = EquipmentBase.EquipmentType.CARRY;
					break;
				case "FEET": 
					unitList [i].GetComponent<UnitBase> ().equipmentSlots [e].GetComponent<EquipmentBase> ().location = EquipmentBase.EquipmentType.FEET;
					break;
				case "UTILITY": 
					unitList [i].GetComponent<UnitBase> ().equipmentSlots [e].GetComponent<EquipmentBase> ().location = EquipmentBase.EquipmentType.UTILITY;
					break;	
				default:  
					//unitList [i].GetComponent<UnitBase> ().equipmentSlots [e].GetComponent<EquipmentBase> ().location = null;
					print("Invalid");
					break;
				}

				unitList [i].GetComponent<UnitBase> ().equipmentSlots [e].GetComponent<EquipmentBase> ().equipmentName = unitList [i].GetComponent<UnitBase> ().equipmentSlots [e].GetComponent<EquipmentBase> ().location.ToString ();

			}



			unitList [i].GetComponent<UnitBase> ().description = unitdata [iterator++]; 
		}
		return unitList;
	}

	public GameObject[] loadEquipment() {

		/*Helmet
		HELMET
		helmet
		0 0 0 0 0 0 0 0
		This is a basic General helmet*/
		equipmentList = new GameObject[5];

		List<string> equipmentData = readFile (dataPath + equipmentFile);

		int iterator = 0;

		for (int i = 0; i < equipmentList.Length; i++) {

			equipmentList[i] = new GameObject();

			equipmentList[i].gameObject.transform.SetParent (this.gameObject.transform);

			equipmentList[i].AddComponent<EquipmentBase>();

			equipmentList[i].GetComponent<EquipmentBase>().equipmentName = equipmentData [iterator];

			equipmentList[i].GetComponent<EquipmentBase>().gameObject.name = equipmentData [iterator++] + "Template";

			if (equipmentData [iterator] == "HEAD") {
				equipmentList [i].GetComponent<EquipmentBase> ().location = EquipmentBase.EquipmentType.HEAD;
			} else if (equipmentData [iterator] == "OPTIC") {
				equipmentList [i].GetComponent<EquipmentBase> ().location = EquipmentBase.EquipmentType.OPTIC;
			} else if (equipmentData [iterator] == "BODY") {
				equipmentList [i].GetComponent<EquipmentBase> ().location = EquipmentBase.EquipmentType.BODY;
			} else if (equipmentData [iterator] == "WEAPON") {
				equipmentList [i].GetComponent<EquipmentBase> ().location = EquipmentBase.EquipmentType.WEAPON;
			} else if (equipmentData [iterator] == "CARRY") {
				equipmentList [i].GetComponent<EquipmentBase> ().location = EquipmentBase.EquipmentType.CARRY;
			} else if (equipmentData [iterator] == "FEET") {
				equipmentList [i].GetComponent<EquipmentBase> ().location = EquipmentBase.EquipmentType.FEET;
			} else if (equipmentData [iterator] == "UTILITY") {
				equipmentList [i].GetComponent<EquipmentBase> ().location = EquipmentBase.EquipmentType.UTILITY;
			}

			iterator++;

			equipmentList[i].GetComponent<EquipmentBase>().portrait = Resources.Load(("Images/Factions/Human/Equipment/" + (equipmentData [iterator++])), typeof (Sprite)) as Sprite;

			string[] wearer = equipmentData [iterator++].Split (' ');
			if (equipmentList [i].GetComponent<EquipmentBase> ().validWearer == null) {
				equipmentList [i].GetComponent<EquipmentBase> ().validWearer = new UnitBase.UnitType[wearer.Length];
			}

			for (int e = 0; e < wearer.Length; e++) {
				if (wearer[e] == "DRONE") {
					equipmentList [i].GetComponent<EquipmentBase> ().validWearer[e] = UnitBase.UnitType.DRONE;
				} else if (wearer[e] == "SENTINEL") {
					equipmentList [i].GetComponent<EquipmentBase> ().validWearer[e] = UnitBase.UnitType.SENTINEL;
				} else if (wearer[e] == "PRISM") {
					equipmentList [i].GetComponent<EquipmentBase> ().validWearer[e] = UnitBase.UnitType.PRISM;
				} else if (wearer[e] == "MACARBIS") {
					equipmentList [i].GetComponent<EquipmentBase> ().validWearer[e] = UnitBase.UnitType.MACARBIS;
				} else if (wearer[e] == "SEEKER") {
					equipmentList [i].GetComponent<EquipmentBase> ().validWearer[e] = UnitBase.UnitType.SEEKER;
				} else if (wearer[e] == "ALL") {
					equipmentList [i].GetComponent<EquipmentBase> ().validWearer = new UnitBase.UnitType[5];
					equipmentList [i].GetComponent<EquipmentBase> ().validWearer[0] = UnitBase.UnitType.DRONE;
					equipmentList [i].GetComponent<EquipmentBase> ().validWearer[1] = UnitBase.UnitType.SENTINEL;
					equipmentList [i].GetComponent<EquipmentBase> ().validWearer[2] = UnitBase.UnitType.PRISM;
					equipmentList [i].GetComponent<EquipmentBase> ().validWearer[3] = UnitBase.UnitType.MACARBIS;
					equipmentList [i].GetComponent<EquipmentBase> ().validWearer[4] = UnitBase.UnitType.SEEKER;
				}
			}

			string[] ints = equipmentData [iterator++].Split (' ');

			if (equipmentList [i].GetComponent<EquipmentBase> ().stat == null) {
				equipmentList [i].GetComponent<EquipmentBase> ().stat = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
			}
			for (int s = 0; s < ints.Length; s++) {
				equipmentList [i].GetComponent<EquipmentBase> ().stat [s] = int.Parse (ints [s]);
			}

			equipmentList [i].GetComponent<EquipmentBase> ().description = equipmentData [iterator++]; 

		}
		return equipmentList;
	}

	public List<string> readFile(string file) {

		List<string> data = new List<string> ();

		StreamReader input = new StreamReader(file);

		while(!input.EndOfStream) {
			data.Add(input.ReadLine());
		}

		return data;
	}
}
