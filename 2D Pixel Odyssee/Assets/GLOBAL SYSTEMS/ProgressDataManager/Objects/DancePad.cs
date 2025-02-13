using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DancePad : ObjectScript
{
    //Variables which are passed onto DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public int Shove_Position = 0;                                                            //relevant to remember the position in the room
    public int Max_Shove;
    public GameObject PadController;
    public GameObject ShoveBox;

    public int[] TargetInput;

    //Object Data Management
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        ObjectList_ID = 7;
        //DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        SeqUReference = this.GetComponent<SequenceUnlock>();
        UnSReference = this.GetComponent<UnlockScript>();
        ObjReference = this.GetComponent<DancePad>();


        PadController = GameObject.FindGameObjectWithTag("DanceControl");

        int currentIndex = 0;                                                                               //remember the currently inspected Index

        foreach (DataManager.DancePadObj StoredObj in DataManager.DancePad_List)                            //Go through the Shovable_List and compare ShovableObj.
        {
            if (ID == StoredObj.Stored_ID)
            {
                FetchData(StoredObj.Stored_Lock_State, StoredObj.Stored_AlreadyTalked);                          //Fetch ObjectInformation from DataManager 
                ObjectIndex = currentIndex;                                                                 //Fetch the Index of the found Object
                NewObject = false;                                                                          //Confirm the Object is already available in DataManager
                break;
            }
            currentIndex++;                                                                                 //Update the currently inspected Index
        }
        if (NewObject == true)                                                                              //If required, pass ObjectInformation to DataManager.
        {
            DMReference.AddDancePadObj(ID, Lock_State, AlreadyTalked);                                           //Call the AddShovableObj Method in DataManager, to add a new DataContainer.
            ObjectIndex = DataManager.DancePad_List.Count - 1;                                                  //When an Object is added, it is added to the end of the list, making its Index I-1.
        }

        ToggleSprites();
    }



    public void FetchData(bool Stored_Lock_State, bool Stored_AlreadyTalked)                                     //Fetch the Variables Lock and Position from the DataManager
    {
        Lock_State = Stored_Lock_State;
        AlreadyTalked = Stored_AlreadyTalked;
        //print(StoredObj.Stored_Type_ID);
    }


    public void UpdateData()                                                                               //Pass Variables Lock and Position to the DataManager
    {
        DMReference.EditDancePadObj(ObjectIndex, Lock_State, AlreadyTalked);
    }

    //Shovable specific position on Load Method
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    //Functions
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void Call_Interact()                                                                                                                 
    {
        Unlock_Object();                                                                                                                        //Try to Unlock the Object
        FetchData(DataManager.DancePad_List[ObjectIndex].Stored_Lock_State, DataManager.DancePad_List[ObjectIndex].Stored_AlreadyTalked);      //Fetch new State from DataManager
        PointerScript.StartCoroutine(PointerScript.CallEnableInput());
        PointerScript.StartCoroutine(PointerScript.CallEnableInteract());

        DataManager.ToInteract.RemoveAt(0);                                                            //Remove the Shovable from the ToShove List
        GameObject.FindGameObjectWithTag("InteractionController").SetActive(false);                    //Deactivate the Shove Arrows

        if (Lock_State == false && DMReference.CurrentCharacter.RosieActive == true)
        {
            ObjectSequenceUnlock();
            //PassTriggerActivate(1); This was moved to line 155 (Movex) to execute after the shove is completed
            InitiateDance();
        }
        else
        {
            ClearHighlight();
            //PassTriggerActivate(2);
            StartCoroutine(FlashRed());
        }
    }

    private void OnTriggerExit2D(Collider2D other)                                                          //This Function Overwrites ObjectScript.OnTriggerExit2D and therefor must reimplement the standard Funtion to deactivate the Interaction Buttons
    {
        if (other.CompareTag("Player") && 0 < DataManager.ToInteract.Count && DataManager.ToInteract[0] == this)    //If the player leaves the Collider and ther is an Object in the To InteractList, which is this Object
        {
            DataManager.ToInteract.RemoveAt(0);                                                                     //Clear Object from ToInteract List

            if (InteractionController != null)                                                                      //If the Interaction Buttons are available
            {
                InteractionController.SetActive(false);                                                             //Disable them 
            }
        }

        if (other.CompareTag("Player"))                                                                     //This Part handles the toggle of the DancePad Buttons
        {
            ClearHighlight();                                                                               //Clear Highlight when moving away
            if(DataManager.ToDance.Count > 0)                                                               //If the Object is in the ToShove List
            {
                DataManager.ToDance.RemoveAt(0);                                                            //Remove it
            }
            if(PadController != null)                                                                     //If the ShoveButtons are available
            {
                PadController.SetActive(false);                                                           //Disable them
            }
        }
    }

    private void InitiateDance()
    {
        DataManager.ToDance.Add(this);                                                                      //add this object to the ToShove List, to make it accessible for the Shove Buttons
        PadController.SetActive(true);                                                                    //Activate ShoveButtons 

        PadController.GetComponent<DanceScript>().ControlButtons();                                       //Control which Buttons appear

        //Activate the required Arrows 
        PadController.transform.position = this.transform.position;
    }


    public void StartMove(Vector3 StartPosition, Vector3 TargetPosition)//, int Direction)
    {
        StartCoroutine(Movex(StartPosition, TargetPosition));
    }

    private IEnumerator Movex(Vector3 StartPosition, Vector3 TargetPosition)
    {
        float new_x = 0;
        while (Mathf.Abs(transform.position.x - TargetPosition.x) > 0.01f)
        {
            new_x += Time.deltaTime;
            transform.position = Vector3.Lerp(StartPosition, TargetPosition, Mathf.SmoothStep(0f,30f,new_x/3));
            
            yield return null;
        }
        
        transform.position = TargetPosition;                                                                        //ensure the Object is at the right position in the end
        PassTriggerActivate(1);
        //add Animation transform.scale animation, requires another coroutine which playe

        DMReference.MoveScript.EnableInput();                                                                     //reactivate Mouse Input
    }

}
