using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGoal : MonoBehaviour
{
    DataManager DMReference;
    ObjectScript ThisObject;

    public bool Dialogue_Triggered;
    public bool Interaction_Triggered;
    public bool ItemUnlock_Triggered;


    public int Goal_ID;
    public bool Add_Goal;

    public int Complete_Goal_ID;
    public bool Complete_Goal;

    void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        ThisObject = gameObject.GetComponent<ObjectScript>();                                               //
        ThisObject.TriggerGoal = true;                                                                      //
        ThisObject.ControlGoalScript = this;                                                                //Set GoalScript in ObjectScript

        if(Complete_Goal_ID == 0)
        {
            Complete_Goal_ID = Goal_ID;
        }
    }



    public void EditGoal()
    {
        if (Add_Goal)
        {

            DMReference.AddGoalObj(Goal_ID, false);                                                             //Call the AddDraggableObj Method in DataManager, to add a new DataContainer.
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
            if (Goal.Stored_ID == Complete_Goal_ID)                                                      //Compare List_Goal_ID with TargetGoal_ID
            {
                Goal.Stored_Completed = true;                                                   //On Match, set Stored Goal as completed
                break;
            }
        }
    }
}
