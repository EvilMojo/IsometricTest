using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is used for orders, which are actions units execute
//Thought: Split this order into different scripts for easier order management
public class Order : ScriptableObject {
	
	public enum OrderType {
		MOVE,
		BASIC_ATTACK,
		CONSTRUCT,
		CONSTRUCTION,
		TRAIN_UNIT
	};

	public string orderName;
	public OrderType type;
	public Player.UnitType unitType;
	public GameObject[] UIPane;
	public Coordinates position;
	public Equipment sourceEquipment; //can be null if Source was from building

	public Order(){
		orderName = "";
		type = OrderType.MOVE;
		unitType = Player.UnitType.GENERIC;
		UIPane = new GameObject[0];
	}

	public void init(string orderName, OrderType type, Player.UnitType unitType, Coordinates position) {
		this.orderName = orderName;
		this.type = type;
		this.unitType = unitType;
		this.position = position;
	}
		
	//Each order will have behaviour dependent on its type. This function carries out the order behaviours
	public void execute(GameObject controller, GameObject unit) {

		Player controllerScript = controller.GetComponent<Player> ();
		Unit unitScript = unit.GetComponent<Unit> ();

		//If order behaviour is instant (like queuing a unit or a non-targetted ability)
		//enact order on currently selected unit

		//If order behaviour is not instant (requires a target)
		//Set player's order stance to this order type
		//ensure that player's active unit 

		//If order behaviour requires additional UI elements (like,unit building choices, targetting cancels, this may not be required)
		//destroy current UI
		//fan out new UI set

		//Each order type should execute 
		if (type == OrderType.MOVE) {
			if (!controllerScript.selection.GetComponent<Unit> ().moved) {
				controllerScript.setPlayerState (Player.State.MOVEUNIT);
				controllerScript.setOrderRecipient (unit);
			}
		//This needs to open a new set of UI Panels with buildings in it
		} else if (type == OrderType.CONSTRUCT) {
			openBuildOptions (controller, unit);

		//Set construction template and await clicking on a tile
		} else if (type == OrderType.CONSTRUCTION) {
			controllerScript.setPlayerState (Player.State.BUILDCONSTRUCTION);
			controllerScript.setBuildingTemplate (Player.UnitType.HQ);
			controllerScript.setOrderRecipient (unit);
			controllerScript.getBuildingTemplate ().name = "BuildingTemplate";

		//Create a unit instantly from the building's spawn location and order the unit to move to the designated spawn destination
		//TODO: Create a tick-down timer so units come out after <x> turns
		} else if (type == OrderType.TRAIN_UNIT) {
			controllerScript.setPlayerState (Player.State.UICLOSED);
			controllerScript.setOrderRecipient (unit);
			controllerScript.createUnit (unit.gameObject.transform.position, Player.UnitType.WARRIOR, position, unit.GetComponent<Unit> ().spawnpoint);

		//This may need to be renamed and re-tweaked to facilitate for equipment capabilities
		//Activate equipment based on Order-type
		} else if (type == OrderType.BASIC_ATTACK) {
			if (!controllerScript.selection.GetComponent<Unit> ().attacked)
				this.sourceEquipment.performFunction ();
		}

	}

	//Fan out a new set of UI with building images
	public void openBuildOptions(GameObject controller, GameObject unit) {

		Player playerScript = controller.GetComponent<Player> ();
		int constructables = playerScript.getConstructableCount();

		if (constructables != 0) {
			Player pScript = controller.GetComponent<Player> ();

			//Set new image for interactable button
			Texture2D texture = Resources.Load ("defaultimg.png") as Texture2D;
			Material material = new Material (Shader.Find ("Diffuse"));
			material.mainTexture = texture;

			//Divide a circle by how many buildings can be build (this is for an even distribution on the UI)
			float sectorSize = 360 / constructables;

			//Create new set of UIPanes
			UIPane = new GameObject[constructables];
			GameObject anchor = new GameObject ();
			anchor.transform.position = unit.gameObject.transform.position;
			GameObject anchorstart = new GameObject ();
			anchorstart.transform.position = unit.gameObject.transform.position;

		
			for (int i = 0; i < constructables; i++) {

				//Instantiate a UI Panel that serves as a button
				UIPane [i] = Instantiate (Resources.Load ("UIPane", typeof(GameObject)) as GameObject);
				UnitUI uiScript = UIPane [i].gameObject.GetComponent<UnitUI> ();
				UIPane [i].gameObject.transform.SetParent (anchor.transform);

				//Give the UI Panel the same position as the unit so it fans out from the unit's position
				uiScript.setPosition(new Vector3 ((unit.gameObject.transform.position.x), (unit.gameObject.transform.position.y), (unit.gameObject.transform.position.z)));

				//Rotate the UI Panel to face the player's Camera, using the player's camera to generate an inverse angle
				UIPane [i].transform.localEulerAngles = new Vector3( 
					playerScript.getPlayerCamera ().transform.eulerAngles.x-90,  
					playerScript.getPlayerCamera ().transform.eulerAngles.y,  
					playerScript.getPlayerCamera ().transform.eulerAngles.z);

				//calculate the angles in which the UI panel will move in
				float angle = sectorSize * i;

				//For prototyping, only the first building does anything
				//Assign an order to the UI Panel so that, when clicked, it will initialise the construction template
				Order order = new Order ();
				order = ScriptableObject.CreateInstance ("Order") as Order;
				order.init ("Building Option", Order.OrderType.CONSTRUCTION, (Player.UnitType)i, position);

				uiScript.setOwnership (controller, unit.gameObject, playerScript.getPlayerDummy(), anchor, anchorstart);
				uiScript.setOrder (order);
				uiScript.setDestination (angle, (i == constructables - 1));
			}

			//Fan out the UI simultaneously
			playerScript.initPanes (UIPane);

		} 
	}
}
