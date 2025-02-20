using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering;
//using static UnityEditor.Progress;

public class LockedDialogue : MonoBehaviour
{
    public AdvancedDialogueSO RosieLockDialogue;
    public AdvancedDialogueSO BeBeLockDialogue;

    NPCDialogue ObjectDialogue = null;
    ObjectScript AccessObjectScript = null;


    public bool ModifyDeniedInteraction;

    public int TargetItemID;

    public AdvancedDialogueSO RosieDeniedInteraction;
    public AdvancedDialogueSO BeBeDeniedInteraction;

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

    public void CollectedItemModifyDeniedInteraction()
    {
        if(ModifyDeniedInteraction)
        {

            foreach (Draggable Item in DataManager.Item_List)                                       //Activate Collected Items
            {
                if (TargetItemID == Item.ID && Item.Available)
                {
                    if (AccessObjectScript.Lock_State == true)
                    {
                        if (RosieLockDialogue != null) { ObjectDialogue.conversation[2] = RosieDeniedInteraction; }
                        if (BeBeLockDialogue != null) { ObjectDialogue.conversation[3] = BeBeDeniedInteraction; }
                    }
                }
            }

        }

    }
}
