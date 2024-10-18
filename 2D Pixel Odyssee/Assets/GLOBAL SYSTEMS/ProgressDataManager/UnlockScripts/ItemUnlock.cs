using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUnlock : UnlockScript
{
    public int Key_Acquired_ID;

    public DataManager DMReference;
    public ObjectScript ObjReference = null;

    void Awake()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        ObjReference = this.GetComponent<ObjectScript>();                                                   //Fetch Object script from this Script (Collectable, Portal, Shovable...)
        ObjReference.UnlockMethod = 1;                                                                      //Set UnlockMethod in Object Script to 2 (Unlock by Item)
    }

    public void CallItemUnlock (int UnlockList_ID, int UnlockObject_Index)                                  //Method is called in ObjectMainScript, takes Object_!!INDEX!! and Object_List
    {
        //print("List:" + UnlockList_ID + "Index:" + UnlockObject_Index + "Key:" + Key_Acquired_ID);
        DMReference.UnlockbyItem(UnlockList_ID, UnlockObject_Index, Key_Acquired_ID);                           //Call UnlockbyItem in DataManager, add required Key Item ID
    }
}
