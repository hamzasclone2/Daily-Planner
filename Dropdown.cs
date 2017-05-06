using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropdown : MonoBehaviour {
    public GameObject dropdown;
    public GameObject canvas;
    private GameObject m_blocker;

    protected virtual void DestroyBlocker(GameObject blocker)
    {
        UnityEngine.Object.Destroy(blocker);
    }
    // Update is called once per frame
    void Update () {
        Transform dropDownList = dropdown.transform.FindChild("Dropdown List");
        //GameObject blocker = 
        if (canvas.activeSelf == false)
        {
            DestroyBlocker(this.m_blocker);
        }
	}
}
