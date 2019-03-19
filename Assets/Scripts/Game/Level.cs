using UnityEngine;
using System.Collections;
using System.IO;
using System;
using Random = UnityEngine.Random;

public class Level : ScriptableObject {

	private GameObject manager;		//Reference to the main Game Manager that will be using the generated level
	private string[] map;			//The map as read by the file containing the map data
	private float[,] mapObstacle;	//Indicates which tiles cannot be traversed to (when otherwise fine)
	private float[] occupied;		//
	private int rows;				//
	private int columns;			//
	private float[,] floatMap;		//

	public void setManager(GameObject manager) {
		this.manager = manager;
	}

	//Generates the playing field from text file
	public float[,] generateMap() {

		GameObject tile;

		string file = Application.dataPath + "/Resources/Text/text.txt";
		string tileType = "";
		float y = 0, r = 0;				//Used to calculate each map tile's height and rotation (if applicable)

		rows = getMapRows(file); 		//Count of total lines in the file, required to know the map size in advance to dynamically allocate map array
		map = new string[rows];			//Initialise map with total rows from file
		map = readMapFile(file, rows);	//Read the map file 

		//For each row, split the individual columns up by whitespace
		//Tiles along a row must always be separated by whitespace
		for (int i = 0; i<map.Length; i++) {
			string[] row = map[i].Split(' ');

			//For each column in the map...
			for (int j = 0; j<row.Length; j++) {
				//0 is treated as a null elevation (water, empty space, etc). Skip generation if it is 0
				if (!("0".Equals(row[j]))) {

					//get elevation from non-float tile
					float elevation = float.Parse(row[j]);
					y = (int)Math.Floor(elevation);

					//If the tile has a decimal place that is not 0, it is a ramp with a fixed rotation outlined by the value of the decimal place. 
					//Create a Ramp and set rotation angle
					if(elevation/1!=y) {
						tile = Instantiate(Resources.Load("ramp", typeof(GameObject)), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
						tileType = "ramp";
						r = elevation%1;
					
					//If tile has a decimal place of 0, it is a regular square tile and requires no rotation
					} else {
						tile = Instantiate(Resources.Load("floor", typeof(GameObject)), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
						r=0;
						tileType = "floor";
					
					}

					//Change the tile materials to a texture dependent on tileset
					//TODO: Learn how to wrap 2D textures/images to a 3D object. UV Wrapping possible?
					Mesh mesh = tile.transform.FindChild("group_Default").GetComponent<MeshFilter>().mesh;
					tile.transform.FindChild ("group_Default").GetComponent<MeshRenderer> ().materials [0].mainTexture = Resources.Load(("shaderexample"), typeof (Texture2D)) as Texture;

					//Relocate map tile to its respective place on the board
					Bounds bounds = mesh.bounds;
					tile.transform.Translate(i*bounds.size.x, y*bounds.size.y, j*bounds.size.z);

					//If the tile has an elevation greater than 1, we need to iterate towards 0 and create a regular block for each elevation
					//Thought: Could do multiple levels on a single row/column space. Might be tricky. Multiple files? 3D array?
					//Thought: Floating blocks?
					for(int k = (int)y-1; k>0; k--) {
						GameObject temptile = Instantiate(Resources.Load("floor", typeof(GameObject))) as GameObject;
						temptile.transform.position=tile.transform.position;
						temptile.transform.Translate(0, -bounds.size.y*k, 0);
						
					}

					//0 - no rotation
					//.1 - NW up to SE
					//.2 - SW up to NE
					//.3 - SE up to NW
					//.4 - NE up to SW

					//Rotate the tile so the ramp faces the right way
					tile.transform.Rotate(new Vector3(0,1,0), 90*(r*10), Space.World);

					//Initialise tile with location and type data
					//Thought: Could be used to characterise tile behaviour
					Tile tileScript = (Tile) tile.GetComponent(typeof(Tile));
					tileScript.setTile(i,(int)y,j, tileType);

					tile.tag = "Tile";

				}
			}
		}

		//Debug map to show valid lines, not compatible with new isValidCoordinate with obstaclemap
		floatMap = dimensionalise(map, rows);
	
		columns = floatMap.Length/rows;

		manager.GetComponent<GameManager> ().setMapWidthMAX (rows);
		manager.GetComponent<GameManager> ().setMapHeightMAX (columns);

		return floatMap;
	}

	//Basic algorithm used to generate obstacles and terrain on the map
	//Thought: Could use further refinement, may affect map balancing and pathing (could be a feature?)
	public float[,] generateTerrain(float[,] map) {

		GameObject tile;

		int divider = Random.Range (map.Length / 4, map.Length / 8);
		int obstaclestarts = map.Length / divider;

		//Create and destroy a tile for the purpose of obtaining tile size
		//Thought: Inefficient, could be done better
		tile = Instantiate(Resources.Load("floor", typeof(GameObject)), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

		Mesh mesh = tile.transform.FindChild("group_Default").GetComponent<MeshFilter>().mesh;
		Bounds bounds = mesh.bounds;

		float x = bounds.size.x;
		float y = bounds.size.y;
		float z = bounds.size.z;

		Destroy (tile);

		//Initialise obstacle map based off original map size
		this.mapObstacle = new float[map.GetLength (0), map.GetLength (1)];

		//Set base mapObstacle. Initialise all tiles to be clear
		for (int r = 0; r < map.GetLength (0); r++) {
			for (int c = 0; c < map.GetLength (1); c++) {
				mapObstacle [r, c] = 0;
			}
		}

		//Fill mapObstacle with some random obstacle roots. These roots will be used to create dynamic, snaking terrain/obstacles
		for (int i = 0; i < obstaclestarts; i++) {
			mapObstacle[Random.Range(0, mapObstacle.GetLength(0)), Random.Range(0, mapObstacle.GetLength(1))] = 1;
		}

		//branch the roots out in a snake-like fashion
		for (int r = 0; r < mapObstacle.GetLength(0); r++) {
			for (int c = 0; c < mapObstacle.GetLength(1); c++) {

				//If the root of the obstacle snake starts on a ramp, ignore it
				//Otherwise, 
				if (mapObstacle [r,c] == 1) {
					if (map [r, c] % 1 == 0) {
						//print ("Snake Start:");
						int currr = r, currc = c;
						for (int snake = Random.Range (0, (mapObstacle.GetLength (0) + mapObstacle.GetLength (1)) / 5); snake > 0; snake--) {

							//print ("Snaking | Snake = " + snake + " at " + (currr) + ", " + (currc));
							int xrand = 0, zrand = 0;
							int xlowerlimit = 0, xupperlimit = 0, zlowerlimit = 0, zupperlimit = 0;

							//Check the current location of the snake's "head" and determine if it is a valid tile to place an obstacle on
							//Invalid tile examples are out of bounds areas or areas with an obstacle already on it!
							//TODO: Need to get player start positions into here
							if ((currr >= 0 && currr < mapObstacle.GetLength (0))
								&& (currc >= 0 && currc < mapObstacle.GetLength (1))) {
								if ((mapObstacle [currr, currc] == 0) || (mapObstacle [currr, currc] == 1)) {

									//Spawn an obstacle object and locate accordingly
									tile = Instantiate (Resources.Load ("obstacle", typeof(GameObject)), new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
									tile.transform.Translate ((currc) * x, (int)map [currr, currc] * y + y, (currr) * z);
									mapObstacle [currr, currc] = 2;	

								} else {
									
									mapObstacle [currr, currc] = 2;
									//don't reduce the snake if tile already has blockages
									snake++;
								}

								//If the obstacle snake still has tiles left to create obstacles on, turn it in a random direction
								if (snake >= 0) {

									int randir = Random.Range (0, 3);

									switch (randir) {
									case 0:
										currr--;
										break;
									case 1:
										currr++;
										break;
									case 2:
										currc--;
										break;
									case 3:
										currc++;
										break;
									default:
										break;
									}
								}
							} 
						}
					}
				}
			}
		}



		return mapObstacle;
	}
		
	public string[] readMapFile(string file, int rows) {
		int i = 0;
		StreamReader input = new StreamReader(file);

		string[] map = new string[rows];

		while(!input.EndOfStream) {
			string line = input.ReadLine();
			map[i] = line;
			i++;
		}

		return map;
	}

	public int getMapRows(string file) {
		return File.ReadAllLines(file).Length;
	}

	//Convert the string map from file into a float 2D array for use with other program elements
	public float[,] dimensionalise(string[] map, int rows) {
		
		string[] size = map[0].Split(' ');
		int columns = size.Length;

		float[,] floatMap = new float[columns, rows];

		for (int i = 0; i<rows; i++) {
			string[] rowItems = map[i].Split(' ');
			for (int j = 0; j<columns; j++) {

				floatMap[j, i] = Convert.ToSingle(rowItems[j]);

			}
		}

		return floatMap;
	}

	//Set the starting position where a player's first building will spawn
	public void setStartPositions(GameObject[] player) {
		foreach (GameObject p in player) {
			int ranx, ranz;
			do {
				//Have x tile buffer so HQ's don't appear half off map
				//Maybe make this 1/8 Map size so player's don't get boxed in or placed too close to map's edge!
				ranx = Random.Range (1, floatMap.GetLength (0) - 2);
				ranz = Random.Range (1, (floatMap.GetLength(1) - 2));

			} while (floatMap [ranz, ranx]%1!=0);

			Player pscript = p.GetComponent<Player> ();
			pscript.setStartPosition(new Coordinates(ranz, floatMap [ranz, ranx], ranx, 0, 0, 0, null), new Vector3(0,0.6f,0));
		}
	}

	//Used to test if a coordinate is a valid destination for traversing using the A* Algorithm
	public static bool isValidCoordinate(Coordinates addition, Coordinates parent, float[,] map, float[,] mapObstacle) {
		//Remember: Z goes across columns, X goes down rows
		//Need to ensure that movement is valid before adding to list of possible nodes

		float ramp = (map[parent.z, parent.x]-(int)map[parent.z, parent.x]);

		//A bit of choppy maths to manipulate variables into a state where they are easily comparable
		float add = map[addition.z, addition.x];
		float rent = map[parent.z, parent.x];

		float incramp = ramp*10;
		float incadd = add*10;
		float incrent = rent*10;

		int intramp = (int)(incramp);
		int intadd = (int)(incadd);
		int intrent = (int)(incrent);


		//Moving to tile with blocker is invalid
		if (mapObstacle [addition.z, addition.x] == 1) {
			return false;
		}

		if (mapObstacle [addition.z, addition.x] == 2) {
			return false;
		}

		//Moving from elevation x to 0 is invalid
		if(map[addition.z, addition.x] == 0) {
		//	print(parent.z + ", " + parent.x + " to " + addition.z + "," + addition.x + " Failed due to 0 floor");
			return false;
		}
	
		//Moving from elevation x to x is valid
		if(intadd == intrent) {
			//not on a ramp, moving along a flat
			if(incramp==0) {
				return true;
			}
			//We can move along same-slope ramps that are adjacent
			if(intadd==22) {
				if(addition.z == parent.z) {
					return true;
				} else {
				//	print(parent.z + ", " + parent.x + " to " + addition.z + "," + addition.x + "Failed due to ramp 2/4 and z not equal");
					return false;
				}
			}
			if(intadd==24) {
				if(addition.z == parent.z) {
					return true;
				} else {
				//	print(parent.z + ", " + parent.x + " to " + addition.z + "," + addition.x + "Failed due to ramp 2/4 and z not equal");
					return false;
				}
			}
			if(intadd==21) {
				if(addition.x == parent.x) {
					return true;
				} else {
				//	print(parent.z + ", " + parent.x + " to " + addition.z + "," + addition.x + "Failed due to ramp 1/3 and x not equal");
					return false;
				}
			}
			if(intadd==23) {
				if(addition.x == parent.x) {
					return true;
				} else {
				//	print(parent.z + ", " + parent.x + " to " + addition.z + "," + addition.x + "Failed due to ramp 1/3 and x not equal");
					return false;
				}
			}

		}

		//Moving from elevation x to x+1 is invalid
		if(intadd == intrent+10) {
			//print(parent.z + ", " + parent.x + " to " + addition.z + "," + addition.x + "Invalid due to elevation");
			return false;
		}

		if(intadd == intrent-10) {
			//print(parent.z + ", " + parent.x + " to " + addition.z + "," + addition.x + "Invalid due to elevation");
			return false;
		}
//-----------------------------------------------------------------------------------------------------------------------------------

		//This is for moving Up the Ramp, Down the Ramp will require opposite
		//	Moving in z+	(right)		required for ramp x.2
		//	Moving in z-	(left)			required for ramp x.4
		//	Moving in x+	(down)		required for ramp x.3
		//	Moving in x-	(up)			required for ramp x.1

		//Moving from elevation x to x+1.x but less than x+2 is valid (ramp, only valid at certain angles) 
		if(	(intadd == intrent+12) || (intadd == intrent-2)) {	//Up and down from/to bottom of ramp
			if(addition.z == parent.z+1) {
				return true;
			}
			else {
			//	print(parent.z + ", " + parent.x + " to " + addition.z + "," + addition.x + "Failed due to 1.2 1.4");
				return false;
			}
		}
		
		if ((intadd == intrent+2) || (intadd == intrent-12 )) {	//Up and down from/to top of ramp	
			if(addition.z == parent.z-1) {
				return true;
			}
			else {
			//	print(parent.z + ", " + parent.x + " to " + addition.z + "," + addition.x + "Failed due to 1.2 1.4");
				return false;
			} 
		}

		//Moving onto ramp from high elevation
		if(	(intadd == intrent+14) || (intadd == intrent-4)) {	//Up and down from/to bottom of ramp
			if(addition.z == parent.z-1) {
				return true;
			}
			else {
				//print(parent.z + ", " + parent.x + " to " + addition.z + "," + addition.x + "Failed due to 1.2 1.4");
				return false;
			}
		}
		
		if ((intadd == intrent+4) || (intadd == intrent-14 )) {	//Up and down from/to top of ramp	
			if(addition.z == parent.z+1) {
				return true;
			}
			else {
			//	print(parent.z + ", " + parent.x + " to " + addition.z + "," + addition.x + "Failed due to 1.2 1.4");
				return false;
			} 
		}

		if(	(intadd == intrent+11) || (intadd == intrent-1)) {	//Up and down from/to bottom of ramp
			if(addition.x == parent.x-1) {
				return true;
			}
			else {
			//	print(parent.z + ", " + parent.x + " to " + addition.z + "," + addition.x + "Failed due to 1.2 1.4");
				return false;
			}
		}
		
		if ((intadd == intrent+1) || (intadd == intrent-11 )) {	//Up and down from/to top of ramp	
			if(addition.x == parent.x+1) {
				return true;
			}
			else {
				//print(parent.z + ", " + parent.x + " to " + addition.z + "," + addition.x + "Failed due to 1.2 1.4");
				return false;
			} 
		}

		//Moving onto ramp from high elevation
		if(	(intadd == intrent+13) || (intadd == intrent-3)) {	//Up and down from/to bottom of ramp
			if(addition.x == parent.x+1) {
				return true;
			}
			else {
			//	print(parent.z + ", " + parent.x + " to " + addition.z + "," + addition.x + "Failed due to 1.2 1.4");
				return false;
			}
		}
		
		if ((intadd == intrent+3) || (intadd == intrent-13 )) {	//Up and down from/to top of ramp	
			if(addition.x == parent.x-1) {
				return true;
			}
			else {
			//	print(parent.z + ", " + parent.x + " to " + addition.z + "," + addition.x + "Failed due to 1.2 1.4");
				return false;
			} 
		}
		//No acceptable case found, assume not traversible
		//print(parent.z + ", " + parent.x + " to " + addition.z + "," + addition.x + "Invalid due to default");
		return false;
	}

}
