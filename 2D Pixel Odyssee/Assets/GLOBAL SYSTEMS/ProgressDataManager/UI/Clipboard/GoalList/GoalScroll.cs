using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScroll : MonoBehaviour
{
    public static int CurrentScroll = 0;
    public static int MaxScroll = 0;
    public DataManager DMReference;
    public GameObject GoalListContainer;
    public static Vector2 ContainerStartPosition = new Vector2(1000,10);

    // Start is called before the first frame update
    void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        GoalListContainer = GameObject.FindGameObjectWithTag("GoalListContainer");                          //Find the List of Goals

        if(ContainerStartPosition.x == 1000 && ContainerStartPosition.y == 10)
        {
            ContainerStartPosition = GoalListContainer.transform.position;
        }
    }


    public void Down()
    {
        IncreaseScrollRange();
        if (CurrentScroll < MaxScroll)
        {
            CurrentScroll++;
            GoalListContainer.transform.position = new Vector2(GoalListContainer.transform.position.x, GoalListContainer.transform.position.y + 90);          //adjust by Slot Shift -38 y per Slot
        }
    }

    public void Up()
    {
        IncreaseScrollRange();
        if (CurrentScroll > 0)
        {
            CurrentScroll--;
            GoalListContainer.transform.position = new Vector2(GoalListContainer.transform.position.x, GoalListContainer.transform.position.y - 90);          //adjust by Slot Shift -38 y per Slot
        }
    }

    public void IncreaseScrollRange()
    {
        if(DataManager.ActiveGoal_List.Count > 6)
        {
            MaxScroll = DataManager.ActiveGoal_List.Count - 6;
        }
    }


}
