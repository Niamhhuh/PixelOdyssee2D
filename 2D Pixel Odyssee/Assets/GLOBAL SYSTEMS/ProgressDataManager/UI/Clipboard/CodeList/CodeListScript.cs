using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeListScript : MonoBehaviour
{
    public GameObject CodeListObj;
    private DataManager DMReference;

    static bool Code1Acquired;
    static bool Code2Acquired;
    static bool Code3Acquired;

    private GameObject Code1;
    private GameObject Code2;
    private GameObject Code3;


    // Start is called before the first frame update
    void Start()
    {
        CodeListObj = GameObject.FindGameObjectWithTag("CodeList");
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager

        Code1 = GameObject.FindGameObjectWithTag("Code1");
        Code2 = GameObject.FindGameObjectWithTag("Code2");
        Code3 = GameObject.FindGameObjectWithTag("Code3");

        CodeListObj.SetActive(false);
    }

    public void CallCodeList()
    {
        DMReference.MoveScript.ClipboardActive = true;
        DMReference.MoveScript.DisableInput();
        CodeListObj.SetActive(true);
        
        ControllCodePanel();
    }

    public void CloseCodeList()
    {
        StartCoroutine(DMReference.MoveScript.CallEnableInput());
        StartCoroutine(DMReference.MoveScript.CallEnableInteract());
        DMReference.MoveScript.ClipboardActive = false;
        CodeListObj.SetActive(false);
    }


    public void ControllCodePanel()
    {
        if (Code1Acquired == true)
        {
            Code1.SetActive(true);
        }else
        {
            Code1.SetActive(false);
        }

        if (Code2Acquired == true)
        {
            Code2.SetActive(true);
        }else
        {
            Code2.SetActive(false);
        }

        if (Code3Acquired == true)
        {
            Code3.SetActive(true);
        }else
        {
            Code3.SetActive(false);
        }
    }

}
