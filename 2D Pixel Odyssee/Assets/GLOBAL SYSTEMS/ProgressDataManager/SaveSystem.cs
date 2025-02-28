using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static DataManager;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{

    // Path to where we save the file (Application.persistentDataPath gives a good location for persistent data)
    private string saveFilePath;


    public DataManager DMReference; // Reference to the DataManager instance
    public DataStorage DStorage = new DataStorage();


    //Modifiyed ChatGPT Solution because I didn't know any of the keywords
    //-----------------------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        // Set the save file path (use persistentDataPath for a location that will survive between sessions)
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
    }


    public void CallSaveGame()                                                  //Access from Button
    {
        SetData();
        SaveGame(DStorage);  // Save the current game data
    }


    public void CallLoadGame()                                                  //Access from Game Start
    {
        DStorage = LoadGame();  // Load the game data and assign it to dataManager
        if (DStorage != null) { FetchData(); }
    }


    public void ClearSaveFile()                                                  //Access from Game Start?
    {
        if (File.Exists(saveFilePath))
        {
            // Delete the save file
            File.Delete(saveFilePath);

            // Optionally, reset any necessary game data here:
            ResetGameData();
        }
    }

    private void ResetGameData()
    {
        // Reset the DataStorage to its initial state or defaults
        DStorage = new DataStorage();
    }


    // Save the DataManager data to a JSON file
    public void SaveGame(DataStorage data)
    {
        // Convert the data to a JSON string
        string json = JsonUtility.ToJson(data);

        // Write the JSON string to the save file
        File.WriteAllText(saveFilePath, json);

        print("Game Saved to: " + saveFilePath);
    }




    // Load the DataStorage data from the JSON file
    public DataStorage LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            // Read the JSON string from the file
            string json = File.ReadAllText(saveFilePath);

            // Deserialize the JSON string back into a DataStorage object
            DataStorage data = JsonUtility.FromJson<DataStorage>(json);
            return data;
        }
        else
        {
            return null;
        }
    }


    public void ContinueSaveGame()
    {
        CallLoadGame();

        if (LastRoom != 0)
        {
            SceneManager.LoadScene(LastRoom);
        }

        if (LastRoom == 0)
        {
            SceneManager.LoadScene(1);
        }
    }

    public void StartNewGame()
    {
        ClearSaveFile();
        CleanDataManager();
        ContinueSaveGame();
    }

    //-----------------------------------------------------------------------------------------------------------------------------

    //Transfer Data from Manager to Storage
    public void SetData() // Set Data on saving the game
    {
        // Object Lists
        DStorage.Collectable_State = new List<DataManager.CollectableObj>(DataManager.Collectable_List);
        print(DStorage.Collectable_State.Count);
        print(DStorage.Collectable_State);
        DStorage.ShovableObj_State = new List<DataManager.ShovableObj>(DataManager.Shovable_List);
        DStorage.PortalObj_State = new List<DataManager.PortalObj>(DataManager.Portal_List);
        DStorage.SwitchStateObj_State = new List<DataManager.SwitchStateObj>(DataManager.SwitchState_List);
        DStorage.EventObj_State = new List<DataManager.EventObj>(DataManager.EventSource_List);
        DStorage.TriggerableObj_State = new List<DataManager.TriggerableObj>(DataManager.Triggerable_List);
        DStorage.DancePadObj_State = new List<DataManager.DancePadObj>(DataManager.DancePad_List);

        // Inventory Lists
        DStorage.Draggable_State = new List<DataManager.DraggableObj>(DataManager.Draggable_List);
        //DStorage.Item_State = new List<Draggable>(DataManager.Item_List);  //Unnecessary but deep copy
        //DStorage.Recipe_State = new List<CraftRecipe>(DataManager.Recipe_List);  //Unnecessary but deep copy

        DStorage.ActiveGoal_State = new List<DataManager.ActiveGoal>(DataManager.ActiveGoal_List);
        //DStorage.Goal_State = new List<GoalObject>(DataManager.GoalObject_List);

        DStorage.Slot_Array_State = (SlotScript[])DataManager.Slot_Array.Clone();
        DStorage.Inventory_Fillstate_State = DataManager.Inventory_Fillstate;
        DStorage.RosieActive_State = DataManager.RosieActive;

        // Other variables (Primitive types don't need deep copying)
        DStorage.DisableClipboard_State = DataManager.DisableClipboard;
        DStorage.DisableCharacterSwap_State = DataManager.DisableCharacterSwap;

        // Reward List
        DStorage.Reward_List_State = new List<CollectableObj>(DataManager.RewardList);                                 //REQUIRES MAKOVER

        // MiniMap + SpawnSystem
        DStorage.SpawnID_State = DataManager.SpawnID;
        DStorage.LastRoom_State = DataManager.LastRoom;
        DStorage.NewGame_State = DataManager.NewGame;

        // Goal List Scroll Settings
        DStorage.CurrentScroll_State = DataManager.CurrentScroll;
        DStorage.MaxScroll_State = DataManager.MaxScroll;
        DStorage.ContainerStartPosition_State = DataManager.ContainerStartPosition;

        // Code List
        DStorage.Code1Acquired_State = DataManager.Code1Acquired;
        DStorage.Code2Acquired_State = DataManager.Code2Acquired;
        DStorage.Code3Acquired_State = DataManager.Code3Acquired;

        // ProgressDialogue List
        DStorage.ProgressDialogueList_State = new List<int>(DataManager.ProgressDialogueList);

        DStorage.CallGlove_State = GloveScript.CallGlove;
        DStorage.GloveProgress_State = GloveScript.GloveProgress;
    }


    //Transfer Data from Storage to Manager
    public void FetchData() // Set Data on saving the game
    {
        //Object Lists
        DataManager.Collectable_List = new List<DataManager.CollectableObj>(DStorage.Collectable_State);
        DataManager.Shovable_List = new List<DataManager.ShovableObj>(DStorage.ShovableObj_State);
        DataManager.Portal_List = new List<DataManager.PortalObj>(DStorage.PortalObj_State);
        DataManager.SwitchState_List = new List<DataManager.SwitchStateObj>(DStorage.SwitchStateObj_State);
        DataManager.EventSource_List = new List<DataManager.EventObj>(DStorage.EventObj_State);
        DataManager.Triggerable_List = new List<DataManager.TriggerableObj>(DStorage.TriggerableObj_State);
        DataManager.DancePad_List = new List<DataManager.DancePadObj>(DStorage.DancePadObj_State);

        //Inventory Lists

        DataManager.Draggable_List = new List<DataManager.DraggableObj>(DStorage.Draggable_State);
        //DataManager.Item_List = new List<Draggable>(DStorage.Item_State);                      //Unnecessary 
        //DataManager.Recipe_List = new List<CraftRecipe>(DStorage.Recipe_State);                  //Unnecessary 

        DataManager.ActiveGoal_List = new List<DataManager.ActiveGoal>(DStorage.ActiveGoal_State);
        //DataManager.GoalObject_List = new List<GoalObject>(DStorage.Goal_State);

        DataManager.Slot_Array = (SlotScript[])DStorage.Slot_Array_State.Clone();
        DataManager.Inventory_Fillstate = DStorage.Inventory_Fillstate_State;
        DataManager.RosieActive = DStorage.RosieActive_State;

        //    public static bool DisableClipboard = true;
        //    public static bool DisableCharacterSwap = true;
        DataManager.DisableClipboard = DStorage.DisableClipboard_State;
        DataManager.DisableCharacterSwap = DStorage.DisableCharacterSwap_State;

        //    public static List<Collectable> RewardList = new List<Collectable>();                 //Create a List to store the Object which is being interacted with            //should probably be an array
        DataManager.RewardList = new List<CollectableObj>(DStorage.Reward_List_State);

        //MiniMap + SpawnSystem
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //public static int SpawnID;                                              //ID of the selected SpawnPointObject. Set in used Portal 
        //public static int LastRoom;                                             //ID of the Last Room the Player was in
        //public static bool NewGame = true;
        DataManager.SpawnID = DStorage.SpawnID_State;
        DataManager.LastRoom = DStorage.LastRoom_State;
        DataManager.NewGame = DStorage.NewGame_State;


        //Goal List Scroll Settings
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        DataManager.CurrentScroll = DStorage.CurrentScroll_State;
        DataManager.MaxScroll = DStorage.MaxScroll_State;
        DataManager.ContainerStartPosition = new Vector2(DStorage.ContainerStartPosition_State.x, DStorage.ContainerStartPosition_State.y);

        //Code List 
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //public static bool Code1Acquired;
        //public static bool Code2Acquired;
        //public static bool Code3Acquired;
        DataManager.Code1Acquired = DStorage.Code1Acquired_State;
        DataManager.Code2Acquired = DStorage.Code2Acquired_State;
        DataManager.Code3Acquired = DStorage.Code3Acquired_State;


        //ProgressDialogue List
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //    public static List<int> ProgressDialogueList = new List<int>();
        DataManager.ProgressDialogueList = new List<int>(DStorage.ProgressDialogueList_State);

        GloveScript.CallGlove = DStorage.CallGlove_State;
        GloveScript.GloveProgress = DStorage.GloveProgress_State;
    }


    private void CleanDataManager()
    {
        //Object Lists
        DataManager.Collectable_List = new List<DataManager.CollectableObj>();
        DataManager.Shovable_List = new List<DataManager.ShovableObj>();
        DataManager.Portal_List = new List<DataManager.PortalObj>();
        DataManager.SwitchState_List = new List<DataManager.SwitchStateObj>();
        DataManager.EventSource_List = new List<DataManager.EventObj>();
        DataManager.Triggerable_List = new List<DataManager.TriggerableObj>();
        DataManager.DancePad_List = new List<DataManager.DancePadObj>();

        //Inventory Lists

        DataManager.Draggable_List = new List<DataManager.DraggableObj>();
        //DataManager.Item_List = new List<Draggable>(DStorage.Item_State);                      //Unnecessary 
        //DataManager.Recipe_List = new List<CraftRecipe>(DStorage.Recipe_State);                  //Unnecessary 

        DataManager.ActiveGoal_List = new List<DataManager.ActiveGoal>();
        //DataManager.GoalObject_List = new List<GoalObject>(DStorage.Goal_State);

        DataManager.Slot_Array = new SlotScript[11];
        DataManager.Inventory_Fillstate = 0;
        DataManager.RosieActive = true;

        //    public static bool DisableClipboard = true;
        //    public static bool DisableCharacterSwap = true;
        DataManager.DisableClipboard = true;
        DataManager.DisableCharacterSwap = true;

        //    public static List<Collectable> RewardList = new List<Collectable>();                 //Create a List to store the Object which is being interacted with            //should probably be an array
        DataManager.RewardList = new List<CollectableObj>();

        //MiniMap + SpawnSystem
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //public static int SpawnID;                                              //ID of the selected SpawnPointObject. Set in used Portal 
        //public static int LastRoom;                                             //ID of the Last Room the Player was in
        //public static bool NewGame = true;
        DataManager.SpawnID = 0;
        DataManager.LastRoom = 0;
        DataManager.NewGame = true;


        //Goal List Scroll Settings
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        DataManager.CurrentScroll = 0;
        DataManager.MaxScroll = 0;
        DataManager.ContainerStartPosition = new Vector2(1000, 10);

        //Code List 
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //public static bool Code1Acquired;
        //public static bool Code2Acquired;
        //public static bool Code3Acquired;
        DataManager.Code1Acquired = false;
        DataManager.Code2Acquired = false;
        DataManager.Code3Acquired = false;


        //ProgressDialogue List
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //    public static List<int> ProgressDialogueList = new List<int>();
        DataManager.ProgressDialogueList = new List<int>();

        GloveScript.CallGlove = false;
        GloveScript.GloveProgress = 0;
    }

}
