using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantRewardScript : MonoBehaviour
{
    DataManager DMReference;
    ObjectScript ThisObjectScript;
    public int Reward_ID;
    private void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        ThisObjectScript = GetComponent<ObjectScript>();
    }
    
    public void GrantReward()
    {
        DMReference.ActivateReward(Reward_ID);
    }
    
}
