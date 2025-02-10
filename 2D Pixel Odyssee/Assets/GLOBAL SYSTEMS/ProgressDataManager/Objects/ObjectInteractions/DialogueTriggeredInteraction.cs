using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggeredInteraction : MonoBehaviour
{
    //This script is attached to an Object
    //The object attaches the Script to itself
    //This script is triggered in the Advanced Dialogue Manager as an alternative to ActivateTrigger
    //This script actives the Object's interaction when the required Dialogue has been reached


    ObjectScript ThisObject;
    //DataManager DMReference;


    void Start()
    {
       // DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        ThisObject = gameObject.GetComponent<ObjectScript>();
        ThisObject.DialogueInteractionScript = this;
    }

    public void TriggerObjectInteraction()
    {
        ThisObject.DialogueInteraction();
    }
}
