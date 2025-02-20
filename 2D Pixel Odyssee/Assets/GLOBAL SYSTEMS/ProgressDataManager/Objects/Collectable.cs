using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : ObjectScript
{
    //Variables which are passed onto DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    Transform Collectables;

    public bool Collected;			                                                //relevant to control Item Spawn
    private bool alreadyAdded;

    public int RewardPosition;

    //Object Data Management
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        ObjectList_ID = 1;
        //DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        SeqUReference = this.GetComponent<SequenceUnlock>();
        UnSReference = this.GetComponent<UnlockScript>();
        ObjReference = this.GetComponent<Collectable>();

        Collectables = transform.parent;

        int currentIndex = 0;                                                                               //remember the currently inspected Index

        
        foreach (DataManager.CollectableObj StoredObj in DataManager.Collectable_List)                      //Go through the Collectable_List and check CollectableObj.
        {
            if (ID == StoredObj.Stored_ID)
            {
                FetchData(StoredObj.Stored_Lock_State, StoredObj.Stored_AlreadyTalked, StoredObj.Stored_Collected);                         //Fetch ObjectInformation from DataManager 
                ObjectIndex = currentIndex;                                                                 //Fetch the Index of the found Object
                NewObject = false;                                                                          //Confirm the Object is already available in DataManager
                break;
            }
            currentIndex++;                                                                                 //Update the currently inspected Index
        }
        if (NewObject == true)                                                                              //If required, pass ObjectInformation to DataManager.
        {
            DMReference.AddCollectableObj(ID, Lock_State, AlreadyTalked, Collected);                                       //Call the AddCollectableObj Method in DataManager, to add a new DataContainer.
            ObjectIndex = DataManager.Collectable_List.Count - 1;                                           //When an Object is added, it is added to the end of the list. 
        }

        ToggleSprites();
        CallColliderToggle();
        RemoveItem();                                                                                       //Remove Items if they have been collected already


        if (IsReward == true && Lock_State == true)                                                         //Handle Reward Case
        {
           TryAddReward();
        }
    }

    public void FetchData(bool Stored_Lock_State, bool Stored_AlreadyTalked, bool Stored_Collected)                                   //Fetch the Variables Lock and Collected from the DataManager
    {
        Lock_State = Stored_Lock_State;
        AlreadyTalked = Stored_AlreadyTalked;
        Collected = Stored_Collected;
    }


    public void UpdateData()                                                                               //Pass Variables Lock and Collected to the DataManager
    {
        DMReference.EditCollectableObj(ObjectIndex, Lock_State, AlreadyTalked, Collected);
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


    public void Call_Interact()
    {
        Unlock_Object();                                                                                                                        //Try to Unlock the Object
        FetchData(DataManager.Collectable_List[ObjectIndex].Stored_Lock_State, DataManager.Collectable_List[ObjectIndex].Stored_AlreadyTalked, DataManager.Collectable_List[ObjectIndex].Stored_Collected);     //Fetch new State from DataManager
        PointerScript.StartCoroutine(PointerScript.CallEnableInput());
        PointerScript.StartCoroutine(PointerScript.CallEnableInteract());

        DataManager.ToInteract.RemoveAt(0);                                                            //Remove the Shovable from the ToShove List
        GameObject.FindGameObjectWithTag("InteractionController").SetActive(false);                    //Deactivate the Shove Arrows

        if (Lock_State == false && AlwaysDenyInteraction == false)
        {
            ClearHighlight();
            PassTriggerActivate(1);
            ObjectSequenceUnlock();
            PickUp();
        }
        else
        {
            ClearHighlight();
            StartCoroutine(FlashRed());
        }
    }

    //Object Specific Functionality
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    private void PickUp()                                                            //Pick up the Item by adding it to the Draggable List.
    {
        if(DataManager.Inventory_Fillstate < 12)
        {
            SuccessfulInteract();
            DataManager.Inventory_Fillstate++;
            //print(DataManager.Inventory_Fillstate);
            DMReference.AddDraggableObj(ID, 0);                                      //Call the AddDraggableObj Method in DataManager, to add a new DataContainer.
            Collected = true;
            UpdateData();
            DMReference.DisplayObjectNameScript.DeactivateNameDisplay();
            RemoveItem();
        }
    }


    private void TryAddReward()
    {
        DMReference.RewardObjects.Add(gameObject);
        foreach (Collectable StoredObjScript in DataManager.RewardList)                      //Go through the Collectable_List and check CollectableObj.
        {
            if (StoredObjScript.ID == ID)
            {
                alreadyAdded = true;
                break;
            }
        }

        if (!alreadyAdded)
        {
            DataManager.RewardList.Add(CoreObject.GetComponent<Collectable>());
        }
        gameObject.SetActive(false);
    }
}
