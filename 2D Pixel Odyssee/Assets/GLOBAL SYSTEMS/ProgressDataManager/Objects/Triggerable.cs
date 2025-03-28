using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fades;                                //enables the usage of functions from the script "Fades"


public class Triggerable : ObjectScript
{
    //Variables which are passed onto DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public bool Trigger_Passed;			                                                //relevant to control Item Spawn
    public bool ForceDialogue;			                                            //relevant to Trigger Dialogue on Interact
    public bool GhostTrigger;                                                                                                                                                                     //HERE

    public bool isTrigger_Portal;
    public int EventScene_ID = 0;                                                       //Set Target Scene
    public int SpawnPointID;                                                            //Set Spawnpoint

    //Object Data Management
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        ObjectList_ID = 6;
        //DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        SeqUReference = this.GetComponent<SequenceUnlock>();
        UnSReference = this.GetComponent<UnlockScript>();
        ObjReference = this.GetComponent<Triggerable>();

        int currentIndex = 0;                                                                               //remember the currently inspected Index

        foreach (DataManager.TriggerableObj StoredObj in DataManager.Triggerable_List)                      //Go through the EventSource_List and check EventObj.
        {
            if (ID == StoredObj.Stored_ID)
            {
                FetchData(StoredObj.Stored_Lock_State, StoredObj.Stored_AlreadyTalked, StoredObj.Stored_Trigger_Passed);                    //Fetch ObjectInformation from DataManager 
                ObjectIndex = currentIndex;                                                                 //Fetch the Index of the found Object
                NewObject = false;                                                                          //Confirm the Object is already available in DataManager
                break;
            }
            currentIndex++;                                                                                 //Update the currently inspected Index
        }
        if (NewObject == true)                                                                              //If required, pass ObjectInformation to DataManager.
        {
            DMReference.AddTriggerableObj(ID, Lock_State, AlreadyTalked, Trigger_Passed, this.gameObject);                 //Call the AddTriggerableObj Method in DataManager, to add a new DataContainer.
            ObjectIndex = DataManager.Triggerable_List.Count - 1;                                        //When an Object is added, it is added to the end of the list. 
        }

        DMReference.TriggeredObjects_List.Add(gameObject);      //Always add a fresh GameObject on Scene Load

        if (Lock_State == true && !GhostTrigger)
        {
            gameObject.SetActive(false);
        }
        if (Lock_State == false && GhostTrigger && !Trigger_Passed)
        {
            TriggerInteract();
            //gameObject.SetActive(false);
        }

        ToggleSprites();
        CallColliderToggle();
        RemoveTrigger();                                                                                       //Remove Event if it has been interacted with already
    }



    public void FetchData(bool Stored_Lock_State, bool Stored_AlreadyTalked, bool Stored_Trigger_Passed)                                  //Fetch the Variables Lock and Event_Passed from the DataManager
    {
        Lock_State = Stored_Lock_State;
        AlreadyTalked = Stored_AlreadyTalked;
        Trigger_Passed = Stored_Trigger_Passed;
    }


    public void UpdateData()                                                                               //Pass Variables Lock and Event_Passed to the DataManager
    {
        DMReference.EditTriggerableObj(ObjectIndex, Lock_State, AlreadyTalked, Trigger_Passed);
    }


    //EventSource Item specific delete on Load Funtion
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void RemoveTrigger()                                                                               //Remove the Event when it is or has been interacted with
    {
        if (Trigger_Passed == true)
        {
            if(DMReference.MoveScript != null)
            {
                DMReference.MoveScript.StartCoroutine(PointerScript.CallEnableInput());                                                //Enable Input when Trigger is cleared
                DMReference.MoveScript.StartCoroutine(PointerScript.CallEnableInteract());                                             //Enable Interact when Trigger is cleared
            }

            if (DataManager.ToInteract.Count > 0 && DataManager.ToInteract[0] == this)
            {
                DataManager.ToInteract.RemoveAt(0);                                                            //Remove the Shovable from the ToShove List
            }

            gameObject.SetActive(false);
        }
    }


    //Functions
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    public void OnMouseOver()
    {
        DMReference.MoveScript.DisableInput();                                                //Enable Input when Trigger is cleared
        DMReference.MoveScript.DisableInteract();                                             //Enable Interact when Trigger is cleared
        if (Input.GetMouseButtonUp(0) && Trigger_Passed == false)
        {
            //Unlock_Object();                                                                                                                        //Try to Unlock the Object
            FetchData(DataManager.Triggerable_List[ObjectIndex].Stored_Lock_State, DataManager.Triggerable_List[ObjectIndex].Stored_AlreadyTalked, DataManager.Triggerable_List[ObjectIndex].Stored_Trigger_Passed);  //Fetch new State from DataManager
            //PointerScript.StartCoroutine(PointerScript.CallEnableInput());
            //PointerScript.StartCoroutine(PointerScript.CallEnableInteract());

            //DataManager.ToInteract.RemoveAt(0);                                                            //Remove the Shovable from the ToShove List
            //GameObject.FindGameObjectWithTag("InteractionController").SetActive(false);                    //Deactivate the Shove Arrows

            if (Lock_State == false)
            {
                ClearHighlight();
                TriggerInteract();
            } 
        }
    }

    //Object Specific Functionality
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void SwitchScene()                                                                              //Pick up the Item by adding it to the Draggable List.
    {
        if (EventScene_ID >= 0 && EventScene_ID < SceneManager.sceneCountInBuildSettings)
        {
            DataManager.SpawnID = SpawnPointID;
            DataManager.LastRoom = EventScene_ID;
            StartCoroutine(switchSceneRoutine());                                                           //NEU --> now leads to coroutine for clean fade-in
        }
    }

    private IEnumerator switchSceneRoutine() {                                                              //NEU --> this is part of the above function
        yield return StartCoroutine(Class_Fades.instance.StartFadeIn()); // Wait for fade-in to finish     ----------------------NEU---------------------
        SceneManager.LoadScene(EventScene_ID);
    }


    public void TriggerInteract()                                                                                                                    //Interact with the Event to end it.
    {
        Trigger_Passed = true;    //Perhaps this will be changed into an Interger -> remember event state.     --> Kimi's note: It's called integer, get it right
        PassTriggerActivate(1); //This won't work for dialogue, because the Scene will be reloaded
        ObjectSequenceUnlock();
        UpdateData();

        SuccessfulInteract();

        //If Force_Dialogue -> Trigger Dialogue
        //Remove Trigger when StepNum = Dialogue Length.
        //
        if (ForceDialogue == true)
        {
            DMReference.MoveScript.DisableInput();                                  //Disable Inpput 
            DMReference.MoveScript.DisableInteract();                               //Disable Interact 
            DMReference.MoveScript.InTriggerDialogue = true;
            GetComponent<NPCDialogue>().advancedDialogueManager.TurnOffDialogue();
            GetComponent<NPCDialogue>().advancedDialogueManager.ForceDialogue(gameObject.GetComponent<NPCDialogue>());
            GetComponent<NPCDialogue>().advancedDialogueManager.ContinueDialogue();
            /*
            if (!DMReference.DialogueManager.dialogueCanvas.activeSelf)
            {
                DMReference.DialogueManager.dialogueCanvas.SetActive(true);
            }
            */
            print("Called");
        }
        if (isTrigger_Portal)
        {
            SwitchScene();
        }

        if (!ForceDialogue && !isTrigger_Portal)
        {
            RemoveTrigger();
        }
    }
}
