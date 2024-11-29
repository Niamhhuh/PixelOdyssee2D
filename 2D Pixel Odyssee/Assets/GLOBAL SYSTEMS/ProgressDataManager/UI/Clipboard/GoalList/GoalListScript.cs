using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalListScript : MonoBehaviour
{
    public GameObject GoalListObj;
    private DataManager DMReference;

    // Start is called before the first frame update
    void Start()
    {
        GoalListObj = GameObject.FindGameObjectWithTag("GoalList");
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        GoalListObj.SetActive(false);
    }

    public void CallGoalList()
    {
        DMReference.MoveScript.ClipboardActive = true;
        DMReference.MoveScript.DisableInput();
        GoalListObj.SetActive(true);
    }

    public void CloseGoalList()
    {
        StartCoroutine(DMReference.MoveScript.CallEnableInput());
        StartCoroutine(DMReference.MoveScript.CallEnableInteract());
        DMReference.MoveScript.ClipboardActive = false;
        GoalListObj.SetActive(false);
    }


}
