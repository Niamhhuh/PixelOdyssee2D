using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static List<CollectableObj> Collectable_List = new List<CollectableObj>();       //Create a List to store all relevant Variables of Collectable Items            //List_ID 1
    public static List<ShovableObj> Shovable_List = new List<ShovableObj>();                //Create a List to store all relevant Variables of Pushable Objects             //List_ID 2
    public static List<PortalObj> Portal_List = new List<PortalObj>();                      //Create a List to store all relevant Variables of Doors and Arcade Machines    //List_ID 3
    public static List<AcquiredObj> Acquired_List = new List<AcquiredObj>();                //Create a List to store all relevant Variables of Inventory Items              //Type... doesnt matter
    public static bool [] Rooms_Loaded = new bool[10];                                      //Array which remembers if rooms have been loaded before.

    private void Awake()
    {
        Rooms_Loaded[0] = false;                                                        //Archieve 
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


    public void AddShovableObj( int newID, bool newLock_State, int newPosition)
    {
        Shovable_List.Add(new ShovableObj { Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_Position = newPosition });
        //Debug.Log(Shovable_List.Count);
    }


    public void AddPortalObj( int newID, bool newLock_State, bool newTraversed)
    {
        Portal_List.Add(new PortalObj { Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_Traversed = newTraversed });
        //Debug.Log(Portal_List.Count);
    }


    public void AddAcquiredObj( int newID, bool newLock_State, int newSlot)
    {
        Acquired_List.Add(new AcquiredObj { Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_Slot = newSlot });
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

    public void EditShovableObj(int ObjectIndex, bool newLock_State, int newPosition)
    {
        Shovable_List[ObjectIndex].Stored_Lock_State = newLock_State;
        Shovable_List[ObjectIndex].Stored_Position = newPosition;
    }

    public void EditPortalObj(int ObjectIndex, bool newLock_State, bool newTraversed)
    {
        Portal_List[ObjectIndex].Stored_Lock_State = newLock_State;
        Portal_List[ObjectIndex].Stored_Traversed = newTraversed;
    }

    public void EditAcquiredObj(int ObjectIndex, bool newLock_State, int newSlot)
    {
        Acquired_List[ObjectIndex].Stored_Lock_State = newLock_State;
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
            break;
        }
    }

    private void SequenceSearchShovable(int Object_ID)
    {
        foreach (ShovableObj StoredObj in Shovable_List)                     // Search through Shovable List and Unlock an Object
        {
            ChangeLockState(StoredObj, Object_ID);                          // Call Method to compare Current Object ID with Target ID and then edit Lock_State
            break;
        }
    }

    private void SequenceSearchPortal(int Object_ID)                         // Search through Portal List and Unlock an Object
    {
        foreach (PortalObj StoredObj in Portal_List)                     
        {
            ChangeLockState(StoredObj, Object_ID);                           // Call Method to compare Current Object ID with Target ID and then edit Lock_State
            break;
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



    //Template
    /*
    private void SequenceSearchPortal(int Object_ID)                         // Search through Portal List and Unlock an Object
    {
        foreach (PortalObj StoredObj in Portal_List)
        {
            if (Object_ID == StoredObj.Stored_ID)
            {
                StoredObj.Stored_Lock_State = false;
                break;
            }
        }
    }
    */

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //By Shovable Position
    //Check for Position of Object_ID -> pass back unlock or lock (can be locked again)
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    /*
    public bool UnlockbyPosition (int Shovable_ID, int Shove_Position, int Unlock_Position, bool Lock_State)
    {
        bool Unlock_State = Lock_State;

        foreach (ShovableObj StoredObj in Shovable_List)                     // Search through Shovable List and Unlock an Object
        {
            if(ComparePosition(StoredObj, Shovable_ID, Shove_Position, Unlock_Position) == false)
            {
                Unlock_State = false;
            }
            else { Unlock_State = true; }
            break;
        }

        if (Unlock_State == false)
        {
            return false;
        }
        else { return true; }
    }

    private bool ComparePosition (ShovableObj StoredObj, int Shovable_ID, int Shove_Position, int Unlock_Position)
    {
        if (Shovable_ID == StoredObj.Stored_ID)
        {
            if (Shove_Position == Unlock_Position)
            {
                return false;
            }
            else { return true; }
        }
        return true;
    }
    */

    public void UnlockbyPosition(int UnlockList_ID, int UnlockObject_Index, int Shovable_ID, int Unlock_Position)
    {
        foreach (ShovableObj StoredObj in Shovable_List)                     // Search through Shovable List and Unlock an Object
        {
            CompareObject(UnlockList_ID, UnlockObject_Index, StoredObj, Shovable_ID, StoredObj.Stored_Position, Unlock_Position);
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
            
        }
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


    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //By Item
    //Check for Object_ID -> pass back unlock or nothing (remains unlocked for rest of the game?)
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    public void UnlockbyItem(int UnlockList_ID, int UnlockObject_Index, int Acquired_ID)
    {
        foreach (AcquiredObj StoredObj in Acquired_List)                     // Search through Shovable List and Unlock an Object
        {
            CompareItem(UnlockList_ID, UnlockObject_Index, StoredObj, Acquired_ID);
        }
    }

    private void CompareItem(int UnlockList_ID, int UnlockObject_Index, AcquiredObj StoredObj, int Acquired_ID)
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







    /*
        public void PrintCollectable()
        {
            Debug.Log(Collectable_List[0].Stored_Collected);
        }
    */



    /*
    public Collectable_List_Edit(ID, Collected, (Unlock_ID_List))
    {
        Search Collectable_Item_List for ID, change Collected to True.
        (Optional: Unlock an Object any List)
        Add ID to Acquired_Item_List        //On Use, remover Item from Acquired_Item_List
                                            //Delete Source Object (Craft Parts or Loose Item).
    }
    */

    /*
    public Pushable_List_Edit (ID, Slide_Position) 
    {
        Search Slider_List for ID, change Slide_Position.
        (Optional: Unlock an Object any List)
    }
    */

    /*
    public Portal_List_Edit (ID) 
    {
        Search Portal_List for ID.
        (Optional: Unlock an Object any List)
    }
    */

    /*
    public Acquired_Item_List_Edit (ID, Slot_Position) 
    {
        Search Acquired_Item_List for ID, change Slot_Position.
    }
    */



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
        public int Stored_Position;
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
