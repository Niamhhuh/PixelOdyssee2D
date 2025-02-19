using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    ObjectScript ThisObject;
    DataManager DMReference;
    
    public bool OffwhenLocked;

    private void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        ThisObject = gameObject.GetComponent<ObjectScript>();
        ThisObject.ColliderScript = this;
    }

    public void ToggleCollider()      //Call this from Object Script Call_Interaction or from AdvanceDialogue -> object must check if it has ActivateTrigger Attached
    {
        if(OffwhenLocked)
        {
            if(ThisObject.Lock_State)
            {
                gameObject.GetComponent<Collider2D>().enabled = false;
            }
            if (!ThisObject.Lock_State)
            {
                gameObject.GetComponent<Collider2D>().enabled = true;
            }
        }

        if (!OffwhenLocked)
        {
            if (ThisObject.Lock_State)
            {
                gameObject.GetComponent<Collider2D>().enabled = true;
            }
            if (!ThisObject.Lock_State)
            {
                gameObject.GetComponent<Collider2D>().enabled = false;
            }
        }
    }

}
