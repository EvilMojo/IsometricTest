using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceView : MonoBehaviour {

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

		if (inward == null && outward == null) {
			initiateLocations ();
		}

		start = outward.transform;
		end = inward.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.gameObject.transform.position.x == end.position.x) {
			print ("FIN");
			moving = false;
			if (!open) {
				this.gameObject.SetActive (false);
			}
		}
		if (moving) {
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			this.gameObject.transform.position = Vector3.Lerp (start.position, end.position, fracJourney);
			if (open) {
				this.gameObject.GetComponent<CanvasGroup> ().alpha = 0+fracJourney;
			} else {
				this.gameObject.GetComponent<CanvasGroup> ().alpha = 1-fracJourney;
			}

		}
	}

	public void toggle() {

		if (inward == null && outward == null) {
			this.gameObject.SetActive (true);
			initiateLocations ();
		}

		speed = initSpeed;
		startTime = Time.time;
		if (open) {
			start = outward.transform;
			end = inward.transform;
			open = false;
		} else {
			this.gameObject.SetActive (true);
			start = inward.transform;
			end = outward.transform;
			open = true;
		}
		journeyLength = Vector3.Distance (start.position, end.position);
		moving = true;
	}

	public void initiateLocations () {
		open = true;
		moving = false;
		initSpeed = 2000.0f;
		speed = initSpeed;
		inward = new GameObject ("StartScroll");
		outward = new GameObject ("EndScroll");
		inward.transform.position = this.gameObject.transform.position;
		outward.transform.position = new Vector3(this.gameObject.transform.position.x + (float)this.gameObject.GetComponent<RectTransform>().rect.width, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
		this.gameObject.SetActive (false);
	}
}