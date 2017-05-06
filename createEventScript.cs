//Written by Hamza Hameed and Jonah Mooradian

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class createEventScript : MonoBehaviour {


	public InputField nameField;
	public InputField locationField; 
	public GameObject Event;
	public Transform weeklyViewCanvas;
	public Transform createEventCanvas;
	public Transform dailyViewCanvas;

	private float startDay = -525f;
	private float startTime = 576f;

	public Toggle[] alarmToggles = new Toggle[11];

	int startHourInt;
	int endHourInt;

	int startHeight;

	int counter = 0;
	int eventLength = 0;

	public Text startHour;
	Text startMinutes;
	Text startAMPM;
	Text endHour;
	Text endMinutes;
	Text endAMPM;
	Toggle allDayToggle;

	ToggleGroup toggleGroup;
	Toggle sundayToggle;
	Toggle mondayToggle;
	Toggle tuesdayToggle;
	Toggle wednesdayToggle;
	Toggle thursdayToggle;
	Toggle fridayToggle;
	Toggle saturdayToggle;

	Text eventMonth;
	Text eventDate;
	Text eventYear;

	Text tagOption;


	void Start(){
		startHour = createEventCanvas.Find("Time/StartTime/hoursDrop/Label").GetComponent<Text>();
		startMinutes = createEventCanvas.Find("Time/StartTime/minutesDrop/Label").GetComponent<Text>();
		startAMPM = createEventCanvas.Find("Time/StartTime/ampm/Label").GetComponent<Text>();
		endHour = createEventCanvas.Find("Time/EndTime/hoursDrop (1)/Label").GetComponent<Text>();
		endMinutes = createEventCanvas.Find("Time/EndTime/minutesDrop (1)/Label").GetComponent<Text>();
		endAMPM = createEventCanvas.Find("Time/EndTime/ampm (1)/Label").GetComponent<Text>();
		allDayToggle = createEventCanvas.Find ("Time/alldayToggle").GetComponent<Toggle> ();

		toggleGroup = createEventCanvas.Find("EventDay/toggleGroup").GetComponent<ToggleGroup>();
		sundayToggle = createEventCanvas.Find("EventDay/SundayToggle").GetComponent<Toggle>();
		mondayToggle = createEventCanvas.Find("EventDay/MondayToggle").GetComponent<Toggle>();
		tuesdayToggle = createEventCanvas.Find("EventDay/TuesdayToggle").GetComponent<Toggle>();
		wednesdayToggle = createEventCanvas.Find("EventDay/WednesdayToggle").GetComponent<Toggle>();
		thursdayToggle = createEventCanvas.Find("EventDay/ThursdayToggle").GetComponent<Toggle>();
		fridayToggle = createEventCanvas.Find("EventDay/FridayToggle").GetComponent<Toggle>();
		saturdayToggle = createEventCanvas.Find("EventDay/SaturdayToggle").GetComponent<Toggle>();

		eventMonth = createEventCanvas.Find ("EventDate/month/Text").GetComponent<Text> ();
		eventDate = createEventCanvas.Find ("EventDate/days/Text").GetComponent<Text> ();
		eventYear = createEventCanvas.Find ("EventDate/year/Text").GetComponent<Text> ();

		tagOption = createEventCanvas.Find ("EventTag/tagOptions/Label").GetComponent<Text> ();
		startHeight = 180;
	}

	public void eventCreate(){

		startHourInt = System.Convert.ToInt32 (startHour.text);
		endHourInt = System.Convert.ToInt32 (endHour.text);

		setEventLength ();

		if (eventLength >= 1) {

			if (toggleGroup.AnyTogglesOn ()) {
				currentWeekSelected ();
			}
				
			Transform newEvent = Instantiate (Event).transform;
			newEvent.transform.SetParent (weeklyViewCanvas, false);

			Transform dailyEvent = Instantiate (Event).transform;
			dailyEvent.transform.SetParent (dailyViewCanvas, false);

			if (tagOption.text == "General") {
				newEvent.GetComponent<Image> ().color = new Color32(255, 241, 197, 255);
			} else if (tagOption.text == "Holiday") {
				newEvent.GetComponent<Image> ().color = new Color32 (224, 255, 182, 255);
			}else if (tagOption.text == "Birthday") {
				newEvent.GetComponent<Image> ().color = new Color32 (255, 161, 233, 255);
			}else if (tagOption.text == "Party") {
				newEvent.GetComponent<Image> ().color = new Color32 (86, 234, 232, 255);
			}else if (tagOption.text == "School") {
				newEvent.GetComponent<Image> ().color = new Color32 (255, 161, 75, 255);
			}else if (tagOption.text == "Work") {
				newEvent.GetComponent<Image> ().color = new Color32 (160, 143, 255, 255);
			}
				
			newEvent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (175, 50 * eventLength);
			if ((startAMPM.text.Equals ("AM") && startHourInt == 12) || eventLength == 24) {
				newEvent.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (startDay, startTime, 0);

			} else if (startAMPM.text.Equals ("PM") && startHourInt == 12) {
				newEvent.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (startDay, startTime - 12 * 50, 0);
			} else {
				for (int i = 1; i <= startHourInt; i++) {
					startTime -= 50;
				}
				newEvent.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (startDay, startTime, 0);
			}
			startTime = 576f;
			startDay = -525f;

			if (eventLength != 24) {
				Text newText = newEvent.Find ("Text").GetComponent<Text> ();
				newText.text = " " + nameField.text + "\n"
				+ " @ " + locationField.text + "\n"
				+ " " + startHour.text + ":" + startMinutes.text + startAMPM.text + " - " +
				endHour.text + ":" + endMinutes.text + endAMPM.text;
			} else {
				Text newText = newEvent.Find ("Text").GetComponent<Text> ();
				newText.text = " " + nameField.text + "\n"
				+ " @ " + locationField.text + "\n"
				+ "All Day";
			}

			string year;
			string month;
			string day;

			DateTime startofWeek = DateTime.Today.AddDays (-1 * (int)DateTime.Today.DayOfWeek);
			DateTime selectedDay = startofWeek;
			if (sundayToggle.isOn) {
				selectedDay = startofWeek;
			} else if (mondayToggle.isOn) {
				selectedDay = startofWeek.AddDays (1);
			} else if (tuesdayToggle.isOn) {
				selectedDay = startofWeek.AddDays (2);
			} else if (wednesdayToggle.isOn) {
				selectedDay = startofWeek.AddDays (3);
			} else if (thursdayToggle.isOn) {
				selectedDay = startofWeek.AddDays (4);
			} else if (fridayToggle.isOn) {
				selectedDay = startofWeek.AddDays (5);
			} else if (saturdayToggle.isOn) {
				selectedDay = startofWeek.AddDays (6);
			}
				
			if (DateTime.Today == selectedDay) {
				if (eventLength != 24) {
					Text newText = dailyEvent.Find ("Text").GetComponent<Text> ();
					newText.text = " " + nameField.text + "\n"
						+ " @ " + locationField.text + "\n"
						+ " " + startHour.text + ":" + startMinutes.text + startAMPM.text + " - " +
						endHour.text + ":" + endMinutes.text + endAMPM.text;
				} else {
					Text newText = dailyEvent.Find ("Text").GetComponent<Text> ();
					newText.text = " " + nameField.text + "\n"
						+ " @ " + locationField.text + "\n"
						+ "All Day";
				}

				if (tagOption.text == "General") {
					dailyEvent.GetComponent<Image> ().color = new Color32(255, 241, 197, 255);
				} else if (tagOption.text == "Holiday") {
					dailyEvent.GetComponent<Image> ().color = new Color32 (224, 255, 182, 255);
				}else if (tagOption.text == "Birthday") {
					dailyEvent.GetComponent<Image> ().color = new Color32 (255, 161, 233, 255);
				}else if (tagOption.text == "Party") {
					dailyEvent.GetComponent<Image> ().color = new Color32 (86, 234, 232, 255);
				}else if (tagOption.text == "School") {
					dailyEvent.GetComponent<Image> ().color = new Color32 (255, 161, 75, 255);
				}else if (tagOption.text == "Work") {
					dailyEvent.GetComponent<Image> ().color = new Color32 (160, 143, 255, 255);
				}

				dailyEvent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (175, 50);
				dailyEvent.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, startHeight, 0);
				startHeight -= 50;
			}

		} else {
			Debug.Log ("Error: Event Length is less than 1");
		}

	}

	void setEventLength(){
		if(allDayToggle.isOn == true){
			eventLength = 24;
		}else{
			if (endAMPM.text.Equals ("PM") && startAMPM.text.Equals ("PM")) {
				endHourInt += 12;
				startHourInt += 12;
			} else if (endAMPM.text.Equals ("PM")) {
				endHourInt += 12;
			} else if (startAMPM.text.Equals ("PM")) {
				startHourInt += 12;
			}
			if (startHour.text.Equals ("12") && startAMPM.text.Equals ("AM")) {
				startHourInt = 0;
			}else if (startHour.text.Equals ("12") && startAMPM.text.Equals ("PM")) {
				startHourInt = 12;
			}
			if (endHour.text.Equals ("12") && endAMPM.text.Equals ("PM")) {
				endHourInt = 12;
			}
			eventLength = endHourInt - startHourInt;
		}
	}

	void currentWeekSelected(){
		if (sundayToggle.isOn == true) {
			//whatever
		}else if (mondayToggle.isOn == true) {
			startDay += 175;
		}else if (tuesdayToggle.isOn == true) {
			startDay += 2 * 175;
		}else if (wednesdayToggle.isOn == true) {
			startDay += 3 * 175;
		}else if (thursdayToggle.isOn == true) {
			startDay += 4 * 175;
		}else if (fridayToggle.isOn == true) {
			startDay += 5 * 175;
		}else if (saturdayToggle.isOn == true) {
			startDay += 6 * 175;
		}
	}
}
