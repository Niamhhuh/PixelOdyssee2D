using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switchable : ObjectScript
{
    //Variables which are passed onto DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public bool SwitchState;                                                                                //relevant to remember whether this Switch is active or inactive
    
    public GameObject SwitchObject;

    private EventInstance ObjectLocked;  //Sound

    //Object Data Management
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        ObjectList_ID = 4;
        SeqUReference = this.GetComponent<SequenceUnlock>();
        UnSReference = this.GetComponent<UnlockScript>();
        ObjReference = this.GetComponent<Switchable>();

        int currentIndex = 0;                                                                               //remember the currently inspected Index

        ObjectLocked = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.ObjectLocked); //Sound

        foreach (DataManager.SwitchStateObj StoredObj in DataManager.SwitchState_List)                      //Go through the SwitchState_List and Fetch SwitchStateObj.
        {
            if (ID == StoredObj.Stored_ID)
            {
                FetchData(StoredObj.Stored_Lock_State, StoredObj.Stored_AlreadyTalked, StoredObj.Stored_SwitchState);                       //Fetch ObjectInformation from DataManager 
                ObjectIndex = currentIndex;                                                                 //Fetch the Index of the found Object
                NewObject = false;                                                                          //Confirm the Object is already available in DataManager
                break;
            }
            currentIndex++;                                                                                 //Update the currently inspected Index
        }
        if (NewObject == true)                                                                              //If required, pass ObjectInformation to DataManager.
        {
            DMReference.AddSwitchStateObj(ID, Lock_State, AlreadyTalked, SwitchState);                                     //Call the AddSwitchStateObj Method in DataManager, to add a new DataContainer.
            ObjectIndex = DataManager.SwitchState_List.Count - 1;                                           //When an Object is added, it is added to the end of the list, making its Index I-1.
        }
        FlipSwitch();
        ToggleSprites();
        CallColliderToggle();
    }



    public void FetchData(bool Stored_Lock_State, bool Stored_AlreadyTalked, bool Stored_SwitchState)                                 //Fetch the Variables Lock and Traversed from the DataManager
    {
        Lock_State = Stored_Lock_State;
        AlreadyTalked = Stored_AlreadyTalked;
        SwitchState = Stored_SwitchState;
        //print(StoredObj.Stored_Type_ID);
    }


    public void UpdateData()                                                                               //Pass Variables Lock and Traversed to the DataManager
    {
        DMReference.EditSwitchStateObj(ObjectIndex, Lock_State, AlreadyTalked, SwitchState);
    }


    //Functions
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Call_Interact()
    {
        Unlock_Object();                                                                                                                        //Try to Unlock the Object
        FetchData(DataManager.SwitchState_List[ObjectIndex].Stored_Lock_State, DataManager.SwitchState_List[ObjectIndex].Stored_AlreadyTalked, DataManager.SwitchState_List[ObjectIndex].Stored_SwitchState);   //Fetch new State from DataManager
        PointerScript.StartCoroutine(PointerScript.CallEnableInput());
        PointerScript.StartCoroutine(PointerScript.CallEnableInteract());

        DataManager.ToInteract.RemoveAt(0);                                                            //Remove the Shovable from the ToShove List
        GameObject.FindGameObjectWithTag("InteractionController").SetActive(false);                    //Deactivate the Shove Arrows

        if (Lock_State == false && AlwaysDenyInteraction == false)
        {
            ClearHighlight();
            PassTriggerActivate(1);                                                                     //Call Activate Trigger -> checks if ActivateTrigger is attached, then continues (1 = Trigger acitvated by interaction)
            ObjectSequenceUnlock();
            FlipSwitch();
        } else
        {
            ClearHighlight();
            ObjectLocked.start(); //Sound
            PassTriggerActivate(2);                                                                     //Call Activate Trigger -> checks if ActivateTrigger is attached, then continues (2 = Trigger acitvated by Object locked)
            StartCoroutine(FlashRed());
        }
    }


    //Object Specific Functionality
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    private void FlipSwitch()                                                                              //Pick up the Item by adding it to the Draggable List.
    {    
        if (SwitchState == false)
        {
            ObjectSprite.enabled = false;
            ObjectSprite = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();     //the alternate Object is Child 2                   (Child 1 reserved for Standard Highlight)            
            ObjectSprite.enabled = true;
            HighlightonHover = gameObject.transform.GetChild(2).gameObject;                     //the alternate Highlight Object is Child 3         (Child 1 reserved for Standard Highlight) 
            SwitchState = true;
        } else
        {
            ObjectSprite.enabled = false;
            ObjectSprite = transform.GetComponent<SpriteRenderer>();                            //the standard Object                                          
            ObjectSprite.enabled = true;
            HighlightonHover = gameObject.transform.GetChild(0).gameObject;                     //the standard Highlight Object is Child 1          
            SwitchState = false;
        }
        UpdateData();
        SuccessfulInteract();
    }
}
