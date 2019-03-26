using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSetup : MonoBehaviour {
	
	public enum UnitType
	{
		NONE
	}

	public string unitName;
	public UnitType type;

	// Use this for initialization
	void Start () {
		unitName = "Unnamed Unit";
		name = unitName;
		type = UnitType.NONE;
	}


}
