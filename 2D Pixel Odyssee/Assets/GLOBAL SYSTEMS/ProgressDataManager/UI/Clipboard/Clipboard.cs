using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clipboard : MonoBehaviour
{
    GameObject ClipboardObject;
    GameObject InventoryObj;
    GameObject MapObj;
    GameObject GoalObj;
    GameObject CodeObj;

    // Start is called before the first frame update
    void Start()
    {
        InventoryObj = GameObject.FindGameObjectWithTag("Inventory");
        MapObj = GameObject.FindGameObjectWithTag("Map");
        GoalObj = GameObject.FindGameObjectWithTag("GoalList");
        CodeObj = GameObject.FindGameObjectWithTag("CodeList");
    }

    public void CallClipboard()                 // this is called via X or the UI Button
    {
        //Call Inventory
        //Call Map
        //Call Goal
        //Call Code
    }
}
