using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

	public float ScrollSpeed = 15;
	public GameObject camTarg;

	void Start () {
		camTarg = GameObject.Find("CamTarg");
		print("Camera: " + camTarg);
	}
	

}
