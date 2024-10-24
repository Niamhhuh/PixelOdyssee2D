using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static List<CollectableObj> Collectable_List = new List<CollectableObj>();       //Create a List to store all relevant Variables of Collectable Items            //List_ID 1
    public static List<ShovableObj> Shovable_List = new List<ShovableObj>();                //Create a List to store all relevant Variables of Pushable Objects             //List_ID 2
    public static List<PortalObj> Portal_List = new List<PortalObj>();                      //Create a List to store all relevant Variables of Doors and Arcade Machines    //List_ID 3
    public static List<AcquiredObj> Acquired_List = new List<AcquiredObj>();                //Create a List to store all relevant Variables of Inventory Items              //Type... doesnt matter

    public static List<ObjectScript> Highlighted_Current = new List<ObjectScript>();        //Create a List to store the currently Highlighted Object           //should probably be an array

    public static List<Shovable> ToShove = new List<Shovable>();                            //Create a List to store the Object that is being shoved            //should probably be an array

    public static SlotScript[] Slot_Array = new SlotScript[15];              

    public static bool [] Rooms_Loaded = new bool[10];                                      //Array which remembers if rooms have been loaded before.

    public UiToMouse MoveScript = null;                                                     //provide easy access to Movescript

    private void Awake()
    {
        MoveScript = GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>();
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


    //Add Object Methods
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void AddCollectableObj( int newID, bool newLock_State, bool newCollected) 
    {
        Collectable_List.Add(new CollectableObj { Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_Collected = newCollected });
        //Debug.Log(Collectable_List.Count);
    }


    public void AddShovableObj( int newID, bool newLock_State, int newShove_Position)
    {
        Shovable_List.Add(new ShovableObj { Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_Shove_Position = newShove_Position });
        //Debug.Log(Shovable_List.Count);
    }


    public void AddPortalObj( int newID, bool newLock_State, bool newTraversed)
    {
        Portal_List.Add(new PortalObj { Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_Traversed = newTraversed });
        //Debug.Log(Portal_List.Count);
    }


    public void AddAcquiredObj( int newID, int newSlot)
    {
        Acquired_List.Add(new AcquiredObj { Stored_ID = newID, Stored_Slot = newSlot });
        //print("Item Collected" + Acquired_List[0].Stored_ID);
        //Debug.Log(Acquired_List.Count);
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

    public void EditAcquiredObj(int ObjectIndex, int newSlot)
    {
        Acquired_List[ObjectIndex].Stored_Slot = newSlot;
    }

    //Unlock Methods
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    //By Sequence
    //Unlock Object in Object_List with Object_ID
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void UnlockbySequence (int List_ID, int Object_ID)
    {
        if(List_ID == 1)
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
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void SequenceSearchCollectable (int Object_ID)
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

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void ChangeLockState (Obj Current_Object, int Key_ID)           // Compare Current select Object_ID with Target_ID and Change LockState on Match.
    {
        if (Key_ID == Current_Object.Stored_ID)
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
            switch(UnlockList_ID)
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
                default:
                    break;
            }
            
        } else
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
                default:
                    break;
            }
        }
        
    }


    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //By Item
    //Check for Object_ID -> pass back unlock or nothing (remains unlocked for rest of the game?)
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    // This might become obsolete / be used for different Item Types.
    public void UnlockbyAcquired(int UnlockList_ID, int UnlockObject_Index, int Acquired_ID)
    {
        foreach (AcquiredObj StoredObj in Acquired_List)                     // Search through Shovable List and Unlock an Object
        {
            CompareAcquired(UnlockList_ID, UnlockObject_Index, StoredObj, Acquired_ID);
        }
    }

    private void CompareAcquired(int UnlockList_ID, int UnlockObject_Index, AcquiredObj StoredObj, int Acquired_ID)
    {
        if (Acquired_ID == StoredObj.Stored_ID)
        {
            ConfirmUnlock(UnlockList_ID, UnlockObject_Index);
        }
    }

    private void ConfirmUnlock(int UnlockList_ID, int UnlockObject_Index)
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
                default:
                    break;
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

    public class AcquiredObj : Obj
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
        public int Event_Progress;
    }
}
