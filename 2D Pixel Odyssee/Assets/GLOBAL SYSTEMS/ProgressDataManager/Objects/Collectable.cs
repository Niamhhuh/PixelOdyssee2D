using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : ObjectScript
{
    //Variables which are passed onto DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public bool Collected;			                                                //relevant to control Item Spawn

    //Local Variables, not saved in the DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    //DataManager.Rooms_Loaded[SourceRoom] == false             use this for "Onetime Events"


    //Object Data Management
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Awake()
    {
        ObjectList_ID = 1;
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        
        UnSReference = this.GetComponent<UnlockScript>();

        int currentIndex = 0;                                                                               //remember the currently inspected Index

        foreach (DataManager.CollectableObj StoredObj in DataManager.Collectable_List)                      //Go through the Collectable_List and check CollectableObj.
        {
            if (ID == StoredObj.Stored_ID)
            {
                FetchData(StoredObj.Stored_Lock_State, StoredObj.Stored_Collected);                         //Fetch ObjectInformation from DataManager 
                ObjectIndex = currentIndex;                                                                 //Fetch the Index of the found Object
                NewObject = false;                                                                          //Confirm the Object is already available in DataManager
                break;
            }
            currentIndex++;                                                                                 //Update the currently inspected Index
        }
        if (NewObject == true)                                                                              //If required, pass ObjectInformation to DataManager.
        {
            DMReference.AddCollectableObj(ID, Lock_State, Collected);                                       //Call the AddCollectableObj Method in DataManager, to add a new DataContainer.
            ObjectIndex = DataManager.Collectable_List.Count - 1;               //FIND PICKUP BUG HERE             //When an Object is added, it is added to the end of the list. 
        }

        RemoveItem();                                                                                       //Remove Items if they have been collected already
    }



    private void FetchData(bool Stored_Lock_State, bool Stored_Collected)                                  //Fetch the Variables Lock and Collected from the DataManager
    {
        Lock_State = Stored_Lock_State;
        Collected = Stored_Collected;
    }


    private void UpdateData()                                                                               //Pass Variables Lock and Collected to the DataManager
    {
        DMReference.EditCollectableObj(ObjectIndex, Lock_State, Collected);
    }


    //Collectable Item specific delete on Load Funtion
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void RemoveItem()                                                                               //Remove the Item when it is or was already collected
    {
        if (Collected == true)
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
            
            Unlock_Object();                                                                                                                        //Try to Unlock the Object
            FetchData(DataManager.Collectable_List[ObjectIndex].Stored_Lock_State, DataManager.Collectable_List[ObjectIndex].Stored_Collected);     //Fetch new State from DataManager

            if (Lock_State == false)
            {
                ClearHighlight();
                PickUp();
                ObjectSequenceUnlock();
            }
        }
    }

    //Object Specific Functionality
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    private void PickUp()                                                                              //Pick up the Item by adding it to the acquired List.
    {
        if (Lock_State == false)
        {
            int InitialSlot = 0;
            DMReference.AddAcquiredObj(ID, InitialSlot);                                      //Call the AddCollectableObj Method in DataManager, to add a new DataContainer.
            Collected = true;
            UpdateData();
            RemoveItem();
        }
    }
}
