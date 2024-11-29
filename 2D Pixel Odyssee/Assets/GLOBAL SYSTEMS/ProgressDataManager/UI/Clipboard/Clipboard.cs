using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clipboard : MonoBehaviour
{
    DataManager DMReference;
    Inventory InventoryScript;
    MapScript MapScript;
    GoalListScript GoalScript;
    CodeListScript CodeScript;

    static int CurrentTab = 1;

    // Start is called before the first frame update
    void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager

        InventoryScript = GameObject.FindGameObjectWithTag("UiCanvas").GetComponent<Inventory>();
        MapScript = GameObject.FindGameObjectWithTag("UiCanvas").GetComponent<MapScript>();
        GoalScript = GameObject.FindGameObjectWithTag("UiCanvas").GetComponent<GoalListScript>();
        CodeScript = GameObject.FindGameObjectWithTag("UiCanvas").GetComponent<CodeListScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.I))
        {
            if(!DMReference.MoveScript.ClipboardActive || !InventoryScript.InventoryObj.activeSelf)
            {
                //InventoryObj.calledbyKey = true;
                ClipboardInventory();
            } else 
            {
                CloseClipboard();
            }
        }


        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.M))
        {
            if (!DMReference.MoveScript.ClipboardActive || !MapScript.MapObj.activeSelf)
            {
                //InventoryObj.calledbyKey = true;
                ClipboardMap();
            } else
            {
                CloseClipboard();
            }
        }


        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Y))         // apparently Unity doesnt track your keyboard language...
        {
            if (!DMReference.MoveScript.ClipboardActive || !GoalScript.GoalListObj.activeSelf)
            {
                //InventoryObj.calledbyKey = true;
                ClipboardGoals();
            } else
            {
                CloseClipboard();
            }
        }


        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.C))
        {
            if (!DMReference.MoveScript.ClipboardActive || !CodeScript.CodeListObj.activeSelf)
            {
                //InventoryObj.calledbyKey = true;
                ClipboardCodes();
            } else
            {
                CloseClipboard();
            }
        }


    }


    public void CallClipboard()                 // this is called via X or the UI Button
    {
        if (!DMReference.MoveScript.ClipboardActive)
        {
            switch (CurrentTab)
            {
                case 1:
                    ClipboardInventory();
                    break;
                case 2:
                    ClipboardMap();
                    break;
                case 3:
                    ClipboardGoals();
                    break;
                case 4:
                    ClipboardCodes();
                    break;
            }
        } else
        {
            CloseClipboard();
        }
    }

    public void ClipboardInventory()
    {
        CurrentTab = 1;
        CodeScript.CloseCodeList();
        GoalScript.CloseGoalList();
        MapScript.CloseMap();

        InventoryScript.CallInventory();
    }

    public void ClipboardMap()
    {
        CurrentTab = 2;
        CodeScript.CloseCodeList();
        GoalScript.CloseGoalList();
        InventoryScript.CloseInventory();

        MapScript.CallMap();
    }

    public void ClipboardGoals()
    {
        CurrentTab = 3;
        CodeScript.CloseCodeList();
        MapScript.CloseMap();
        InventoryScript.CloseInventory();

        GoalScript.CallGoalList();
    }

    public void ClipboardCodes()
    {
        CurrentTab = 4;
        GoalScript.CloseGoalList();
        MapScript.CloseMap();
        InventoryScript.CloseInventory();

        CodeScript.CallCodeList();
    }

    public void CloseClipboard()
    {
        InventoryScript.CloseInventory();
        MapScript.CloseMap();
        GoalScript.CloseGoalList();
        CodeScript.CloseCodeList();
    }
}
