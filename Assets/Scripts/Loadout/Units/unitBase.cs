using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour {


	public enum UnitType
	{
		DRONE,
		SENTINEL,
		PRISM,
		MACARBIS
	}

	public string unitName;
	public UnitType type;
	public Texture2D portrait;
	public int[] stat;
	public EquipmentBase[] equipment;
	public string description;

	// Use this for initialization
	void Start () {
		init ();
	}

	public virtual void init() {

		unitName = "Unnamed Unit";
		name = unitName;

		portrait = new Texture2D ((int)GameObject.Find("UnitPicker").transform.GetChild(0).GetChild(0).FindChild("Portrait").GetComponent<RectTransform>().sizeDelta.x, (int)GameObject.Find("UnitPicker").transform.GetChild(0).GetChild(0).FindChild("Portrait").GetComponent<RectTransform>().sizeDelta.y);

		stat = new int[10];
		for (int i = 0; i < 10; i++) {
			stat [i] = 0;
		}

		equipment = new EquipmentBase[6];
		for (int i = 0; i < 6; i++) {
			equipment[i] = new EquipmentBase();
		}
	}

	public void readUnit() {

	}
}
