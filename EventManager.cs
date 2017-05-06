using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Xml;

public class EventManager : MonoBehaviour {

	public List<Event> selectedWeek = new List<Event>();
    //List<Event> selectedWeek = new List<Event>();
    public List<Event> selectedDay = new List<Event>();
    ToDoList generalToDo;
    AlarmManager am;

	fileManager fm = new fileManager();

    public bool encrypt;

	// Use this for initialization
	void Start () {
        encrypt = false;

        am = GetComponent<AlarmManager>();

        generalToDo = ScriptableObject.CreateInstance("ToDoList") as ToDoList;
        loadGenToDo();
		Debug.Log (DateTime.Now.Year.ToString () + "/" + DateTime.Now.Month.ToString () + "/" + DateTime.Now.Day.ToString ());
        loadSelectedDay(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());


		fm.initialize ();

        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //does subtask deleting relating to events
    public void deleteEvent(Event oldEvent)
    {
        //delete the event from the list
        for (int i = 0; i < selectedDay.Count; i++)
        {
            if (selectedDay[i] == oldEvent)
            {
                selectedDay.RemoveAt(i);
                break; //exits for loop if day has been removed already
            }
        }

        //delete the alarms tied to this event
        am.deleteAlarm(oldEvent);
    }

    
    //save what is currently in the selectedDay list
    public void saveSelectedDay(){
        //creates a folder directory if one does not already exist
        fm.createDirectory (selectedDay[0].date [0] + "/" + selectedDay[0].date [1] + "/");

        //creates or overwrites a file at the given location with what's in selectedDay
        fm.createFile(selectedDay[0].date[0] + "/" + selectedDay[0].date[1] + "/",
            selectedDay[0].date[2], "xml", saveEventList(selectedDay), encrypt);
    }

    //sets the selectedDay List with the events of the given date
	public bool loadSelectedDay( string year, string month, string day)
    {
		if (fm.checkFile (year + "/" + month + "/" + day + ".xml")) {
			
			XmlDocument xmlDoc = fm.useXmlFile (year + "/" + month + "/" + "/",
				                     day, "xml", encrypt);

			XmlNodeList eventList = xmlDoc.GetElementsByTagName ("Day");

			selectedDay.Clear (); //clears the list for loading
			foreach (XmlNode eventInfo in eventList) {
				Event newEvent = ScriptableObject.CreateInstance ("Event") as Event;

				XmlNodeList eventContent = eventInfo.ChildNodes;

				foreach (XmlNode eventItems in eventContent) {
					if (eventItems.Name == "Date") {
						newEvent.date [0] = eventItems.Attributes ["year"].Value;
						newEvent.date [1] = eventItems.Attributes ["month"].Value;
						newEvent.date [2] = eventItems.Attributes ["day"].Value;
					} else if (eventItems.Name == "Name") {
						newEvent.eventName = eventItems.InnerText;
					} else if (eventItems.Name == "ToDoList") {
						XmlNodeList toDoContent = eventItems.ChildNodes;
						foreach (XmlNode toDoCheck in toDoContent) {
							if (toDoCheck.Name == "Finished")
								newEvent.eventToDoList.finished.Add ("True" == toDoCheck.Attributes ["value"].Value);
							else if (toDoCheck.Name == "Label")
								newEvent.eventToDoList.label.Add (toDoCheck.Attributes ["value"].Value);
						}
					} else if (eventItems.Name == "StartTime") {
						XmlNodeList sTime = eventItems.ChildNodes;
						foreach (XmlNode sZone in sTime) {
							if (sZone.Name == "Hours")
								newEvent.startTime.Hours = sZone.InnerText;
							else if (sZone.Name == "Minutes")
								newEvent.startTime.Minutes = sZone.InnerText;
						}
					} else if (eventItems.Name == "EndTime") {
						XmlNodeList eTime = eventItems.ChildNodes;
						foreach (XmlNode eZone in eTime) {
							if (eZone.Name == "Hours")
								newEvent.startTime.Hours = eZone.InnerText;
							else if (eZone.Name == "Minutes")
								newEvent.startTime.Minutes = eZone.InnerText;
						}
					} else if (eventItems.Name == "Tag") {
						newEvent.eTag = eventItems.InnerText;
					} else if (eventItems.Name == "AlarmTimes") {
						int counter = 0;
						XmlNodeList aTimes = eventItems.ChildNodes;
						foreach (XmlNode aCheck in aTimes) {
							newEvent.alarmTimes [counter] = ("True" == aCheck.InnerText);
							counter++;
						}
					}
				}

				selectedDay.Add (newEvent);
			}
			return true;
		} else
			return false;
    }

    public void saveGenToDo(){

        XmlDocument xml = new XmlDocument();
        XmlElement rootElement = xml.CreateElement("List");
        xml.AppendChild(rootElement);
        saveEventToDoList(xml, rootElement, generalToDo);

        //creates or overwrites a file at the given location with what's in selectedDay
        fm.createFile("", "My List", "xml", xml.OuterXml, encrypt);
    }

    public void loadGenToDo(){

		if (fm.checkFile ("My List.xml")) {
			XmlDocument xmlDoc = fm.useXmlFile ("", "My List", "xml", encrypt);
			XmlNodeList myToDoList = xmlDoc.GetElementsByTagName ("List");

			foreach (XmlNode subToDo in myToDoList) {
				XmlNodeList toDoContent = subToDo.ChildNodes;
				foreach (XmlNode toDoCheck in toDoContent) {
					if (toDoCheck.Name == "Finished")
						generalToDo.finished.Add ("True" == toDoCheck.Attributes ["value"].Value);
					else if (toDoCheck.Name == "Label")
						generalToDo.label.Add (toDoCheck.Attributes ["value"].Value);
				}
			}
		}
    }

    //saves a list of events to a string
    public string saveEventList(List<Event> eventList){
        Debug.Log("Beginning XML saving...");

        XmlDocument xml = new XmlDocument();
        XmlElement rootElement = xml.CreateElement("Day");
        xml.AppendChild(rootElement);
        for (int i = 0; i < eventList.Count; i++)
        {
            saveEvent(xml, rootElement, eventList[i]);
        }
        return xml.OuterXml;
    }

    //saves an event to the given document
    private void saveEvent(XmlDocument xml, XmlElement rootElement, Event newEvent){
        
        //create event element
        XmlElement eventElement = fm.createXmlElement(xml,"Event");

        //add the date to the document
        eventElement.AppendChild(fm.createXmlElement(xml, "Date", "",
            new string[] { "year", "month", "day" },
            new string[] { newEvent.date[0], newEvent.date[1], newEvent.date[2] }));

        //add the name of the date to the xml document
        eventElement.AppendChild(fm.createXmlElement(xml, "Name", newEvent.eventName));

        //call the function that adds the to do list
        saveEventToDoList(xml, eventElement, newEvent.eventToDoList);

        //create start time subcategory
        XmlElement startTimeElement = fm.createXmlElement(xml,"StartTime");
        startTimeElement.AppendChild(fm.createXmlElement(xml, "Hours", newEvent.startTime.Hours));
        startTimeElement.AppendChild(fm.createXmlElement(xml, "Minutes", newEvent.startTime.Minutes));
        eventElement.AppendChild(startTimeElement);

        //create end time subcategory
        XmlElement endTimeElement = fm.createXmlElement(xml, "EndTime");
        endTimeElement.AppendChild(fm.createXmlElement(xml, "Hours", newEvent.endTime.Hours));
        endTimeElement.AppendChild(fm.createXmlElement(xml, "Minutes", newEvent.endTime.Minutes));
        eventElement.AppendChild(endTimeElement);

        //add the tag of this object
        eventElement.AppendChild(fm.createXmlElement(xml, "Tag", newEvent.eTag));

        //save the player's list of checked alarms
        saveAlarmTimes(xml, eventElement, newEvent);

        //creates the actual alarms within the alarm manager

        //Adds the event element to the rootElement
        rootElement.AppendChild(eventElement);

    }

    //saves the to do list of the event and the to do list given (subfunction of saveEvent)
    private void saveEventToDoList(XmlDocument xml, XmlElement eventElement, ToDoList newToDo){
        XmlElement ToDoListElement = xml.CreateElement("ToDoList");

        for (int i = 0; i < newToDo.finished.Count; i++)
        {
            ToDoListElement.AppendChild(fm.createXmlElement(xml, "Finished", "", "value", newToDo.finished[i].ToString()));
        }

        for (int i = 0; i < newToDo.label.Count; i++)
        {
            ToDoListElement.AppendChild(fm.createXmlElement(xml, "Label", "", "value", newToDo.label[i]));
        }

        eventElement.AppendChild(ToDoListElement);
    }

    //saves the alarm times to the xml document given (sub function of saveEvent)
    private void saveAlarmTimes(XmlDocument xml, XmlElement eventElement, Event newEvent){
        XmlElement alarmTimesElement = xml.CreateElement("AlarmTimes");
        for (int i = 0; i < newEvent.alarmTimes.Length; i++)
        {
            alarmTimesElement.AppendChild(fm.createXmlElement(xml, "alarm", "", "value", newEvent.alarmTimes[i].ToString()));
        }
        eventElement.AppendChild(alarmTimesElement);
    }
}
