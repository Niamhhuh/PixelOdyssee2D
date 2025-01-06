using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDialogue : MonoBehaviour
{
    public AdvancedDialogueSO RosieLockDialogue;
    public AdvancedDialogueSO BeBeLockDialogue;

    NPCDialogue ObjectDialogue = null;
    ObjectScript AccessObjectScript = null;

    void Start () 
    {

        AccessObjectScript = gameObject.GetComponent<ObjectScript>();
        ObjectDialogue = gameObject.GetComponent<NPCDialogue>();
        
    } 

    public void ModifyDialogue () 
    {

        if(AccessObjectScript.Lock_State == true) 
        {
            if(RosieLockDialogue != null) {ObjectDialogue.conversation[0] = RosieLockDialogue;}
            if(BeBeLockDialogue != null) {ObjectDialogue.conversation[1] = BeBeLockDialogue;}
        }

    }
}
