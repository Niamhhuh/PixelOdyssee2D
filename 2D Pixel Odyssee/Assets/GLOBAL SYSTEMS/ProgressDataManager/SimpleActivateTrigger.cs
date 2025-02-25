using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleActivateTrigger : MonoBehaviour
{
    DataManager DMReference;
    public int Trigger_ID;

    private void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
    }

    public void CallTriggerActivation()
    {
        DMReference.TriggerActivate(Trigger_ID);        //Call Trigger activate in DataManager -> on success, this also diables Pointer Interact + Input
    }
}