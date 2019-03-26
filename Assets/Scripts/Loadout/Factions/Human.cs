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
	public string equipmentFile = "humanEquipment.txt";


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

		unitList = new GameObject[4];

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
			}
			iterator++;

			unitList[i].GetComponent<UnitBase>().portrait = Resources.Load(("Images/Factions/Human/Units/" + (unitdata [iterator++])), typeof (Texture2D)) as Texture2D;

			string[] ints = unitdata [iterator++].Split (' ');

			for (int s = 0; s < ints.Length; s++) {
				print (s + " " + ints [s]);
				if (unitList [i].GetComponent<UnitBase> ().stat == null) {
					unitList [i].GetComponent<UnitBase> ().stat = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
				} 
				unitList [i].GetComponent<UnitBase> ().stat [s] = int.Parse (ints [s]);
			}

			string[] equipmentstring = unitdata [iterator++].Split(' ');
			unitList[i].GetComponent<UnitBase>().equipmentSlots = new GameObject[equipmentstring.Length];
			for (int e = 0; e < equipmentstring.Length; e++) {

				unitList [i].GetComponent<UnitBase> ().equipmentSlots [e] = new GameObject ();
				unitList [i].GetComponent<UnitBase> ().equipmentSlots [e].transform.SetParent (unitList [i].gameObject.transform);

				/*Maybe not here, either in equipment or equipment needs to come first
				 * if (equipmentstring [e] == "HEAD") {
					unitList [i].GetComponent<UnitBase> ().equipmentSlots [e].AddComponent<EquipmentBase> ();
				} else if (equipmentstring [e] == "BODY") {
				} else if (equipmentstring [e] == "WEAPON") {
				} else if (equipmentstring [e] == "CARRY") {
				} else if (equipmentstring [e] == "FEET") {
				} else if (equipmentstring [e] == "UTILITY") {
				}*/
			}
			unitList[i].GetComponent<UnitBase>().description = unitdata [iterator++];
		}
		return unitList;
	}

	public void loadEquipment() {

		equipmentList = new GameObject[10];

		List<string> equipmentData = readFile (equipmentFile);

		int iterator = 0;

		for (int i = 0; i < unitList.Length; i++) {

		}
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
