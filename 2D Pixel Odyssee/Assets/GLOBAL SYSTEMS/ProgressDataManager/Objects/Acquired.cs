using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acquired : MonoBehaviour
{
    //Variables which are passed onto DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public int ID;				                                                    //ID of the Object, required to find it in the list

    public int Slot;                                                                //relevant to control the position in the Inventory

    //Local Variables, not saved in the DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
   
    private int ObjectIndex;                                 //Likely Redundant     //Index of this Object in its list                          //used for UnlockMethods

    private bool NewObject = true;

    private DataManager DMReference;
    private SequenceUnlock SeqUReference = null;                                     //MIGHT BE DELETED
    private UnlockScript UnSReference = null;                                        //MIGHT BE DELETED

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
                FetchData(StoredObj.Stored_Slot);                              //Fetch ObjectInformation from DataManager 
                ObjectIndex = currentIndex;                                                                 //Fetch the Index of the found Object
                NewObject = false;                                                                          //Confirm the Object is already available in DataManager
                break;
            }
            currentIndex++;                                                                                 //Update the currently inspected Index
        }
        if (NewObject == true)                                                                              //If required, pass ObjectInformation to DataManager.
        {
            DMReference.AddAcquiredObj(ID, Slot);                                               //Call the AddAcquiredObj Method in DataManager, to add a new DataContainer.
            ObjectIndex = DataManager.Acquired_List.Count - 1;                                                  //When an Object is added, it is added to the end of the list, making its Index I-1.
        }

    }



    private void FetchData(int Stored_Slot)                                          //Fetch the Variables Lock and Slot from the DataManager
    {
        Slot = Stored_Slot;
    }


    private void UpdateData()                                                                               //Pass Variables Lock and Slot to the DataManager
    {
        DMReference.EditAcquiredObj(ObjectIndex, Slot);
    }

    //Functions
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


}
