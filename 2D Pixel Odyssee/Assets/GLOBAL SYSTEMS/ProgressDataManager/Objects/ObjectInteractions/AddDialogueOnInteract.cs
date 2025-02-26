using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDialogueOnInteract : MonoBehaviour
{
    ObjectScript ThisObject;
    public int Dialogue_ID;

    private void Start()
    {
        ThisObject = gameObject.GetComponent<ObjectScript>();
        ThisObject.InteractTransformDialogue = this;
    }

    public void AddTransformDialogue()
    {
        bool ID_Found = false;

        foreach (int ID in DataManager.ProgressDialogueList)
        {
            if(ID == Dialogue_ID)
            {
                ID_Found = true;
                break;
            }
        }
        if(!ID_Found)
        {
            DataManager.ProgressDialogueList.Add(Dialogue_ID);
        }
    }
}
