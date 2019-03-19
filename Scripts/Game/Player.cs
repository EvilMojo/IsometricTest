using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public enum State	{
		NONE,
		UIOPEN,
		UICLOSED,
		MOVEUNIT,
		BUILDCONSTRUCTION,
		ACQUIREATTACKTARGET
	}

	public enum UnitType {
		GENERIC = 0,
		HQ = 1,
		BARRACKS = 2,
		HOUSE = 3,
		BUILDER = 4,
		WARRIOR = 5
	}

	public class StartPosition {
		public Coordinates coordinates;
		public Vector3 vector;
	}

	public float cameraAngle;
	public float cameraDistance;

	public int playerIndex;
	public int totalUnitsMade;
	public string playerName;
	public GameObject selection;
	public GameObject cameraTarget;
	public Camera playerCamera;
	public GameObject dummyTarget;
	public GameObject userInterface;
	public Canvas UIMain;
	public GameObject orderRecipient;
	public State playerState; 
	public UnitType currentlyBuilding;
	public GameObject buildingTemplate;
	public int constructableBuildings;
	public StartPosition startPosition;
	public Color teamColor;

	public GameManager manager;

	public GameObject[] UIPanes;

	public GameObject[,] validTiles;

	public List<GameObject> unitList;

	//UI objects for player interaction
	public GameObject UICanvas;
	public GameObject UIEndTurnButton;

	void Update() {
		//If this is not a bot, the player's camera should move with WASD
		if (this.gameObject.GetComponent<AI> () == null) {
			float camx = cameraTarget.transform.position.x, 
			camy = cameraTarget.transform.position.y, 
			camz = cameraTarget.transform.position.z;

			//Movement keys
			if (Input.GetKey("w")) {
				cameraTarget.transform.Translate(.2f,0f,.2f);
			} if (Input.GetKey("d")) {
				cameraTarget.transform.Translate(.2f,0f,-.2f);
			} if (Input.GetKey("s")) {
				cameraTarget.transform.Translate(-.2f,0f,-.2f);
			} if (Input.GetKey("a")) {
				cameraTarget.transform.Translate(-.2f,0f,.2f);
			} 

			//Rotate dummy target to destination position/rotation target 
			//so the dummy also rotates and serves as a destination for the actual camera
			if (cameraTarget.transform.rotation == dummyTarget.transform.rotation)
			if (Input.GetKeyDown("q")) {
				dummyTarget.transform.Rotate(0f, 90.0f, 0f);
			} else if (Input.GetKeyDown("e")) {
				dummyTarget.transform.Rotate(0f,-90.0f,0f);
			}

			//Rotate the camera so player can view playing field from one of four angles
			if (cameraTarget.transform.rotation != dummyTarget.transform.rotation)
				cameraTarget.transform.rotation = Quaternion.Slerp(cameraTarget.transform.rotation,
					dummyTarget.transform.rotation, Time.deltaTime * 5f);

		}
	}
	//This should create a player which holds default values (money, camera, starting values)
	public void createPlayer(int playerIndex, string playerName, GameObject selection, GameManager manager) {

		this.playerName = playerName;
		this.selection = selection;
		this.manager = manager;
		this.playerIndex = playerIndex;
		this.totalUnitsMade = 0;

		unitList = new List<GameObject> ();

		if (this.gameObject.GetComponent<AI> () == null) {
			//Set default camera properties
			playerCamera = this.gameObject.AddComponent<Camera> () as Camera;
			cameraTarget = new GameObject ();
			cameraTarget.name = "Player " + playerIndex.ToString () + " CameraTarget";
			dummyTarget = new GameObject ();
			dummyTarget.name = "Player " + playerIndex.ToString () + " DummyTarget";

			playerCamera.orthographic = true;
			playerCamera.transform.position = (new Vector3 (-5.0f, 5.0f, -5.0f));
			playerCamera.orthographicSize = 3;
			playerCamera.nearClipPlane = 1;
			playerCamera.farClipPlane = 15;
			playerCamera.transform.LookAt (cameraTarget.transform);
			playerCamera.transform.SetParent (cameraTarget.transform);

			//Set default UI properties for Player UI
			//TODO: Create proper UI and interfaces for player interaction
			UICanvas = new GameObject ();
			UICanvas.AddComponent<Canvas> ();
			UICanvas.GetComponent<Canvas> ().renderMode = RenderMode.ScreenSpaceCamera;
			UICanvas.GetComponent<Canvas> ().worldCamera = playerCamera;
			UICanvas.GetComponent<Canvas> ().transform.SetParent (playerCamera.transform);
			UICanvas.GetComponent<Canvas> ().name = "Canvas";
			UICanvas.GetComponent<Canvas> ().planeDistance = 2;
			UICanvas.GetComponent<Canvas> ();
			UICanvas.AddComponent<GraphicRaycaster> ();

			//Create button for ending player Turn
			UIEndTurnButton = new GameObject ();
			UIEndTurnButton.transform.SetParent (UICanvas.transform);

			UIEndTurnButton.AddComponent<CanvasRenderer> ();

			UIEndTurnButton.AddComponent<Image> ();
			UIEndTurnButton.GetComponent<Image> ().sprite = Resources.Load ("defaultimg", typeof(Sprite)) as Sprite;

			UIEndTurnButton.AddComponent<Button> ();
			UIEndTurnButton.GetComponent<Button> ().name = "End Turn Button";
			UIEndTurnButton.GetComponent<Button> ().targetGraphic = UIEndTurnButton.GetComponent<Image> ();
			UIEndTurnButton.GetComponent<Button> ().transform.SetParent (UIEndTurnButton.transform);
			UIEndTurnButton.GetComponent<Button> ().transform.localScale = new Vector3 (.25f, .25f);
			UIEndTurnButton.GetComponent<Button> ().transform.SetPositionAndRotation (UICanvas.transform.position, UICanvas.transform.rotation);

			UIEndTurnButton.GetComponent<Button> ().transform.localPosition = new Vector2 (-UICanvas.GetComponent<Canvas> ().rootCanvas.pixelRect.width / 4, UICanvas.GetComponent<Canvas> ().rootCanvas.pixelRect.height / 4);

			UIEndTurnButton.GetComponent<Button> ().onClick.AddListener (delegate {
				manager.endTurn (playerIndex);
			});
		}
			
		//Initialise potential buildings and templates
		constructableBuildings = 5;
		buildingTemplate = new GameObject ();

		//Initialise start position
		startPosition = new StartPosition ();

		//Commented out as static start is needed for testing purposes
		//UNcomment later
		//startPosition.coordinates = new Coordinates (0, 1.0f, 0, 0, 0, 0, null);
		//startPosition.vector = new Vector3 (startPosition.coordinates.x, (int)startPosition.coordinates.y, startPosition.coordinates.z);
	}

	public void startTurn() {
		//Go through units and restore their actions so they can do stuff again
		foreach (GameObject unit in unitList) {
				print (unit.GetComponent<Unit>().unitName);
			unit.GetComponent<Unit> ().restoreUnit ();
		}

		//Tick down all unit cooldowns/movement points

		//Get resources?

		//run AI actions (Shouldn't this be handled as a component?)
	}

	//Initialise the player in the game by creating a simple HQ unit
	public void initialise() {


		//Create HQ object
		GameObject unit = Instantiate(Resources.Load("HumanHQ"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

		unit.AddComponent<BoxCollider> ();
		unit.GetComponent<BoxCollider> ().center = new Vector3 (0.2f, 0.0f, 0.0f);
		unit.GetComponent<BoxCollider> ().size = new Vector3 (0.5f, 0.5f, 1.0f);
		if (unit.GetComponent<Unit> () == null)
			unit.AddComponent<Unit> ();
		Unit unitScript = unit.GetComponent<Unit> ();

		//Set static player start locations for testing purposes (consistency)
		if (playerIndex == 0) {
			startPosition.coordinates.x = 5;
			startPosition.coordinates.z = 3;
		}if (playerIndex == 1) {
			startPosition.coordinates.x = 8;
			startPosition.coordinates.z = 8;
		}

		//Move building to start location
		unit.transform.Translate (new Vector3 (startPosition.coordinates.x*0.4f, 0.3f+(startPosition.coordinates.y*.3f), startPosition.coordinates.z*0.4f));

		//Handles where the unit will move when spawned
		unitScript.setController (this.gameObject);
		unitScript.setCoordinates (startPosition.coordinates);
		if (playerIndex == 0) unitScript.setSpawnPoint (new Coordinates (5, 3, 3, 0, 0, 0, startPosition.coordinates));
		unitScript.setSpawnPoint(new Coordinates(startPosition.coordinates.x-1, startPosition.coordinates.y, startPosition.coordinates.z, 0, 0, 0, startPosition.coordinates));

		//List of orders created when the object is made
		Order[] orders = new Order[1];
		orders [0] = ScriptableObject.CreateInstance ("Order") as Order;
		orders [0].init ("Train Unit", Order.OrderType.TRAIN_UNIT, Player.UnitType.WARRIOR, startPosition.coordinates);
		unitScript.addUnitOrders (orders);

		unit.transform.FindChild ("group_Ball").GetComponent<MeshRenderer> ().materials [0].color = this.getTeamColor ();

		unitScript.unitName = "P" + playerIndex.ToString() + ": Human HQ " + totalUnitsMade++.ToString();
		unitScript.size = 2;
		unitScript.obstructMap ();

		unitList.Add (unit);
	}

	public string getPlayerName() {
		return playerName;
	}

	public GameObject getSelection() {
		return selection;
	}

	public void setName(string playerName) {
		this.playerName = playerName;
	}

	//This function outlines which unit the player is selecting, who actions given orders
	public void setSelection(GameObject unit) {
		if (selection != null) {
			destroyValidTiles ();
			closeUI();
		}
		selection = unit;
	}

    public Camera getPlayerCamera() {
        return playerCamera;
    }

	public GameObject getPlayerDummy() {
		return dummyTarget;
	}

	public void deselect() {
		selection = null;
		destroyValidTiles ();
	}

	public StartPosition getStartPosition() {
		return this.startPosition;
	}

	public void setStartPosition(Coordinates startCoordinates, Vector3 startVector) {
		this.startPosition.coordinates = startCoordinates;
		this.startPosition.vector = startVector;
	}

	public Color getTeamColor() {
		return teamColor;
	}

	public void setTeamColor(Color teamColor) {
		this.teamColor = teamColor;
	}

	public void deployUI() {
		//Should probably get the device used for UI locations

        //Desktop Setup
        
        //Phone Setup
	}

	public void addToUnitList(GameObject unit) {
		this.unitList.Add (unit);
	}
	public void removeFromUnitList(GameObject unit) {
		this.unitList.Add (unit);
	}

	//This function sets the player as intending to build
	//Tiles will show a "silhouette" of the building when hovered over.
	public void setBuildingTemplate(UnitType type) {
		currentlyBuilding = type;
		Unit unitScript = this.getSelection ().GetComponent<Unit> ();
		if (unitScript == null)
			print ("WARNING");

		unitScript.setUnitState (Unit.State.BUILDING);

		if (type == UnitType.HQ) {
			buildingTemplate = Instantiate(Resources.Load("HumanHQ", typeof(GameObject))) as GameObject;
			buildingTemplate.AddComponent<Template> ();	
			buildingTemplate.GetComponent<Template> ().setPlayerScript(this);
			buildingTemplate.GetComponent<Template> ().setUnitScript(unitScript);
			Destroy(buildingTemplate.GetComponent<BoxCollider>());

		}
	}

	public GameObject getBuildingTemplate(){
		return buildingTemplate;
	}

	//This function is used when a unit is created, it takes the units position, type, coordinates and the location it will move to on spawn
	public void createUnit(Vector3 pos, UnitType type, Coordinates position, Coordinates spawn) {

		//Determine what unit is being created
		//Thought: Would these be better off as individual classes/scripts?
		if (type == UnitType.HQ) {

			//Instantiate object and set initial values based on function parameters
			GameObject construction = Instantiate (Resources.Load ("HumanHQ"), new Vector3 (pos.x, pos.y, pos.z), Quaternion.identity) as GameObject;
			if (construction.GetComponent<Unit> () == null)
				construction.AddComponent<Unit> ();

			Unit unitScript = construction.GetComponent<Unit> ();

			unitScript.setController (this.gameObject);
			unitScript.setCoordinates (position);
			unitScript.setSpawnPoint(new Coordinates(position.x-1, position.y, position.z, 0, 0, 0, position));

			construction.transform.FindChild ("group_Ball").GetComponent<MeshRenderer> ().materials [0].color = this.getTeamColor ();


			//Allocate Orders to the unit
			Order[] orders = new Order[1];
			orders [0] = ScriptableObject.CreateInstance ("Order") as Order;
			orders [0].init ("Train Unit", Order.OrderType.TRAIN_UNIT, Player.UnitType.WARRIOR, position);
			unitScript.addUnitOrders (orders);

		} else if (type == UnitType.WARRIOR) {

			//Instantiate object and set initial values
			GameObject construction = Instantiate (Resources.Load ("Rover"), new Vector3 (pos.x, pos.y, pos.z), Quaternion.identity) as GameObject;

			//Set collider (For onclick needs)
			construction.AddComponent<BoxCollider> ();
			construction.GetComponent<BoxCollider> ().center = new Vector3 (0.0f, 0.0f, 0.0f);
			construction.GetComponent<BoxCollider> ().size = new Vector3 (0.3f, 0.2f, 0.4f);

			//Add a unit script if not currently existing
			if (construction.GetComponent<Unit> () == null)
				construction.AddComponent<Unit> ();

			//Set ID Properties
			Unit unitScript = construction.GetComponent<Unit> ();
			unitScript.name = "Test creation 1";

			unitScript.unitName = "P" + playerIndex.ToString() + ": Rover " + totalUnitsMade;
			unitScript.setController (this.gameObject);
			unitScript.setOwner (this.gameObject);
			unitScript.setCoordinates (position);

			//Allocate Orders to the Unit
			Order[] orders = new Order[2];

			orders[0] = ScriptableObject.CreateInstance ("Order") as Order;
			orders [0].init ("Move", Order.OrderType.MOVE, Player.UnitType.WARRIOR, position);
			orders[1] = ScriptableObject.CreateInstance ("Order") as Order;
			orders [1].init ("Construct", Order.OrderType.CONSTRUCT, Player.UnitType.WARRIOR, position);

			unitScript.addUnitOrders (orders);

			//Add some basic equipment for prototyping purposes
			Equipment[] initEquipment = new Equipment[3];

			initEquipment [0] = ScriptableObject.CreateInstance ("Equipment") as Equipment;
			initEquipment [0].location = Equipment.Location.HEAD;
			initEquipment [0].name = "BASIC_HEAD";

			initEquipment [1] = ScriptableObject.CreateInstance ("Equipment") as Equipment;
			initEquipment [1].location = Equipment.Location.RIGHTHAND;
			initEquipment [1].name = "BASIC_RIGHTHAND";
			initEquipment [1].range = 3;
			initEquipment [1].damage = 10;
			initEquipment [1].twohanded = false;

			initEquipment [2] = ScriptableObject.CreateInstance ("Equipment") as Equipment;
			initEquipment [2].location = Equipment.Location.BODY;
			initEquipment [2].name = "BASIC_BODY";

			unitScript.setEquipmentSlots (initEquipment);

			//Set movement properties, traversal index will need to be -1 so that the unit moves to its spawn point
			unitScript.traversalIndex = -1;
			print ("Moving Unit from Player: " + unitScript.name);
			unitScript.setCoordinates (new Coordinates(position.x, position.y, position.z, position.h, position.g, position.f, null));

			//Change this unit's colour
			if (construction.transform.FindChild ("group_Default") != null) {
				construction.transform.FindChild ("group_Default").GetComponent<MeshRenderer> ().materials [0].color = this.getTeamColor ();
			} else if (construction.transform.FindChild ("default") != null) {
				construction.transform.FindChild ("default").GetComponent<MeshRenderer> ().materials [0].color = this.getTeamColor ();
			}

			//Get map information from the Game Manager so this unit can move in consideration to obstacles
			float[,] map = manager.getMap ();
			float[,] mapObstacle = manager.getMapObstacle ();

			unitScript.moveToSpawn(spawn);

			//Add this unit to the player's list of units
			unitList.Add (construction);

		}


		//this.setBuildingTemplate (UnitType.NONE);

	}


	public int getConstructableCount() {
		return constructableBuildings;
	}


	public void setPlayerState(State playerState) {
		this.playerState = playerState;
	}

	public void setOrderRecipient(GameObject orderRecipient) {
		this.orderRecipient = orderRecipient;
	}

	public State getPlayerState() {
		return playerState;
	}

	public GameObject getOrderRecipient() {
		return orderRecipient;
	}

	public UnitType getCurrentlyBuilding() {
		return currentlyBuilding;
	}
		
	public void closeUI() {
		foreach (GameObject pane in UIPanes) {
			if (pane != null) {
				UnitUI uiScript = pane.gameObject.GetComponent<UnitUI> ();
				uiScript.reversal ();
			}
		}
	}

	public void initPanes(GameObject[] UIPanes) {
		this.UIPanes = UIPanes;
	}

	//Quick function to find a nearby available destination for the unit to move to
	public Coordinates findSpawnPoint(float[,] map, float[,] mapObstacle, Coordinates buildingCoordinates) {
		if (Level.isValidCoordinate(new Coordinates(buildingCoordinates.x+1, buildingCoordinates.y, buildingCoordinates.z, 0, 0, 0, null), buildingCoordinates, map, mapObstacle)) {
			return new Coordinates(buildingCoordinates.x+1, buildingCoordinates.y, buildingCoordinates.z, 0, 0, 0, null);
		} else if (Level.isValidCoordinate(new Coordinates(buildingCoordinates.x-1, buildingCoordinates.y, buildingCoordinates.z, 0, 0, 0, null), buildingCoordinates, map, mapObstacle)) {
			return new Coordinates(buildingCoordinates.x-1, buildingCoordinates.y, buildingCoordinates.z, 0, 0, 0, null);
		} else if (Level.isValidCoordinate(new Coordinates(buildingCoordinates.x, buildingCoordinates.y, buildingCoordinates.z+1, 0, 0, 0, null), buildingCoordinates, map, mapObstacle)) {
			return new Coordinates(buildingCoordinates.x, buildingCoordinates.y, buildingCoordinates.z+1, 0, 0, 0, null);
		} else if (Level.isValidCoordinate(new Coordinates(buildingCoordinates.x, buildingCoordinates.y, buildingCoordinates.z-1, 0, 0, 0, null), buildingCoordinates, map, mapObstacle)) {
			return new Coordinates(buildingCoordinates.x, buildingCoordinates.y, buildingCoordinates.z-1, 0, 0, 0, null);
		} else {
			print ("HELP THIS ISN'T SUPPOSED TO HAPPEN");
			return null;
		}
	}

	//Valid tile functions relate to tiles generated that indicate ranges and move distances
	public GameObject[,] getValidTiles() {
		return validTiles;
	}
		
	public void setValidTiles(GameObject[,] validTiles) {
		this.validTiles = validTiles;
	}

	public void destroyValidTiles() {
		if (validTiles != null) {
			foreach (GameObject t in validTiles) {
				Destroy (t);
			}
		}
	}
}