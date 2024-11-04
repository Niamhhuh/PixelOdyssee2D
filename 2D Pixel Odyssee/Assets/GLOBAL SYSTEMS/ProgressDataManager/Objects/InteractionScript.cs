using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    private UiToMouse PointerScript;
    private void Start()
    {
        PointerScript = GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>();
    }
    //Call Dialouge
    public void TriggerDialogue()
    {
        DataManager.ToInteract[0].GetComponent<NPCDialogue>().advancedDialogueManager.ContinueDialogue();

        PointerScript.StartCoroutine(PointerScript.CallEnableInput());
        DataManager.ToInteract.RemoveAt(0);                                                            //Remove the Shovable from the ToShove List
        GameObject.FindGameObjectWithTag("InteractionController").SetActive(false);                    //Deactivate the Shove Arrows
        //GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>().DisableInput();
    }


    //Call Interaction

    public void TriggerInteraction ()
    {
        switch (DataManager.ToInteract[0].ObjectList_ID)                  //
        {
            //Turn ObjectScript into Specific Object
            case 1:
                //print("I'm called2");
                Collectable ColReference = null;                                                    //Create a Reference Variable, which will be used to access the Collectable.Call_Interact Method
                ColReference = (Collectable)DataManager.ToInteract[0].ObjReference;                 //Convert the Parent ObjectScript Type(ObjReference) into the Collectable Type 
                ColReference.Call_Interact();                                                       //Call Collectable.Call_Interact
                break;
            case 2:
                Shovable ShovReference = null;                                                      //Create a Reference Variable, which will be used to access the Shovable.Call_Interact Method
                ShovReference = (Shovable)DataManager.ToInteract[0].ObjReference;                   //Convert the Parent ObjectScript Type(ObjReference) into the Shovable Type 
                ShovReference.Call_Interact();                                                      //Call Shovable.Call_Interact
                break;
            case 3:
                Portal PortReference = null;                                                        //Create a Reference Variable, which will be used to access the Portal.Call_Interact Method
                PortReference = (Portal)DataManager.ToInteract[0].ObjReference;                     //Convert the Parent ObjectScript Type(ObjReference) into the Portal Type 
                PortReference.Call_Interact();                                                      //Call Portal.Call_Interact
                break;
            case 4:
                Switchable SwitchReference = null;                                                   //Create a Reference Variable, which will be used to access the Switchable.Call_Interact Method
                SwitchReference = (Switchable)DataManager.ToInteract[0].ObjReference;                //Convert the Parent ObjectScript Type(ObjReference) into the Switchable Type 
                SwitchReference.Call_Interact();                                                     //Call Switchable.Call_Interact
                break;
            case 5:
                EventSource EventReference = null;                                                   //Create a Reference Variable, which will be used to access the EventSource.Call_Interact Method
                EventReference = (EventSource)DataManager.ToInteract[0].ObjReference;                //Convert the Parent ObjectScript Type(ObjReference) into the EventSource Type 
                EventReference.Call_Interact();                                                      //Call EventSource.Call_Interact
                break;
        }
    }
}
