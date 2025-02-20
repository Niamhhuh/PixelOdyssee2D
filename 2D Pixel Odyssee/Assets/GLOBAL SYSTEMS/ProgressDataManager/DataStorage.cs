using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class DataStorage
{

    /*
     
     public static List<CollectableObj> Collectable_List = new List<CollectableObj>();       //Create a List to store all relevant Variables of Collectable Items            //List_ID 1
    public static List<ShovableObj> Shovable_List = new List<ShovableObj>();                //Create a List to store all relevant Variables of Pushable Objects             //List_ID 2
    public static List<PortalObj> Portal_List = new List<PortalObj>();                      //Create a List to store all relevant Variables of Doors and Arcade Machines    //List_ID 3
    public static List<SwitchStateObj> SwitchState_List = new List<SwitchStateObj>();       //Create a List to store all relevant Variables of Switches                     //List_ID 4
    public static List<EventObj> EventSource_List = new List<EventObj>();                   //Create a List to store all relevant Variables of EventSources                 //List_ID 5
    public static List<TriggerableObj> Triggerable_List = new List<TriggerableObj>();       //Create a List to store all relevant Variables of Triggers                     //List_ID 6
    */

    //Object Lists
    public List<DataManager.CollectableObj> Collectable_State;
    public List<DataManager.ShovableObj> ShovableObj_State;
    public List<DataManager.PortalObj> PortalObj_State;
    public List<DataManager.SwitchStateObj> SwitchStateObj_State;
    public List<DataManager.EventObj> EventObj_State;
    public List<DataManager.TriggerableObj> TriggerableObj_State;
    public List<DataManager.DancePadObj> DancePadObj_State;


    /*
    public static List<DancePadObj> DancePad_List = new List<DancePadObj>();       //Create a List to store all relevant Variables of Switches                     //List_ID 7
    //----------------------

     public static List<DraggableObj> Draggable_List = new List<DraggableObj>();             //Create a List to store all relevant Variables of Inventory Items              //ID... doesnt matter
    public static List<Draggable> Item_List;                                                //Create a List to store all Items                                              //Intialized on Awake, the List Object are Sorted by ID
    public static List<CraftRecipe> Recipe_List = new List<CraftRecipe>();
    //----------------------

     public static List<ActiveGoal> ActiveGoal_List = new List<ActiveGoal>();                //Create a List to store all relevant Variables of currently Active Goals                 
    public static List<GoalObject> GoalObject_List;                                         //Create a List to store all Goals                                              //Intialized on Awake, the List Object are Sorted by ID

     */

    //Inventory Lists

    public List<DataManager.DraggableObj> Draggable_State;
    public List<Draggable> Item_State;                      //Unneccesary 
    public List<CraftRecipe> Recipe_State;                  //Unneccesary 

    public List<DataManager.ActiveGoal> ActiveGoal_State;
    public List<GoalObject> Goal_State;

    //public List<GameObject> TriggeredObjects_List = new List<GameObject>();          //Create a List to store all Triggers
    //public static SlotScript[] Slot_Array = new SlotScript[11];
    //public static int Inventory_Fillstate = 0;

    public List<GameObject> TriggerObject_State;
    public SlotScript[] Slot_Array_State;
    public int Inventory_Fillstate_State;

    //    public static bool DisableClipboard = true;
    //    public static bool DisableCharacterSwap = true;
    public bool DisableClipboard_State;
    public bool DisableCharacterSwap_State;

    //    public static List<Collectable> RewardList = new List<Collectable>();                 //Create a List to store the Object which is being interacted with            //should probably be an array
    public List<Collectable> Reward_List_State;

    //MiniMap + SpawnSystem
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    //public static int SpawnID;                                              //ID of the selected SpawnPointObject. Set in used Portal 
    //public static int LastRoom;                                             //ID of the Last Room the Player was in
    //public static bool NewGame = true;
    public int SpawnID_State;
    public int LastRoom_State;
    public bool NewGame_State;


    //Goal List Scroll Settings
    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    //public static int CurrentScroll = 0;
    //public static int MaxScroll = 0;
    //public static Vector2 ContainerStartPosition = new Vector2(1000, 10);
    public int CurrentScroll_State;
    public int MaxScroll_State;
    public Vector2 ContainerStartPosition_State;

    //Code List 
    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    //public static bool Code1Acquired;
    //public static bool Code2Acquired;
    //public static bool Code3Acquired;
    public bool Code1Acquired_State;
    public bool Code2Acquired_State;
    public bool Code3Acquired_State;


    //ProgressDialogue List
    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //    public static List<int> ProgressDialogueList = new List<int>();
    public List<int> ProgressDialogueList_State;


    public void SetData() // Set Data on saving the game
    {
        //Object Lists
        Collectable_State = DataManager.Collectable_List;
        ShovableObj_State = DataManager.Shovable_List;
        PortalObj_State = DataManager.Portal_List;
        SwitchStateObj_State = DataManager.SwitchState_List;
        EventObj_State = DataManager.EventSource_List;
        TriggerableObj_State = DataManager.Triggerable_List;
        DancePadObj_State = DataManager.DancePad_List;

        //Inventory Lists

        Draggable_State = DataManager.Draggable_List;
        Item_State = DataManager.Item_List;                      //Unneccesary 
        Recipe_State = DataManager.Recipe_List;                  //Unneccesary 

        ActiveGoal_State = DataManager.ActiveGoal_List;
        Goal_State = DataManager.GoalObject_List;

        Slot_Array_State = DataManager.Slot_Array;
        Inventory_Fillstate_State = DataManager.Inventory_Fillstate;

        //    public static bool DisableClipboard = true;
        //    public static bool DisableCharacterSwap = true;
        DisableClipboard_State = DataManager.DisableClipboard;
        DisableCharacterSwap_State = DataManager.DisableCharacterSwap;

        //    public static List<Collectable> RewardList = new List<Collectable>();                 //Create a List to store the Object which is being interacted with            //should probably be an array
        Reward_List_State = DataManager.RewardList;

        //MiniMap + SpawnSystem
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //public static int SpawnID;                                              //ID of the selected SpawnPointObject. Set in used Portal 
        //public static int LastRoom;                                             //ID of the Last Room the Player was in
        //public static bool NewGame = true;
        SpawnID_State = DataManager.SpawnID;
        LastRoom_State = DataManager.LastRoom;
        NewGame_State = DataManager.NewGame;


        //Goal List Scroll Settings
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //public static int CurrentScroll = 0;
        //public static int MaxScroll = 0;
        //public static Vector2 ContainerStartPosition = new Vector2(1000, 10);
        CurrentScroll_State = DataManager.CurrentScroll;
        MaxScroll_State = DataManager.MaxScroll;
        ContainerStartPosition_State = DataManager.ContainerStartPosition;

        //Code List 
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        //public static bool Code1Acquired;
        //public static bool Code2Acquired;
        //public static bool Code3Acquired;
        Code1Acquired_State = DataManager.Code1Acquired;
        Code2Acquired_State = DataManager.Code2Acquired;
        Code3Acquired_State = DataManager.Code3Acquired;


        //ProgressDialogue List
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //    public static List<int> ProgressDialogueList = new List<int>();
        ProgressDialogueList_State = DataManager.ProgressDialogueList;
    }

}
