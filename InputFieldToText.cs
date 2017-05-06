using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputFieldToText : MonoBehaviour {
	public InputField Field;
	//public Text TextBox;
	public GameObject toggle;
	public Transform canvas;
	private float xcoord = -150;
	private float ycoord = -150;
	public List<Transform> toggleList = new List<Transform>();
	Transform newToggle;

	public void createToggle(){
		if (Field.text.Equals ("")) {

		} else {
			newToggle = Instantiate (toggle).transform;
			newToggle.transform.parent = canvas.transform;
			newToggle.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (xcoord, ycoord, 0);
			//newToggle.position = new Vector3 (xcoord, ycoord, 0);x
			ycoord -= 40;
			Text newTextInstance = newToggle.Find ("Label").GetComponent<Text> ();
			newTextInstance.text = Field.text;
			newToggle.GetComponent<Toggle> ().isOn = false;
			toggleList.Add (newToggle);
		}
	}

	public void Delete(){
		toggleList.Clear ();
		foreach (Transform newToggle in toggleList) {
			Transform.Destroy (newToggle);
		}
	}
}

