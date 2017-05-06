using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToDoList : ScriptableObject {

    public List<bool> finished = new List<bool>();
    public List<string> label = new List<string>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void delete(int index)
    {
        finished.RemoveAt(index);
        label.RemoveAt(index);
    }

    public void create(string labelName){
        finished.Add(false);
        label.Add(labelName);
    }
}
