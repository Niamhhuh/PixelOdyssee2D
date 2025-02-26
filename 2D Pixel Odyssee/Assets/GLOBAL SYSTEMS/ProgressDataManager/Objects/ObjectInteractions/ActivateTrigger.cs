using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTrigger : MonoBehaviour
{
    ObjectScript ThisObject;
    DataManager DMReference;
    public int Trigger_ID;

    public bool CharacterBound;

    public bool Rosie;
    public bool Bebe;


    public bool InteractionTriggered;
    public bool LockTriggered;
    public bool DialogueTriggered;

    public bool TriggerTutorialPanel;

    private EventInstance TutorialPopUp; //Sound

    private void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        ThisObject = gameObject.GetComponent<ObjectScript>();
        ThisObject.TriggerScript = this;
        TutorialPopUp = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.TutorialPopUp); //Sound
    }

    public void CallTriggerActivation(int TriggerType)      //Call this from Object Script Call_Interaction or from AdvanceDialogue -> object must check if it has ActivateTrigger Attached
    {
        if(TriggerTutorialPanel)
        {
            TutorialPopUp.start(); //Sound
        }
        
        if (CharacterBound)
        {
            if (Rosie && DataManager.RosieActive)
            {
                PassTriggerActivation(TriggerType);
            }

            if (Bebe && !DataManager.RosieActive)
            {
                PassTriggerActivation(TriggerType);
            }
        }

        if (!CharacterBound)
        {
            PassTriggerActivation(TriggerType);
        }
    }

    private void PassTriggerActivation(int TriggerType)
    {
        switch (TriggerType)
        {
            case 1:
                if (InteractionTriggered == true)
                {
                    DMReference.TriggerActivate(Trigger_ID);        //Call Trigger activate in DataManager -> on success, this also diables Pointer Interact + Input
                }
                break;
            case 2:
                if (LockTriggered == true)
                {
                    DMReference.TriggerActivate(Trigger_ID);        //Call Trigger activate in DataManager -> on success, this also diables Pointer Interact + Input

                }
                break;
            case 3:
                if (DialogueTriggered == true)
                {
                    print("Activated");
                    DMReference.TriggerActivate(Trigger_ID);        //Call Trigger activate in DataManager -> on success, this also diables Pointer Interact + Input

                }
                break;
            default:
                break;
        }
    }
}