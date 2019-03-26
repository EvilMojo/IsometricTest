using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentBase : MonoBehaviour {

	public enum EquipmentType
	{
		HEAD,
		BODY,
		WEAPON,
		CARRY,
		FEET,
		UTILITY
	}

	public string equipmentName;
	public Texture2D portrait;
	public EquipmentType type;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
