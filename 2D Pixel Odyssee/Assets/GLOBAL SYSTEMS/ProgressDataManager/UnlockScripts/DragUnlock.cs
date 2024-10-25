using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragUnlock : MonoBehaviour, IDropHandler
{
    public int Key_ID;     //Change

    public DataManager DMReference;
    public ObjectScript ObjReference = null;

    void Awake()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        ObjReference = this.GetComponent<ObjectScript>();                                                   //Fetch Object script from this Script (Collectable, Portal, Shovable...)
        ObjReference.UnlockMethod = 0;                                                                      //Set UnlockMethod in Object Script to 2 (Unlock by Item)
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null )  //When Item is dropped, check if the Object is unlocked
        {
            print("I'm called");
            ObjReference.Lock_State = false;
            //DataManager.Draggable_List.RemoveAt(eventData.pointerDrag.GetComponent<Draggable>().ObjectIndex);
        }
    }
}
