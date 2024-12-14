using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class DataManager : MonoBehaviour
{
    public static List<CollectableObj> Collectable_List = new List<CollectableObj>();       //Create a List to store all relevant Variables of Collectable Items            //List_ID 1
    public static List<ShovableObj> Shovable_List = new List<ShovableObj>();                //Create a List to store all relevant Variables of Pushable Objects             //List_ID 2
    public static List<PortalObj> Portal_List = new List<PortalObj>();                      //Create a List to store all relevant Variables of Doors and Arcade Machines    //List_ID 3
    public static List<SwitchStateObj> SwitchState_List = new List<SwitchStateObj>();       //Create a List to store all relevant Variables of Switches                     //List_ID 4
    public static List<EventObj> EventSource_List = new List<EventObj>();                   //Create a List to store all relevant Variables of Switches                     //List_ID 5
    public static List<TriggerableObj> Triggerable_List = new List<TriggerableObj>();             //Create a List to store all relevant Variables of Switches               //List_ID 6

    public static List<DraggableObj> Draggable_List = new List<DraggableObj>();             //Create a List to store all relevant Variables of Inventory Items              //ID... doesnt matter
    public static List<Draggable> Item_List;                                                //Create a List to store all Items                                              //Intialized on Awake, the List Object are Sorted by ID
    public static List<CraftRecipe> Recipe_List = new List<CraftRecipe>();

    public static List<ActiveGoal> ActiveGoal_List = new List<ActiveGoal>();                //Create a List to store all relevant Variables of currently Active Goals                 
    public static List<GoalObject> GoalObject_List;                                         //Create a List to store all Goals                                              //Intialized on Awake, the List Object are Sorted by ID

    public static List<ObjectScript> Highlighted_Current = new List<ObjectScript>();        //Create a List to store the currently Highlighted Object           //should probably be an array

    public static List<ObjectScript> ToInteract = new List<ObjectScript>();                 //Create a List to store the Object which is being interacted with            //should probably be an array

    public static List<Shovable> ToShove = new List<Shovable>();                            //Create a List to store the Object that is being shoved            //should probably be an array

    public static SlotScript[] Slot_Array = new SlotScript[11];

    public List<GameObject> TriggeredObjects_List = new List<GameObject>();          //Create a List to store all Triggers

    public static bool[] Rooms_Loaded = new bool[10];                                       //Array which remembers if rooms have been loaded before.

    public UiToMouse MoveScript = null;                                                     //provide easy access to Movescript


    public CursorImageScript CursorScript = null;
    public DisplayName DisplayObjectNameScript = null;

    public Inventory InventoryRef = null;
    public CharacterScript CurrentCharacter = null;

    public static int Inventory_Fillstate = 0;

    public AdvancedDialogueManager DialogueManager = null;

    //Tutorial Mechanics
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public GameObject SwitchChaButton = null;                                               //Button for Character Swap
    public GameObject ClipboardButton = null;                                               //Button for Clipboard

    public static bool DisableClipboard = true;
    public static bool DisableCharacterSwap = true;

    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public static bool FroggerCleared = false;

    public static List<Collectable> RewardList = new List<Collectable>();                 //Create a List to store the Object which is being interacted with            //should probably be an array
    public List<GameObject> RewardObjects = new List<GameObject>();                 //Create a List to store the Object which is being interacted with            //should probably be an array


    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    [HideInInspector] public GameObject RosieComment = null;
    [HideInInspector] public GameObject BebeComment = null;

    public TMP_Text ObjectCommentRosie;
    public TMP_Text ObjectCommentBebe;

    //MiniMap + SpawnSystem
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public int currentRoom = 0;                                          //set this in Portal

    public List<GameObject> SpawnList = new List<GameObject>();          //Create a List to store all SpawnPoints in a Scene 
    public static int SpawnID;                                              //ID of the selected SpawnPointObject. Set in used Portal 
    public static int LastRoom;                                             //ID of the Last Room the Player was in
    public static bool NewGame = true;


    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    private void Start()                                                                                                            //Disable Inventory and Switch Buttons for the tutorial
    {
        currentRoom = SceneManager.GetActiveScene().buildIndex;

        if(SpawnID == 0) { SpawnID = 1; }                                                                                           //Set Spawn to 1 if no Spawner has been set previously
        foreach (GameObject Obj in GameObject.FindGameObjectsWithTag("Spawn"))                                                      //Search Spawners in the Room
        {
            SpawnList.Add(Obj);                                                                                                     //Remember Spawners for this load
        }
        SpawnPlayer();                                                                                                              //Position the Player
    }

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            NewGame = false;
        }

        if (NewGame)
        {
            LastRoom = 1;
        }

        Time.timeScale = 1;

        //Fetch and Sort all Items
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        Item_List = new List<Draggable>(FindObjectsOfType<Draggable>());                    //Fetch all Items into a List
        Item_List.Sort((Item1, Item2) => Item1.ID.CompareTo(Item2.ID));                     //Sort the Items by their ID to quicken access later                    


        //Fetch and Sort all Goals
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        GoalObject_List = new List<GoalObject>(FindObjectsOfType<GoalObject>());            //Fetch all Items into a List
        GoalObject_List.Sort((Goal1, Goal2) => Goal1.ID.CompareTo(Goal2.ID));               //Sort the Items by their ID to quicken access later     



        if (GameObject.FindGameObjectWithTag("UiCanvas") != null)
        {
            InventoryRef = GameObject.FindGameObjectWithTag("UiCanvas").GetComponent<Inventory>();
            CurrentCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterScript>();

            SwitchChaButton = GameObject.FindGameObjectWithTag("SwitchCharacterButton");                                                //SwitchCharacterButton 
            ClipboardButton = GameObject.FindGameObjectWithTag("ClipboardButton");                                                      //ClipboardButton 

            DisplayObjectNameScript = GameObject.FindGameObjectWithTag("ObjectNameDisplay").GetComponent<DisplayName>();
            MoveScript = GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>();
            CursorScript = GameObject.FindGameObjectWithTag("DataManager").GetComponent<CursorImageScript>();
            DialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<AdvancedDialogueManager>();
        }

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

        RosieComment = GameObject.FindGameObjectWithTag("CommentSpriteRosie");
        BebeComment = GameObject.FindGameObjectWithTag("CommentSpriteBebe");

        if(CurrentCharacter != null)
        {
            ObjectCommentRosie = RosieComment.GetComponent<TMP_Text>();
            ObjectCommentBebe = BebeComment.GetComponent<TMP_Text>();
        }
        UpdateUI();
    }



    //Control UI Buttons
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void DisableCharacterSwapButton()                                //Disable CharacterSwap
    {
        DisableCharacterSwap = true;
        UpdateUI();
    }

    public void EnableCharacterSwapButton()                                 //Enable CharacterSwap
    {
        DisableCharacterSwap = false;
        UpdateUI();
    }

    public void DisableClipboardButton()                                    //Disable Clipboard
    {
        DisableClipboard = true;
        UpdateUI();
    }

    public void EnableClipboardButton()                                     //Enable Clipboard
    {
        DisableClipboard = false;
        UpdateUI();
    }

    public void UpdateUI()                                                  //Control UI Elements
    {
        //Control Character Swap Button
        if (DisableCharacterSwap)
        {
            if(SwitchChaButton != null)
            {
                SwitchChaButton.SetActive(false);
            }
        }
        else
        {
            if(SwitchChaButton != null)
            {
                SwitchChaButton.SetActive(true);
            }
        }


        //Control Clipboard Button
        if (DisableClipboard)
        {
            if (ClipboardButton != null)
            {
                ClipboardButton.SetActive(false);
            }
        }
        else
        {
            if (ClipboardButton!= null)
            {
                ClipboardButton.SetActive(true);
            }
        }
    }


    //Set Player to Right Position
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void SpawnPlayer()
    {
        if(SpawnList.Count > 0)
        {
            bool SpawnFound = false;
            foreach (GameObject Spawn in SpawnList)
            {
                if (Spawn.GetComponent<SpawnObjectScript>().ID == SpawnID)
                {
                    MoveScript.player.position = new Vector3(Spawn.GetComponent<Transform>().position.x, MoveScript.player.position.y, MoveScript.player.position.z);
                    SpawnFound = true;
                }
            }
            if (!SpawnFound)
            {
                SpawnID = 1;
                SpawnPlayer();
            }
        }
    }



    //Add Object Methods
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void AddCollectableObj(int newID, bool newLock_State, bool newAlreadyTalked, bool newCollected)
    {
        Collectable_List.Add(new CollectableObj { Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_AlreadyTalked = newAlreadyTalked, Stored_Collected = newCollected });
        //Debug.Log(Collectable_List.Count);
    }


    public void AddShovableObj(int newID, bool newLock_State, bool newAlreadyTalked, int newShove_Position)
    {
        Shovable_List.Add(new ShovableObj { Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_AlreadyTalked = newAlreadyTalked, Stored_Shove_Position = newShove_Position });
        //Debug.Log(Shovable_List.Count);
    }


    public void AddPortalObj(int newID, bool newLock_State, bool newAlreadyTalked, bool newTraversed)
    {
        Portal_List.Add(new PortalObj { Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_AlreadyTalked = newAlreadyTalked, Stored_Traversed = newTraversed });
        //Debug.Log(Portal_List.Count);
    }


    public void AddSwitchStateObj(int newID, bool newLock_State, bool newAlreadyTalked, bool newSwitchState)
    {
        SwitchState_List.Add(new SwitchStateObj { Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_AlreadyTalked = newAlreadyTalked, Stored_SwitchState = newSwitchState });
    }

    public void AddEventObj(int newID, bool newLock_State, bool newAlreadyTalked, bool newEvent_Passed)
    {
        EventSource_List.Add(new EventObj { Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_AlreadyTalked = newAlreadyTalked, Stored_Event_Passed = newEvent_Passed });
    }

    public void AddTriggerableObj(int newID, bool newLock_State, bool newAlreadyTalked, bool newTrigger_Passed, GameObject newTrigger)
    {
        Triggerable_List.Add(new TriggerableObj { Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_AlreadyTalked = newAlreadyTalked, Stored_Trigger_Passed = newTrigger_Passed, Stored_Trigger = newTrigger });
    }


    public void AddDraggableObj(int newID, int newSlot)
    {
        Draggable_List.Add(new DraggableObj { Stored_ID = newID, Stored_Slot = newSlot });
    }




    //Add Active Goals
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void AddGoalObj(int newID, bool newCompleted)
    {
        ActiveGoal_List.Add(new ActiveGoal { Stored_ID = newID, Stored_Completed = newCompleted });
    }


    //Edit Object Methods
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    //Stored_AlreadyInteracted
    public void EditCollectableObj(int ObjectIndex, bool newLock_State, bool newAlreadyTalked, bool newCollected)
    {
        Collectable_List[ObjectIndex].Stored_Lock_State = newLock_State;
        Collectable_List[ObjectIndex].Stored_Collected = newCollected;
        Collectable_List[ObjectIndex].Stored_AlreadyTalked = newAlreadyTalked;
    }

    public void EditShovableObj(int ObjectIndex, bool newLock_State, bool newAlreadyTalked, int newShove_Position)
    {
        Shovable_List[ObjectIndex].Stored_Lock_State = newLock_State;
        Shovable_List[ObjectIndex].Stored_Shove_Position = newShove_Position;
        Shovable_List[ObjectIndex].Stored_AlreadyTalked = newAlreadyTalked;
    }

    public void EditPortalObj(int ObjectIndex, bool newLock_State, bool newAlreadyTalked, bool newTraversed)
    {
        Portal_List[ObjectIndex].Stored_Lock_State = newLock_State;
        Portal_List[ObjectIndex].Stored_Traversed = newTraversed;
        Portal_List[ObjectIndex].Stored_AlreadyTalked = newAlreadyTalked;
    }

    public void EditSwitchStateObj(int ObjectIndex, bool newLock_State, bool newAlreadyTalked, bool newSwitchState)
    {
        SwitchState_List[ObjectIndex].Stored_Lock_State = newLock_State;
        SwitchState_List[ObjectIndex].Stored_SwitchState = newSwitchState;
        SwitchState_List[ObjectIndex].Stored_AlreadyTalked = newAlreadyTalked;
    }

    public void EditEventObj(int ObjectIndex, bool newLock_State, bool newAlreadyTalked, bool newEvent_Passed)
    {
        EventSource_List[ObjectIndex].Stored_Lock_State = newLock_State;
        EventSource_List[ObjectIndex].Stored_Event_Passed = newEvent_Passed;
        EventSource_List[ObjectIndex].Stored_AlreadyTalked = newAlreadyTalked;
    }

    public void EditTriggerableObj(int ObjectIndex, bool newLock_State, bool newAlreadyTalked, bool newTrigger_Passed)
    {
        Triggerable_List[ObjectIndex].Stored_Lock_State = newLock_State;
        Triggerable_List[ObjectIndex].Stored_Trigger_Passed = newTrigger_Passed;
        Triggerable_List[ObjectIndex].Stored_AlreadyTalked = newAlreadyTalked;
    }

    public void EditDraggableObj(int ObjectIndex, int newSlot)
    {
        if (Draggable_List.Count > 0)
        {
            Draggable_List[ObjectIndex].Stored_Slot = newSlot;
        }
    }



    //Edit Goal
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    /*
    public void EditGoalObj(int ObjectIndex, bool newCompleted)
    {
            ActiveGoal_List[ObjectIndex].Stored_Completed = newCompleted;
    } 
     */


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
        foreach (EventObj StoredObj in EventSource_List)
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

    public void TriggerActivate(int Target_ID)                               //Activate the Trigger
    {
        foreach (GameObject TriggerObj in TriggeredObjects_List)             //Search through List of Trigger Objects
        {
            TriggerActivateFunction(TriggerObj, Target_ID);                 //Call the Function of this Loop
        }
    }

    private void TriggerActivateFunction(GameObject TriggerObj, int Target_ID)
    {
        if (TriggerObj != null && TriggerObj.GetComponent<Triggerable>().ID == Target_ID)   //If the Trigger is found 
        {
            if (!TriggerObj.GetComponent<Triggerable>().Trigger_Passed)              //If the Trigger hasn't been activated before
            {
                TriggerObj.SetActive(true);                                                 //Activate the Trigger
                MoveScript.DisableInput();                                                  //Disable Pointer Inpput 
                MoveScript.DisableInteract();                                               //Disable Pointer Interact 
                if(TriggerObj.GetComponent<Triggerable>().ForceDialogue)
                {
                    TriggerObj.GetComponent<Triggerable>().Lock_State = false;
                    TriggerObj.GetComponent<Triggerable>().UpdateData();
                    TriggerObj.GetComponent<Triggerable>().TriggerInteract();               //Immediatly Activate Trigger
                }
            }
        }
    }



    //
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void FlickerSwitchChaButton()
    {
       //add a Method to Flicker the Button use Coroutine.
    }



    //Classes for Object DataTypes
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void ActivateReward(int Reward_ID) 
    {
        GrantInRoomReward(Reward_ID);
        foreach (Collectable StoredObjScript in RewardList)                      //Go through the Collectable_List and check CollectableObj.
        {
            if (StoredObjScript.ID == Reward_ID)                      //Find the Collectable in the Reward List
            {
                StoredObjScript.Lock_State = false;                   //Unlock the Collectable
                StoredObjScript.UpdateData();                         //Update the CollectableData   
            }
        }
    }                                

    private void GrantInRoomReward(int Reward_ID)                          // Use this only when the Object is in the same Room
    {
        foreach (GameObject StoredObj in RewardObjects)                      //Go through the Collectable_List and check CollectableObj.
        {
            if(StoredObj.GetComponent<Collectable>().ID == Reward_ID)
            {
                StoredObj.SetActive(true);
                StoredObj.GetComponent<Collectable>().Lock_State = false;                   //Unlock the Collectable
                StoredObj.GetComponent<Collectable>().UpdateData();                         //Update the CollectableData   
            }
        }
    }



    public void RewardFrogger()                                //Activate the Trigger
    {
        //
    }


    //Classes for Object DataTypes
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public class Obj
    {
        public int Stored_ID;
        public bool Stored_Lock_State;
        public bool Stored_AlreadyTalked;
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


    public class ActiveGoal
    {
        public int Stored_ID;
        public bool Stored_Completed;

    }
}
