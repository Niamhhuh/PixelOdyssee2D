using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalObject : MonoBehaviour
{
    public int ID;                                          //ID of the Goal
    public bool Completed;                                  //Status of the Goal
    public bool Available;                                  //Mark the Goal as Available

    public int Slot;                                        //Slot in Goal List
    public int ObjectIndex;

    public Sprite CompletedGoal; 
    private Image CurrentImage;
    private RectTransform GoalPosition;
    private GameObject GoalListStart;

    // Start is called before the first frame update
    void Awake()
    {
        CurrentImage = gameObject.GetComponent<Image>();
        GoalPosition = gameObject.GetComponent<RectTransform>();
        GoalListStart = GameObject.FindGameObjectWithTag("GoalListStart");
    }

    public void FetchData()
    {
        //Fetch Data From DataManager
        int currentIndex = 0;
        foreach (DataManager.ActiveGoal StoredObj in DataManager.ActiveGoal_List)                           //Go through the Draggable_List and check DraggableObj.
        {
            if (ID == StoredObj.Stored_ID)
            {
                Completed = StoredObj.Stored_Completed;                                                     //Fetch Completed State from DataManager -> Completed is only Edited by Outside Objects
                ObjectIndex = currentIndex;                                                                 //Fetch the Index of the found Object
                break;
            }
            currentIndex++;                                                                                 //Update the currently inspected Index
        }
    }

    public void SetUpGoal()                                                                                 //when an Item already has an assigned Slot, Place it there
    {

        if( GoalPosition == null)
        {
            CurrentImage = gameObject.GetComponent<Image>();
            GoalPosition = gameObject.GetComponent<RectTransform>();
            GoalListStart = GameObject.FindGameObjectWithTag("GoalListStart");
        }

        //Check Completion State
        if (Completed)                                                                                       //Completed Goals are Crossed Out
        {
            CurrentImage.sprite = CompletedGoal;
        }

        //Position the Goal on the Canvas
        GoalPosition.transform.position = GoalListStart.GetComponent<RectTransform>().transform.position;
        GoalPosition.transform.position = new Vector2(GoalPosition.position.x, GoalPosition.position.y - (Slot-1) * 90);          //adjust by Slot Shift -38 y per Slot
    }

    public void UpdateData()
    {

    }
}
