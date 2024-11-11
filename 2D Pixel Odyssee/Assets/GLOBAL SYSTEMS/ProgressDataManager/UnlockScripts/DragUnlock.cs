using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragUnlock : MonoBehaviour
{
    public int Key_ID;

    public DataManager DMReference;
    public ObjectScript ObjReference = null;

    void Awake()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        ObjReference = this.GetComponent<ObjectScript>();                                                   //Fetch Object script from this Script (Collectable, Portal, Shovable...)
        ObjReference.UnlockMethod = 0;                                                                      //Set UnlockMethod in Object Script to 2 (Unlock by Item)
    }

    private void OnMouseOver()                                                                          
    {
        if (Input.GetMouseButtonUp(0) && DMReference.InventoryRef.TryDragUnlock == true && DMReference.InventoryRef.DraggedItemID == Key_ID)
        {
            ObjReference.Lock_State = false;
            UpdateDragUnlock();
            DataManager.Item_List[DMReference.InventoryRef.DraggedItemID-1].RemoveOnUse(); //Error: Dragged_Item_Index does not Equal Index in Item_list, but in Draggable List!!!!!!!!!!!!!!!!!!!!!!!
            // Delete Item from Draggable List
        }
    }

    private void UpdateDragUnlock()
    {
        switch (ObjReference.ObjectList_ID)
        {
            case 1:
                ObjReference.ToggleSprites();
                Collectable CollectableObjectRef = null;                                                       //Create an Unlock Variable, which will be used to access the CallSwitchState Method
                CollectableObjectRef = (Collectable)ObjReference;                                              //Convert the Parent UnlockScript Type(UnSReference) into the SwitchStateUnlock Type 
                CollectableObjectRef.UpdateData();                              //Call Switch Unlock Initiator in SwitchUnlock Script, pass this Object's List and Index
                break;
            case 2:
                ObjReference.ToggleSprites();
                Shovable ShovableObjectRef = null;                                                       //Create an Unlock Variable, which will be used to access the CallSwitchState Method
                ShovableObjectRef = (Shovable)ObjReference;                                              //Convert the Parent UnlockScript Type(UnSReference) into the SwitchStateUnlock Type 
                ShovableObjectRef.UpdateData();                              //Call Switch Unlock Initiator in SwitchUnlock Script, pass this Object's List and Index
                break;
            case 3:
                ObjReference.ToggleSprites();
                Portal PortalObjectRef = null;                                                       //Create an Unlock Variable, which will be used to access the CallSwitchState Method
                PortalObjectRef = (Portal)ObjReference;                                              //Convert the Parent UnlockScript Type(UnSReference) into the SwitchStateUnlock Type 
                PortalObjectRef.UpdateData();                              //Call Switch Unlock Initiator in SwitchUnlock Script, pass this Object's List and Index
                break;
            case 4:
                ObjReference.ToggleSprites();
                Switchable SwitchObjectRef = null;                                                       //Create an Unlock Variable, which will be used to access the CallSwitchState Method
                SwitchObjectRef = (Switchable)ObjReference;                                              //Convert the Parent UnlockScript Type(UnSReference) into the SwitchStateUnlock Type 
                SwitchObjectRef.UpdateData();                              //Call Switch Unlock Initiator in SwitchUnlock Script, pass this Object's List and Index
                break;
            default:
                break;
        }
    }
}
