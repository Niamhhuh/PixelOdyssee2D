using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Acquired : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //Variables which are passed onto DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public int ID;				                                                    //ID of the Object, required to find it in the list
    public int Slot;                                                                //relevant to control the position in the Inventory

    //Local Variables, not saved in the DataManager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private Canvas canvasStats;
    public RectTransform AcquiredPosition;
    private CanvasGroup ControlInteract;
    public SlotScript CurrentSlot;

    private int ObjectIndex;                                                         //Index of this Object in its list                          //used for UnlockMethods
    private bool NewObject = true;

    private DataManager DMReference;

    //Object Data Management
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
     void Awake()
    {
        AcquiredPosition = GetComponent<RectTransform>();
        canvasStats = GameObject.FindGameObjectWithTag("UiCanvas").GetComponent<Canvas>();
        ControlInteract = GetComponent<CanvasGroup>();

        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManage

        int currentIndex = 0;                                                                               //remember the currently inspected Index

        foreach (DataManager.AcquiredObj StoredObj in DataManager.Acquired_List)                            //Go through the Acquired_List and check AcquiredObj.
        {
            if (ID == StoredObj.Stored_ID)
            {
                FetchData(StoredObj.Stored_Slot);                              //Fetch ObjectInformation from DataManager 
                ObjectIndex = currentIndex;                                                                 //Fetch the Index of the found Object
                NewObject = false;                                                                          //Confirm the Object is already available in DataManager
                break;
            }
            currentIndex++;                                                                                 //Update the currently inspected Index
        }

        if (NewObject == true)                                                                              //If required, pass ObjectInformation to DataManager.
        {
            DMReference.AddAcquiredObj(ID, Slot);                                               //Call the AddAcquiredObj Method in DataManager, to add a new DataContainer.
            ObjectIndex = DataManager.Acquired_List.Count - 1;                                                  //When an Object is added, it is added to the end of the list, making its Index I-1.
        }

        SearchSlot();
    }



    private void FetchData(int Stored_Slot)                                          //Fetch the Variables Lock and Slot from the DataManager
    {
        Slot = Stored_Slot;
    }


    private void UpdateData()                                                                               //Pass Variables Lock and Slot to the DataManager
    {
        DMReference.EditAcquiredObj(ObjectIndex, Slot);
    }
    //Acquired Item specific Search Slot on Load
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void SearchSlot()
    {
        if (Slot == 0)
        {
            SearchSlotArray();
        } else
        {
            AcquiredPosition.anchoredPosition = DataManager.Slot_Array[Slot-1].SlotPosition.anchoredPosition;
            CurrentSlot = DataManager.Slot_Array[Slot - 1].GetComponent<SlotScript>();
        }
    }

    private void SearchSlotArray()
    {
        foreach (SlotScript SlotPointer in DataManager.Slot_Array)
        {
            if (SlotPointer != null && SlotPointer.SlotOccupied == false)
            {
                AcquiredPosition.anchoredPosition = SlotPointer.SlotPosition.anchoredPosition;
                Slot = SlotPointer.SlotID;
                CurrentSlot = SlotPointer;
                SlotPointer.SetOccupied();
                UpdateData();
                print(DataManager.Slot_Array[Slot-1]);
                break;
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
        AcquiredPosition.anchoredPosition += eventData.delta / canvasStats.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlInteract.blocksRaycasts = false;
        CurrentSlot.ResetOccupied();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Add Slot lock when already occupied!!! (here and in Slot)
        AcquiredPosition.anchoredPosition = CurrentSlot.SlotPosition.anchoredPosition;   //Move Slot to AcquiredItem to center of SelectedSlot
        CurrentSlot.SetOccupied();
        ControlInteract.blocksRaycasts = true;
        UpdateData();
    }
}
