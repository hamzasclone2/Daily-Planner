using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cTime : ScriptableObject {

    public string Hours = "";
    public string Minutes = "";

    public void setTime( string hours, string minutes){
        Hours = hours;
        Minutes = minutes;
    }
}
