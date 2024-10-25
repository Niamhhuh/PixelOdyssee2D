using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switchable : ObjectScript
{
    //Variables which are passed onto DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public bool SwitchState;                                                                                //relevant to remember whether this Switch is active or inactive

    //Object Data Management
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Awake()
    {
        ObjectList_ID = 4;
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        SeqUReference = this.GetComponent<SequenceUnlock>();
        UnSReference = this.GetComponent<UnlockScript>();

        int currentIndex = 0;                                                                               //remember the currently inspected Index

        foreach (DataManager.SwitchStateObj StoredObj in DataManager.SwitchState_List)                      //Go through the SwitchState_List and Fetch SwitchStateObj.
        {
            if (ID == StoredObj.Stored_ID)
            {
                FetchData(StoredObj.Stored_Lock_State, StoredObj.Stored_SwitchState);                       //Fetch ObjectInformation from DataManager 
                ObjectIndex = currentIndex;                                                                 //Fetch the Index of the found Object
                NewObject = false;                                                                          //Confirm the Object is already available in DataManager
                break;
            }
            currentIndex++;                                                                                 //Update the currently inspected Index
        }
        if (NewObject == true)                                                                              //If required, pass ObjectInformation to DataManager.
        {
            DMReference.AddSwitchStateObj(ID, Lock_State, SwitchState);                                     //Call the AddSwitchStateObj Method in DataManager, to add a new DataContainer.
            ObjectIndex = DataManager.SwitchState_List.Count - 1;                                           //When an Object is added, it is added to the end of the list, making its Index I-1.
        }

    }



    private void FetchData(bool Stored_Lock_State, bool Stored_SwitchState)                                 //Fetch the Variables Lock and Traversed from the DataManager
    {
        Lock_State = Stored_Lock_State;
        SwitchState = Stored_SwitchState;
        //print(StoredObj.Stored_Type_ID);
    }


    private void UpdateData()                                                                               //Pass Variables Lock and Traversed to the DataManager
    {
        DMReference.EditSwitchStateObj(ObjectIndex, Lock_State, SwitchState);
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
            FetchData(DataManager.SwitchState_List[ObjectIndex].Stored_Lock_State, DataManager.SwitchState_List[ObjectIndex].Stored_SwitchState);   //Fetch new State from DataManager

            if (Lock_State == false)
            {
                ClearHighlight();
                ObjectSequenceUnlock();
            }
        }
    }
}
