using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : ObjectScript
{
    //Variables which are passed onto DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public int ID;				                                                    //ID of the Object, required to find it in the list
    public bool Lock_State;                                                         //check if this Object is Interaction_Locked/Limited
    //public(Dialogue)			                                                    //Dialogue of this object

    public bool Collected;			                                                //relevant to control Item Spawn

    //Local Variables, not saved in the DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    private int ObjectList_ID = 1;                                                  //ID which marks the List this Object is stored in          //used for UnlockMethods
    public int ObjectIndex;                                                        //Index of this Object in its list                          //used for UnlockMethods


    private int SourceRoom;                                                         //reference to the Room in which tis Object is instantiated
    private bool NewObject = true;

    private DataManager DMReference;                                                 //
    private SequenceUnlock SeqUReference = null;                                     //
    private UnlockScript UnSReference = null;                                        //


    public bool Player_Detected = false;                                                                                                                                    //NEW
    //public BoxCollider2D Interaction_Collider;                                                                                                                              //NEW

    //DataManager.Rooms_Loaded[SourceRoom] == false             use this for "Onetime Events"


    //Object Data Management
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Awake()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
       // Interaction_Collider = InteractionDetector.GetComponent<BoxCollider2D>();                           //Find the InteractionRange Collider                            //NEW
        SeqUReference = this.GetComponent<SequenceUnlock>();
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



    //Unlock this Object with a Key (Item or ShovablePosition)
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void Unlock_Object()                                                                        //Call on Object Interaction to check for Unlock
    {
        if (UnlockMethod == 1)                                                                           //If the Unlock Method is 1 use ItemUnlock
        {
            //print("I'm called2");
            AcquireUnlock IUReference = null;                                                              //Create a ItemUnlock Variable, which will be used to access the CallItemUnlock Method
            IUReference = (AcquireUnlock)UnSReference;                                                     //Convert the Parent UnlockScript Type(UnSReference) into the ItemUnlock Type 
            IUReference.CallAcquiredUnlock(ObjectList_ID, ObjectIndex);                                     //Create a ItemUnlock Variable, which will be used to access the CallItemUnlock Method
        }
        if (UnlockMethod == 2)                                                                           //If the Unlock Method is 2 use ShovableUnlock
        {
            ShovableUnlock IUReference = null;                                                          //Create a ItemUnlock Variable, which will be used to access the CallItemUnlock Method
            IUReference = (ShovableUnlock)UnSReference;                                                 //Convert the Parent UnlockScript Type(UnSReference) into the ItemUnlock Type 
            IUReference.CallShovableUnlock(ObjectList_ID, ObjectIndex);                                 //Create a ItemUnlock Variable, which will be used to access the CallItemUnlock Method
        }
    }




    //Functions
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            print("Hi");
            //Player_Detected = true;
        }
    }

    private void OnMouseOver()  //This is the Main Function Controller
    {
        if (Input.GetMouseButtonDown(0))
        {
            //print("I'm called1");
            Unlock_Object();                                                                                                                        //Try to Unlock the Object
            FetchData(DataManager.Collectable_List[ObjectIndex].Stored_Lock_State, DataManager.Collectable_List[ObjectIndex].Stored_Collected);     //Fetch new State from DataManager

            if (Lock_State == false)
            {
                PickUp();
                ObjectSequenceUnlock();
            }
        }
    }

    //SequenceUnlock
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void ObjectSequenceUnlock()
    {
        if (CanSequenceUnlock == true)
        {
            SeqUReference.CallSequenceUnlock();                                                             //Call Sequence Unlock Method in Sequence Unlock Script
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
