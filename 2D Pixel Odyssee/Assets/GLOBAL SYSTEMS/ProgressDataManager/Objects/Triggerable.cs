using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerable : ObjectScript
{
    //Variables which are passed onto DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public bool Trigger_Passed;			                                                //relevant to control Item Spawn
    public bool ForceDialogue;			                                            //relevant to Trigger Dialogue on Interact
    
    //Object Data Management
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Awake()
    {
        ObjectList_ID = 6;
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        SeqUReference = this.GetComponent<SequenceUnlock>();
        UnSReference = this.GetComponent<UnlockScript>();
        ObjReference = this.GetComponent<Triggerable>();

        int currentIndex = 0;                                                                               //remember the currently inspected Index

        foreach (DataManager.TriggerableObj StoredObj in DataManager.Triggerable_List)                      //Go through the EventSource_List and check EventObj.
        {
            if (ID == StoredObj.Stored_ID)
            {
                FetchData(StoredObj.Stored_Lock_State, StoredObj.Stored_Trigger_Passed);                    //Fetch ObjectInformation from DataManager 
                ObjectIndex = currentIndex;                                                                 //Fetch the Index of the found Object
                NewObject = false;                                                                          //Confirm the Object is already available in DataManager
                break;
            }
            currentIndex++;                                                                                 //Update the currently inspected Index
        }
        if (NewObject == true)                                                                              //If required, pass ObjectInformation to DataManager.
        {
            DMReference.AddTriggerableObj(ID, Lock_State, Trigger_Passed, this.gameObject);                 //Call the AddTriggerableObj Method in DataManager, to add a new DataContainer.
            ObjectIndex = DataManager.Triggerable_List.Count - 1;                                        //When an Object is added, it is added to the end of the list. 
        }

        if (NewObject == true)
        {
            DataManager.TriggeredObjects_List.Add(gameObject);
        }

        if (Lock_State == true)
        {
            gameObject.SetActive(false);
        }

        RemoveTrigger();                                                                                       //Remove Event if it has been interacted with already
    }



    private void FetchData(bool Stored_Lock_State, bool Stored_Trigger_Passed)                                  //Fetch the Variables Lock and Event_Passed from the DataManager
    {
        Lock_State = Stored_Lock_State;
        Trigger_Passed = Stored_Trigger_Passed;
    }


    public void UpdateData()                                                                               //Pass Variables Lock and Event_Passed to the DataManager
    {
        DMReference.EditTriggerableObj(ObjectIndex, Lock_State, Trigger_Passed);
    }


    //EventSource Item specific delete on Load Funtion
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void RemoveTrigger()                                                                               //Remove the Event when it is or has been interacted with
    {
        if (Trigger_Passed == true)
        {
            gameObject.SetActive(false);
        }
    }


    //Functions
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    public void OnMouseOver()
    {
        if(Input.GetMouseButtonUp(0) && Trigger_Passed == false)
        {
            //Unlock_Object();                                                                                                                        //Try to Unlock the Object
            FetchData(DataManager.Triggerable_List[ObjectIndex].Stored_Lock_State, DataManager.Triggerable_List[ObjectIndex].Stored_Trigger_Passed);  //Fetch new State from DataManager
            //PointerScript.StartCoroutine(PointerScript.CallEnableInput());
            //PointerScript.StartCoroutine(PointerScript.CallEnableInteract());

            //DataManager.ToInteract.RemoveAt(0);                                                            //Remove the Shovable from the ToShove List
            //GameObject.FindGameObjectWithTag("InteractionController").SetActive(false);                    //Deactivate the Shove Arrows

            if (Lock_State == false)
            {
                ClearHighlight();
                PassTriggerActivate(1);
                ObjectSequenceUnlock();
                TriggerInteract();
            } 
        }
    }

    //Object Specific Functionality
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    public void TriggerInteract()                                                                                                                    //Interact with the Event to end it.
    {
        Trigger_Passed = true;    //Perhaps this will be changed into an Interger -> remember event state.
        UpdateData();

        //If Force_Dialogue -> Trigger Dialogue
        //Remove Trigger when StepNum = Dialogue Length.
        //
        if(ForceDialogue == true)
        {
            DMReference.MoveScript.DisableInput();                                  //Disable Inpput 
            DMReference.MoveScript.DisableInteract();                               //Disable Interact 
            DMReference.MoveScript.InTriggerDialogue = true;
            GetComponent<NPCDialogue>().advancedDialogueManager.ContinueDialogue();
        }
        else 
        {
            RemoveTrigger();
        }
    }
}