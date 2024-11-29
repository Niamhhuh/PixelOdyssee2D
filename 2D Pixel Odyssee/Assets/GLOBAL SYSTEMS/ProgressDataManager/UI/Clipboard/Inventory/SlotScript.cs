using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IDropHandler
{
    public int SlotID;
    public bool SlotOccupied;                                                                               //Store whether the SLot is occupied or not

    public RectTransform SlotPosition;
    //DataManager DMReference;

    void Awake()
    {
        //DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        SlotOccupied = false;                                                                               //On Wake, every Slot is unoccupied
        if (SlotID == 11)                                                                                   //Except for the Craft Result Slot, which is always Locked
        {
            SlotOccupied = true;
        }

        if (DataManager.Slot_Array[SlotID - 1] != this)                                                     //If the Slot is not already in the Slot_Array of the DataManager, it adds itself
        {                                                                                                   //!!!!ATTENTION: this might break. This MUST be executed befor Items search the Slot Array!!!!!! (Check Script Execution Order or add Sequence System)
            DataManager.Slot_Array[SlotID - 1] = this;
        }
        
        SlotPosition = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && SlotOccupied == false)                                          //When Item is dropped, check if the Slot is free
        {
            eventData.pointerDrag.GetComponent<Draggable>().Slot = SlotID;                                  //Pass current SlotNumber to DraggableItem

            if (SlotID == 9 || SlotID == 10 || SlotID == 11)                                               //If the Item is Placed onto Slot 9/10 (Crafting Slots) it is assigned Slot 0 to be mixed back into the Inventory.
            {
                eventData.pointerDrag.GetComponent<Draggable>().Slot = 0;
            }

            eventData.pointerDrag.GetComponent<Draggable>().CurrentSlot = this;                             //Pass current SlotScript to DraggableItem
        }
    }
    public void SetOccupied()                                                                               //Set the Slot to occupied
    {
        SlotOccupied = true;
    }

    public void ResetOccupied()                                                                             //Set the Slot to unoccuipied
    {
        if(SlotID != 11)                                                                                    //If the Slot isn't Slot 11(CraftResult) it is set unoccupied when an Item is removed from it.
        {
            SlotOccupied = false;
        }
    }
}
