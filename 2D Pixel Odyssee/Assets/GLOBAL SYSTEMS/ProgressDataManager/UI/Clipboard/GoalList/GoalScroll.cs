using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScroll : MonoBehaviour
{
    public DataManager DMReference;
    public GameObject GoalListContainer;

    // Start is called before the first frame update
    void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        GoalListContainer = GameObject.FindGameObjectWithTag("GoalListContainer");                          //Find the List of Goals

        if(DataManager.CurrentScroll == 0)
        {
            print("Set Start");
            DataManager.ContainerStartPosition = GoalListContainer.transform.position;
        }
    }

    public void Down()
    {
        IncreaseScrollRange();
        if (DataManager.CurrentScroll < DataManager.MaxScroll)
        {
            DataManager.CurrentScroll++;
            GoalListContainer.transform.position = new Vector2(GoalListContainer.transform.position.x, GoalListContainer.transform.position.y + 90);          //adjust by Slot Shift -38 y per Slot
        }
    }

    public void Up()
    {
        IncreaseScrollRange();
        if (DataManager.CurrentScroll > 0)
        {
            DataManager.CurrentScroll--;
            GoalListContainer.transform.position = new Vector2(GoalListContainer.transform.position.x, GoalListContainer.transform.position.y - 90);          //adjust by Slot Shift -38 y per Slot
        }
    }

    public void IncreaseScrollRange()
    {
        if(DataManager.ActiveGoal_List.Count > 6)
        {
            DataManager.MaxScroll = DataManager.ActiveGoal_List.Count - 6;
        }
    }


}
