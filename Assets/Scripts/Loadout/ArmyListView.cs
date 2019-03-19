using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyListView : MonoBehaviour {

	public bool open;
	public GameObject inward;
	public GameObject outward;
	public Transform start;
	public Transform end;
	public float maxRatio = 1.8f;
	public bool moving;

	public float speed;
	public float initSpeed;
	private float startTime;
	private float journeyLength;

	void Start() {

		open = true;
		moving = false;
		inward = new GameObject ("StartScroll");
		outward = new GameObject ("EndScroll");
		initSpeed = 1000.0f;
		speed = initSpeed;

		inward.transform.position = this.gameObject.transform.position;
		outward.transform.position = new Vector3(this.gameObject.transform.position.x, (float)(Screen.height-(Screen.height/maxRatio)), this.gameObject.transform.position.z);

		start = outward.transform;
		end = inward.transform;
	}
	//(float)(Screen.height+(Screen.height/10))
	void FixedUpdate() {
		if (this.gameObject.transform.position.y == end.position.y) {
			print ("FIN");
			moving = false;
		}
		if (moving) {
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			this.gameObject.transform.position = Vector3.Lerp (start.position, end.position, fracJourney);
			speed = speed -5;
		}
	}

	// Use this for initialization
	public void toggle() {

		speed = initSpeed;
		startTime = Time.time;
		if (open) {
			start = outward.transform;
			end = inward.transform;
			open = false;
		} else {
			start = inward.transform;
			end = outward.transform;
			open = true;
		}
		journeyLength = Vector3.Distance (start.position, end.position);
		moving = true;
	}
}
