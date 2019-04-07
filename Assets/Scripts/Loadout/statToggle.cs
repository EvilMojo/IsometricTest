using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statToggle : MonoBehaviour {

	public void toggle() {
		print (this.gameObject.name);
		if (this.gameObject.GetComponent<CanvasGroup> ().alpha == 1) {
			this.gameObject.GetComponent<CanvasGroup> ().alpha = 0;
		} else {
			this.gameObject.GetComponent<CanvasGroup> ().alpha = 1;
		}
	}

}
