using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour {


	public enum UnitType
	{
		DRONE,
		SENTINEL,
		PRISM,
		MACARBIS,
		SEEKER
	}

	public string unitName;
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

	public void readUnit() {

	}
}
