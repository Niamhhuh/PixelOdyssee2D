using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : ObjectScript
{
    //Variables which are passed onto DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public bool Traversed;                                                          //relevant to remember whether this door has been used already

    //DataManager.Rooms_Loaded[SourceRoom] == false             use this for "Onetime Events"


    //Object Data Management
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Awake()
    {
        ObjectList_ID = 3;
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        SeqUReference = this.GetComponent<SequenceUnlock>();
        UnSReference = this.GetComponent<UnlockScript>();

        int currentIndex = 0;                                                                               //remember the currently inspected Index

        foreach (DataManager.PortalObj StoredObj in DataManager.Portal_List)                                //Go through the Portal_List and cech PortalObj.
        {
            if (ID == StoredObj.Stored_ID)
            {
                FetchData(StoredObj.Stored_Lock_State, StoredObj.Stored_Traversed);                         //Fetch ObjectInformation from DataManager 
                ObjectIndex = currentIndex;                                                                 //Fetch the Index of the found Object
                NewObject = false;                                                                          //Confirm the Object is already available in DataManager
                break;
            }
            currentIndex++;                                                                                 //Update the currently inspected Index
        }
        if (NewObject == true)                                                                              //If required, pass ObjectInformation to DataManager.
        {
            DMReference.AddPortalObj(ID, Lock_State, Traversed);                                            //Call the AddPortalObj Method in DataManager, to add a new DataContainer.
            ObjectIndex = DataManager.Portal_List.Count - 1;                                                  //When an Object is added, it is added to the end of the list, making its Index I-1.
        }

    }



    private void FetchData(bool Stored_Lock_State, bool Stored_Traversed)                                   //Fetch the Variables Lock and Traversed from the DataManager
    {
        Lock_State = Stored_Lock_State;
        Traversed = Stored_Traversed;
        //print(StoredObj.Stored_Type_ID);
    }


    private void UpdateData()                                                                               //Pass Variables Lock and Traversed to the DataManager
    {
        DMReference.EditPortalObj(ObjectIndex, Lock_State, Traversed);
    }


    //Functions
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void OnMouseOver()  //This is the Main Function Controller
    {
        if (Input.GetMouseButtonDown(0))
        {
            //print("I'm called1");
            Unlock_Object();                                                                                                              //Try to Unlock the Object
            FetchData(DataManager.Portal_List[ObjectIndex].Stored_Lock_State, DataManager.Portal_List[ObjectIndex].Stored_Traversed);     //Fetch new State from DataManager

            if (Lock_State == false)
            {
             
            }
        }
    }
}
