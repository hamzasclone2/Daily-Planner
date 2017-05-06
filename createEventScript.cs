using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class createEventScript : MonoBehaviour {

	public EventMaker eventMaker;

	public InputField nameField;
	public InputField locationField; 
	public GameObject Event;
	public Transform weeklyViewCanvas;
	public Transform createEventCanvas;

	private float startDay = -525.5f;
	private float startTime = 576;

	public Toggle[] alarmToggles = new Toggle[11];

	int startHourInt;
	int endHourInt;

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

	//John's scripts
	public EventManager em;

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
		eventMaker.saveNewAlarms (alarmToggles);
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
			startTime = 576;
			startDay = -525.5f;

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

			/*string year;
			string month;
			string day;

			if (toggleGroup.AnyTogglesOn ()) {
				DateTime sunday = DateTime.Today.AddDays (-1 * (int)DateTime.Today.DayOfWeek + (7 * counter));
				DateTime monday = sunday.AddDays (1);
				DateTime tuesday = sunday.AddDays (2);
				DateTime wednesday = sunday.AddDays (3);
				DateTime thursday = sunday.AddDays (4);
				DateTime friday = sunday.AddDays (5);
				DateTime saturday = sunday.AddDays (6);

				if (sundayToggle.isOn) {
					day = sunday.Day
				} else if (mondayToggle.isOn) {

				} else if (tuesdayToggle.isOn) {

				} else if (wednesdayToggle.isOn) {

				} else if (thursdayToggle.isOn) {

				} else if (fridayToggle.isOn) {

				} else if (saturdayToggle.isOn) {

				}
			}*/

			/*
			em.loadSelectedDay(sunday.year, sunday.month, sunday.day);
			Event firstEvent = em.selectedDay [0];
			*/

			eventMaker.setEventName (nameField.text);
			eventMaker.setLocation (locationField.text);
			eventMaker.setTag (tagOption.text);
			eventMaker.setYear (eventYear.text);
			eventMaker.setMonth (eventMonth.text);
			eventMaker.setDay (eventDate.text);
			eventMaker.setEndHour (endHour.text);
			eventMaker.setEndMinute (endMinutes.text);
			eventMaker.setStartMinute (startMinutes.text);
			eventMaker.setStartHour (startHour.text);
			eventMaker.saveNewAlarms (alarmToggles);

			//loadEvents (DateTime.Today);

		} else {
			Debug.Log ("Error: Event Length is less than 1");
		}

	}

	void loadEvents(DateTime day){
		if (em.loadSelectedDay (day.Year.ToString(), day.Month.ToString(), day.Day.ToString())) {
			for(int i = 0; i < em.selectedDay.Count; i++){
				em.selectedDay[i].eventName = "hi";
				Debug.Log ("it worked?");
			}
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

	public void fillUI(string monthString, string dateString, string yearString){
		for (int i = 0; i < 7; i++) {
			DateTime startofWeek = DateTime.Today.AddDays (-1 * (int)DateTime.Today.DayOfWeek + i + (counter * 7));
			monthString = startofWeek.Month.ToString ();
			dateString = startofWeek.Day.ToString ();
			yearString = startofWeek.Year.ToString ();


			em.loadSelectedDay (yearString, monthString, dateString);

			for (int j = 0; j < em.selectedDay.Count; j++) {
				loadEvent (em.selectedDay [j]);
			}
		}
	}

	public void loadEvent(Event chosenEvent)
	{
		startHourInt = int.Parse(chosenEvent.startTime.Hours);
		endHourInt = int.Parse(chosenEvent.endTime.Hours);
	}
}
