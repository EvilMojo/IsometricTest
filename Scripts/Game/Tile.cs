using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {


	private int x, y, z; 	//Basic coordinates
	private string type; 	//Type of tile to dictate tile properties

	GameObject manager;
	GameObject player0;		//Cheating: Ideally we would get the player from the game client user (or the current player's turn)
	Player playerScript;	
	Bounds bounds;			//Holds the bounds which is the basic object size for each tile

	public void setTile(int x, int y, int z, string type) {

		this.x = x;
		this.y = y;
		this.z = z;
		this.type = type;
		manager = GameObject.Find("GameManager");
		player0 = manager.GetComponent<GameManager>().getPlayer(0);
		playerScript = player0.GetComponent<Player> ();
		Mesh mesh = this.gameObject.transform.FindChild("group_Default").GetComponent<MeshFilter>().mesh;
		bounds = mesh.bounds;
	}


	void OnMouseDown(){

		//Get clicking playerM
		//Get who's turn it is
		//If a==b and player owns the unit
			//get selection from player
			//assign co-ordinates to unit's destination

		Unit unitScript = playerScript.getSelection ().GetComponent<Unit> ();

		//If a player had selected a build order, execute the move order on this tile
		if (playerScript.getPlayerState () == Player.State.MOVEUNIT) {
			if (x != 0 || y != 0 || z != 0)
				playerScript.getOrderRecipient ().GetComponent<Unit> ().setTargetCoordinate (new Coordinates(x, y, z, 0, 0, 0, null));
			playerScript.setPlayerState (Player.State.NONE);
			unitScript.setUnitState (Unit.State.MOVING);

		//If a player had selected a unit but not chosen an order, reset selections on both
		} else if (playerScript.getPlayerState() == Player.State.UIOPEN) {
			playerScript.closeUI ();
			playerScript.selection = null;
			unitScript.setUnitState (Unit.State.IDLE);

		//If a player had selected a build order, execute the build order on this tile
		} else if (playerScript.getPlayerState () == Player.State.BUILDCONSTRUCTION) {
			Destroy (playerScript.getBuildingTemplate ());
			unitScript.setUnitState (Unit.State.BUILDING);
			print (unitScript.getUnitState ());
			playerScript.getOrderRecipient ().GetComponent<Unit> ().setTargetCoordinate (new Coordinates(x, y, z, 0, 0, 0, null));
			playerScript.setPlayerState (Player.State.NONE);
			//unitScript.setUnitState (Unit.State.IDLE);


		}

//		GameObject unit = player.GetComponent<Player>().getSelection();
	
//			unit.GetComponent<Unit>().setDestination(x,y,z);
//			unit.GetComponent<Unit>().setStartTime(Time.time);
//			unit.GetComponent<Unit>().move();

//			unit.GetComponent<Unit>().setStartTime(Time.time);
//			unit.GetComponent<Unit>().move();
	

	}

	void OnMouseEnter() {

		//If the player is opting to build a building, show a dummy building originating from this tile's position
		//Set the building template's coordinates so it builds on this tile on-click
		if (playerScript!=null &&
			playerScript.getBuildingTemplate()!=null &&
			playerScript.getBuildingTemplate().GetComponent<Template>()!=null) {
			playerScript.getBuildingTemplate().transform.position = this.transform.position;
			playerScript.getBuildingTemplate ().transform.Translate(0, bounds.size.y, 0);
			playerScript.getBuildingTemplate ().GetComponent<Template>().setCoordinates (new Coordinates (x, y, z, 0, 0, 0, null));
		}

	}
	void OnMouseExit() {
	}

	public int getX() {
		return x;
	}

	public int getY() {
		return y;
	}
	
	public int getZ() {
		return z;
	}

	public string getType() {
		return type;
	}

	public void setX(int x) {
		this.x = x;
	}

	public void setY(int y) {
		this.y = y;
	}
	
	public void setZ(int z) {
		this.z = z;
	}


	public void setType() {
		this.type = type;
	}
}
