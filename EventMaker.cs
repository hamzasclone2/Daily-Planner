//Written by John Ying

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventMaker : MonoBehaviour {

    Event newEvent;
    EventManager em;

    int sHour = 0;
    int sMin = 0;
    int eHour = 0;
    int eMin = 0;

    Toggle[] alarms = new Toggle[11];



    /*
    public string[] date = new string[3]; //this represents the year, month, day of the event (in that order)
    public string eventName;
    public cTime startTime; //a class that has hours and minutes
    public cTime endTime; //same as startTime

    public bool[] alarmTimes = new bool[11]; //alarm time zones displayed at bottom for code elegance (Ctrl+F "alarm times values") still need to setup alarm times

    //setup to do list
    public ToDoList eventToDoList;
    public string eTag;

    public Text labelText;
    */

	// Use this for initialization
	void Start () {
        em = GetComponent<EventManager>();
		newEvent = ScriptableObject.CreateInstance("Event") as Event;
		newEvent.initialize ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setEventName(string eventName)
    {
        newEvent.eventName = eventName;
    }

	public void setLocation(string eventLocation)
	{
		newEvent.eventLocation = eventLocation;
	}

    public void setTag(string eventTag)
    {
        newEvent.eTag = eventTag;
    }

    public void setYear(string year)
    {
        newEvent.setYear(year);
    }

    public void setMonth(string month)
    {
        newEvent.setMonth(month);
    }

    public void setDay(string day)
    {
        newEvent.setDay(day);
    }

    public void setEndHour(string hour)
    {
        newEvent.endTime.Hours = hour;
    }

    public void setEndMinute(string minute)
    {
        newEvent.endTime.Minutes = minute;
    }

    public void setStartMinute(string minute)
    {
        newEvent.startTime.Minutes = minute;
    }

    public void setStartHour(string hour)
    {
        newEvent.startTime.Hours = hour;
    }

    public void addToList(string label)
    {
        newEvent.eventToDoList.create(label);
    }

    public void saveNewAlarms(Toggle[] alarms)
    {
        for (int i = 0; i < 11; i++)
        {
            newEvent.alarmTimes[i] = alarms[i].isOn;
        }
    }

    public void saveNewAlarms(bool[] alarms)
    {
        for (int i = 0; i < 11; i++)
        {
            newEvent.alarmTimes[i] = alarms[i];
        }
    }

    /// <summary>
    /// Commits all of the remaining changes to the actual day chosen in the database
    /// </summary>
    public void create()
    {

		em.selectedDay.Add(newEvent);
		//Debug.Log ("Selected day name: " + em.selectedDay [0].eventName);
		em.saveSelectedDay ();
    }


}
