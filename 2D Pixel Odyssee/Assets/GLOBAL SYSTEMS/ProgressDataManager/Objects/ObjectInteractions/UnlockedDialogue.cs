using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedDialogue : MonoBehaviour
{
    public AdvancedDialogueSO RosieUnlockDialogue;
    public AdvancedDialogueSO BeBeUnlockDialogue;

    public NPCDialogue ObjectDialogue;
    public Collectable ThisObject;

    void start () {

        //ThisObject = gameObject.GetComponent<Collectable>();
        //ObjectDialogue = gameObject.GetComponent<NPCDialogue>();
        
    } 

    public void ModifyDialogue () {

        if(ThisObject.Lock_State == false) {
            if(RosieUnlockDialogue != null) {ObjectDialogue.conversation[0] = RosieUnlockDialogue;}
            if(BeBeUnlockDialogue != null) {ObjectDialogue.conversation[1] = BeBeUnlockDialogue;}
        }

    }
}
