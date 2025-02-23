using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DataManager;


[System.Serializable] 
public class ConditionArray
{
    public int[] ConditoinArray;
}


public class TransformativeDialogueScript : MonoBehaviour
{
    //This script edits the Dialogue of a Script in case it changes based on Gameplay Progression.
    //

    public NPCDialogue StoredObjectDialogue;

    int StepPosition = 0;
    int CondtionsMet = 0;
    bool ConditionNotFound = false;

    public ConditionArray[] StepArray;
    public AdvancedDialogueSO[] TransformDialogue;                              
    


    // Start is called before the first frame update
    void Start()
    {
        StoredObjectDialogue = gameObject.GetComponent<NPCDialogue>();          //
    }


    public void TransformeDialogue()                                            //Step through ConditionList
    {
        for (int i = 0; i < StepArray.Length; ++i)                              //
        {
            if(!ConditionNotFound)
            {
                CondtionsMet = 0;
                StepPosition = i;                                               //Remember the last Step, where Conditions where met.
                CycleDialogueConditions(i);
            }
        }
        FetchNewDialogue();
    }



    private void CycleDialogueConditions(int i)                                 //Step through Conditions
    {
        for (int o = 0; o < StepArray[i].ConditoinArray.Length; o++)
        {
            CompareIDtoConditions(i, o);
        }
    }



    private void CompareIDtoConditions(int i, int o)                            //Check Conditions
    {
        foreach (int DialogueID in DataManager.ProgressDialogueList)
        {
            if (DialogueID == StepArray[i].ConditoinArray[o])                   //compare to ProgressDialogue List in DataManager
            {
                CondtionsMet++;                                                 //Advance the Condition Counter
                break;
            }
            ConditionNotFound = true;
        }
    }


    private void FetchNewDialogue()
    {
        print("-------------------------------------------");
        print("Conditions:" + CondtionsMet);
        print("StepPosition:" + StepPosition);
        print("-------------------------------------------");
        if (StepArray[StepPosition].ConditoinArray.Length == CondtionsMet)
        {
            if(TransformDialogue.Length > StepPosition*2)
            {
                StoredObjectDialogue.conversation[0] = TransformDialogue[StepPosition * 2];
                StoredObjectDialogue.conversation[1] = TransformDialogue[StepPosition * 2 + 1];
                StepPosition = 0;
                CondtionsMet = 0;
                ConditionNotFound = false;
                print("Dialogue1:" + StoredObjectDialogue.conversation[0]);
                print("Dialogue2:" + StoredObjectDialogue.conversation[1]);
                print("-------------------------------------------");
                print("-------------------------------------------");
                print("-------------------------------------------");
            }
        }
        StepPosition = 0;
        CondtionsMet = 0;
        ConditionNotFound = false;

    }
}
