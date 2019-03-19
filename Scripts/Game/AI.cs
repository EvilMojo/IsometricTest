using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

	Player playerScript;

	void Start () {
		playerScript = this.gameObject.GetComponent<Player> ();
	}

	//Run the AI, at each turn the AI should do the following
	/*
	 *  Step 1: Analyse what resources are available to it
	 *  Step 2: Determine long term goals. May include analysis of current state along with gaps to its desired state
	 * 	Step 3: Determine actions to take to achieve the above. (Explore? Create? Attack?)
	 * 	Step 4: Action Step 3 on each unit
	 *  Step 5: (Maybe?) Analyse if progressing towards desired state
	 * 
	 */
	public void runAI () {

		int HQs = 0;
		int Rovers = 0;

		//Check what was in previous list of units from PlayerScript and ensure they are deleted
		List<GameObject> preAIUnitList = new List<GameObject>();

		foreach (GameObject u in playerScript.unitList) {
			preAIUnitList.Add (u);
		}

		HQs=0;
		Rovers = 0;

		List<GameObject> toRemove = new List<GameObject> ();

		foreach (GameObject u in preAIUnitList) {
			if (u == null) {
				toRemove.Add (u);
			}
		}

		//Clear preAIUnitList of any null objects
		foreach (GameObject u in toRemove) {
			preAIUnitList.Remove (u);
		}

		//Take a new, clean check of current state
		foreach (GameObject u in preAIUnitList) {
			if (u != null) {
				if (u.GetComponent<Unit> ().unitName.Contains ("HQ")) {
					HQs++;
				}
				if (u.GetComponent<Unit> ().unitName.Contains ("Rover")) {
					Rovers++;
				}
			} else {
				print ("Removal Error");
			}
		}

		//Determine turn agenda (Depends on unit types in possession, resources, current state)

		//Move Units
		foreach (GameObject u in preAIUnitList) {
			if (u.GetComponent<Unit>().unitName.Contains("Rover")) {
				print ("Moving Rover");
				Unit s = u.GetComponent<Unit> ();
				u.GetComponent<Unit>().setTargetCoordinate(new Coordinates (s.getCoordinates().x-1, s.getCoordinates().y, s.getCoordinates().z-1, 0, 0, 0, null));
			}
		}

		//Delegate attacks

		//Build and Train
		//Prototype: Only creates 1 unit
		foreach (GameObject u in preAIUnitList) {
			if (u.GetComponent<Unit>().unitName.Contains("HQ")) {
				if (playerScript.unitList.Count <= 1) {
					playerScript.createUnit (u.gameObject.transform.position, Player.UnitType.WARRIOR, u.GetComponent<Unit> ().getCoordinates (), u.GetComponent<Unit> ().spawnpoint);
				}
			}
		}

		foreach (GameObject u in preAIUnitList) {
			playerScript.unitList.Add (u);
		}

		//Pass turn
		playerScript.manager.GetComponent<GameManager>().endTurn(playerScript.playerIndex);
	}

}
