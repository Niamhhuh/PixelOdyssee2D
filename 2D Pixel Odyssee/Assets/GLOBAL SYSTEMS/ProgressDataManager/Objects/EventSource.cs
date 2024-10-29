using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSource : ObjectScript
{
    //Variables which are passed onto DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public bool Event_Passed;			                                                //relevant to control Item Spawn

    //Object Data Management
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Awake()
    {
        ObjectList_ID = 5;
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager

        UnSReference = this.GetComponent<UnlockScript>();

        int currentIndex = 0;                                                                               //remember the currently inspected Index

        foreach (DataManager.EventObj StoredObj in DataManager.EventSource_List)                      //Go through the EventSource_List and check EventObj.
        {
            if (ID == StoredObj.Stored_ID)
            {
                FetchData(StoredObj.Stored_Lock_State, StoredObj.Stored_Event_Passed);                         //Fetch ObjectInformation from DataManager 
                ObjectIndex = currentIndex;                                                                 //Fetch the Index of the found Object
                NewObject = false;                                                                          //Confirm the Object is already available in DataManager
                break;
            }
            currentIndex++;                                                                                 //Update the currently inspected Index
        }
        if (NewObject == true)                                                                              //If required, pass ObjectInformation to DataManager.
        {
            DMReference.AddEventObj(ID, Lock_State, Event_Passed);                                       //Call the AddEventObj Method in DataManager, to add a new DataContainer.
            ObjectIndex = DataManager.EventSource_List.Count - 1;                                        //When an Object is added, it is added to the end of the list. 
        }

        RemoveEvent();                                                                                       //Remove Event if it has been interacted with already
    }



    private void FetchData(bool Stored_Lock_State, bool Stored_Event_Passed)                                  //Fetch the Variables Lock and Event_Passed from the DataManager
    {
        Lock_State = Stored_Lock_State;
        Event_Passed = Stored_Event_Passed;
    }


    public void UpdateData()                                                                               //Pass Variables Lock and Event_Passed to the DataManager
    {
        DMReference.EditEventObj(ObjectIndex, Lock_State, Event_Passed);
    }


    //EventSource Item specific delete on Load Funtion
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void RemoveEvent()                                                                               //Remove the Event when it is or has been interacted with
    {
        if (Event_Passed == true)
        {
            Destroy(gameObject);
        }
    }


    //Functions
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && RequestInteract == true)
        {
            DMReference.MoveScript.targetPosition = DMReference.MoveScript.player.position;
            Unlock_Object();                                                                                                                        //Try to Unlock the Object
            FetchData(DataManager.EventSource_List[ObjectIndex].Stored_Lock_State, DataManager.EventSource_List[ObjectIndex].Stored_Event_Passed);  //Fetch new State from DataManager

            if (Lock_State == false)
            {
                ClearHighlight();
                EventInteract();
                ObjectSequenceUnlock();
            }
        }
    }

    //Object Specific Functionality
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    private void EventInteract()                                                                                                                    //Interact with the Event to end it.
    {
            Event_Passed = true;    //Perhaps this will be changed into an Interger -> remember event state.
            UpdateData();
            RemoveEvent();
    }
}
