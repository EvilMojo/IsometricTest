using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template : MonoBehaviour {

	Coordinates coordinates;
	Unit unitScript;
	Player playerScript;

	void start() {
		coordinates = new Coordinates (0,0,0,0,0,0,null);
		unitScript = new Unit ();
		playerScript = new Player ();
	}

	void OnMouseDown() {
		unitScript.setUnitState (Unit.State.BUILDING);
		playerScript.getOrderRecipient ().GetComponent<Unit> ().setTargetCoordinate (coordinates);
		playerScript.setPlayerState (Player.State.NONE);
		Destroy (this.gameObject);
		Destroy (this);
	}

	public void setCoordinates(Coordinates coordinates) {
		this.coordinates = coordinates;
	}

	public void setUnitScript(Unit unitScript) {
		this.unitScript = unitScript;
	}

	public void setPlayerScript(Player playerScript) {
		this.playerScript = playerScript;
	}
}
