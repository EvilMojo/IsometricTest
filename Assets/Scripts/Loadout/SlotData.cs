using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotData : MonoBehaviour {

	public EquipmentBase.EquipmentType type;
	public int index;

	public void toggleEquipment(GameObject unit) {
		GameObject.Find ("Options").GetComponent<OptionsView> ().toggleEquipment (unit, index);	
	}

}
