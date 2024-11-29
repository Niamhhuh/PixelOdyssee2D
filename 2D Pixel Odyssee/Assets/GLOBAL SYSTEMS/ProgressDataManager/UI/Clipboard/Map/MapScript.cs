using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapScript : MonoBehaviour
{
    public GameObject MapObj;
    private GameObject RoomPlanObj;
    private DataManager DMReference;



    // Start is called before the first frame update
    void Start()
    {
        MapObj = GameObject.FindGameObjectWithTag("Map");
        RoomPlanObj = GameObject.FindGameObjectWithTag("RoomPlan");
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        MapObj.SetActive(false);
    }


    public void CallMap()
    {
        DMReference.MoveScript.ClipboardActive = true; 
        DMReference.MoveScript.DisableInput();
        MapObj.SetActive(true);
        RoomPlanObj.transform.GetChild(DMReference.currentRoom-1).gameObject.SetActive(true);
    }

    public void CloseMap()
    {
        StartCoroutine(DMReference.MoveScript.CallEnableInput());
        StartCoroutine(DMReference.MoveScript.CallEnableInteract());
        DMReference.MoveScript.ClipboardActive = false;
        MapObj.SetActive(false);
    }


}
