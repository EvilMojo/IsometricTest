using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : ScriptableObject {

	public GameObject equippedTo;

	public enum Location {
		HEAD,
		BODY,
		ARMS,
		BACK,
		LEFTHAND,
		RIGHTHAND,
		LEFTSHOULDER,
		RIGHTSHOULDER,
		LEGS,
		PERIPHERPAL,
		STORAGE
	};

	public string name;
	public float damage;
	public int range;
	public bool twohanded;		//Determines if an equipment piece (weapon) is 2H, disallow use of additional weapons for this case
	public Location location;	//Where the equipment fits onto theo unit

	//attach "equips" this unit with this object/equipment
	//When attaching, an order needs to be added to the units order list so the equipment can be used by the player
	public void attach(GameObject unit) {
		equippedTo = unit;
		switch(this.name) {
		case "BASIC_HEAD":
			break;
		case "BASIC_RIGHTHAND":
			Order[] order = new Order[1];
			order [0] = new Order ();
			order [0].name = "BASIC_ATTACK";
			order [0].type = Order.OrderType.BASIC_ATTACK;
			order [0].sourceEquipment = this;
			unit.GetComponent<Unit> ().addUnitOrders (order);
			break;
		case "BASIC_BODY":
			break;
		}
	}

	//When removing equipment from a unit, its order list will need to be adjusted to remove the ability
	public void detach(GameObject unit) {
		//Destroy (this);
	}


	//This performs the function of the equipment piece.
	public void performFunction() {
		switch(this.name) {
		case "BASIC_HEAD":
			break;
		case "BASIC_RIGHTHAND":
			equippedTo.GetComponent<Unit> ().getController ().GetComponent<Player> ().setPlayerState (Player.State.ACQUIREATTACKTARGET);
			equippedTo.GetComponent<Unit> ().getController ().GetComponent<Player> ().setSelection (equippedTo.gameObject);
			equippedTo.GetComponent<Unit> ().activeEquipment = this;
			equippedTo.GetComponent<Unit> ().displayActionRange (range, "attack");
			break;
		case "BASIC_BODY":
			break;
		}
	}

	//As attacking units with range will require the range statistic on this equipment, 
	//this function uses this equipment's range and the distance to the target (given in the parameter
	public bool targetWithRange(GameObject target) {
		//If distance between firing unit and target < equipment range
		//Fire weapon using stats for target, owning unit irrelevant (Friendly fire is possible)

		//If target is outside range
		if (Mathf.Abs (equippedTo.GetComponent<Unit> ().position.x - target.GetComponent<Unit> ().position.x) > range || Mathf.Abs (equippedTo.GetComponent<Unit> ().position.z - target.GetComponent<Unit> ().position.z) > range) {
			return false;
		} else {
			//Inside Range
			useEquipment(target);
			return true;
		}
	}

	public void useEquipment(GameObject target) {
		//Ensure that unit cannot attack again this turn, return player to default state and apply damage to target unit
		equippedTo.GetComponent<Unit> ().attacked = true;
		equippedTo.GetComponent<Unit> ().getController ().GetComponent<Player> ().setPlayerState (Player.State.NONE);
		target.GetComponent<Unit>().receiveDamage (this.damage);
	}
}
