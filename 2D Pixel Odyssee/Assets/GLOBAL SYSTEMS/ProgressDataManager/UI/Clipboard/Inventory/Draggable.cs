using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
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

    public bool Available = false;
    public int ObjectIndex;                                                                                //Index of this Object in its list                          //used for UnlockMethods

    private DataManager DMReference;

    private EventInstance InventoryItem;  //Sound

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

        InventoryItem = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.InventoryItem); //Sound
    }


    public void FetchData()                                                                                 //Fetch the Variables Lock and Slot from the DataManager
    {
        int currentIndex = 0;
        foreach (DataManager.DraggableObj StoredObj in DataManager.Draggable_List)                          //Go through the Draggable_List and check DraggableObj.
        {
            if (ID == StoredObj.Stored_ID)
            {
                Slot = StoredObj.Stored_Slot;                                                               //Fetch ObjectInformation from DataManager 
                ObjectIndex = currentIndex;                                                                 //Fetch the Index of the found Object
                break;
            }
            currentIndex++;                                                                                 //Update the currently inspected Index
        }
    }

    private void UpdateData()                                                                               //Pass Variables Lock and Slot to the DataManager
    {
        DMReference.EditDraggableObj(ObjectIndex, Slot);
    }
    //Draggable Item specific Search Slot on Load
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    public void TakeSlot()                                                                                       //when an Item already has an assigned Slot, Place it there
    {
        if(Slot != 0)
        {
            DraggablePosition.anchoredPosition = DataManager.Slot_Array[Slot - 1].SlotPosition.anchoredPosition; //Set Draggable Position to Position of its Slot
            CurrentSlot = DataManager.Slot_Array[Slot - 1].GetComponent<SlotScript>();                           //Grab the SlotScript of the CurrentSlot
            CurrentSlot.SetOccupied();                                                                           //Set the Current Slot to Occupied 
            UpdateData();
        }
    }

    public void SearchSlot()                                                                                    //when an Item doesn't have a Slot yet or was last placed into Slot 9/10, assign a Slot
    {
        if (Slot == 0 || Slot == 11)
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

    //DMReference.DisplayObjectNameScript.SetDisplayPosition();
    //DMReference.DisplayObjectNameScript.ActivateNameDisplay(gameObject.name);
    //DMReference.DisplayObjectNameScript.DeactivateNameDisplay();

    public void OnPointerEnter(PointerEventData eventData)
    {
        DMReference.DisplayObjectNameScript.ActivateNameDisplay(gameObject.name);                                   //Activate the Object Name Panel
        DMReference.DisplayObjectNameScript.SetDisplayPosition();                                                   //Set the Position of the Object Name Panel
        DMReference.CursorScript.DeactivateCursorSprite();                                                          //Make the Cursor Transparent
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DMReference.DisplayObjectNameScript.DeactivateNameDisplay();                                                //Deactivate the Object Name Panel
        DMReference.CursorScript.ActivateCursorSprite();                                                            //Make the Cursor Opaque
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }
    public void OnDrag(PointerEventData eventData)
    {
        DraggablePosition.anchoredPosition += eventData.delta / canvasStats.scaleFactor;                            //On Drag, attack Item to mouse Position                  
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        DMReference.InventoryRef.ItemDragged = true;                                                                //Mark that an Item is being dragged (required for drag Item from inventory)
        DMReference.InventoryRef.DraggedItemID = ID;                                                                //Remember the ID of the dragged Item (required for DragUnlock)

        InventoryItem.start(); //Sound

        transform.SetParent(GameObject.FindGameObjectWithTag("DragItem").GetComponent<Transform>());                //Change parent of Dragged Item to keep it active when inventory is closed for DragUnlock
        ControlInteract.blocksRaycasts = false;                                                                     //Item doesn't Block Raycast on Drag -> allows interaction with Objects and Slots
        if (CurrentSlot != null)
        {
            CurrentSlot.ResetOccupied();                                                                            //Set the Last occupied Slot as Unoccupied
        }
        if (CurrentSlot.SlotID == 9)                                                                               //Reset ID Stored for CraftSlot1
        {
            DMReference.InventoryRef.InputKey1 = 0;
        }

        if (CurrentSlot.SlotID == 10)                                                                               //Reset ID Stored for CraftSlot2
        {
            DMReference.InventoryRef.InputKey2 = 0;
        }
    }

    public void OnEndDrag(PointerEventData eventData)                                                               //On Drag End, the Item first recieves the CurrentSlot Script and ID
    {
        DMReference.InventoryRef.ItemDragged = false;                                                               //Mark that an Item is no longer dragged
        transform.SetParent(ParentObj);                                                                             //Parent Item back to the Item Collection

        InventoryItem.start(); //Sound

        if (CurrentSlot != null)                                                                             
        {
            DraggablePosition.anchoredPosition = CurrentSlot.SlotPosition.anchoredPosition;                         //Move DraggableItem to center of SelectedSlot
            CurrentSlot.SetOccupied();                                                                              //Set new Slot as occupied
        }

        //CRITICAL SECTION

        if (CurrentSlot != null && CurrentSlot.SlotID == 9)                                                                                //Pass ID to Inventory, Inventory remembers ID of Item on CraftSlot1
        {
            DMReference.InventoryRef.InputKey1 = ID;
            //print(DMReference.InventoryRef.InputKey1);
        }

        if (CurrentSlot != null && CurrentSlot.SlotID == 10)                                                                               //Pass ID to Inventory, Inventory remembers ID of Item on CraftSlot2
        {
            DMReference.InventoryRef.InputKey2 = ID;
            //print(DMReference.InventoryRef.InputKey2);
        }

        if (DataManager.Slot_Array[8].SlotOccupied == true && DataManager.Slot_Array[9].SlotOccupied == true)     //Initiate Crafting
        {
            DMReference.InventoryRef.InitiateCrafting();
        }

        //CRITICAL SECTION

        ControlInteract.blocksRaycasts = true;                                                                      //Set Block Raycast true, to make the Item Interactable again (can be dragged out of Slot again)
        DMReference.InventoryRef.DraggedItemID = 0;                                                                 //Reset Dragged Item ID !!!!!!(0 is an unused ID)!!!!!!!

        if (DMReference.InventoryRef.TryDragUnlock == false && ObjectIndex != -1)                                                        //If the Item is not Dragged from the inventory to attempt DragUnlock
        {
            UpdateData();                                                                                           //Update its Position in the DataManager. On DragUnlock, it doesnt Remember its Positon and will be reset to its original Slot
        }
        DMReference.InventoryRef.TryDragUnlock = false;                                                             //Reset TryDragUnlock for next Inventory Call
                                                                                        

    }

    public void RemoveOnUse()
    {
        //Detach the Item from it's Slot
        CurrentSlot = null;                                                                                         //Detach the current Slot Script
        Available = false;                                                                                          //Set this Item as no longer available
        
        //Adjust Index of all Draggable Objects above this one.
        UpdateItemIndex();                                                                                          //Update the DraggableObj Index of other Items

        //Remove the Item from DraggableObj List
        DataManager.Draggable_List.RemoveAt(ObjectIndex);                                                           //Remove this Item's referenceObject from the DraggableObj List -> it will no longer be loaded
        ObjectIndex = -1;                                                                                           //Set this ObjectIndex to an Invalid Value 

        DataManager.Inventory_Fillstate--;
        //print(DataManager.Inventory_Fillstate);
    }

    private void UpdateItemIndex()                                                                                  //Update ObjectIndex of Items with higher Index, on remove their true index will be reduced by 1
    {
        foreach (Draggable Item in DataManager.Item_List)                                                           
        {
            if (Item.ObjectIndex > ObjectIndex)                                                                     //find Item with higher Index
            {
                Item.ObjectIndex--;                                                                                 //reduce Index reference by 1
            }
        }
    }

}
