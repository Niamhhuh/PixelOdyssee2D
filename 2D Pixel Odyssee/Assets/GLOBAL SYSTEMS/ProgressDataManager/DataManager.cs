using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static List<CollectableObj> Collectable_List = new List<CollectableObj>();       //Create a List to store all relevant Variables of Collectable Items
    public static List<ShovableObj> Shovable_List = new List<ShovableObj>();                //Create a List to store all relevant Variables of Pushable Objects
    public static List<PortalObj> Portal_List = new List<PortalObj>();                      //Create a List to store all relevant Variables of Doors and Arcade Machines
    public static List<AcquiredObj> Acquired_List = new List<AcquiredObj>();                //Create a List to store all relevant Variables of Inventory Items
    public static bool [] Rooms_Loaded = new bool[10];                                  //Array which remembers if rooms have been loaded before.

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
    public void AddCollectableObj(int newType_ID, int newID, int newLock_State, int newCollected)
    {
        Collectable_List.Add(new CollectableObj { Stored_Type_ID = newType_ID, Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_Collected = newCollected });
        //Debug.Log(Collectable_List.Count);
    }


    public void AddShovableObj(int newType_ID, int newID, int newLock_State, int newPosition)
    {
        Shovable_List.Add(new ShovableObj { Stored_Type_ID = newType_ID, Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_Position = newPosition });
        //Debug.Log(Collectable_List.Count);
    }


    public void AddPortalObj(int newType_ID, int newID, int newLock_State, bool newTraversed)
    {
        Portal_List.Add(new PortalObj { Stored_Type_ID = newType_ID, Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_Traversed = newTraversed });
        //Debug.Log(Collectable_List.Count);
    }


    public void AddAcquiredObj(int newType_ID, int newID, int newLock_State, int newSlot)
    {
        Acquired_List.Add(new AcquiredObj { Stored_Type_ID = newType_ID, Stored_ID = newID, Stored_Lock_State = newLock_State, Stored_Slot = newSlot });
        //Debug.Log(Collectable_List.Count);
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
        public int Stored_Type_ID;
        public int Stored_ID;
        public int Stored_Lock_State;
        // public int Dialogue_Progress;
    }
    
    public class CollectableObj : Obj
    {
        public int Stored_Collected;
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
