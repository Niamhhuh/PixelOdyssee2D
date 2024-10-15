using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovableUnlock : UnlockScript
{
    public int Key_Shovable_ID;
    public int Unlock_Position;

    public DataManager DMReference;
    public ObjectScript ObjReference = null;

    void Awake()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();                      //Find and Connect to DataManager
        ObjReference = this.GetComponent<ObjectScript>();                                                               //Fetch Object script from this Script (Collectable, Portal, Shovable...)
        ObjReference.UnlockMethod = 1;                                                                                  //Set UnlockMethod in Object Script to 1 (Unlock by Shove_Position)
    }

    public void CallShovableUnlock(int UnlockList_ID, int UnlockObject_Index)                                           //Method is called in ObjectMainScript, takes Object_!!INDEX!! and Object_List
    {
        DMReference.UnlockbyPosition(UnlockList_ID, UnlockObject_Index, Key_Shovable_ID, Unlock_Position);
    }
}
