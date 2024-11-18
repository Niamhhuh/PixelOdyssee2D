using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedDialogue : MonoBehaviour
{
    public AdvancedDialogueSO RosieUnlockDialogue;
    public AdvancedDialogueSO BeBeUnlockDialogue;

    NPCDialogue ObjectDialogue = null;
    ObjectScript AccessObjectScript = null;

    void Start () 
    {

        AccessObjectScript = gameObject.GetComponent<ObjectScript>();
        ObjectDialogue = gameObject.GetComponent<NPCDialogue>();
        
    } 

    public void ModifyDialogue () 
    {

        if(AccessObjectScript.Lock_State == false) 
        {
            if(RosieUnlockDialogue != null) {ObjectDialogue.conversation[0] = RosieUnlockDialogue;}
            if(BeBeUnlockDialogue != null) {ObjectDialogue.conversation[1] = BeBeUnlockDialogue;}
        }

    }
}
