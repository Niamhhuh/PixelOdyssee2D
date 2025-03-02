using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalListScript : MonoBehaviour
{
    public GameObject GoalListObj;
    private DataManager DMReference;
    private GoalScroll ScrollScript;
    // Start is called before the first frame update
    void Start()
    {
        GoalListObj = GameObject.FindGameObjectWithTag("GoalList");
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        ScrollScript = GameObject.FindGameObjectWithTag("ScrollControll").GetComponent<GoalScroll>();
        ScrollScript.GoalListContainer = GameObject.FindGameObjectWithTag("GoalListContainer");                          //Find the List of Goals in Scroll Script -> this is set here, to prevent load error!
        GoalListObj.SetActive(false);
    }

    public void CallGoalList()
    {
        DMReference.MoveScript.ClipboardActive = true;
        DMReference.MoveScript.DisableInput();
        GoalListObj.SetActive(true);

        //Fetch Active Goals
        //---------------------------------------------------------------------------------------------------------------------------------------------------
        FetchGoals();


        //Call Goal Position Function
        //---------------------------------------------------------------------------------------------------------------------------------------------------
        PlaceGoals();
        ScrollScript.GoalListContainer.transform.position = DataManager.ContainerStartPosition;


        ScrollScript.GoalListContainer.transform.position = new Vector2(GameObject.FindGameObjectWithTag("ScrollArea").GetComponent<RectTransform>().position.x, GameObject.FindGameObjectWithTag("ScrollArea").GetComponent<RectTransform>().position.y + DataManager.CurrentScroll * 90);          //adjust by Slot Shift -38 y per Slot
        
        //Call Goal Completed Function
        //---------------------------------------------------------------------------------------------------------------------------------------------------

    }



    public void FetchGoals()                                                                     //Activate all collected, active Items
    {
        int Counter = 0;
        foreach (GoalObject Goal in DataManager.GoalObject_List)                                 //Deactivate all Goals
        {
            Goal.gameObject.SetActive(false);
        }

        foreach (DataManager.ActiveGoal Goal in DataManager.ActiveGoal_List)                     //Search through the ActiveGoal_List, to which Goals are added, when they have been added 
        {
            Counter++;
            DataManager.GoalObject_List[Goal.Stored_ID - 1].Available = true;                    //For each Goal in the ActiveGoal_List, access the GoalObject_List (which is sorted by ID, check DataManager Awake) -> Set the Goal with the matching ID as available
            DataManager.GoalObject_List[Goal.Stored_ID - 1].Slot = Counter;                      //Set the Slot of the Goal Object 
            DataManager.GoalObject_List[Goal.Stored_ID - 1].gameObject.SetActive(true);          //Activate the Goal
        }
    }




    public void PlaceGoals()
    {
        foreach (GoalObject Goal in DataManager.GoalObject_List)                                 //Call Item Functions for all found Items
        {
            if (Goal.Available == true)                                                          //If the Item is available
            {
                Goal.FetchData();                                                                //Refresh Item Data 
                Goal.SetUpGoal();                                                                //Place Goal into List
            }   
        }
    } 




    public void CloseGoalList()
    {
        StartCoroutine(DMReference.MoveScript.CallEnableInput());
        StartCoroutine(DMReference.MoveScript.CallEnableInteract());
        DMReference.MoveScript.ClipboardActive = false;
        GoalListObj.SetActive(false);
    }


}
