using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Xml;

public class AlarmManager : MonoBehaviour {

    List<string[]> alarmList = new List<string[]>();

    fileManager fm = new fileManager();

    private int tempHours = 0;
    private int tempMins = 0;
    private int tempDay = 0;
    string[] alarmTime = new string[5];
    Event tempEvent;
    public bool encrypt;

    string[] myTempAlarmTime = new string[5];//used solely for storing current time in string format

    public AudioClip alarmClip;

	// Use this for initialization
	void Start () {
        fm.initialize ();
        loadAlarms();

        //initialize whether to be encrypted or not here
        encrypt = false;
	}
	
	// Update is called once per frame
	void Update () {

        //change the false parameter with call to match cur time with any alarm time
        if (isTimeToPlay())
        {
            playAlarm();
        }
	}

    private void playAlarm()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
    }

    private bool isTimeToPlay()
    {
        DateTime moment = DateTime.Now;

        myTempAlarmTime[0] = moment.Year.ToString();
        myTempAlarmTime[0] = moment.Month.ToString();
        myTempAlarmTime[0] = moment.Day.ToString();
        myTempAlarmTime[0] = moment.Hour.ToString();
        myTempAlarmTime[0] = moment.Minute.ToString();
        for (int i = 0; i < alarmList.Count; i++)
        {
            if (alarmList[i][0] == myTempAlarmTime[0] && alarmList[i][1] == myTempAlarmTime[1] && alarmList[i][2] == myTempAlarmTime[2]
                && alarmList[i][3] == myTempAlarmTime[3] && alarmList[i][4] == myTempAlarmTime[4])
            {
                alarmList.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    //used by the function saveAlarms
    private void saveAlarmTimes(XmlDocument xml, XmlElement alarmElement, string[] newAlarmTime){
        for (int i = 0; i < alarmList.Count; i++)
        {
            alarmElement.AppendChild(fm.createXmlElement(xml, "Info", "",
                new string[] { "year", "month", "day", "hours", "minutes" },
                new string[] { newAlarmTime[0], newAlarmTime[1], newAlarmTime[2], newAlarmTime[3], newAlarmTime[4] }));
        }
    }

    public void deleteAlarm(Event oldEvent)
    {
        //goes through each alarm
        for (int i = 0; i < alarmList.Count; i++)
        {
            //inputs needed event info into a readable format
            alarmTime[0] = oldEvent.date[0];
            alarmTime[1] = oldEvent.date[1];
            alarmTime[2] = oldEvent.date[2];


            tempEvent = oldEvent;
            for (int j = 0; j < 11; j++)
            {
                if (oldEvent.alarmTimes[j] == true)
                {
                    switch (j)
                    {
                        case 0:
                            sub(0, 10);
                            break;
                        case 1:
                            sub(0, 15);
                            break;
                        case 2:
                            sub(0, 20);
                            break;
                        case 3:
                            sub(0, 25);
                            break;
                        case 4:
                            sub(0, 30);
                            break;
                        case 5:
                            sub(0, 45);
                            break;
                        case 6:
                            sub(1, 0);
                            break;
                        case 7:
                            sub(2, 0);
                            break;
                        case 8:
                            sub(3, 0);
                            break;
                        case 9:
                            sub(12, 0);
                            break;
                        case 10:
                            sub(24, 0);
                            break;
                    }

                    if (alarmList[i][0] == alarmTime[0] && alarmList[i][1] == alarmTime[1] && alarmList[i][2] == alarmTime[2] && alarmList[i][3] == alarmTime[3] && alarmList[i][4] == alarmTime[4])
                        alarmList.RemoveAt(i);
                }
            }
        }
    }

    //general use function to save the alarms in the current list alarmList
    public void saveAlarms(){

        XmlDocument xml = new XmlDocument();
        XmlElement rootElement = xml.CreateElement("Alarms");
        xml.AppendChild(rootElement);

        for (int i = 0; i < alarmList.Count; i++)
        {
            saveAlarmTimes(xml, rootElement, alarmList[i]);
        }

        //creates or overwrites a file at the given location with what's in selectedDay
        fm.createFile("", "Alarms", "xml", xml.OuterXml, encrypt);
    }

    public void loadAlarms(){

		if (fm.checkFile ("Alarms")) {
			XmlDocument xmlDoc = fm.useXmlFile ("", "Alarms", "xml", encrypt);
			XmlNodeList alarmsList = xmlDoc.GetElementsByTagName ("Alarms");

			alarmList.Clear ();
			foreach (XmlNode subAlarmsList in alarmsList) {
				if (subAlarmsList.Name == "Info") {
					alarmTime [0] = subAlarmsList.Attributes ["year"].Value;
					alarmTime [1] = subAlarmsList.Attributes ["month"].Value;
					alarmTime [2] = subAlarmsList.Attributes ["day"].Value;
					alarmTime [3] = subAlarmsList.Attributes ["hours"].Value;
					alarmTime [4] = subAlarmsList.Attributes ["minutes"].Value;
					alarmList.Add (alarmTime);
				}
			}
		}
    }

    public void createEventAlarm(Event newEvent)
    {
        
        alarmTime[0] = newEvent.date[0];
        alarmTime[1] = newEvent.date[1];
        alarmTime[2] = newEvent.date[2];


        tempEvent = newEvent;
        for (int i = 0; i < 11; i++)
        {
            if (newEvent.alarmTimes[i] == true)
            {
                switch (i)
                {
                    case 0:
                        sub(0, 10);
                        break;
                    case 1:
                        sub(0, 15);
                        break;
                    case 2:
                        sub(0, 20);
                        break;
                    case 3:
                        sub(0, 25);
                        break;
                    case 4:
                        sub(0, 30);
                        break;
                    case 5:
                        sub(0, 45);
                        break;
                    case 6:
                        sub(1, 0);
                        break;
                    case 7:
                        sub(2, 0);
                        break;
                    case 8:
                        sub(3, 0);
                        break;
                    case 9:
                        sub(12, 0);
                        break;
                    case 10:
                        sub(24, 0);
                        break;
                }
                alarmList.Add(alarmTime);
            }
        }
    }

    //subtracts time and adds that time to the alarm list
    private void sub(int hours, int minutes)
    {
        tempHours = int.Parse(tempEvent.startTime.Hours);
        tempMins = int.Parse(tempEvent.startTime.Minutes);
        tempDay = int.Parse(tempEvent.date[2]);
        tempMins -= minutes;
        if (tempMins < 0)
        {
            tempHours--;
            tempMins += 60;
        }
        tempHours -= hours;
        if (tempHours < 0)
        {
            tempDay--;
            tempHours += 24;
        }

        alarmTime[2] = tempDay.ToString();
        alarmTime[3] = tempHours.ToString();
        alarmTime[4] = tempMins.ToString();

        
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