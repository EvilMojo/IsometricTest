using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

	public enum State {
		IDLE = 1,
		MOVING = 2,
		ATTACKING = 3,
		BUILDING = 4
	}

	public string unitName;
	public GameObject owner;
	public GameObject controller;
	public GameObject startpos, endpos;
	public GameObject anchor;
	public GameObject anchorstart;
	public Coordinates position;
	public Coordinates spawnpoint;
	public List<Coordinates> path;

	public float startTime;
	public float distance;
	public bool selected = false;
	public int MAX_POS = 999999;
	public int traversalIndex;
	public GameObject[] UIPane;
	public List<Order> order;
	public Equipment[] equipment;
	public Equipment activeEquipment;
	public GameObject manager;

	public bool moved;
	public int moveDistancePerTurn;
	public int movesThisTurn;
	public bool attacked;
	public bool utilitied;
	public float maxHealth = -1;
	public float health;
	public int size; //Size = area on map, 0 = single tile, 4 = 2x2, 9 = 3x3, 16 = 4x4

	private Quaternion rot;
	private string meshString;
	private string endOrientation;

	public Player.UnitType type;

	private State state;

	void Start() {

		//Set manager
		manager = GameObject.Find ("GameManager");

		//Set A* path to new empty list
		if (path==null) path = new List<Coordinates>();
		traversalIndex = -1;

		//Set how this object's mesh will be accessed
		if (this.gameObject.transform.FindChild ("group_Default") != null) {
			meshString = "group_Default";
		} else if (this.gameObject.transform.FindChild ("default") != null) {
			meshString = "default";
		}

		//Set default values and initialise unit actions to be available
		health = 20;
		maxHealth = 20;
		moved = false;
		moveDistancePerTurn = 3;
		movesThisTurn = 0;
		attacked = false;
		utilitied = false;
	}
	
	void Update() {

		//IF this unit's health falls below 0, destroy
		//Thought: This can probably be just checked at each time this unit receives damage and not every frame
		if (this.health <= 0 && this.maxHealth!=-1) {
			destroyUnit();
		}

		//When unit has moves available, move the unit along its path
		if(movesThisTurn<=moveDistancePerTurn)
		if (path != null && path.Count > 0) {
			
			//Initiate traversal to the next node
			if (traversalIndex == -1) {
				traversalIndex = path.Count - 1;
				int i = path.Count-1;
				foreach (Coordinates c in path) {
					i--;
				}
				startpos = new GameObject (unitName + "start");
				startpos.transform.position = gameObject.transform.position;
				endpos = new GameObject (unitName + "end");
				endpos.transform.position = gameObject.transform.position;
				
			}

			//If the unit has reached its destination
			if (Vector3.SqrMagnitude (endpos.transform.position - gameObject.transform.position) < 0.00000001) {

				//if we haven't reached the end of the path, 
				if (traversalIndex != 0) {
					this.setStartTime (Time.time);

					//Test to see if there is anything on the destination node and move to the next node by decrementing which node we are on
					this.move (path [traversalIndex], path [traversalIndex - 1]);
					movesThisTurn++;
					traversalIndex--;
				}
				//if we have no more nodes on path
				else if (traversalIndex == 0) {

					//Set this unit's coordinates to the final path item's coordinates
					this.setCoordinates (path [traversalIndex]);

					//If we are building a building, we will need to build it and move off the current tile to a different tile
					if (this.state == State.BUILDING) {
						this.state = State.IDLE;
						Player playerScript = controller.GetComponent<Player> ();
						playerScript.createUnit (this.gameObject.transform.position, playerScript.getCurrentlyBuilding (), this.position, this.spawnpoint);
						this.setTargetCoordinate (path [traversalIndex + 1]);
						traversalIndex = -1;

						//Otherwise (And after building) we can remove the movement variables
					} else {
						path.Clear ();
						traversalIndex = -1;
						Destroy (startpos);
						Destroy (endpos);
					}
				}
			//If we aren't at the start or end of a traversal from one tile to the next, handle a smooth movement between tiles
			} else {
				float distCovered = (Time.time - startTime) * 1.0f;
				float fracJourney = distCovered / distance;

				//Rotation
				gameObject.transform.rotation = Quaternion.Lerp(startpos.transform.rotation, rot, 1.0f);
				//Lerp Movement
				gameObject.transform.position = Vector3.Lerp (startpos.transform.position, endpos.transform.position, fracJourney);
			}
		} 


	}

	//This function handles a unit occupying a set of tiles dependent on size
	public void obstructMap() {
		float[,] map = getMapObstacle ();

		if (size <= 0) 
			map [this.getCoordinates ().z, this.getCoordinates ().x] = 1;
		if (size <= 4) {
			map [this.getCoordinates ().z - 1, this.getCoordinates ().x] = 1;
			map [this.getCoordinates ().z, this.getCoordinates ().x - 1] = 1;
			map [this.getCoordinates ().z - 1, this.getCoordinates ().x - 1] = 1;
		} if (size <= 9) {
			map [this.getCoordinates ().z + 1, this.getCoordinates ().x + 1] = 1;
			map [this.getCoordinates ().z + 1, this.getCoordinates ().x] = 1;
			map [this.getCoordinates ().z, this.getCoordinates ().x + 1] = 1;
			map [this.getCoordinates ().z - 1, this.getCoordinates ().x + 1] = 1;
			map [this.getCoordinates ().z + 1, this.getCoordinates ().x - 1] = 1;
		} if (size <= 16) {

		}

		if (manager == null) {
			manager = GameObject.Find ("GameManager");
		}

		manager.GetComponent<GameManager> ().setMapObstacle (map);
	}

	//This function empties the tile areas of obstruction once the unit is not there, dependent on size
	public void clearMap() {
		float[,] map = getMapObstacle ();

		if (size <= 0) 
			map [this.getCoordinates ().z, this.getCoordinates ().x] = 0;
		if (size <= 4) {
			map [this.getCoordinates ().z - 1, this.getCoordinates ().x] = 0;
			map [this.getCoordinates ().z, this.getCoordinates ().x - 1] = 0;
			map [this.getCoordinates ().z - 1, this.getCoordinates ().x - 1] = 0;
		} if (size <= 9) {
			map [this.getCoordinates ().z + 1, this.getCoordinates ().x + 1] = 0;
			map [this.getCoordinates ().z + 1, this.getCoordinates ().x] = 0;
			map [this.getCoordinates ().z, this.getCoordinates ().x + 1] = 0;
			map [this.getCoordinates ().z - 1, this.getCoordinates ().x + 1] = 0;
			map [this.getCoordinates ().z + 1, this.getCoordinates ().x - 1] = 0;
		} if (size <= 16) {

		}

		if (manager == null) {
			manager = GameObject.Find ("GameManager");
		}
		print (manager);
		print (manager.GetComponent<GameManager>().getMapObstacle());
		manager.GetComponent<GameManager> ().setMapObstacle (map);
	}

	void OnMouseDown(){
		
		Player playerScript = controller.GetComponent<Player> ();

		//The below code is debug where player[0] is actioning unit owner (Need to make dynamically coded)
		//Determine if the unit is being selected or receiving damage depending on unit owner
		if (controller == manager.GetComponent<GameManager> ().player [0]) {
			if (selected) {
				
				playerScript.deselect ();
				playerScript.closeUI ();
			} else {
				
				playerScript.setSelection (this.gameObject);
				showUnitUI ();
			}
			selected = !selected;
		} else {
			//Need to specify which player is clicking on this thing?
			//This is where the player clicks on this unit to do damage. Currently executed via equipment
		}
	}

	public void receiveDamage(float damage) {
		this.health = this.health - damage;
	}

	public void createUnit(string unitName, GameObject owner, GameObject controller, int x, int y, int z, bool selected, List<float> path) {
		this.unitName = unitName;
		this.owner = owner;
		this.controller = controller;
		print(this.controller.GetComponent<Player>().getPlayerName());
		this.position = new Coordinates(x, y, z, 0, 0, 0, null);
		this.selected = selected;
	}

	public void destroyUnit() {

		this.clearMap ();
		controller.GetComponent<Player> ().removeFromUnitList (this.gameObject);
		Destroy (this.gameObject);
		Destroy (this);
	}

	//Restores unit's actions for use again
	public void restoreUnit() {
		moved = false;
		movesThisTurn = 0;
		attacked = false;
		utilitied = false;
		if (this.path != null) {
			print ("Not null path");
			path.Add (this.position);
		} else {
			print ("null path");
		}
	}


	public string getUnitName() {
		return unitName;
	}	

	public float getDistance() {
		return distance;
	}

	public float getStartTime() {
		return startTime;
	}

	public GameObject getOwner() {
		return owner;
	}
	
	public GameObject getController() {
		return controller;
	}

	public void setUnitName(string unitName) {
		this.unitName = unitName;
	}	

	public void setStartTime(float startTime) {
		this.startTime = startTime;
	}

	public void setDistance(float distance) {
		this.distance = distance;
	}

	public void setOwner(GameObject owner) {
		this.owner = owner;
	}
	
	public void setController(GameObject controller) {
		this.controller = controller;
	}

	public void setCoordinates(Coordinates position) {
		this.position = position;
	}
	public Coordinates getCoordinates() {
		return this.position;
	}

	public State getUnitState() {
		return state;
	}

	public void setUnitState(State state) {
		this.state = state;
	}

	public List<Order> getOrderList() {
		return order;
	}

	public void setSpawnPoint(Coordinates spawn) {
		this.spawnpoint = spawn;
	}

	public void setTeamColour (Color color) {
		//nothing
	}

	//applies equipment to the unit
	public void setEquipmentSlots(Equipment[] initEquipment) {
		equipment = new Equipment[initEquipment.Length];
		int i = 0;
		foreach (Equipment e in initEquipment) {
			equipment[i] = e;
			equipment[i].attach (this.gameObject);
			i++;
		}
	}

	//Show the range for the unit's current ability/weapon in use by the player
	public void displayActionRange(int range, string behaviour) {

		Player controllerScript = this.getController ().GetComponent < Player> ();
		GameObject[,] validTiles = new GameObject[1+range*2, 1+range*2];
		float[,] map = getMap ();

		//For every unit of range, 
		int x = this.getCoordinates ().x-range;
		for (int i = 0; i <= range*2; i++) {
			int z = this.getCoordinates ().z-range;
			for (int j = 0; j <= range*2; j++) {
				
				if (inWidthBounds(z) && inHeightBounds(x)) {

					validTiles [i, j] = Instantiate (Resources.Load ("valid", typeof(GameObject)) as GameObject);
					validTiles [i, j].transform.localPosition = this.gameObject.transform.localPosition;

					//Going to need to get the map coords to move the range tile upwards
					validTiles [i, j].transform.Translate (new Vector3 ((i - range) * 0.4f, (map [x, z] - 1) * 0.4f, (j - range) * 0.4f));

					validTiles [i, j].AddComponent<BoxCollider> ();
					validTiles [i, j].GetComponent<BoxCollider> ().size = new Vector3 (0.4f, 0.4f, 0.4f);
					validTiles [i, j].GetComponent<BoxCollider> ().center = new Vector3 (0.0f, 0.4f, 0.0f);

					RangeTile rt = validTiles [i, j].AddComponent<RangeTile> () as RangeTile;
					validTiles [i, j].GetComponent<RangeTile> ().init (i, j, controllerScript, this, x, z, behaviour);
				} else {
				}
				z++;
			}
			x++;
		}
		print ("set validtiles");
		controllerScript.setValidTiles (validTiles);
	}

	public bool inWidthBounds(int x) {
		if (x >= 0 && x < manager.GetComponent<GameManager> ().getMapWidthMAX ()) {
			return true;
		}
		return false;
	}


	public bool inHeightBounds(int x) {
		if (x >= 0 && x < manager.GetComponent<GameManager> ().getMapHeightMAX ()) {
			return true;
		}
		return false;
	}

	public void moveToSpawn(Coordinates destination) {
		
		//make path for unit to follow
		List<Coordinates> spawnpath = new List<Coordinates>();
		Coordinates parent = position;
		int x = position.x;
		int z = position.z;
		print ("position to " + position.x + ", " + position.z);
		print ("destination to " + destination.x + ", " + destination.z);

	

		spawnpath.Add (new Coordinates (x, position.y, z, position.h, position.g, position.f, null));

		while (destination.x != x || destination.z != z) {
			print ("before " + x + ", " + z);
			//Coordinates next = new Coordinates();
			if (destination.x > x) {
				print ("X+");
				parent = new Coordinates (x + 1, position.y, z, position.h, position.g, position.f, parent);
				x++;
				traversalIndex++;
			} else if (destination.x < x) {
				print ("X-");
				parent = new Coordinates (x - 1, position.y, z, position.h, position.g, position.f, parent);
				x--;
				traversalIndex++;
			} else if (destination.z > z) {
				print ("Z+");
				parent = new Coordinates (x, position.y, z + 1, position.h, position.g, position.f, parent);
				z++;
				traversalIndex++;
			} else if (destination.z < z) {
				print ("Z-");
				parent = new Coordinates (x, position.y, z - 1, position.h, position.g, position.f, parent);
				z--;
				traversalIndex++;
			} 
			spawnpath.Add (parent);
			//parent = next;
		}

		if (destination.x == x && destination.z == z) {
//			path = spawnpath;
			//add all parents of nodes to the Path list

			List<Coordinates> path = new List<Coordinates>();
			Coordinates current = parent;

			while (current !=null) {
				path.Add(current);
				current = current.parent;
			}

			//Execute movement on the path
			this.setPath(path);
			//unsetPositiontoObstacleMap (start);
			//setPositiontoObstacleMap(end);
			print ("Spawn Unit Position " + x + ", " + z);

		} else {
			print ("SPAWNING ERROR");
		}

	}

	public void setTargetCoordinate(Coordinates coordinates) {

		if (!moved) {
			print ("traversal index: " + traversalIndex);

			int startingHeuristic = createHeuristic (position.x, position.z, coordinates.x, coordinates.z);

			//print("Starting ASTAR");
			aStar (new Coordinates (position.x, 0, position.z, startingHeuristic, 0, startingHeuristic, null),
				new Coordinates (coordinates.x, (int)coordinates.y, coordinates.z, 0, startingHeuristic, startingHeuristic, null));

			moved = true;
		} 
	}

	public void setPath(List<Coordinates> path) {
		/*foreach (Coordinates p in path) {
			print("Y = " + p.y);
		}*/
		this.path=path;
	}

	public void addUnitOrders(Order[] orders) {
		if (this.order == null) {
			this.order = new List<Order>();
		}
		for (int i = 0; i<orders.Length; i++) {
			this.order.Add (orders[i]);
		}
	}

	public void removeUnitOrders(Order[] orders) {
		foreach (Order o in orders) {
			int i = 0;
			foreach (Order curr in order) {
				if (o == curr)
					order.RemoveAt (i);
				i++;
			}
		}
	}

	//This function calculates the move start and end locations in world space for the unit to move
	public void move(Coordinates start, Coordinates end) {
		Mesh mesh = GameObject.FindGameObjectWithTag ("Tile").gameObject.transform.FindChild ("group_Default").GetComponent<MeshFilter> ().mesh;

		//Get object size and start location so we can use it as a base for where to move from and to
		Bounds bounds = mesh.bounds;
		startpos.transform.position = gameObject.transform.position;

		if(start.x<end.x) {	//x axis increase
			endpos.transform.position = startpos.transform.position + new Vector3(bounds.size.x, 0, 0);
		}
		else if(start.z<end.z) {	//z axis increase
			endpos.transform.position = startpos.transform.position + new Vector3(0, 0, bounds.size.z);
		}
		else if(start.x>end.x) {	//x axis decrease
			endpos.transform.position = startpos.transform.position - new Vector3(bounds.size.x, 0, 0);
		}
		else if(start.z>end.z) {	//z axis decrease
			endpos.transform.position = startpos.transform.position - new Vector3(0, 0, bounds.size.z);
		}

		rot = Quaternion.LookRotation(endpos.transform.position - startpos.transform.position);
		//THinking: Rotation for ramps? Might be awkward and may need to do a different pose for unit
		
		float startplus = start.y*10;
		float endplus = end.y*10;

		float starty = (int)startplus;
		float endy = (int)endplus;

		//start=1, end=2.x - Goes up
		if(			starty==endy+11
			|| 		starty==endy+12
			|| 		starty==endy+13
			|| 		starty==endy+14) {
			endpos.transform.position = endpos.transform.position - new Vector3(0, bounds.size.y/2.8f, 0);
		} //start=2.x, end=2 - Goes up
		else if (  	starty==endy+1
				|| 	starty==endy+2
				|| 	starty==endy+3
				|| 	starty==endy+4) {
			endpos.transform.position = endpos.transform.position + new Vector3(0, bounds.size.y/2.8f, 0);
		} //start=2, end=2.x - Goes down
		else if (	starty==endy-1
				||	starty==endy-2
				||	starty==endy-3
				||	starty==endy-4) {
			endpos.transform.position = endpos.transform.position - new Vector3(0, bounds.size.y/2.8f, 0);
		} //start=2.x, end=1 - Goes down
		else if (	starty==endy-11
				||	starty==endy-12
				||	starty==endy-13
				||	starty==endy-14) {
			endpos.transform.position = endpos.transform.position + new Vector3(0, bounds.size.y/2.8f, 0);
		}

		distance = Vector3.Distance(startpos.transform.position, endpos.transform.position);

	}

	//Create a heuristic for which to base the A* estimates
	//Currently easy as all tiles have a 1 cost, may need to increase costs in future
	public int createHeuristic(int posx, int posz, int desx, int desz) {
		int x = 0;
		if (posx >= desx)
			x = posx-desx;
		else x = desx-posx;
		int z = 0;
		if (posz >= desz)
			z = posz-desz;
		else z = desz-posz;

		return x + z;
	}

	public float[,] getMap() {
		GameObject tempManager = GameObject.Find("GameManager");
		return tempManager.GetComponent<GameManager>().map;
	}

	public float[,] getMapObstacle() {
		GameObject tempManager = GameObject.Find("GameManager");
		return tempManager.GetComponent<GameManager>().mapObstacle;
	}

	public void aStar(Coordinates start, Coordinates end) {

		bool finished=false;

		Coordinates lowestHeuristic = new Coordinates(MAX_POS, MAX_POS, MAX_POS, MAX_POS, MAX_POS, MAX_POS, null);

		List<Coordinates> open = new List<Coordinates>();	//Open list for tiles that may be on the final path, investigate these to continue along potential path
		List<Coordinates> closed = new List<Coordinates>(); //Closed list for tiles that cannot be a part of the final path

		float[,] map = getMap();
		float[,] mapObstacle = getMapObstacle();
		
		start.y = map[start.z, start.x];
		open.Add(start);

		while(!(finished)) {
	
			int i = 0;
			int loopcount = 10;
			if (loopcount!=i) {
				i++;
			} else {
				finished=true;
			}

			//if open holds no more nodes, then we are done and there is no path to goal
			if (open.Count==0) {
				//No more nodes, cannot get to destination error, take no action
				finished=true;
			}
			lowestHeuristic.h = MAX_POS; // reset the heuristic so we have fair judgement
			foreach (Coordinates option in open) {
				//get lowest (valid) value
				if(option.h<lowestHeuristic.h) {
					lowestHeuristic = option;
				}
			}

			closed.Add(lowestHeuristic);

			removeFromOpen(open, lowestHeuristic);

			//if we just added the end node, we are done and have found our goal
			if(lowestHeuristic.x == end.x && lowestHeuristic.z == end.z) {
				//add all parents of nodes to the Path list
				Coordinates current = lowestHeuristic;
				List<Coordinates> path = new List<Coordinates>();

				//
				while (current !=null) {
					path.Add(current);
					current = current.parent;
				}

				//Execute movement on the path
				this.setPath(path);
				finished=true;
				unsetPositiontoObstacleMap (start);
				setPositiontoObstacleMap(end);
				print ("New Unit Position " + end.x + ", " + end.z);
			}

			//Depending on the heuristic, add the tile closest to the path destination
			if(!finished) {
				if (lowestHeuristic.x!=0) {
					addToOpen(open, closed, closed[closed.Count-1], end, map, mapObstacle, new Coordinates(lowestHeuristic.x-1, map[lowestHeuristic.z, lowestHeuristic.x-1], lowestHeuristic.z, 0, 0, 0, null));
				}
				if (lowestHeuristic.x!=map.GetLength(1)-1) {
					addToOpen(open, closed, closed[closed.Count-1], end, map, mapObstacle, new Coordinates(lowestHeuristic.x+1, map[lowestHeuristic.z, lowestHeuristic.x+1], lowestHeuristic.z, 0, 0, 0, null));
				}
				if (lowestHeuristic.z!=0) {
					addToOpen(open, closed, closed[closed.Count-1], end, map, mapObstacle, new Coordinates(lowestHeuristic.x, map[lowestHeuristic.z-1, lowestHeuristic.x], lowestHeuristic.z-1, 0, 0, 0, null));
				}
				if (lowestHeuristic.z!=map.GetLength(0)-1) {
					addToOpen(open, closed, closed[closed.Count-1], end, map, mapObstacle, new Coordinates(lowestHeuristic.x, map[lowestHeuristic.z+1, lowestHeuristic.x], lowestHeuristic.z+1, 0, 0, 0, null));
				}
			}

		}
	}

	//Adds the tile to the list of open nodes
	public void addToOpen(List<Coordinates> open, List<Coordinates> closed, Coordinates parent, Coordinates end, float[,] map, float[,] obstacleMap, Coordinates addition) {

		//Only add the tile if it is not already in the open list and only if it is a valid place for the unit to move
		if (!inList(closed, addition)) {
			if(Level.isValidCoordinate(addition, parent, map, obstacleMap)) {
				addition.g = parent.g+1;
				addition.h = createHeuristic(addition.x, addition.z, end.x, end.z);
				addition.f = addition.g+addition.h;
				addition.parent = new Coordinates(parent);
				if (addition.parent==null) print ("NULL");
				open.Add(addition);
				Debug.DrawLine(new Vector3(parent.x*0.4f, 0.4f, parent.z*0.4f), new Vector3(addition.x*0.4f, 0.4f, addition.z*0.4f), Color.blue, 10000, false);

			} else {
				Debug.DrawLine(new Vector3(parent.x*0.4f, 0.4f, parent.z*0.4f), new Vector3(addition.x*0.4f, 0.4f, addition.z*0.4f), Color.red, 10000, false);

			}
		}
	}

	//Removes the tile from the list of open nodes
	public void removeFromOpen(List<Coordinates> open, Coordinates removal) {
		
		for (int i = 0; i < open.Count; i++) {
			if(	open[i].x == removal.x && open[i].z == removal.z) {
				open.RemoveAt(i);
			}
		}
	}

	//Check to see if the new tile is already in the list
	public bool inList(List<Coordinates> list, Coordinates addition) {
		foreach(Coordinates o in list) {
			if ((o.x == addition.x && o.z == addition.z)) {
				if (o.g > addition.g) {
					o.g = addition.g;
					o.f = addition.f;
					o.parent = addition.parent;
				}
				return true;
				
			}
		}
		return false;
	}
	
	//Display the unit UI when the unit is selected
    public void showUnitUI() {

		if (order.Count != 0) {
			Player playerScript = controller.GetComponent<Player> ();

			//Set new image for interactable button
			Texture2D texture = Resources.Load ("defaultimg.png") as Texture2D;
			Material material = new Material (Shader.Find ("Diffuse"));
			material.mainTexture = texture;

			//Divide a circle by how many orders can be executed (this is for an even distribution on the UI)
			float sectorSize = 360 / order.Count;

			//Create new set of UIPanes
			UIPane = new GameObject[order.Count];
			anchor = new GameObject ();
			anchor.transform.position = this.gameObject.transform.position;
			anchorstart = new GameObject ();
			anchorstart.transform.position = this.gameObject.transform.position;

			for (int i = 0; i < order.Count; i++) {

				//Instantiate a UI Panel that serves as a button
				UIPane [i] = Instantiate (Resources.Load ("UIPane", typeof(GameObject)) as GameObject);
				UnitUI uiScript = UIPane [i].gameObject.GetComponent<UnitUI> ();
				UIPane [i].gameObject.transform.SetParent (anchor.transform);

				//Give the UI Panel the same position as the unit so it fans out from the unit's position
				uiScript.setPosition(new Vector3 ((this.gameObject.transform.position.x), (this.gameObject.transform.position.y), (this.gameObject.transform.position.z)));

				//Rotate the UI Panel to face the player's Camera, using the player's camera to generate an inverse angle
				UIPane [i].transform.localEulerAngles = new Vector3( 
					playerScript.getPlayerCamera ().transform.eulerAngles.x-90,  
					playerScript.getPlayerCamera ().transform.eulerAngles.y,  
					playerScript.getPlayerCamera ().transform.eulerAngles.z);



				//calculate the angles in which the UI panel will move in
				float angle = sectorSize * i;

				uiScript.setOwnership (controller, this.gameObject, playerScript.getPlayerDummy(), anchor, anchorstart);
				uiScript.setOrder (order [i]);
				uiScript.setDestination (angle, (i == order.Count - 1));

			
			}

			playerScript.initPanes (UIPane);

		} else {
			print ("Unit has no order options");
		}
    }

	public void setPositiontoObstacleMap(Coordinates pos) {
		float[,] obstacleMap = this.getMapObstacle ();
		obstacleMap [pos.z, pos.x] = 1;
	}

	public void unsetPositiontoObstacleMap(Coordinates pos) {
		float[,] obstacleMap = this.getMapObstacle ();
		obstacleMap [pos.z, pos.x] = 0;
	}
}

