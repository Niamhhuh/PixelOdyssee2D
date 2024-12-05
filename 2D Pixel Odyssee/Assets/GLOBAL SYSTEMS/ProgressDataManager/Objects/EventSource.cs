using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSource : ObjectScript
{
    //Variables which are passed onto DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public bool Event_Passed;			                                                //relevant to control Item Spawn
    public bool Talk_Event;

    //Object Data Management
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        ObjectList_ID = 5;
        //DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        SeqUReference = this.GetComponent<SequenceUnlock>();
        UnSReference = this.GetComponent<UnlockScript>();
        ObjReference = this.GetComponent<EventSource>();

        int currentIndex = 0;                                                                               //remember the currently inspected Index

        foreach (DataManager.EventObj StoredObj in DataManager.EventSource_List)                      //Go through the EventSource_List and check EventObj.
        {
            if (ID == StoredObj.Stored_ID)
            {
                FetchData(StoredObj.Stored_Lock_State, StoredObj.Stored_AlreadyTalked, StoredObj.Stored_Event_Passed);                         //Fetch ObjectInformation from DataManager 
                ObjectIndex = currentIndex;                                                                 //Fetch the Index of the found Object
                NewObject = false;                                                                          //Confirm the Object is already available in DataManager
                break;
            }
            currentIndex++;                                                                                 //Update the currently inspected Index
        }
        if (NewObject == true)                                                                              //If required, pass ObjectInformation to DataManager.
        {
            DMReference.AddEventObj(ID, Lock_State, AlreadyTalked, Event_Passed);                                       //Call the AddEventObj Method in DataManager, to add a new DataContainer.
            ObjectIndex = DataManager.EventSource_List.Count - 1;                                        //When an Object is added, it is added to the end of the list. 
        }

        ToggleSprites();
        RemoveEvent();                                                                                       //Remove Event if it has been interacted with already
    }



    public void FetchData(bool Stored_Lock_State, bool Stored_AlreadyTalked, bool Stored_Event_Passed)                                  //Fetch the Variables Lock and Event_Passed from the DataManager
    {
        Lock_State = Stored_Lock_State;
        AlreadyTalked = Stored_AlreadyTalked;
        Event_Passed = Stored_Event_Passed;
    }


    public void UpdateData()                                                                               //Pass Variables Lock and Event_Passed to the DataManager
    {
        DMReference.EditEventObj(ObjectIndex, Lock_State, AlreadyTalked, Event_Passed);
    }


    //EventSource Item specific delete on Load Funtion
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void RemoveEvent()                                                                               //Remove the Event when it is or has been interacted with
    {
        if (Event_Passed == true)
        {
            if (DMReference.MoveScript != null)
            {
                DMReference.MoveScript.StartCoroutine(PointerScript.CallEnableInput());                                                //Enable Input when Trigger is cleared
                DMReference.MoveScript.StartCoroutine(PointerScript.CallEnableInteract());                                             //Enable Interact when Trigger is cleared
            }
            Destroy(gameObject);
        }
    }


    //Functions
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    public void Call_Interact()
    {
        Unlock_Object();                                                                                                                        //Try to Unlock the Object
        FetchData(DataManager.EventSource_List[ObjectIndex].Stored_Lock_State, DataManager.EventSource_List[ObjectIndex].Stored_AlreadyTalked, DataManager.EventSource_List[ObjectIndex].Stored_Event_Passed);  //Fetch new State from DataManager


        Lock_State = DataManager.EventSource_List[ObjectIndex].Stored_Lock_State;

        GameObject.FindGameObjectWithTag("InteractionController").SetActive(false);                    //Deactivate the Shove Arrows
        DataManager.ToInteract.RemoveAt(0);                                                            //Remove the Shovable from the ToShove List

        if (Lock_State == false)
        {
            gameObject.GetComponent<Collider2D>().enabled = false;                                     //disable Collider to Trigger TurnOffDialogue in AdvancedDialogueManager early -> would otherwise remove Forced Dialogue from line 99!!!!
            ClearHighlight();
            PassTriggerActivate(1); 
            ObjectSequenceUnlock();
            EventInteract();
        }
        else
        {
            ClearHighlight();
            //PassTriggerActivate(2);
            StartCoroutine(FlashRed());
        }
    }


    //Object Specific Functionality
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    public void EventInteract()                                                                                                                    //Interact with the Event to end it.
    {
        ClearHighlight();
        Event_Passed = true;    //Perhaps this will be changed into an Interger -> remember event state.
        UpdateData();
        RemoveEvent();
    }
}
