using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUI : MonoBehaviour {

	private GameObject controller, unit;						//Unit and player this UI belongs to
	private GameObject startpos, endpos, anchor, anchorstart;	//Locations that are used for UI positioning
	private GameObject playerDummy;								//Used for setting rotation of the UI elements
	private float startTime;									//Used for LERP animation
	private float distance = 1.5f;								//Dictates how far UI Panels travel
	private float x;											//Used for angle calculation
	private float y;											//Used for angle calculation
	private float speed = 10f;									//How fast UI Panels travel
	private bool destinationSet = false;						//Stops and starts UIPanel movement
	private int index=0, total=0;								//Counts for this UIPanel count vs total UI Panels
	private bool selfdestruct;									//Indicates when UI Panels need to be destroyed
	private Order order;										//The order that this UI Panel executes

	void Awake () {

		this.startpos = new GameObject ("UnitStart");
		this.endpos = new GameObject ("UnitEnd");

		//Move start and end positions to the gameobject so they originate from the same place
		startpos.transform.position = gameObject.transform.position;
		startpos.transform.localEulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
		endpos.transform.position = startpos.transform.position;
		endpos.transform.localEulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
	
	}

	void Update () {

		anchor.transform.rotation = Quaternion.Euler (0, 0, 0) * playerDummy.transform.localRotation;

		if (destinationSet) {

			//Basic LERP properties
			speed=speed +.7f;
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / distance;

			//Gradually move the UI panel at an increasing speed towards its destination
			gameObject.transform.position = Vector3.Lerp (startpos.transform.position, endpos.transform.position, fracJourney);

			//Unset destination as we are no longer moving UI elements
			if (endpos.transform.position == gameObject.transform.position) {
				destinationSet = false;
			}

			//This is for the reverse fan, where UI goes back in. Destroy everything
			if (endpos.transform.position == gameObject.transform.position && selfdestruct) {
				Destroy (endpos);
				Destroy (anchor);
				Destroy (this.gameObject);
			}
		}

	}

    private void OnMouseDown() {
    }

    private void OnMouseUp() {

        //execute UI Panel action based on action type
		Player playerScript = controller.GetComponent<Player> ();
		playerScript.closeUI ();
		order.execute(controller, unit);

    }


	public void setOrder(Order order) {
		this.order = order;
	}

	public void setOwnership(GameObject controller, GameObject unit, GameObject playerDummy, GameObject anchor, GameObject anchorstart) {
		this.controller = controller;
		this.unit = unit;
		this.playerDummy = playerDummy;
		this.anchor = anchor;
		this.anchorstart = anchorstart;
		this.anchorstart.name = "anchor-start";
		print ("anchored");
	}

	public void setDestination(float angle, bool final) {
		//set position along a line determined from angle (mx+y?)
		//Trigonometry required, sin and cos required for x and y, alternating between opposite and adjacent to angle

		startTime = Time.time;

		this.x = Mathf.Sin (angle*Mathf.PI/180);
		this.y = Mathf.Cos (angle*Mathf.PI/180);

		this.endpos.transform.Translate (new Vector3 (x, 5f, y), gameObject.transform);
		this.distance = Vector3.Distance (gameObject.transform.position, endpos.transform.position);
		this.destinationSet = true;
	}

	public void setIndexes(int index, int total) {
		this.index = index;
		this.total = total;
	}

	//Set position of all elements for UI Panels in the same place so they can be changed relative to each other
	public void setPosition(Vector3 position) {
		gameObject.transform.position = new Vector3 (position.x, position.y, position.z);
		startpos.transform.position = new Vector3 (position.x, position.y, position.z);
		endpos.transform.position = new Vector3 (position.x, position.y, position.z);
	}

	public int getIndex() {
		return total;
	}
		
	//When the UI Panel needs to go back to its original point, swap the start and end points
	//When the UI Panels are closed, they will retreat back inwards
	public void reversal() {
		endpos.transform.position = startpos.transform.position;
		startpos.transform.position = gameObject.transform.position;
		Destroy (anchorstart);
		startTime = Time.time;
		destinationSet = true;
		selfdestruct = true;
	}
}