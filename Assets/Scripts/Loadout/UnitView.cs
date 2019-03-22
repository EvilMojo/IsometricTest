using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitView : View {

	public override void initiateLocations () {
		open = true;
		moving = false;
		initSpeed = 2000.0f;
		xdiff = 0; ydiff = 0;
		speed = initSpeed;
		inward = new GameObject ("StartScroll");
		outward = new GameObject ("EndScroll");
		inward.transform.position = this.gameObject.transform.position;
		outward.transform.position = new Vector3((Screen.width/2)/maxRatio, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
		this.gameObject.SetActive (false);
		//this.gameObject.transform.position.x + (float)this.gameObject.GetComponent<RectTransform>().rect.width
	}

	public void dismiss() {
		inward.transform.position = this.gameObject.transform.position;
		outward.transform.position = new Vector3((Screen.width), this.gameObject.transform.position.y, this.gameObject.transform.position.z);
	}
	public void enlist() {
		inward.transform.position = this.gameObject.transform.position;
		outward.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
		outward.transform.localScale = new Vector3 (outward.transform.localScale.x/3, outward.transform.localScale.y/3, outward.transform.localScale.z/3);

		startTime = Time.time;
		//outward.GetComponent<RectTransform> ().rect.width = 50.0f;
		//outward.GetComponent<RectTransform> ().rect.height = 50.0f;
	}
}