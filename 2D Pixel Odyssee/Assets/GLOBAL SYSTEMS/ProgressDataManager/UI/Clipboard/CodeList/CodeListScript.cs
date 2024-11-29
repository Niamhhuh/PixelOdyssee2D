using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeListScript : MonoBehaviour
{
    public GameObject CodeListObj;
    private DataManager DMReference;

    // Start is called before the first frame update
    void Start()
    {
        CodeListObj = GameObject.FindGameObjectWithTag("CodeList");
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        CodeListObj.SetActive(false);
    }

    public void CallCodeList()
    {
        DMReference.MoveScript.ClipboardActive = true;
        DMReference.MoveScript.DisableInput();
        CodeListObj.SetActive(true);
    }

    public void CloseCodeList()
    {
        StartCoroutine(DMReference.MoveScript.CallEnableInput());
        StartCoroutine(DMReference.MoveScript.CallEnableInteract());
        DMReference.MoveScript.ClipboardActive = false;
        CodeListObj.SetActive(false);
    }

}
