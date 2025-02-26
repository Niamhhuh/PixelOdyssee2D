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

    private AdvancedDialogueSO OriginalRosie;
    private AdvancedDialogueSO OriginalBebe;

    private AdvancedDialogueSO OriginalRosieDenied;
    private AdvancedDialogueSO OriginalBebeDenied;


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

        OriginalRosie = ObjectDialogue.conversation[0];
        OriginalBebe = ObjectDialogue.conversation[1];

        if(ObjectDialogue.conversation.Length > 2)
        {
            OriginalRosieDenied = ObjectDialogue.conversation[2];
            OriginalBebeDenied = ObjectDialogue.conversation[3];

        }

    } 

    public void ModifyDialogue () 
    {

        if(AccessObjectScript.Lock_State == true) 
        {

            if (RosieLockDialogue != null) {ObjectDialogue.conversation[0] = RosieLockDialogue;}
            if(BeBeLockDialogue != null) {ObjectDialogue.conversation[1] = BeBeLockDialogue;}
        }

        if (AccessObjectScript.Lock_State == false)
        {
            if (RosieLockDialogue != null) { ObjectDialogue.conversation[0] = OriginalRosie; }
            if (BeBeLockDialogue != null) { ObjectDialogue.conversation[1] = OriginalBebe; }
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

                    if (AccessObjectScript.Lock_State == false)
                    {
                        if (RosieLockDialogue != null) { ObjectDialogue.conversation[2] = OriginalRosieDenied; }
                        if (BeBeLockDialogue != null) { ObjectDialogue.conversation[3] = OriginalBebeDenied; }
                    }
                }
            }

        }

    }
}
