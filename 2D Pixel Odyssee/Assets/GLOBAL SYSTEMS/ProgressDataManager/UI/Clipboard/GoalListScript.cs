using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalListScript : MonoBehaviour
{
    private GameObject GoalListObj;
    private DataManager DMReference;

    // Start is called before the first frame update
    void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
    }


}
