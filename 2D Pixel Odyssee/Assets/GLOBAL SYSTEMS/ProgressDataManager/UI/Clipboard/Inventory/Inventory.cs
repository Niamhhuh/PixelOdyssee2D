using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{


    //Open Inventory
    //Activate a Canvas and disable Movement and Interaction GameInputs

    //Fetch Current Items and their Positions from Draggable Items
    //Place Items as Draggable Objects in Inventory Slots


    //Craft New Item
    //Combine 2 Items to instantiate new Object, add new Object to Draggable List, remove the 2 used Objects from that List

    // Start is called before the first frame update

    public GameObject InventoryObj;
    private GameObject ItemCollection;
    private DataManager DMReference;
    public bool calledbyKey;

    public bool ItemDragged;
    
    public bool TryDragUnlock;
    public int DraggedItemID;

    public int InputKey1;
    public int InputKey2;

    void Start()
    {
        TryDragUnlock = false;
        calledbyKey = false;
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        InventoryObj = GameObject.FindGameObjectWithTag("Inventory");
        ItemCollection = GameObject.FindGameObjectWithTag("ItemCollection");
        ItemCollection.SetActive(false);
        InventoryObj.SetActive(false);
    }

    public void ControllInventory ()
    {
        if (InventoryObj.activeSelf == false)
        {
            CallInventory();
        }
        else
        {
            CloseInventory();
        }
    }

    public void CallInventory ()
    {
        DMReference.MoveScript.ClipboardActive = true;
        DMReference.MoveScript.DisableInput();
        calledbyKey = false;

        FetchItems();

        foreach (Draggable Item in DataManager.Item_List)
        {
            if (Item.Available == true)
            {
                Item.FetchData();
                Item.TakeSlot();
            }
        }

        //Assign Unkown Slots
        foreach (Draggable Item in DataManager.Item_List)
        {
            if(Item.Available == true)
            {
                Item.SearchSlot();
            }
        }

        DataManager.Slot_Array[9 - 1].GetComponent<SlotScript>().ResetOccupied();                      //Set the Crafting Slots as unoccupied on Inventory Load
        DataManager.Slot_Array[10 - 1].GetComponent<SlotScript>().ResetOccupied();                      //Set the Crafting Slots as unoccupied on Inventory Load

        ItemCollection.SetActive(true);
        InventoryObj.SetActive(true);
    }

    public void CloseInventory()
    {
        StartCoroutine(DMReference.MoveScript.CallEnableInput());
        StartCoroutine(DMReference.MoveScript.CallEnableInteract());
        DMReference.MoveScript.ClipboardActive = false;
        if(calledbyKey == true)
        {
            DMReference.MoveScript.EnableInput();
        }
        calledbyKey = false;
        InputKey1 = 0;                                                              //0 is an unused ID
        InputKey2 = 0;                                                              //0 is an unused ID
        ItemCollection.SetActive(false);
        InventoryObj.SetActive(false);
    }


    public void FetchItems()
    {
        foreach (Draggable Item in DataManager.Item_List)
        {
            Item.gameObject.SetActive(false);
        }

        foreach (DataManager.DraggableObj Collected in DataManager.Draggable_List)
        {
            DataManager.Item_List[Collected.Stored_ID - 1].Available = true;
            DataManager.Item_List[Collected.Stored_ID - 1].gameObject.SetActive(true);
        }
    }


    public void DragItemFromInventory()                             //Call from Inventory Background
    {
        if(ItemDragged)
        {
            DMReference.MoveScript.ClipboardActive = false;
            DMReference.MoveScript.EnableInput();
            calledbyKey = false;
            ItemCollection.SetActive(false);
            InventoryObj.SetActive(false);
            TryDragUnlock = true;
        }
    }

    public void InitiateCrafting()
    {
        bool Craft_Success = false;
        //var InputKeys = new HashSet<int> { InputKey1, InputKey2 };

        foreach (CraftRecipe Recipe in DataManager.Recipe_List)
        {
            //var RequiredKeys = new HashSet<int> { Recipe.KeyID_A, Recipe.KeyID_B };

            //InputKeys.SetEquals(RequiredKeys) == true
            if ((InputKey1 == Recipe.KeyID_A && InputKey2 == Recipe.KeyID_B) || (InputKey1 == Recipe.KeyID_B && InputKey2 == Recipe.KeyID_A))
            {
                //Craft the Item
                DMReference.AddDraggableObj(Recipe.Crafted_Item_ID, 15);                                    //Call the AddDraggableObj Method in DataManager, to add a new DataContainer.
                DataManager.Item_List[Recipe.Crafted_Item_ID - 1].FetchData();                              //Fetch Crafted_Item Data from DataManager (Index and Slot)
                DataManager.Item_List[Recipe.Crafted_Item_ID - 1].TakeSlot();                               //Place Crafted_Item into its Starting Slot(15)
                DataManager.Item_List[Recipe.Crafted_Item_ID - 1].Available = true;                         //Set Crafted_Item as Available
                DataManager.Item_List[Recipe.Crafted_Item_ID - 1].gameObject.SetActive(true);               //Make Crafted_Item visible
                DataManager.Item_List[Recipe.KeyID_A - 1].RemoveOnUse();                                    //Remove Part_Item 1 from Draggable and Slot etc.
                DataManager.Item_List[Recipe.KeyID_B - 1].RemoveOnUse();                                    //Remove Part_Item 2 from Draggable and Slot etc.
                DataManager.Item_List[Recipe.KeyID_A - 1].gameObject.SetActive(false);                      //Deactivate Part_Item 1 
                DataManager.Item_List[Recipe.KeyID_B - 1].gameObject.SetActive(false);                      //Deactivate Part_Item 2
                DataManager.Slot_Array[8].ResetOccupied();                                                  //Open Craft Slot 1 (9)
                DataManager.Slot_Array[9].ResetOccupied();                                                  //Open Craft Slot 2 (10)
                InputKey1 = 0;                                                                               //Reset Craft Slot 1 Item_ID
                InputKey2 = 0;                                                                              //Reset Craft Slot 2 Item_ID
                DataManager.Inventory_Fillstate++;                                                          
                Craft_Success = true;                                                                       //Mark Crafting as Successful
                break;                                                                                      //End 
            }
        }
        if(Craft_Success == false)                                                                          //If no item was crafted
        {
            DataManager.Item_List[InputKey1 - 1].SearchSlot();                                              //Find new Slot for Part_Item 1 
            DataManager.Item_List[InputKey2 - 1].SearchSlot();                                              //Find new Slot for Part_Item 2
            DataManager.Slot_Array[8].ResetOccupied();                                                     //Open Craft Slot 1 (9)
            DataManager.Slot_Array[9].ResetOccupied();                                                     //Open Craft Slot 2 (10)
        }
    }
}