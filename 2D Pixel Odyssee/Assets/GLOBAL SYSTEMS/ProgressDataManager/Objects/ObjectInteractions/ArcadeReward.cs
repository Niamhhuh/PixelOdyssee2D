using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeReward : MonoBehaviour
{
    //Access the DataManager and an Add Item. Call this in the Win of an Arcade Game.
    public int Reward_ID; 
    DataManager DMReference;
    bool alreadyCompleted;

    private void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();

    }

    public void GrantReward ()
    {
        foreach (DataManager.DraggableObj Item in DataManager.Draggable_List)
        {
            if(Item.Stored_ID == Reward_ID)
            {
                alreadyCompleted = true;
                break;
            }
        }
        
        if(alreadyCompleted == false)
        {
            DMReference.AddDraggableObj(Reward_ID, 0);                                      //Call the AddDraggableObj Method in DataManager, to add a new DataContainer.
        }
    }

}
