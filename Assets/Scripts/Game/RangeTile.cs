using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to create temporary "tiles" which show valid movement and attack distances for use
//Currently not in use and requires refinement
public class RangeTile : MonoBehaviour {

	private int i, j, x, y, z;
	private string behaviour;
	private Player controller;
	private Unit owningUnit;

	public void init(int i, int j, string behaviour, Player controllerScript, Unit owningUnit, int x, int y, int z) {
		this.i = i;
		this.j = j;
		this.behaviour = behaviour;
		this.controller = controllerScript;
		this.owningUnit = owningUnit;
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public void init(int i, int j, Player controllerScript, Unit owningUnit, int x, int z, string behaviour) {
		this.i = i;
		this.j = j;
		this.controller = controllerScript;
		this.x = x;
		this.z = z;
		this.behaviour = behaviour;
	}

	void OnMouseOver() {
		//print ("Tile Coords: " + x + ", " + z);
	}

	void OnMouseDown() {
		print ("Range Tile at " + i + ", " + j + " | Tile Coords: " + x + ", " + z);

		switch (behaviour) {
		case ("move"):
			//if (x != 0 || y != 0 || z != 0)
			//if(y!=0)
			controller.getOrderRecipient ().GetComponent<Unit> ().setTargetCoordinate (new Coordinates (x, y, z, 0, 0, 0, null));
			print(controller.getValidTiles ());
			controller.destroyValidTiles ();
			controller.setPlayerState (Player.State.NONE);
			//owningUnit.setUnitState (Unit.State.MOVING);
			break;
		case ("attack"):
			break;
		}
	}

	/*void OnMouseEnter() {
		//print(player.GetComponent<Player>().getPlayerName());
		if (controller != null &&
			controller.getBuildingTemplate () != null &&
			controller.getBuildingTemplate ().GetComponent<Template> () != null) {
			controller.getBuildingTemplate ().transform.position = this.transform.position;
			controller.getBuildingTemplate ().transform.Translate (0, bounds.size.y, 0);
			controller.getBuildingTemplate ().GetComponent<Template> ().setCoordinates (new Coordinates (x, y, z, 0, 0, 0, null));
		}

	}*/

}
