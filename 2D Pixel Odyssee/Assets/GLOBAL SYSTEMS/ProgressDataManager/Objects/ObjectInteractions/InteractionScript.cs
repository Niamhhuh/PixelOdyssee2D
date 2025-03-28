using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class InteractionScript : MonoBehaviour
{
    private UiToMouse PointerScript;
    private DataManager DMReference;
    [HideInInspector] public AdvancedDialogueManager advancedDialogueManager;

    GameObject TempObject;
    public Animator animator;
    public Animator animatorBebe;

    private EventInstance ObjectPickUp;//Sound for Interaction
    private EventInstance DoorOpen;
    public EventInstance ObjectSlideRosie;
    private EventInstance DDRArrow;

    private void Start()
    {
        PointerScript = GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>();
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        advancedDialogueManager = GameObject.Find("DialogueManager").GetComponent<AdvancedDialogueManager>();


        ObjectPickUp = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.ObjectPickUp); //Sound
        DoorOpen = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.DoorOpen);
        ObjectSlideRosie = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.ObjectSlideRosie);
        DDRArrow = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.DDRArrow);
    }
    //Call Dialouge
    public void TriggerDialogue()
    {
        DMReference.DisplayObjectNameScript.DeactivateNameDisplay();
        //PointerScript.StartCoroutine(PointerScript.CallEnableInput());
        //PointerScript.StartCoroutine(PointerScript.CallEnableInteract());
        if (DataManager.ToInteract[0].GetComponent<NPCDialogue>() != null)
        {
            if (DataManager.ToInteract[0].TransformDialogueScript != null)
            {
                DataManager.ToInteract[0].TransformDialogueScript.TransformeDialogue();
            }

            advancedDialogueManager.InitiateDialogue(DataManager.ToInteract[0].GetComponent<NPCDialogue>());

            //DMReference.DialogueManager.dialogueActivated = true;

            if (TempObject != null && DataManager.ToInteract[0].CoreObject == TempObject)
            {
                DMReference.DialogueManager.InitiateDialogue(TempObject.GetComponent<NPCDialogue>());
            }
            
            DataManager.ToInteract[0].TalkedtoObject();
            DataManager.ToInteract[0].GetComponent<NPCDialogue>().advancedDialogueManager.canContinueText = true;
            DataManager.ToInteract[0].GetComponent<NPCDialogue>().advancedDialogueManager.ContinueDialogue();
        }
        TempObject = DataManager.ToInteract[0].CoreObject;
        DataManager.ToInteract.RemoveAt(0);                                                            //Remove the Shovable from the ToShove List
        GameObject.FindGameObjectWithTag("InteractionController").SetActive(false);                    //Deactivate the Shove Arrows
        if(TempObject != null && TempObject.GetComponent<Triggerable>() == null)
        {
            TempObject.GetComponent<ObjectScript>().CallInteractionButtons();
        }
        //GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>().DisableInput();
    }


        //Call Interaction

    public void CallAnimation()
    {
        animator.SetTrigger("interact");
        animatorBebe.SetTrigger("interact");
    }

        public void TriggerInteraction ()
    {
        //animator.SetTrigger("interact");
        //animatorBebe.SetTrigger("interact");
        //ToInteract[0].
        DMReference.DisplayObjectNameScript.DeactivateNameDisplay();
        switch (DataManager.ToInteract[0].ObjectList_ID)                  //
        {
            //Turn ObjectScript into Specific Object
            case 1:
                //print("I'm called2");
                ObjectPickUp.start(); //Sound
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
                DoorOpen.start(); //Sound
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
            case 6:
                Triggerable TriggerReference = null;                                                   //Create a Reference Variable, which will be used to access the EventSource.Call_Interact Method
                TriggerReference = (Triggerable)DataManager.ToInteract[0].ObjReference;                //Convert the Parent ObjectScript Type(ObjReference) into the EventSource Type 
                TriggerReference.TriggerInteract();                                                      //Call EventSource.Call_Interact
                break;
            case 7:
                DDRArrow.start(); //Sound
                DancePad DancePadReference = null;                                                   //Create a Reference Variable, which will be used to access the EventSource.Call_Interact Method
                DancePadReference = (DancePad)DataManager.ToInteract[0].ObjReference;                //Convert the Parent ObjectScript Type(ObjReference) into the EventSource Type 
                DancePadReference.Call_Interact();                                                      //Call EventSource.Call_Interact
                break;
        }   
    }
}
