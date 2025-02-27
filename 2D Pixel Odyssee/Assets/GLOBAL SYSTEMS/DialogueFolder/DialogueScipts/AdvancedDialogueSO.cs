using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AdvancedDialogueSO : ScriptableObject
{
    public bool ProgressDialogue;                                           //Control Dialogue
    public int DialogueID;

    public bool UnlockableOptions;                                          //Control Dialogue Option Buttons
    public int KeyOption1;
    public int KeyOption2;
    public int KeyOption3;
    public int KeyOption4;

    public bool CallTrigger;

    public bool Dialogue_Goal;
    public bool DialogueAdd_Goal;
    public bool Complete_Goal;
    public int Goal_ID;


    public DialogueActors[] actors;

    [Tooltip("Only needed if Random is selected as the actor name")]
    [Header("Random Actor Info")]
    public string randomActorName;
    public Sprite randomActorPortait;

    [Header("Dialogue")]
    [TextArea]
    public string[] dialogue;

    [Tooltip("The words that will appear on the option buttons")]
    public string[] optionText;

    public AdvancedDialogueSO option0;
    public AdvancedDialogueSO option1;
    public AdvancedDialogueSO option2;
    public AdvancedDialogueSO option3;

    private void Awake()
    {
        if(!ProgressDialogue)
        {
            DialogueID = 0;
        }

        if (!UnlockableOptions)
        {
            KeyOption1 = 0;
            KeyOption2 = 0;
            KeyOption3 = 0;
            KeyOption4 = 0;
        }
    }

    public void ControlGoalonDialogue()
    {
        if (DialogueAdd_Goal)
        {
            DataManager.ActiveGoal_List.Add(new DataManager.ActiveGoal { Stored_ID = Goal_ID, Stored_Completed = false});                                                          //Call the AddDraggableObj Method in DataManager, to add a new DataContainer.
        }

        if (Complete_Goal)
        {
            Complete_The_Goal();
        }
    }

    private void Complete_The_Goal()                                                            //Search through the Goal List to set the correct Goal to Completed
    {
        foreach (DataManager.ActiveGoal Goal in DataManager.ActiveGoal_List)
        {
            if (Goal.Stored_ID == Goal_ID)                                                      //Compare List_Goal_ID with TargetGoal_ID
            {
                Goal.Stored_Completed = true;                                                   //On Match, set Stored Goal as completed
                break;
            }
        }
    }
}
