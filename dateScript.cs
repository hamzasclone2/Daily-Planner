//Written by Hamza Hameed

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dateScript : MonoBehaviour {

	public Transform canvas; 
	//DateTime today = DateTime.Today;
	DateTime startofWeek = DateTime.Today.AddDays (-1 * (int)DateTime.Today.DayOfWeek);
	Text sundayText;
	Text mondayText; 
	Text tuesdayText;
	Text wednesdayText;
	Text thursdayText;
	Text fridayText;
	Text saturdayText;

	void Start () {
		findTextBoxes ();
		fillTextBoxes ();
	}

	void findTextBoxes(){		
		sundayText = canvas.Find ("Scroll View/Viewport/WeekHeader/Sunday/Text").GetComponent<Text>();
		mondayText = canvas.Find ("Scroll View/Viewport/WeekHeader/Monday/Text").GetComponent<Text>();
		tuesdayText = canvas.Find ("Scroll View/Viewport/WeekHeader/Tuesday/Text").GetComponent<Text>();
		wednesdayText = canvas.Find ("Scroll View/Viewport/WeekHeader/Wednesday/Text").GetComponent<Text>();
		thursdayText = canvas.Find ("Scroll View/Viewport/WeekHeader/Thursday/Text").GetComponent<Text>();
		fridayText = canvas.Find ("Scroll View/Viewport/WeekHeader/Friday/Text").GetComponent<Text>();
		saturdayText = canvas.Find ("Scroll View/Viewport/WeekHeader/Saturday/Text").GetComponent<Text>();
	}
		
	void fillTextBoxes(){
		sundayText.text += "\n" + startofWeek.ToShortDateString ();
		DateTime monday = startofWeek.AddDays (1);
		mondayText.text += "\n" + monday.ToShortDateString();
		DateTime tuesday = startofWeek.AddDays (2);
		tuesdayText.text += "\n" + tuesday.ToShortDateString();
		DateTime wednesday = startofWeek.AddDays (3);
		wednesdayText.text += "\n" + wednesday.ToShortDateString();
		DateTime thursday = startofWeek.AddDays (4);
		thursdayText.text += "\n" + thursday.ToShortDateString();
		DateTime friday = startofWeek.AddDays (5);
		fridayText.text += "\n" + friday.ToShortDateString();
		DateTime saturday = startofWeek.AddDays (6);
		saturdayText.text += "\n" + saturday.ToShortDateString();
	}

	void resetTextBoxes(){
		sundayText.text = "Sunday";
		mondayText.text = "Monday";
		tuesdayText.text = "Tuesday";
		wednesdayText.text = "Wednesday";
		thursdayText.text = "Thursday";
		fridayText.text = "Friday";
		saturdayText.text = "Saturday";
	}

	public void forwardWeek (){
		findTextBoxes ();
		resetTextBoxes ();
		startofWeek = startofWeek.AddDays (7);
		fillTextBoxes ();
	}

	public void backWeek (){
		findTextBoxes ();
		resetTextBoxes ();
		startofWeek = startofWeek.AddDays (-7);
		fillTextBoxes ();
	}

	public void currentWeek(){
		startofWeek = DateTime.Today.AddDays (-1 * (int)DateTime.Today.DayOfWeek);
		findTextBoxes ();
		resetTextBoxes ();
		fillTextBoxes ();
	}
}
