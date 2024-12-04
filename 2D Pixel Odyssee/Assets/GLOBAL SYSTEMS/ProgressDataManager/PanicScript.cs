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

    public void UnlockCharacter()
    {
        DMReference.MoveScript.InTriggerDialogue = false;
        DMReference.MoveScript.EnableInput();
        DMReference.MoveScript.EnableInteract();

        DataManager.DisableClipboard = false;
        DataManager.DisableCharacterSwap = false;
        DMReference.UpdateUI();
    }
}
