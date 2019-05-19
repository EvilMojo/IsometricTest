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
	public float rescaleRate;
	public float rescaleBase;
	public float xrate, yrate;
	public bool moving;
	public bool resizingx, resizingy;

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
	void FixedUpdate () {
		if (end != null) {
			if (sameSize ()) {
				
				if (this.gameObject.transform.position == end.position) {
					moving = false;
					if (resizingx && resizingy) {
						reset();
					}
					if (!open) {
						//this.gameObject.SetActive (false);
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
				//float scaleCovered = (Time.time - startTime) * rescaleRate;
				//float fracJourney = scaleCovered / journeyLength;
				//this.gameObject.transform.position = Vector3.Lerp (start.localScale, end.localScale, fracJourney);


				if (this.gameObject.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta.x > outward.GetComponent<RectTransform> ().sizeDelta.x) {
					this.gameObject.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta = new Vector2(this.gameObject.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta.x - (xrate * rescaleRate), this.gameObject.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta.y);
				} 
				if (this.gameObject.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta.y > outward.GetComponent<RectTransform> ().sizeDelta.y) {
					this.gameObject.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta = new Vector2(this.gameObject.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta.x, this.gameObject.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta.y - (yrate * rescaleRate));
				} 

				if (this.gameObject.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta.x < outward.GetComponent<RectTransform> ().sizeDelta.x) {
					this.gameObject.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta = new Vector2 (outward.GetComponent<RectTransform> ().sizeDelta.x, this.gameObject.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta.y);
					resizingx = true;
				}
				if (this.gameObject.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta.y < outward.GetComponent<RectTransform> ().sizeDelta.y) {
					this.gameObject.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta = new Vector2 (this.gameObject.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta.x, outward.GetComponent<RectTransform> ().sizeDelta.y);
					resizingy = true;
				}
	
				rescaleRate -= rescaleRate * 0.12f;

				float distCovered = (Time.time - startTime) * speed;
				float fracJourney = distCovered / journeyLength;
				this.gameObject.transform.position = Vector3.Lerp (start.position, end.position, fracJourney);

				if (resizingx && resizingy && this.gameObject.transform.position.x == end.position.x) {
					this.gameObject.GetComponent<UnitView> ().toggleAfterSize ();

				}
				//this.gameObject.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta = new Vector2(outward.GetComponent<RectTransform> ().sizeDelta.x, outward.GetComponent<RectTransform> ().sizeDelta.y);

				//this.gameObject.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta = new Vector2 (outward.GetComponent<RectTransform> ().rect.width, outward.GetComponent<RectTransform> ().rect.height);
				//Debug.Log (this.gameObject.GetComponent<RectTransform> ().sizeDelta);
				//Debug.Log (outward.GetComponent<RectTransform> ().sizeDelta);
			}
		}
	}

	public void toggle() {

		if (inward == null && outward == null) {
			this.gameObject.SetActive (true);
			print ("Is this doing?");
			initiateLocations ();
		}

		if (open) {

			if (this.gameObject.GetComponent <CanvasGroup> () != null) {
				this.gameObject.GetComponent<CanvasGroup> ().blocksRaycasts = false;
				this.gameObject.GetComponent<CanvasGroup> ().interactable = false;
			}
			start = outward.transform;
			end = inward.transform;
			open = false;
		} else {

			if (this.gameObject.GetComponent <CanvasGroup> () != null) {
				this.gameObject.GetComponent<CanvasGroup> ().blocksRaycasts = true;
				this.gameObject.GetComponent<CanvasGroup> ().interactable = true;
			}
			this.gameObject.SetActive (true);
			start = inward.transform;
			end = outward.transform;
			open = true;
		}

		speed = initSpeed;
		startTime = Time.time;
		journeyLength = Vector3.Distance (start.position, end.position);
		moving = true;
	}

	public bool sameSize() {

		/*if (this.gameObject.transform.localScale.x == outward.transform.localScale.x
			&& this.gameObject.transform.localScale.y == outward.transform.localScale.y
			&& this.gameObject.transform.localScale.z == outward.transform.localScale.z) {
			return true;
		}*/
		if (this.gameObject.transform.GetChild (0).GetComponent<RectTransform> () != null && outward.GetComponent<RectTransform> () != null) {
			if (this.gameObject.transform.GetChild (0).GetComponent<RectTransform> ().rect.width == outward.GetComponent<RectTransform> ().rect.width
			    && this.gameObject.transform.GetChild (0).GetComponent<RectTransform> ().rect.height == outward.GetComponent<RectTransform> ().rect.height) {
				return true;
			}
		} else {
			return true;
		}
		return false;
	}

	public virtual void initiateLocations () {
		Debug.Log ("Using base View class is forbidden");
	}

	public virtual void reset() {
		Debug.Log ("Using base View class is forbidden");
	}
}