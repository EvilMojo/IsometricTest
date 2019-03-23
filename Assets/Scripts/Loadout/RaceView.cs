using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceView : View {

	public override void initiateLocations () {
		open = false;
		moving = false;
		initSpeed = 2000.0f;
		speed = initSpeed;
		inward = new GameObject ("StartScroll");
		outward = new GameObject ("EndScroll");
		inward.transform.position = this.gameObject.transform.position;
		outward.transform.position = new Vector3((Screen.width)/maxRatio, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
		//this.gameObject.SetActive (false);
		//this.gameObject.transform.position.x + (float)this.gameObject.GetComponent<RectTransform>().rect.width
	}
}