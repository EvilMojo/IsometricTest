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
	public List<GameObject> unitList;

	// Use this for initialization
	void Start () {
		faction = Faction.NONE;
	}

	public void setFaction(Faction faction) {
		this.faction = faction;
	}

	public void makeNewUnit(GameObject unitView) {

		List<GameObject> unitList = GameObject.Find ("PlayerSetup").GetComponent<PlayerSetup> ().unitList;

		GameObject unit = new GameObject ();
		unit.AddComponent<UnitSetup>();
		unit.transform.SetParent (this.gameObject.transform);

		unitView.GetComponent<UnitView> ().unitIndex = unitList.Count;
		unitView.GetComponent<UnitView> ().unit = unit;
	}

	public void enlist(GameObject unitView) {
		
		unitList.Add (this.gameObject.transform.GetChild(unitList.Count).gameObject);


		unitView.GetComponent<UnitView> ().unitIndex = 0;
		unitView.GetComponent<UnitView> ().unit = null;
	}
}
