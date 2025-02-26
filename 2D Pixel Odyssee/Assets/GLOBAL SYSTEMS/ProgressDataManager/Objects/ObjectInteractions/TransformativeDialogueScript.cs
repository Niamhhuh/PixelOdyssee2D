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

    int TotalConditions = 0;

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
            CondtionsMet = 0;
            if (!ConditionNotFound)
            {
                CycleDialogueConditions(i);
            }
        }
        FetchNewDialogue();
    }



    private void CycleDialogueConditions(int i)                                 //Step through Conditions
    {
        for (int o = 0; o < StepArray[i].ConditoinArray.Length; ++o)
        {
            CompareIDtoConditions(i, o);
        }
        if(!ConditionNotFound)
        {
            TotalConditions = CondtionsMet;
        }
    }



    private void CompareIDtoConditions(int i, int o)                            //Check Conditions
    {
        bool MatchedID = false;
        foreach (int DialogueID in DataManager.ProgressDialogueList)
        {
            if (DialogueID == StepArray[i].ConditoinArray[o])                   //compare to ProgressDialogue List in DataManager
            {
                StepPosition = i;                                               //Remember the last Step, where Conditions where met.
                MatchedID = true;
                CondtionsMet++;                                                 //Advance the Condition Counter
                break;
            }
        }
        if (!MatchedID)
        {
            ConditionNotFound = true;
        }
    }


    private void FetchNewDialogue()
    {
        if (StepArray[StepPosition].ConditoinArray.Length == TotalConditions)
        {
            if(TransformDialogue.Length > StepPosition*2)
            {
                StoredObjectDialogue.conversation[0] = TransformDialogue[StepPosition * 2];
                StoredObjectDialogue.conversation[1] = TransformDialogue[StepPosition * 2 + 1];
                StepPosition = 0;
                CondtionsMet = 0;
                ConditionNotFound = false;
            }
        }
        StepPosition = 0;
        CondtionsMet = 0;
        ConditionNotFound = false;

    }
}