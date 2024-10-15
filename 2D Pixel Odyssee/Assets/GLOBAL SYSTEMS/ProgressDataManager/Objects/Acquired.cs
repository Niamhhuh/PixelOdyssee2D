using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acquired : ObjectScript
{
    //Variables which are passed onto DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public int ID;                                                                  //ID of the Object, required to find it in the list
    public bool Lock_State;                                 //Likely Redundant      //check if this Object is Interaction_Locked/Limited
    //public(Dialogue)			                                                    //Dialogue of this object

    public int Slot;                                                                //relevant to control the position in the Inventory

    //Local Variables, not saved in the DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //public sprite Highlight;		                                               	//store the highlight sprite of this object
    private int ObjectList_ID = 4;                           //Likely Redundant     //ID which marks the List this Object is stored in          //used for UnlockMethods
    private int ObjectIndex;                                 //Likely Redundant     //Index of this Object in its list                          //used for UnlockMethods


    private int SourceRoom;                                                          //reference to the Room in which tis Object is instantiated
    private bool NewObject = true;

    private DataManager DMReference;
    private SequenceUnlock SeqUReference = null;                                     //
    private UnlockScript UnSReference = null;                                        //

    //DataManager.Rooms_Loaded[SourceRoom] == false             use this for "Onetime Events"


    //Object Data Management
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Awake()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        SeqUReference = this.GetComponent<SequenceUnlock>();
        UnSReference = this.GetComponent<UnlockScript>();

        int currentIndex = 0;                                                                               //remember the currently inspected Index

        foreach (DataManager.AcquiredObj StoredObj in DataManager.Acquired_List)                            //Go through the Acquired_List and check AcquiredObj.
        {
            if (ID == StoredObj.Stored_ID)
            {
                FetchData(StoredObj.Stored_Lock_State, StoredObj.Stored_Slot);                              //Fetch ObjectInformation from DataManager 
                ObjectIndex = currentIndex;                                                                 //Fetch the Index of the found Object
                NewObject = false;                                                                          //Confirm the Object is already available in DataManager
                break;
            }
            currentIndex++;                                                                                 //Update the currently inspected Index
        }
        if (NewObject == true)                                                                              //If required, pass ObjectInformation to DataManager.
        {
            DMReference.AddAcquiredObj(ID, Lock_State, Slot);                                               //Call the AddAcquiredObj Method in DataManager, to add a new DataContainer.
            ObjectIndex = DataManager.Acquired_List.Count - 1;                                                  //When an Object is added, it is added to the end of the list, making its Index I-1.
        }

    }



    private void FetchData(bool Stored_Lock_State, int Stored_Slot)                                          //Fetch the Variables Lock and Slot from the DataManager
    {
        Lock_State = Stored_Lock_State;
        Slot = Stored_Slot;
        //print(StoredObj.Stored_Type_ID);
    }


    private void UpdateData()                                                                               //Pass Variables Lock and Slot to the DataManager
    {
        DMReference.EditAcquiredObj(ObjectIndex, Lock_State, Slot);
    }

    //UnLock Object Functions
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    //Unlock this Object with a Key (Item or ShovablePosition)
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void Unlock_Object()                                                                        //Call on Object Interaction to check for Unlock
    {
        if (UnlockMethod == 1)                                                                          //If the Unlock Method is 1 use ItemUnlock
        {
            ItemUnlock IUReference = null;                                                              //Create a ItemUnlock Variable, which will be used to access the CallItemUnlock Method
            IUReference = (ItemUnlock)UnSReference;                                                     //Convert the Parent UnlockScript Type(UnSReference) into the ItemUnlock Type 
            IUReference.CallItemUnlock(ObjectList_ID, ObjectIndex);                                     //Create a ItemUnlock Variable, which will be used to access the CallItemUnlock Method
        }
        if (UnlockMethod == 2)                                                                          //If the Unlock Method is 2 use ShovableUnlock
        {
            ShovableUnlock IUReference = null;                                                          //Create a ItemUnlock Variable, which will be used to access the CallItemUnlock Method
            IUReference = (ShovableUnlock)UnSReference;                                                 //Convert the Parent UnlockScript Type(UnSReference) into the ItemUnlock Type 
            IUReference.CallShovableUnlock(ObjectList_ID, ObjectIndex);                                 //Create a ItemUnlock Variable, which will be used to access the CallItemUnlock Method
        }
    }


    //Optional Initially Locked Functions
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    //SequenceUnlock
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void ObjectSequenceUnlock()
    {
        SeqUReference.CallSequenceUnlock();             //Call Sequence Unlock Method in Sequence Unlock Script
    }



    /*
    private void Update()
    {
        if(Activate == true)
        {
            DMReference.PrintCollectable();
            Activate = false;
        }
    }
    */
}
