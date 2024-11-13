using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTheGame : MonoBehaviour
{
    DataManager DMReference;


    private void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
    }

    public void ResetGame()
    {
        DataManager.Collectable_List.Clear();
        DataManager.Shovable_List.Clear();
        DataManager.Portal_List.Clear();
        DataManager.SwitchState_List.Clear();
        DataManager.EventSource_List.Clear();
        DataManager.Triggerable_List.Clear();

        DataManager.Draggable_List.Clear();
        DataManager.Item_List.Clear();
        DataManager.Recipe_List.Clear();

        DataManager.Highlighted_Current.Clear();

        DataManager.ToInteract.Clear();

        DataManager.ToShove.Clear();

        //DataManager.Slot_Array.Clear();

        DataManager.TriggeredObjects_List.Clear();

        DataManager.Rooms_Loaded = new bool[10];                                       //Array which remembers if rooms have been loaded before.

        DMReference.MoveScript = null;                                                     //provide easy access to Movescript
        DMReference.InventoryRef = null;
        DMReference.CurrentCharacter = null;

        DataManager.Inventory_Fillstate = 0;

        DataManager.TutorialStarted = false;

        DataManager.FroggerCleared = false;

        DMReference.Reward = null;

    }
}