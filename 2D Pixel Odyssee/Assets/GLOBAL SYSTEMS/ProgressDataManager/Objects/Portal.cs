using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    //Variables which are passed onto DataManager
    public int Type_ID;			                                                    //ID of the Type, required to choose the list 
    public int ID;                                                                  //ID of the Object, required to find it in the list
    public int Lock_State;                                                          //check if this Object is Interaction_Locked/Limited
    //public(Dialogue)			                                                    //Dialogue of this object

    public bool Traversed;			                                                    //relevant to remember whether this door has been used already

    //Local Variables, not saved in the DataManager
    //public sprite Highlight;		                                               	//store the highlight sprite of this object
    public int Key_List;			                                                //reference the List in which the Key is found
    public int Key_ID;                                                              //reference the ID of the Key 
    public int SourceRoom;                                                          //reference to the Room in which tis Object is instantiated
    public bool NewObject = true;
    public DataManager DMReference;

    //DataManager.Rooms_Loaded[SourceRoom] == false             use this for "Onetime Events"


    //Object Data Management
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Awake()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager

        foreach (DataManager.PortalObj StoredObj in DataManager.Portal_List)                                //Go through the Portal_List and cech PortalObj.
        {
            if (ID == StoredObj.Stored_ID)
            {
                FetchData(StoredObj.Stored_Lock_State, StoredObj.Stored_Traversed);                         //Fetch ObjectInformation from DataManager 
                print("ID Found:" + ID);
                NewObject = false;                                                                          //Confirm the Object is already available in DataManager
                break;
            }
        }
        if (NewObject == true)                                                                              //If required, pass ObjectInformation to DataManager.
        {
            DMReference.AddPortalObj(Type_ID, ID, Lock_State, Traversed);                                   //Call the AddPortalObj Method in DataManager, to add a new DataContainer.
            print("ID Added:" + ID);
        }

    }



    private void FetchData(int Stored_Lock_State, bool Stored_Traversed)                                    //Fetch the Variables Lock and Traversed from the DataManager
    {
        Lock_State = Stored_Lock_State;
        Traversed = Stored_Traversed;
        //print(StoredObj.Stored_Type_ID);
    }


    //Object Functionality
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


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
