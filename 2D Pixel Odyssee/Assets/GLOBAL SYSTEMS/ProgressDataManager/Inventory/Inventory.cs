using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{


    //Open Inventory
    //Activate a Canvas and disable Movement and Interaction GameInputs

    //Fetch Current Items and their Positions from Acquired Items
    //Place Items as Draggable Objects in Inventory Slots


    //Craft New Item
    //Combine 2 Items to instantiate new Object, add new Object to Acquired List, remove the 2 used Objects from that List

    // Start is called before the first frame update

    private GameObject InventoryObj;
    private DataManager DMReference;
    private bool calledbyKey;

    void Start()
    {
        calledbyKey = false;
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        InventoryObj = GameObject.FindGameObjectWithTag("Inventory");
        InventoryObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("i"))
        {
            calledbyKey = true;
            ControllInventory();
        }

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
        DMReference.MoveScript.InventoryActive = true;
        DMReference.MoveScript.DisableInput();
        calledbyKey = false;
        InventoryObj.SetActive(true);
    }

    public void CloseInventory()
    {
        DMReference.MoveScript.InventoryActive = false;
        if(calledbyKey == true)
        {
            DMReference.MoveScript.EnableInput();
        }
        calledbyKey = false;
        InventoryObj.SetActive(false);
    }

    public void InitiateCrafting()
    {
        //Check ID of Items in Slot 13 and 14
        //If they are a vallid combo, place Item in Slot 15
    }
}
