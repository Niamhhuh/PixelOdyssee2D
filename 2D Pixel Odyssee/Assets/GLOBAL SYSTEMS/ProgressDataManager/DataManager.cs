using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static List<CollectableObj> Collectable_List = new List<CollectableObj>();       //Create a List to store all relevant Variables of Collectable Items            //List_ID 1
    public static List<ShovableObj> Shovable_List = new List<ShovableObj>();                //Create a List to store all relevant Variables of Pushable Objects             //List_ID 2
    public static List<PortalObj> Portal_List = new List<PortalObj>();                      //Create a List to store all relevant Variables of Doors and Arcade Machines    //List_ID 3
    public static List<SwitchStateObj> SwitchState_List = new List<SwitchStateObj>();       //Create a List to store all relevant Variables of Switches                     //List_ID 4
    public static List<EventObj> EventSource_List = new List<EventObj>();                   //Create a List to store all relevant Variables of Switches                     //List_ID 5
    public static List<TriggerableObj> Triggerable_List = new List<TriggerableObj>();             //Create a List to store all relevant Variables of Switches               //List_ID 6

    public static List<DraggableObj> Draggable_List = new List<DraggableObj>();             //Create a List to store all relevant Variables of Inventory Items              //ID... doesnt matter
    public static List<Draggable> Item_List;                                                //Create a List to store all Items                                              //ID... doesnt matter
    public static List<CraftRecipe> Recipe_List = new List<CraftRecipe>();

    public static List<ObjectScript> Highlighted_Current = new List<ObjectScript>();        //Create a List to store the currently Highlighted Object           //should probably be an array

    public static List<ObjectScript> ToInteract = new List<ObjectScript>();                 //Create a List to store the Object which is being interacted with            //should probably be an array

    public static List<Shovable> ToShove = new List<Shovable>();                            //Create a List to store the Object that is being shoved            //should probably be an array

    public static SlotScript[] Slot_Array = new SlotScript[15];

    public static List<GameObject> TriggeredObjects_List = new List<GameObject>();          //Create a List to store all relevant Variables of Switches                     //List_ID 5

    public static bool[] Rooms_Loaded = new bool[10];                                       //Array which remembers if rooms have been loaded before.

    public UiToMouse MoveScript = null;                                                     //provide easy access to Movescript
    public Inventory InventoryRef = null;
    public CharacterScript CurrentCharacter = null;

    public static int Inventory_Fillstate = 0;

    public static bool TutorialStarted;

    public static bool FroggerCleared = false;

    public GameObject Reward = null;


    private void Awake()
    {
        Item_List = new List<Draggable>(FindObjectsOfType<Draggable>());
        Item_List.Sort((Item1, Item2) => Item1.ID.CompareTo(Item2.ID));
        //print("a" + Item_List[0].ID + "b" + Item_List[1].ID + "c" + Item_List[2].ID + "d" + Item_List[3].ID + "e" + Item_List[4].ID + "f" + Item_List[5].ID + "g" + Item_List[6].ID + "h" + Item_List[7].ID + "i" + Item_List[8].ID + "j" + Item_List[9].ID + "k" + Item_List[10].ID + "l" + Item_List[11].ID + "m" + Item_List[12].ID);

        InventoryRef = GameObject.FindGameObjectWithTag("UiCanvas").GetComponent<Inventory>();
        MoveScript = GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>();
        CurrentCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterScript>();

        Rooms_Loaded[0] = false;                                                        //Archive 
        Rooms_Loaded[1] = false;                                                        //RaceArcade
        Rooms_Loaded[2] = false;                                                        //Exit
        Rooms_Loaded[3] = false;                                                        //SpaceWar 
        Rooms_Loaded[4] = false;                                                        //Hub 
        Rooms_Loaded[5] = false;                                                        //OldArcade 
        Rooms_Loaded[6] = false;                                                        //Adventure
        Rooms_Loaded[7] = false;                                                        //SensationInteraction
        Rooms_Loaded[8] = false;                                                        //Indie
        Rooms_Loaded[9] = false;                                                        //BossRoom
    }

    private void Start()                                                                                                            //Disable Inventory and Switch Buttons for the tutorial
    {
        if (TutorialStarted == false && GameObject.FindObjectOfType<TutorialToggleButtons>() != null)
        {

            MoveScript.AllowInput = false;
            GameObject.FindObjectOfType<TutorialToggleButtons>().GetComponent<TutorialToggleButtons>().DisableInventoryButton();
            GameObject.FindObjectOfType<TutorialToggleButtons>().GetComponent<TutorialToggleButtons>().DisableSwitchButton();
            TutorialStarted = true;
        }
        TutorialStarted = true;

        if(Reward != null)
        {
            Reward.SetActive(false);
            RewardFrogger();
        }
    }


    //Add Object Methods
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void AddCollectableObj(int newID, bool newLock_State, bool newCollected)
    {
        Collectable_List.Add(new CollectableObj { Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_Collected = newCollected });
        //Debug.Log(Collectable_List.Count);
    }


    public void AddShovableObj(int newID, bool newLock_State, int newShove_Position)
    {
        Shovable_List.Add(new ShovableObj { Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_Shove_Position = newShove_Position });
        //Debug.Log(Shovable_List.Count);
    }


    public void AddPortalObj(int newID, bool newLock_State, bool newTraversed)
    {
        Portal_List.Add(new PortalObj { Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_Traversed = newTraversed });
        //Debug.Log(Portal_List.Count);
    }


    public void AddSwitchStateObj(int newID, bool newLock_State, bool newSwitchState)
    {
        SwitchState_List.Add(new SwitchStateObj { Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_SwitchState = newSwitchState });
    }

    public void AddDraggableObj(int newID, int newSlot)
    {
        Draggable_List.Add(new DraggableObj { Stored_ID = newID, Stored_Slot = newSlot });
    }

    public void AddEventObj(int newID, bool newLock_State, bool newEvent_Passed)
    {
        EventSource_List.Add(new EventObj { Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_Event_Passed = newEvent_Passed });
    }

    public void AddTriggerableObj(int newID, bool newLock_State, bool newTrigger_Passed, GameObject newTrigger)
    {
        Triggerable_List.Add(new TriggerableObj { Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_Trigger_Passed = newTrigger_Passed, Stored_Trigger = newTrigger });
    }

    //Edit Object Methods
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void EditCollectableObj(int ObjectIndex, bool newLock_State, bool newCollected)
    {
        Collectable_List[ObjectIndex].Stored_Lock_State = newLock_State;
        Collectable_List[ObjectIndex].Stored_Collected = newCollected;
    }

    public void EditShovableObj(int ObjectIndex, bool newLock_State, int newShove_Position)
    {
        Shovable_List[ObjectIndex].Stored_Lock_State = newLock_State;
        Shovable_List[ObjectIndex].Stored_Shove_Position = newShove_Position;
    }

    public void EditPortalObj(int ObjectIndex, bool newLock_State, bool newTraversed)
    {
        Portal_List[ObjectIndex].Stored_Lock_State = newLock_State;
        Portal_List[ObjectIndex].Stored_Traversed = newTraversed;
    }

    public void EditSwitchStateObj(int ObjectIndex, bool newLock_State, bool newSwitchState)
    {
        SwitchState_List[ObjectIndex].Stored_Lock_State = newLock_State;
        SwitchState_List[ObjectIndex].Stored_SwitchState = newSwitchState;
    }

    public void EditDraggableObj(int ObjectIndex, int newSlot)
    {
        if (Draggable_List.Count > 0)
        {
            Draggable_List[ObjectIndex].Stored_Slot = newSlot;
        }
    }

    public void EditEventObj(int ObjectIndex, bool newLock_State, bool newEvent_Passed)
    {
        EventSource_List[ObjectIndex].Stored_Lock_State = newLock_State;
        EventSource_List[ObjectIndex].Stored_Event_Passed = newEvent_Passed;
    }

    public void EditTriggerableObj(int ObjectIndex, bool newLock_State, bool newTrigger_Passed)
    {
        Triggerable_List[ObjectIndex].Stored_Lock_State = newLock_State;
        Triggerable_List[ObjectIndex].Stored_Trigger_Passed = newTrigger_Passed;
    }

    //Unlock Methods
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    //By Sequence
    //Unlock Object in Object_List with Object_ID
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void UnlockbySequence(int List_ID, int Object_ID)
    {
        if (List_ID == 1)
        {
            SequenceSearchCollectable(Object_ID);
        }
        if (List_ID == 2)
        {
            SequenceSearchShovable(Object_ID);
        }
        if (List_ID == 3)
        {
            SequenceSearchPortal(Object_ID);
        }
        if (List_ID == 4)
        {
            SequenceSearchSwitchState(Object_ID);
        }
        if (List_ID == 5)
        {
            SequenceSearchEventSource(Object_ID);
        }
        if (List_ID == 6)
        {
            SequenceSearchTriggerable(Object_ID);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void SequenceSearchCollectable(int Object_ID)
    {
        foreach (CollectableObj StoredObj in Collectable_List)              // Search through Collectable List and Unlock an Object
        {
            ChangeLockState(StoredObj, Object_ID);                          // Call Method to compare Current Object ID with Target ID and then edit Lock_State
        }
    }

    private void SequenceSearchShovable(int Object_ID)
    {
        foreach (ShovableObj StoredObj in Shovable_List)                     // Search through Shovable List and Unlock an Object
        {
            ChangeLockState(StoredObj, Object_ID);                          // Call Method to compare Current Object ID with Target ID and then edit Lock_State
        }
    }

    private void SequenceSearchPortal(int Object_ID)                         // Search through Portal List and Unlock an Object
    {
        foreach (PortalObj StoredObj in Portal_List)
        {
            ChangeLockState(StoredObj, Object_ID);                           // Call Method to compare Current Object ID with Target ID and then edit Lock_State
        }
    }

    private void SequenceSearchSwitchState(int Object_ID)                         // Search through Portal List and Unlock an Object
    {
        foreach (SwitchStateObj StoredObj in SwitchState_List)
        {
            ChangeLockState(StoredObj, Object_ID);                           // Call Method to compare Current Object ID with Target ID and then edit Lock_State
        }
    }

    private void SequenceSearchEventSource(int Object_ID)                         // Search through Portal List and Unlock an Object
    {
        foreach (SwitchStateObj StoredObj in SwitchState_List)
        {
            ChangeLockState(StoredObj, Object_ID);                           // Call Method to compare Current Object ID with Target ID and then edit Lock_State
        }
    }
    private void SequenceSearchTriggerable(int Object_ID)                         // Search through Portal List and Unlock an Object
    {
        foreach (TriggerableObj StoredObj in Triggerable_List)
        {
            ChangeLockState(StoredObj, Object_ID);                           // Call Method to compare Current Object ID with Target ID and then edit Lock_State
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void ChangeLockState(Obj Current_Object, int Target_ID)           // Compare Current select Object_ID with Target_ID and Change LockState on Match.
    {
        if (Target_ID == Current_Object.Stored_ID)
        {
            Current_Object.Stored_Lock_State = false;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //By Shovable Position
    //Check for Position of Object_ID -> pass back unlock or lock (can be locked again)
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void UnlockbyPosition(int UnlockList_ID, int UnlockObject_Index, int Shovable_ID, int Unlock_Shove_Position)
    {
        foreach (ShovableObj StoredObj in Shovable_List)                     // Search through Shovable List and Unlock an Object
        {
            CompareObject(UnlockList_ID, UnlockObject_Index, StoredObj, Shovable_ID, StoredObj.Stored_Shove_Position, Unlock_Shove_Position);
        }
    }

    private void CompareObject(int UnlockList_ID, int UnlockObject_Index, ShovableObj StoredObj, int Shovable_ID, int Stored_Shove_Position, int Unlock_Position)
    {
        if (Shovable_ID == StoredObj.Stored_ID)
        {
            ComparePosition(UnlockList_ID, UnlockObject_Index, Stored_Shove_Position, Unlock_Position);
        }
    }

    private void ComparePosition(int UnlockList_ID, int UnlockObject_Index, int Stored_Shove_Position, int Unlock_Position)
    {
        if (Stored_Shove_Position == Unlock_Position)
        {
            UnLockObjects(UnlockList_ID, UnlockObject_Index);

        }
        else
        {
            LockObjects(UnlockList_ID, UnlockObject_Index);
        }

    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //By SwitchState
    //Check for State of a Switch and Unlock/Lock
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void UnlockbySwitchState(int UnlockList_ID, int UnlockObject_Index, int Switch_ID, bool Unlock_SwitchState)
    {
        foreach (SwitchStateObj StoredObj in SwitchState_List)                     // Search through SwitchState List and Unlock an Object
        {
            CompareSwitchObj(UnlockList_ID, UnlockObject_Index, StoredObj, Switch_ID, StoredObj.Stored_SwitchState, Unlock_SwitchState);
        }
    }
    private void CompareSwitchObj(int UnlockList_ID, int UnlockObject_Index, SwitchStateObj StoredObj, int Switch_ID, bool Stored_SwitchState, bool Unlock_SwitchState)
    {
        if (Switch_ID == StoredObj.Stored_ID)
        {
            CompareSwitchState(UnlockList_ID, UnlockObject_Index, Stored_SwitchState, Unlock_SwitchState);
        }
    }

    private void CompareSwitchState(int UnlockList_ID, int UnlockObject_Index, bool Stored_SwitchState, bool Unlock_SwitchState)
    {
        if (Stored_SwitchState == Unlock_SwitchState)
        {
            UnLockObjects(UnlockList_ID, UnlockObject_Index);

        }
        else
        {
            LockObjects(UnlockList_ID, UnlockObject_Index);
        }
    }



    private void UnLockObjects(int UnlockList_ID, int UnlockObject_Index)
    {
        switch (UnlockList_ID)
        {
            case 1:
                Collectable_List[UnlockObject_Index].Stored_Lock_State = false;
                break;
            case 2:
                Shovable_List[UnlockObject_Index].Stored_Lock_State = false;
                break;
            case 3:
                Portal_List[UnlockObject_Index].Stored_Lock_State = false;
                break;
            case 4:
                SwitchState_List[UnlockObject_Index].Stored_Lock_State = false;
                break;
            case 5:
                EventSource_List[UnlockObject_Index].Stored_Lock_State = false;
                break;
            case 6:
                Triggerable_List[UnlockObject_Index].Stored_Lock_State = false;
                break;
            default:
                break;
        }
    }



    private void LockObjects(int UnlockList_ID, int UnlockObject_Index)
    {
        switch (UnlockList_ID)
        {
            case 1:
                Collectable_List[UnlockObject_Index].Stored_Lock_State = true;
                break;
            case 2:
                Shovable_List[UnlockObject_Index].Stored_Lock_State = true;
                break;
            case 3:
                Portal_List[UnlockObject_Index].Stored_Lock_State = true;
                break;
            case 4:
                SwitchState_List[UnlockObject_Index].Stored_Lock_State = true;
                break;
            case 5:
                EventSource_List[UnlockObject_Index].Stored_Lock_State = true;
                break;
            case 6:
                Triggerable_List[UnlockObject_Index].Stored_Lock_State = true;
                break;
            default:
                break;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //Activate Trigger
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void TriggerActivate(int Target_ID)                                //Activate the Trigger
    {
        print(TriggeredObjects_List.Count);
        foreach (GameObject TriggerObj in TriggeredObjects_List)
        {
            if (TriggerObj != null && TriggerObj.GetComponent<Triggerable>().ID == Target_ID)
            {
                if(TriggerObj.GetComponent<Triggerable>().Trigger_Passed != true)
                TriggerObj.SetActive(true);
            }
        }
    }


    //Classes for Object DataTypes
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void RewardFrogger()                                //Activate the Trigger
    {
        if(FroggerCleared == true)
        {
            Reward.SetActive(true);
        }
    }


    //Classes for Object DataTypes
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public class Obj
    {
        public int Stored_ID;
        public bool Stored_Lock_State;
        // public int Dialogue_Progress;
    }

    public class CollectableObj : Obj
    {
        public bool Stored_Collected;
    }

    public class SwitchStateObj : Obj
    {
        public bool Stored_SwitchState;
    }

    public class DraggableObj : Obj
    {
        public int Stored_Slot;
    }

    public class ShovableObj : Obj
    {
        public int Stored_Shove_Position;
    }

    public class PortalObj : Obj
    {
        public bool Stored_Traversed;
    }
    public class EventObj : Obj
    {
        public bool Stored_Event_Passed;
    }
    public class TriggerableObj : Obj
    {
        public bool Stored_Trigger_Passed;
        public GameObject Stored_Trigger;
    }
}
