using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IDropHandler
{
    public int SlotID;
    public bool SlotOccupied;

    public RectTransform SlotPosition;
    DataManager DMReference;

    void Awake()
    {
        SlotOccupied = false;
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
       
        
        if(DataManager.Slot_Array[SlotID - 1] != this)                                                      //If the Slot is not already in the Slot_Array of the DataManager, it adds itself
        {                                                                                                   //!!!!ATTENTION: this might break. This MUST be executed befor Items search the Slot Array!!!!!! (Check Script Execution Order or add Sequence System)
            DataManager.Slot_Array[SlotID - 1] = this;
        }
        
        SlotOccupied = DataManager.Slot_Array[SlotID - 1].SlotOccupied;  
        SlotPosition = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null && SlotOccupied == false)                                          //When Item is dropped, check if the Slot is free
        {
            eventData.pointerDrag.GetComponent<Draggable>().Slot = SlotID;                                   //Pass current SlotNumber to DraggableItem
            eventData.pointerDrag.GetComponent<Draggable>().CurrentSlot = this;                              //Pass current SlotScript to DraggableItem
        }
    }
    public void SetOccupied()
    {
        SlotOccupied = true;
    }

    public void ResetOccupied()
    {
        SlotOccupied = false;
    }
}
