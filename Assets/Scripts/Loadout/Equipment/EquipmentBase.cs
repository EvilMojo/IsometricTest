using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentBase : MonoBehaviour {

	public enum EquipmentType
	{
		NONE,
		HEAD,
		OPTIC,
		BODY,
		WEAPON,
		CARRY,
		FEET,
		UTILITY
	}

	public string equipmentName;
	public EquipmentType location;
	public Sprite portrait;
	public UnitBase.UnitType[] validWearer;
	public int[] stat;
	public string description;

	// Use this for initialization
	void Start () {
		stat = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void assignEquipmentDetails(GameObject equipment) {

		name = equipment.GetComponent<EquipmentBase> ().equipmentName;
		equipmentName = equipment.GetComponent<EquipmentBase> ().equipmentName;
		description = equipment.GetComponent<EquipmentBase> ().description;
		//print (location + " = " + equipment.GetComponent<EquipmentBase> ().location);
		location = equipment.GetComponent<EquipmentBase> ().location;
		portrait = equipment.GetComponent<EquipmentBase> ().portrait;
		validWearer = equipment.GetComponent<EquipmentBase> ().validWearer;
	
		stat = new int[equipment.GetComponent<EquipmentBase> ().stat.Length];
		for (int i = 0; i < equipment.GetComponent<EquipmentBase> ().stat.Length; i++) {
			stat [i] = equipment.GetComponent<EquipmentBase> ().stat [i];
		}
	}
}
