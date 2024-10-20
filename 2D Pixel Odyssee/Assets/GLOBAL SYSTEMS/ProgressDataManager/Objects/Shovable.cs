using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovable : ObjectScript
{
    //Variables which are passed onto DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public int ID;                                                                  //ID of the Object, required to find it in the list
    public bool Lock_State;                                                         //check if this Object is Interaction_Locked/Limited
    //public(Dialogue)			                                                    //Dialogue of this object

    public int Position;                                                            //relevant to remember the position in the room

    //Local Variables, not saved in the DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
 
    private int ObjectList_ID = 2;                                                  //ID which marks the List this Object is stored in          //used for UnlockMethods
    private int ObjectIndex;                                                        //Index of this Object in its list                          //used for UnlockMethods


    private int SourceRoom;                                                         //reference to the Room in which tis Object is instantiated
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

        foreach (DataManager.ShovableObj StoredObj in DataManager.Shovable_List)                            //Go through the Shovable_List and compare ShovableObj.
        {
            if (ID == StoredObj.Stored_ID)
            {
                FetchData(StoredObj.Stored_Lock_State, StoredObj.Stored_Position);                          //Fetch ObjectInformation from DataManager 
                ObjectIndex = currentIndex;                                                                 //Fetch the Index of the found Object
                NewObject = false;                                                                          //Confirm the Object is already available in DataManager
                break;
            }
            currentIndex++;                                                                                 //Update the currently inspected Index
        }
        if (NewObject == true)                                                                              //If required, pass ObjectInformation to DataManager.
        {
            DMReference.AddShovableObj(ID, Lock_State, Position);                                           //Call the AddShovableObj Method in DataManager, to add a new DataContainer.
            ObjectIndex = DataManager.Shovable_List.Count - 1;                                                  //When an Object is added, it is added to the end of the list, making its Index I-1.
        }

    }



    private void FetchData(bool Stored_Lock_State, int Stored_Position)                                     //Fetch the Variables Lock and Position from the DataManager
    {
        Lock_State = Stored_Lock_State;
        Position = Stored_Position;
        //print(StoredObj.Stored_Type_ID);
    }


    private void UpdateData()                                                                               //Pass Variables Lock and Position to the DataManager
    {
        DMReference.EditShovableObj(ObjectIndex, Lock_State, Position);
    }



    //Unlock this Object with a Key (Item or ShovablePosition)
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void Unlock_Object()                                                                        //Call on Object Interaction to check for Unlock
    {
        if (UnlockMethod == 1)                                                                          //If the Unlock Method is 1 use ItemUnlock
        {
            AcquireUnlock IUReference = null;                                                              //Create a ItemUnlock Variable, which will be used to access the CallItemUnlock Method
            IUReference = (AcquireUnlock)UnSReference;                                                     //Convert the Parent UnlockScript Type(UnSReference) into the ItemUnlock Type 
            IUReference.CallAcquiredUnlock(ObjectList_ID, ObjectIndex);                                     //Create a ItemUnlock Variable, which will be used to access the CallItemUnlock Method
        }
        if (UnlockMethod == 2)                                                                          //If the Unlock Method is 2 use ShovableUnlock
        {
            ShovableUnlock IUReference = null;                                                          //Create a ItemUnlock Variable, which will be used to access the CallItemUnlock Method
            IUReference = (ShovableUnlock)UnSReference;                                                 //Convert the Parent UnlockScript Type(UnSReference) into the ItemUnlock Type 
            IUReference.CallShovableUnlock(ObjectList_ID, ObjectIndex);                                 //Create a ItemUnlock Variable, which will be used to access the CallItemUnlock Method
        }
    }


    //Functions
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void OnMouseOver()  //This is the Main Function Controller
    {
        if (Input.GetMouseButtonDown(0))
        {
            //print("I'm called1");
            Unlock_Object();                                                                                                                        //Try to Unlock the Object
            FetchData(DataManager.Shovable_List[ObjectIndex].Stored_Lock_State, DataManager.Shovable_List[ObjectIndex].Stored_Position);     //Fetch new State from DataManager

            if (Lock_State == false)
            {

            }
        }
    }

    //SequenceUnlock
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void ObjectSequenceUnlock()
    {
        SeqUReference.CallSequenceUnlock();             //Call Sequence Unlock Method in Sequence Unlock Script
    }


    //Object Specific Functionality
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

}
