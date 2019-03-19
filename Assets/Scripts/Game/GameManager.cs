using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	public GameObject[] player;			//Array containing play objects for general player data
	public Level level;					//Level script used for level generation
	public float[,] map; 				//contains elevations and block types of the map
	public float[,] mapObstacle;		//contains map of which tiles are non-traversable
	public int mapWidthMAX;				
	public int mapHeightMAX;
	public int playerTurn;				//indicates which player's turn it is

	void Start () {

		mapWidthMAX = 0;
		mapHeightMAX = 0;

		//Set arbitrary number of players for testing purposes.
		//TODO get players from parameter from a menu system
		player = new GameObject[2];


		//Set players from last to first, assign name and colours

		player[1] = new GameObject("Player " + 1);
		//Instantiate(Resources.Load("Player", typeof(GameObject))) as GameObject;playerIndex.ToString() + 
		player[1].AddComponent<AI>();
		player[1].AddComponent<Player>();
		player[1].GetComponent<Player>().createPlayer(1, "Player 2", null, this);
		player[1].GetComponent<Player>().setTeamColor (Color.red);

		player[0] = new GameObject("Player " + 0);
		//Instantiate(Resources.Load("Player", typeof(GameObject))) as GameObject;
		player[0].AddComponent<Player>();
		player[0].GetComponent<Player>().createPlayer(0, "Player 1", null, this);
		player[0].GetComponent<Player>().setTeamColor (Color.blue);

		//Instantiate level and generate from file
		//TODO create level generation algorithms that ideally provide interesting, balanced and possible gameplay maps and 
		//place players in suitable, near-equidistant locations
		level = new Level();
		level.setManager (this.gameObject);
		map = level.generateMap();
		mapObstacle = level.generateTerrain(map);
		level.setStartPositions (player);

		//Activate players by creating objects needed to begin play
		foreach (GameObject p in player) {
			Player pscript = p.GetComponent<Player> ();
			pscript.initialise ();
		}

		//Assign which player goes first
		playerTurn = 0;
	}

	/* When the current player ends their turn, ensure that the next player is not out of 
	 * max player range.
	 * Ensure that non-AI players have their actions refreshed
	 * Ensure that AI carries out its actions before passing turn to the next player
	*/
	public void endTurn (int playerIndex) {
		int players = player.Length;
		if (playerIndex == playerTurn) {
			
			if (playerTurn < players - 1) {
				playerTurn++;
			} else if (playerTurn==players-1) {
				playerTurn=0;
			}
			if (player [playerTurn].GetComponent<AI> () == null) {
				player [playerTurn].GetComponent<Player> ().startTurn ();
			} else {
				player [playerTurn].GetComponent<AI> ().runAI ();
			}

		} else {
			print ("Not player " + playerIndex + "'s turn");
		}
	}

	public int getPlayerTurn() {
		return playerTurn;
	}

	public GameObject getPlayer(int x) {
		return player[x];
	}

	public float[,] getMap() {
		return map;
	}

	public float[,] getMapObstacle() {
		return mapObstacle;
	}

	public void setMapObstacle(float[,] newObstacles) {
		for (int i = 0; i < mapObstacle.GetLength (0); i++) {
			for (int j = 0; j < mapObstacle.GetLength (1); j++) {
				mapObstacle [i, j] = newObstacles [i, j];
			}
		}
	}

	public void setMapWidthMAX(int width) {
		mapWidthMAX = width;
	}

	public void setMapHeightMAX(int height) {
		mapHeightMAX = height;
	}

	public int getMapWidthMAX() {
		return mapWidthMAX;
	}

	public int getMapHeightMAX() {
		return mapHeightMAX;
	}
}
