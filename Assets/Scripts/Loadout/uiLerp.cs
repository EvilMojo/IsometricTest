using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiLerp : MonoBehaviour {

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

	// Use this for initialization
	void Start () {

		open = true;
		moving = false;
		inward = new GameObject ("StartScroll");
		outward = new GameObject ("EndScroll");
		initSpeed = 1000.0f;
		speed = initSpeed;

		inward.transform.position = this.gameObject.transform.position;

		//This is the only line that changes across Lerp objects
		outward.transform.position = new Vector3(this.gameObject.transform.position.x, (float)(Screen.height-(Screen.height/maxRatio)), this.gameObject.transform.position.z);

		start = outward.transform;
		end = inward.transform;
	}

	// Update is called once per frame
	void Update () {

	}

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
