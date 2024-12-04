using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchStateUnlock : UnlockScript
{
    public int Key_Switch_ID;                                                                               //ID of the switch, which unlocks this object
    public bool Unlock_SwitchState;                                                                         //Active State of the Switch (On / Off)

    public DataManager DMReference;
    public ObjectScript ObjReference = null;

    void Awake()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        ObjReference = this.GetComponent<ObjectScript>();                                                   //Fetch Object script from this Script (Collectable, Portal, Shovable...)
        ObjReference.UnlockMethod = 1;                                                                      //Set UnlockMethod in Object Script to 2 (Unlock by Item)
    }

    public void CallSwitchStateUnlock(int UnlockList_ID, int UnlockObject_Index)                                  //Method is called in ObjectMainScript, takes Object_!!INDEX!! and Object_List
    {
        DMReference.UnlockbySwitchState(UnlockList_ID, UnlockObject_Index, Key_Switch_ID, Unlock_SwitchState);                           //Call UnlockbyItem in DataManager, add required Key Item ID
    }
}
