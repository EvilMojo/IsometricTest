using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparency : MonoBehaviour {

	public bool alpha;
	public float transitionRate;
	public bool transition;

	// Use this for initialization
	void Start () {
		transitionRate = 0.05f;	
		transition = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (transition) {
			if (this.alpha) {
				if (this.gameObject.GetComponent<CanvasGroup> ().alpha < 1)
					this.gameObject.GetComponent<CanvasGroup> ().alpha = this.gameObject.GetComponent<CanvasGroup> ().alpha + transitionRate;
				else {
					this.gameObject.GetComponent<CanvasGroup> ().alpha = 1;
					this.gameObject.GetComponent<CanvasGroup> ().interactable = true;
					this.gameObject.GetComponent<CanvasGroup> ().blocksRaycasts = true;
					transition = false;
				}
			} else {
				if (this.gameObject.GetComponent<CanvasGroup> ().alpha > 0)
					this.gameObject.GetComponent<CanvasGroup> ().alpha = this.gameObject.GetComponent<CanvasGroup> ().alpha - transitionRate;
				else {
					this.gameObject.GetComponent<CanvasGroup> ().alpha = 0;
					this.gameObject.GetComponent<CanvasGroup> ().interactable = false;
					this.gameObject.GetComponent<CanvasGroup> ().blocksRaycasts = false;
					transition = false;
				}
			}

			if (this.gameObject.GetComponent<CanvasGroup> ().alpha == 0) {
			}
		}
	}

	public void transitionAlpha() {
		if (this.gameObject.GetComponent<CanvasGroup>().alpha == 1) {
			this.alpha = false;
		} else {
			this.alpha = true;
		}
		transitionRate = 0.05f;	
		transition = true;
	}
}
