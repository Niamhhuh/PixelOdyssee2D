using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{                                 
    //Variables which are passed onto DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public int ID;				                                                                           //ID of the Object, required to find it in the list
    public int Slot;                                                                                       //relevant to control the position in the Inventory

    //Local Variables, not saved in the DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private Canvas canvasStats;
    public RectTransform DraggablePosition;
    private CanvasGroup ControlInteract;
    public SlotScript CurrentSlot;
    private Transform ParentObj;

    public int ObjectIndex;                                                                                //Index of this Object in its list                          //used for UnlockMethods
    private bool NewObject = true;

    private DataManager DMReference;

    //Object Data Management
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
     void Start()
    {
        DraggablePosition = GetComponent<RectTransform>();
        canvasStats = GameObject.FindGameObjectWithTag("UiCanvas").GetComponent<Canvas>();                  
        ControlInteract = GetComponent<CanvasGroup>();
        ParentObj = GameObject.FindGameObjectWithTag("ItemCollection").GetComponent<Transform>();

        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManage

        int currentIndex = 0;                                                                               //remember the currently inspected Index

        foreach (DataManager.DraggableObj StoredObj in DataManager.Draggable_List)                          //Go through the Draggable_List and check DraggableObj.
        {
            if (ID == StoredObj.Stored_ID)
            {
                FetchData(StoredObj.Stored_Slot);                                                           //Fetch ObjectInformation from DataManager 
                ObjectIndex = currentIndex;                                                                 //Fetch the Index of the found Object
                NewObject = false;                                                                          //Confirm the Object is already available in DataManager
                break;
            }
            currentIndex++;                                                                                 //Update the currently inspected Index
        }

        if (NewObject == true)                                                                              //If required, pass ObjectInformation to DataManager.
        {
            DMReference.AddDraggableObj(ID, Slot);                                                           //Call the AddDraggableObj Method in DataManager, to add a new DataContainer.
            ObjectIndex = DataManager.Draggable_List.Count - 1;                                              //When an Object is added, it is added to the end of the list, making its Index I-1.
        }

    }



    private void FetchData(int Stored_Slot)                                                                 //Fetch the Variables Lock and Slot from the DataManager
    {
        Slot = Stored_Slot;
    }


    private void UpdateData()                                                                               //Pass Variables Lock and Slot to the DataManager
    {
        DMReference.EditDraggableObj(ObjectIndex, Slot);
    }
    //Draggable Item specific Search Slot on Load
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    public void TakeSlot()
    {
        if(Slot != 0)
        {
            DraggablePosition.anchoredPosition = DataManager.Slot_Array[Slot - 1].SlotPosition.anchoredPosition; //Set Draggable Position to Position of its Slot
            CurrentSlot = DataManager.Slot_Array[Slot - 1].GetComponent<SlotScript>();                           //Grab the SlotScript of the CurrentSlot
            CurrentSlot.SetOccupied();                                                                           //Set the Current Slot to Occupied 
            UpdateData();
        }
    }

    public void SearchSlot()
    {
        if (Slot == 0)
        {
            SearchSlotArray();                                                                                  //when Slot = 0, assign a Slot
        } 
    }

    private void SearchSlotArray()
    {
        foreach (SlotScript SlotPointer in DataManager.Slot_Array)                                              //Search through the Slot Array
        {
            if (SlotPointer != null && SlotPointer.SlotOccupied == false)                                       //When finding a Slot which is unoccupied
            {
                DraggablePosition.anchoredPosition = SlotPointer.SlotPosition.anchoredPosition;                 //Set Draggable Position to Slot Position
                Slot = SlotPointer.SlotID;                                                                      //Assign SlotID to Draggable Slot 
                CurrentSlot = SlotPointer;                                                                      //Pass SlotScript to Draggable
                SlotPointer.SetOccupied();                                                                      //Set the Slot as occupied
                UpdateData();                                                                                   //Update the DataManager
                break;                                                                                          //Break
            }
        }
    }
    //Functions
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void OnPointerDown(PointerEventData eventData)
    {

    }
    public void OnDrag(PointerEventData eventData)
    {
        DraggablePosition.anchoredPosition += eventData.delta / canvasStats.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        DMReference.InventoryRef.ItemDragged = true;
        DMReference.InventoryRef.DraggedItemID = ID;
        transform.SetParent(GameObject.FindGameObjectWithTag("DragItem").GetComponent<Transform>());
        ControlInteract.blocksRaycasts = false;
        CurrentSlot.ResetOccupied();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DMReference.InventoryRef.ItemDragged = false;
        transform.SetParent(ParentObj);
        DraggablePosition.anchoredPosition = CurrentSlot.SlotPosition.anchoredPosition;                                                  //Move DraggableItem to center of SelectedSlot
        CurrentSlot.SetOccupied();
        
        ControlInteract.blocksRaycasts = true;
        DMReference.InventoryRef.DraggedItemID = 0;

        UpdateData();

        //Add alternate version when inventory is closed while dragged
    }
}
