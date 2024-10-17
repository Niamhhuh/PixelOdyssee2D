using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceUnlock : MonoBehaviour
{
    public int OtherUnlockList_ID;
    public int OtherUnlockObject_ID;
    public DataManager DMReference;
    public ObjectScript ObjReference = null;

    void Awake()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        ObjReference = this.GetComponent<ObjectScript>();
        ObjReference.CanSequenceUnlock = true;
    }

    public void CallSequenceUnlock()                                                                        //Method is called in ObjectMainScript, takes Object_ID and Object_List
    {
        DMReference.UnlockbySequence(OtherUnlockList_ID, OtherUnlockObject_ID);                             //Call UnlockbyItem in DataManager, add required Key Item ID
    }
}
