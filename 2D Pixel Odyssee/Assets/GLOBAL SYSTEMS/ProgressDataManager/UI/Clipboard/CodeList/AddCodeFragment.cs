using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCodeFragment : MonoBehaviour
{
    //private DataManager DMReference;
    //private ObjectScript AccessObjectScript;

    public int AddCodeID;

    private void Start()
    {
        //AccessObjectScript = gameObject.GetComponent<ObjectScript>();

                
        //CodeList = GameObject.FindGameObjectWithTag("UiCanvas").GetComponent<CodeListScript>();
    }

    public void AddCode()
    {
        switch (AddCodeID)
        {
            case 1:
                DataManager.Code1Acquired = true;
                break;
            case 2:
                DataManager.Code2Acquired = true;
                break;
            case 3:
                DataManager.Code3Acquired = true;
                break;
        }
    }
}
