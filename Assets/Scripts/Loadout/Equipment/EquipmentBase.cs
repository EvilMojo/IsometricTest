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
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
