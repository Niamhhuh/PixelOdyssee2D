using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanicScript : MonoBehaviour
{
    DataManager DMReference;

    void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
    }

    private void Update()
    {
        print(DataManager.DisableClipboard);

        print(DataManager.DisableCharacterSwap);
        DMReference.UpdateUI();
    }
}
