using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTrigger : MonoBehaviour
{
    DataManager DMReference;
    public int Trigger_ID;

    private void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
    }
    private void OnMouseOver()
    {
        if(Input.GetMouseButtonUp(0))
        {
            DMReference.TriggerActivate(Trigger_ID);
        }
    }

}
