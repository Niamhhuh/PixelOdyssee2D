using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : ObjectScript
{
    //Variables which are passed onto DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public bool Traversed = false;                                                          //relevant to remember whether this door has been used already

    public int LoadScene_ID;

    //Object Data Management
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Awake()
    {
        ObjectList_ID = 3;
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        SeqUReference = this.GetComponent<SequenceUnlock>();
        UnSReference = this.GetComponent<UnlockScript>();
        ObjReference = this.GetComponent<Portal>();

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


    public void UpdateData()                                                                               //Pass Variables Lock and Traversed to the DataManager
    {
        DMReference.EditPortalObj(ObjectIndex, Lock_State, Traversed);
    }


    //Functions
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Call_Interact()
    {
        Unlock_Object();                                                                                                                        //Try to Unlock the Object
        FetchData(DataManager.Portal_List[ObjectIndex].Stored_Lock_State, DataManager.Portal_List[ObjectIndex].Stored_Traversed);     //Fetch new State from DataManager
        PointerScript.StartCoroutine(PointerScript.CallEnableInput());
        PointerScript.StartCoroutine(PointerScript.CallEnableInteract());

        DataManager.ToInteract.RemoveAt(0);                                                            //Remove the Shovable from the ToShove List
        GameObject.FindGameObjectWithTag("InteractionController").SetActive(false);                    //Deactivate the Shove Arrows

        if (Lock_State == false)
        {
            ClearHighlight();
            ObjectSequenceUnlock();
            PassTriggerActivate(1); //This won't work for dialogue, because the Scene will be reloaded
            SwitchScene();
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


    private void SwitchScene()                                                                              //Pick up the Item by adding it to the Draggable List.
    {
        if (LoadScene_ID >= 0 && LoadScene_ID < SceneManager.sceneCountInBuildSettings)
        {
            Traversed = true;
            UpdateData();
            SceneManager.LoadScene(LoadScene_ID);
        }
    }
}
