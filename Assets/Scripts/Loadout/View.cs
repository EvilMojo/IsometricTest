using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour {
	
	public bool open;
	public GameObject inward;
	public GameObject outward;
	public Transform start;
	public Transform end;
	public float maxRatio = 1.8f;
	public float rescaleRate = 1.0f;
	public float xdiff, ydiff;
	public bool moving;

	public float speed;
	public float initSpeed;
	protected float startTime;
	protected float journeyLength;

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
		if (end != null) {
			if (sameSize ()) {
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
						this.gameObject.GetComponent<CanvasGroup> ().alpha = 0 + fracJourney;
					} else {
						this.gameObject.GetComponent<CanvasGroup> ().alpha = 1 - fracJourney;
					}		
				}
			} else { //Not same size
				float scaleCovered = (Time.time - startTime) * rescaleRate;
				float fracJourney = scaleCovered / journeyLength;
				this.gameObject.transform.position = Vector3.Lerp (start.localScale, end.localScale, fracJourney);
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

	public virtual void initiateLocations () {
		Debug.Log ("Invalid View Type");
	}

	public bool sameSize() {

		if (this.gameObject.transform.localScale.x == outward.transform.localScale.x
			&& this.gameObject.transform.localScale.y == outward.transform.localScale.y
			&& this.gameObject.transform.localScale.z == outward.transform.localScale.z) {
			return true;
		}

		/*if (this.gameObject.GetComponent<RectTransform> ().rect.width == outward.GetComponent<RectTransform> ().rect.width
		&& this.gameObject.GetComponent<RectTransform> ().rect.height == outward.GetComponent<RectTransform> ().rect.height) {
			return true;
		}*/
		Debug.Log ("Not same size");
		return false;
	}

	public void getSizeDifference() {

		//Start - goal, take away from difference at each Update
		//float xdiff = this.gameObject.GetComponent<RectTransform> ().rect.width - outward.GetComponent<RectTransform> ().rect.width;
		//float ydiff = this.gameObject.GetComponent<RectTransform> ().rect.height - outward.GetComponent<RectTransform> ().rect.height;
		//startTime = Time.time;
		//start = inward.transform;
		//end = outward.transform;
	}
}