using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

		open = false;
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
			//print ("FIN");
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

	public void displayArmy(GameObject PlayerObject) {
		if (open) {
			int iterator = 0;
			foreach (GameObject unit in PlayerObject.GetComponent<PlayerSetup>().unitList) {
				GameObject unitCard = Instantiate (Resources.Load ("UIPrefabs/unitCard", typeof(GameObject)) as GameObject);

				unitCard.AddComponent<UnitBase> ();
				unitCard.GetComponent<UnitBase> ().copyUnitFrom (unit);

				unitCard.transform.SetParent (this.gameObject.transform.FindChild ("Scroll View").transform.FindChild ("Viewport").transform.FindChild ("Content").transform);
				unitCard.transform.FindChild ("Panel").FindChild ("Name").gameObject.GetComponent<Text> ().text = unit.GetComponent<UnitBase> ().unitName;
				unitCard.transform.FindChild ("Panel").FindChild ("Portrait").gameObject.GetComponent<Image> ().sprite = unit.GetComponent<UnitBase> ().portrait;
				//unitCard.transform.FindChild ("Panel").FindChild("Cost").gameObject.GetComponent<Text> ().text = unitCard.GetComponent<UnitBase> ().cost;


				//unitCard.transform.FindChild ("Panel").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(this.gameObject.transform.FindChild ("Scroll View").gameObject.GetComponent<RectTransform>().sizeDelta.x, 

				unitCard.transform.localScale = new Vector3 (1, 1, 1);
				unitCard.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 1);
				unitCard.GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 1);
				unitCard.GetComponent<RectTransform> ().pivot = new Vector2 (0.5f, 0.5f);
				unitCard.GetComponent<RectTransform> ().sizeDelta = new Vector2 (815, 120);
				//print((this.gameObject.transform.FindChild ("Scroll View").gameObject.GetComponent<RectTransform>().sizeDelta.x/5) + " " + (iterator*unitCard.GetComponent<RectTransform> ().sizeDelta.x));
				unitCard.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (
					(this.gameObject.transform.FindChild ("Scroll View").gameObject.GetComponent<RectTransform> ().sizeDelta.x / 2), -((unitCard.GetComponent<RectTransform> ().sizeDelta.y * (iterator + 1)) - unitCard.GetComponent<RectTransform> ().sizeDelta.y / 2));
				//Multiply horizontal by width of display / 5 + width of cards + some arbitrary amount
				//Vertidcal measurements some arbitrary amount + (card size + arbitrary)* rowiterator
				iterator++;

				unitCard.GetComponent<Button> ().onClick.AddListener (delegate {
					displayUnitCard(unit);
				});
			}
		} else {
			foreach (Transform child in this.gameObject.transform.FindChild("Scroll View").FindChild("Viewport").FindChild("Content").transform) {
				print (child.gameObject.name);
				Destroy (child.gameObject);
			}
		}
	}

	public void displayUnitCard(GameObject unit) {
		print ("No");
		GameObject.Find ("UnitPicker").GetComponent<UnitView> ().clearUnitDetails ();
		GameObject.Find ("UnitPicker").GetComponent<UnitView> ().assignUnitDetails (unit);
		GameObject.Find ("UnitPicker").GetComponent<UnitView> ().slideUpward (this.gameObject.GetComponent<RectTransform>());
		toggle ();
	}

	public void scrapArmy() {
		this.transform.FindChild ("ScrapConfirm").FindChild ("Text").GetComponent<UnityEngine.UI.Text> ().text = "Scrap";
		toggle ();
	}

	public void confirmArmy() {
		this.transform.FindChild ("ScrapConfirm").FindChild ("Text").GetComponent<UnityEngine.UI.Text> ().text = "Confirm";
		toggle ();
	}
}
