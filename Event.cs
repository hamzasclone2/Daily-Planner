//Written by John Ying

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Event : ScriptableObject {


    /// <summary>
    /// Variable Declarations
    /// </summary>
    public string[] date = new string[3]; //this represents the year, month, day of the event (in that order)
    public string eventName;
	public string eventLocation;
    public cTime startTime; //a class that has hours and minutes
    public cTime endTime; //same as startTime
    public string eTag;
    public bool[] alarmTimes = new bool[11]; //alarm time zones displayed at bottom for code elegance (Ctrl+F "alarm times values") still need to setup alarm times

    //setup to do list
    public ToDoList eventToDoList;


    /// <summary>
    /// Initializes the event with empty string values and no alarm calls
    /// </summary>
	// Use this for initialization
    public void initialize () {
		for (int i = 0; i < date.Length; i++)
            date[i] = "";

        eventName = "";
		eventLocation = "";
        startTime = ScriptableObject.CreateInstance("cTime") as cTime;
        startTime.setTime("", "");
        endTime = ScriptableObject.CreateInstance("cTime") as cTime;
        endTime.setTime("", "");

        eventToDoList = ScriptableObject.CreateInstance("ToDoList") as ToDoList;
        eTag = "";

		for (int i = 0; i < alarmTimes.Length; i++)
			alarmTimes[i] = false;
        
		//loadDefault ();//used to initialize dummy values for testing (remove for proper UI testing)
	}

    /// <summary>
    /// loads dummy values for one event into an event
    /// </summary>
	public void loadDefault(){
		date [0] = "2017";
		date [1] = "08";
		date [2] = "22";

		eventName = "My Birthday";
		startTime.setTime("08", "23");
		endTime.setTime("09", "23");

        for (int i = 0; i < alarmTimes.Length; i++)
        {
            alarmTimes[i] = false;
        }
        //set two alarms
        alarmTimes[0] = true;
        alarmTimes[4] = true;

        eTag = "Birthday";

		eventToDoList.create ("wash dishes");
	}

    /// <summary>
    /// loads dummy values for another event into an event
    /// </summary>
    public void loadDefault2(){
        date [0] = "2017";
        date [1] = "08";
        date [2] = "22";

        eventName = "Fight Carl";
        startTime.setTime("12", "23");
        endTime.setTime("13", "23");

        for (int i = 0; i < alarmTimes.Length; i++)
            alarmTimes[i] = false;

        eTag = "Fight";

        eventToDoList.create ("clean car");
        eventToDoList.create ("brush teeth");
        eventToDoList.create ("throw away trash");
    }

	public void setYear( string year){
        int output;
        if (!int.TryParse(year, out output))
        {
            invalidInput("date");
        }
        else
        {
            date[0] = year;
        }
    }

    public void setMonth( string month ){
        int output;
        if (!int.TryParse(month, out output))
        {
            invalidInput("date");
        }
        else
        {
            date[1] = month;
        }
    }

    public void setDay(string day){
        int output;
        if (!int.TryParse(day, out output))
        {
            invalidInput("date");
        }
        else
        {
            date[2] = day;
        }
    }

    /// <summary>
    /// Put your invalid input code here
    /// </summary>
    /// <param name="invalidType">Invalid type.</param>
    private void invalidInput( string invalidType)
    {
        //probably create a dialog box popup with the error message?

        string errorText = "Invalid " + invalidType + ".";
        Debug.Log(errorText);
    }

    public void setAlarms(bool[] newAlarmTimes){
		for (int i = 0; i < alarmTimes.Length; i++)
            alarmTimes[i] = newAlarmTimes[i];

        // setup and/or coordinate with alarm system
    }
}

/// <summary>
/// alarm times values
/// 10 minutes
/// 15 minutes
/// 20 minutes
/// 25 minutes
/// 30 minutes
/// 45 minutes
/// 1 hour
/// 2 hours
/// 3 hours
/// 12 hours
/// 24 hours
/// </summary>
