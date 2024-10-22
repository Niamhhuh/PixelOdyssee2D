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

    public GameObject InventoryObj;
    void Start()
    {
        InventoryObj = GameObject.FindGameObjectWithTag("Inventory");
        InventoryObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("i"))
        {
            print("I'm called");
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
        InventoryObj.SetActive(true);
    }

    public void CloseInventory()
    {
        InventoryObj.SetActive(false);
    }
}
