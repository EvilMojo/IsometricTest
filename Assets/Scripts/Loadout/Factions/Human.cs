using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Human : MonoBehaviour {
	
	//Store available units and equipment as GameObjects

	public GameObject[] unitList;
	public GameObject[] equipmentList;
	public string unitsData = "humanUnits";
	public string equipmentData = "humanEquipment";


	// Use this for initialization
	void Start () {
	}

	public void loadUnits(){

		/*public string unitName;
		public UnitType type;
		public Texture2D portrait;
		public int[] stat;				* 10
		public EquipmentBase[] equipment; * 6
		public string description;*/

		unitList = new GameObject[4];

		List<string> unitdata = readFile (unitsData);

		int iterator = 0;

		for (int i = 0; i < unitList.Length; i++) {
			unitList[i].AddComponent<UnitBase>();
			unitList[i].GetComponent<UnitBase>().unitName = unitdata [iterator];
		}
	}

	public void loadEquipment() {

		equipmentList = new GameObject[10];

		List<string> equipmentData = readFile (equipmentData);

		for (int i = 0; i < unitList.Length; i++) {

		}
	}

	public List<string> readFile(string file) {

		List<string> data = new List<string> ();

		StreamReader input = new StreamReader(file);

		while(!input.EndOfStream) {
			data.Add(input.ReadLine());
		}
	}
}
