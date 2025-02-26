using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceUnlock : MonoBehaviour
{
    public bool Active_Unlock;
    public int OtherUnlockList_ID;
    public int [] OtherUnlockObject_ID;
    public DataManager DMReference = null;
    public ObjectScript ObjReference = null;

    public GameObject TargetObject;

    void Awake()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        ObjReference = gameObject.GetComponent<ObjectScript>();
        ObjReference.CanSequenceUnlock = true;
    }

    public void CallSequenceUnlock()                                                                        //Method is called in ObjectMainScript, takes Object_ID and Object_List
    {
        foreach (int UnlockObject_ID in OtherUnlockObject_ID)
        {
            DMReference.UnlockbySequence(OtherUnlockList_ID, UnlockObject_ID);                             //Call UnlockbyItem in DataManager, add required Key Item ID
        }
        

        if (Active_Unlock && TargetObject != null && TargetObject.activeInHierarchy == true)
        {
           TargetObject.GetComponent<ObjectScript>().FetchAllData();

            if (TargetObject.GetComponent<ObjectScript>().TriggeronUnlock)
            {
                DMReference.MoveScript.targetPosition = DMReference.MoveScript.player.position;
                DataManager.ToInteract.Clear();
                DataManager.ToInteract.Add(TargetObject.GetComponent<ObjectScript>());

                //if (UnlockDialogueScript != null) { UnlockDialogueScript.ModifyDialogue(); }                //Modify the Dialogue if unique Un/LockedObject Dialogue is available

                ObjReference.InteractionController.SetActive(true);
                ObjReference.InteractionController.transform.GetChild(0).gameObject.SetActive(false);                     //Enable Dialogue Button 
                ObjReference.InteractionController.transform.GetChild(1).gameObject.SetActive(false);                     //Enable Interact Button 
                ObjReference.InteractionController.GetComponent<InteractionScript>().TriggerInteraction();
            }
        }

    }

}
