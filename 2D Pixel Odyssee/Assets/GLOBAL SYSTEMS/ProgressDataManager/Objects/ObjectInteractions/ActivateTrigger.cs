using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTrigger : MonoBehaviour
{
    DataManager DMReference;
    public int Trigger_ID;

    public bool InteractionTriggered;
    public bool LockTriggered;
    public bool DialogueTriggered;

    private void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
    }

    public void CallTriggerActivation(int TriggerType)      //Call this from Object Script Call_Interaction or from AdvanceDialogue -> object must check if it has ActivateTrigger Attached
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
                    
                    DMReference.TriggerActivate(Trigger_ID);        //Call Trigger activate in DataManager -> on success, this also diables Pointer Interact + Input

                }
                break;
            default:
                break;
        }
    }
}