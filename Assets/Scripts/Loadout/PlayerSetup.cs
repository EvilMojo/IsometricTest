using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour {

	public enum Faction {
		NONE,
		HUMAN,
		IAXIN
	}

	public Faction faction;
	public GameObject[] unitTemplates;
	public GameObject army;
	public List<GameObject> unitList;

	// Use this for initialization
	void Start () {
		faction = Faction.NONE;
		army = new GameObject ();
		army.name = "Player Army";
		army.transform.SetParent (this.gameObject.transform);
	}

	public void setFaction(Faction faction) {
		this.faction = faction;

		if (faction == Faction.HUMAN) {
			this.gameObject.AddComponent<Human> ();
			unitTemplates = this.gameObject.GetComponent<Human>().loadUnits ();
			unitTemplates = this.gameObject.GetComponent<Human>().loadEquipment ();
		} else if (faction == Faction.IAXIN) {
			//this.gameObject.AddComponent<Iaxin> ();
		}
	}

	public void makeNewUnit(GameObject unitView) {

		//List<GameObject> unitList = GameObject.Find ("PlayerSetup").GetComponent<PlayerSetup> ().unitList;

		//GameObject unit = new GameObject ();
		//unit.AddComponent<UnitBase>();
		//unit.transform.SetParent (this.gameObject.transform);

		//unitView.GetComponent<UnitBase> ().unitIndex = unitList.Count;
		//unitView.GetComponent<UnitBase> ().unit = unit;
	}

	public void enlist(GameObject unitView) {
		
		GameObject unitInterface = unitView.transform.FindChild ("Unit Interface").gameObject;

		GameObject newUnit = new GameObject();

		newUnit.AddComponent<UnitBase> ();
		newUnit.GetComponent<UnitBase> ().init();
		newUnit.GetComponent<UnitBase> ().unitIndex = unitList.Count;
		newUnit.name = "Player Unit " + unitList.Count;
		//newUnit.transform.SetParent (army.transform);
		newUnit.GetComponent<UnitBase> ().assignUnitDetails (unitInterface);
		newUnit.GetComponent<UnitBase> ().initEquipment ();

		newUnit.GetComponent<UnitBase> ().assignEquipmentDetails (unitInterface);

/*		newUnit.GetComponent<UnitBase> ().unitName = unitInterface.GetComponent<UnitBase>().unitName;
		newUnit.GetComponent<UnitBase> ().description = unitInterface.GetComponent<UnitBase>().description;
		newUnit.GetComponent<UnitBase> ().portrait = unitInterface.GetComponent<UnitBase>().portrait;
		newUnit.GetComponent<UnitBase> ().type = unitInterface.GetComponent<UnitBase>().type;
		newUnit.GetComponent<UnitBase> ().stat = unitInterface.GetComponent<UnitBase>().stat;

		newUnit.GetComponent<UnitBase>().equipmentSlots = new GameObject[unitInterface.GetComponent<UnitBase>().equipmentSlots.Length];

		for (int i = 0; i < unitInterface.GetComponent<UnitBase> ().equipmentSlots.Length; i++) {

			newUnit.GetComponent<UnitBase> ().equipmentSlots [i] = new GameObject();
			newUnit.GetComponent<UnitBase> ().equipmentSlots [i].AddComponent<EquipmentBase>();

			newUnit.GetComponent<UnitBase> ().equipmentSlots [i].name = unitInterface.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().equipmentName;
			newUnit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().equipmentName = unitInterface.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().equipmentName;
			newUnit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().portrait = unitInterface.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().portrait;
			newUnit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().description = unitInterface.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().description;
			newUnit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().location = unitInterface.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().location;
			newUnit.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().validWearer = unitInterface.GetComponent<UnitBase> ().equipmentSlots [i].GetComponent<EquipmentBase> ().validWearer;

			newUnit.GetComponent<UnitBase> ().equipmentSlots [i].transform.SetParent (newUnit.transform);

		}*/

		newUnit.transform.parent = this.gameObject.transform;

		this.unitList.Add (newUnit);
	}

	public void modify(GameObject unitView) {
		GameObject unitInterface = unitView.transform.FindChild ("Unit Interface").gameObject;

		GameObject modifiedUnit = GameObject.Find ("Player Unit " + unitInterface.GetComponent<UnitBase> ().unitIndex);

		print (modifiedUnit.name);
		//newUnit.transform.SetParent (army.transform);
		modifiedUnit.GetComponent<UnitBase> ().assignUnitDetails (unitInterface);
		modifiedUnit.GetComponent<UnitBase> ().initEquipment ();
		modifiedUnit.GetComponent<UnitBase> ().assignEquipmentDetails (unitInterface);

		//modifiedUnit.transform.parent = this.gameObject.transform;

		//this.unitList.Add (modifiedUnit);
	}
}
