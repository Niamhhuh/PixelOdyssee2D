using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ObjectScript : MonoBehaviour
{
    //Generic Object Variables ------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public int ID;				                                                    //ID of the Object, required to find it in the list
    public bool AlreadyTalked;
    public bool Lock_State;                                                         //check if this Object is Interaction_Locked/Limited
    public bool ForcedInteraction;
    public bool TriggeronUnlock;
    public bool AlwaysDenyInteraction;
    public bool Unlock_by_Item;                                                     //check if this Object is Unlocked by an Item
    public int Item_Key_ID;                                                         //ID of the Key
    public bool ChangeNameonLock;
    public string LockName;

    public bool DialogueUnlock;
    public int DialogueKey_ID;

    

    //public(Dialogue)			                                                    //Dialogue of this object

    // private bool InteractFired = false;

    private Sprite BaseSprite;
    private Sprite BaseHighlightSprite;

    public Sprite LockSprite;                                                       //LockedObject Spirite
    public Sprite LockHighlightSprite;                                              //SpriteRenderer of Object, which is disabled on Highlight

    [HideInInspector] public SpriteRenderer ObjectSprite = null;                    //SpriteRenderer of Object, which is disabled on Highlight
    [HideInInspector] public SpriteRenderer HighlightObjectSprite = null;           //SpriteRenderer of Object, which is disabled on Highlight

    //Interaction Toggle Variables --------------------------------------------------------------------------------------------------------------------------------------------
    public bool CannotInteract;
    public bool CannotTalk;

    [HideInInspector] public bool NewObject = true;
    [HideInInspector] public bool IsFullTrigger = false;

    //public CharacterScript CurrentCharacter;

    //Interaction Variables ---------------------------------------------------------------------------------------------------------------------------------------------------
    private BoxCollider2D Object_Collider = null;                             //Collider of the Object, which is expanded when the Object is marked for interaction
    private Vector2 Original_Collider;                                        //This Vector stores the initial size of the collider
    private Color originalColor;
    private Color originalHighlightColor;

    private ObjectScript ThisObject = null;                                   //ObjectScript, which is added to the Currently_Highlighted List, check if object is selected
    [HideInInspector] public GameObject HighlightonHover = null;              //Child Object, which contains the Highlighted Sprite of the Object
    [HideInInspector] public GameObject CoreObject = null;
    [HideInInspector] public UiToMouse PointerScript = null;


    [HideInInspector] public bool RequestInteract = false;                     //Request is set true, when the Object is clicked and reset, when another object is clicked. 
    [HideInInspector] public bool AlreadyActive = false;                       //AlreadyActive marks an Object as already highlighted, preventing it from expanding its collider multiple times
    //private bool PlayerDetected = false;
    public bool isBackground;                                                  //isBackground is set true on the background, disabling all functions for it



    public GameObject InteractionController = null;          //Store Interaction Buttons
    [HideInInspector] public ObjectScript ObjReference = null;                 //Store this Object to pass to Interaction Buttons

    //Lock/Unlock Variables ---------------------------------------------------------------------------------------------------------------------------------------------------
    [HideInInspector] [Range(0, 2)] public int UnlockMethod = 0;               //Pass Unlock Method from attached Unlock Script to Object Script
    [HideInInspector] public bool CanSequenceUnlock = false;                   //Enable Object Script to use SequenceUnlock Method, when SequenceUnlock is attached


     public int ObjectList_ID;                                //ID which marks the List this Object is stored in          //used for UnlockMethods
     public int ObjectIndex;                                  //Index of this Object in its list                          //used for UnlockMethods

    public DataManager DMReference = null;                   //Store the DataManager
    [HideInInspector] public SequenceUnlock SeqUReference = null;              //Store the Sequence Unlock
    [HideInInspector] public UnlockScript UnSReference = null;                 //Store the Unlock Script

    [HideInInspector] public ActivateTrigger TriggerScript = null;
    [HideInInspector] public AddDialogueOnInteract InteractTransformDialogue = null;
    [HideInInspector] public DialogueTriggeredInteraction DialogueInteractionScript = null;

    public bool IsReward;

    [HideInInspector] public LockedDialogue LockedDialogueScript = null;
     public GrantRewardScript GrantReward_Script = null;

    [HideInInspector] public AddCodeFragment AddCodeScript = null;


    [HideInInspector] public ColliderController ColliderScript = null;
    public Comment ObjectComment;

    private PauseMenu PauseScript;

    

    //Connect Transformative Dialogue System
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public TransformativeDialogueScript TransformDialogueScript;

    //Expand Effect
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private Vector3 ObjectSize;



    //Trigger Goal (Add Goal to List or Complete Goal)
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    [HideInInspector] public bool TriggerGoal;                                      //Set True automatically from ControlGoal Script attached to the Object
    [HideInInspector] public ControlGoal ControlGoalScript = null;                                      //



    private EventInstance ItemSuccess; //Sound
    private EventInstance ItemFailed;



    //Set Data
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Awake()
    {
        ObjectSize = gameObject.transform.localScale;
        CoreObject = this.gameObject;
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        if (gameObject.GetComponent<Triggerable>() != null)
        {
            IsFullTrigger = true;
        }
        ThisObject = this.GetComponent<ObjectScript>();

        if (!Unlock_by_Item)
        {
            Item_Key_ID = -10;                                                                              //if the Object is not Unlocked by an Item, set the KeyID to an unused ID
            //UnlockMethod = 0;
        }

        //CurrentCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterScript>();
        PointerScript = GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>();

        PauseScript = GameObject.FindGameObjectWithTag("PauseController").GetComponent<PauseMenu>();

        if (!isBackground && !IsFullTrigger)                                                                //These Variables are only set on Objects that are neither Background nor Triggers
        {
            ObjectSprite = this.GetComponent<SpriteRenderer>();
            originalColor = ObjectSprite.color;
            Object_Collider = this.GetComponent<BoxCollider2D>();
            Original_Collider = Object_Collider.size;
            HighlightonHover = this.transform.GetChild(0).gameObject;                                       //the first child must ALWAYS be the Highlight Object
            HighlightObjectSprite = HighlightonHover.GetComponent<SpriteRenderer>();
            originalHighlightColor = HighlightObjectSprite.color;
            HighlightonHover.SetActive(false);
            InteractionController = GameObject.FindGameObjectWithTag("InteractionController");

            BaseSprite = ObjectSprite.sprite;
            BaseHighlightSprite = HighlightObjectSprite.sprite;
        }

        if (gameObject.GetComponent<LockedDialogue>() != null)                                              //Attach LockedDialogue 
        {
            LockedDialogueScript = GetComponent<LockedDialogue>();
        }

        if (gameObject.GetComponent<GrantRewardScript>() != null)                                           //Attach GrantRewardScript
        {
            GrantReward_Script = gameObject.GetComponent<GrantRewardScript>();
        }

        if (gameObject.GetComponent<AddCodeFragment>() != null)                                              //Attach LockedDialogue 
        {
            AddCodeScript = GetComponent<AddCodeFragment>();
        }


        if (gameObject.GetComponent<TransformativeDialogueScript>() != null)                                //Attach Transform Dialogue
        {
            TransformDialogueScript = gameObject.GetComponent<TransformativeDialogueScript>();
        }
    }


    private void Start()
    {
        ItemSuccess = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.ItemSuccess); //Sound
        ItemFailed = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.ItemFailed); //Sound
    }
    public void ToggleSprites()
    {
        if (Lock_State == true)
        {
            if (LockSprite != null) { ObjectSprite.sprite = LockSprite; }
            if (LockHighlightSprite != null) { HighlightObjectSprite.sprite = LockHighlightSprite; }
        }

        if (Lock_State == false)
        {
            if (BaseSprite != null) { ObjectSprite.sprite = BaseSprite; }
            if (BaseHighlightSprite != null) { HighlightObjectSprite.sprite = BaseHighlightSprite; }
        }
    }


    //Interaction Manager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void OnMouseEnter()                                                                         //When the Cursor enters an Object, Highlight it, mark it as Highlighted
    {
        if (PointerScript.ClipboardActive == false && !isBackground && !IsFullTrigger && !DMReference.DialogueManager.InDialogue && !PauseScript.InPause)
        {
            DMReference.CursorScript.DeactivateCursorSprite();

            if (ChangeNameonLock)
            {
                if(Lock_State)
                {
                    DMReference.DisplayObjectNameScript.ActivateNameDisplay(LockName);
                }
                if (!Lock_State)
                {
                    DMReference.DisplayObjectNameScript.ActivateNameDisplay(gameObject.name);
                }
            }

            if (!ChangeNameonLock)
            {
                DMReference.DisplayObjectNameScript.ActivateNameDisplay(gameObject.name);                                   //Activate ObjectNamePanel
            }
        }

        if (PointerScript.ClipboardActive == false && !isBackground && !AlreadyActive && !IsFullTrigger && !PauseScript.InPause)
        {
            AlreadyActive = true;
            HighlightonHover.SetActive(true);

        

            this.ObjectSprite.enabled = false;
            //Object_Collider.size = new Vector2(Object_Collider.size.x + 1, Object_Collider.size.y + 2);             //ATTENTION: MAYBE COLLIDER SIZE SHOULD BE MODULAR
        }
    }

    private void OnMouseExit()                                                                          //When the Cursor Exits an Object, clear the Highlight and Mark
    {
        DMReference.DisplayObjectNameScript.DeactivateNameDisplay();                                    //Deactivate ObjectNamePanel
        DMReference.CursorScript.ActivateCursorSprite();
        if (!RequestInteract)
        {
            ClearHighlight();
        }
    }

    private void OnMouseOver()                                                                          //When the Object is clicked, it remains marked and is added to the Highlighted List in DataManager
    {
        DMReference.DisplayObjectNameScript.SetDisplayPosition();                                       //SetNamePanelPosition


        // Call InteractButtons
        //----------------------------------------------------------------------------------------------------------------------------------------------------
        if (PointerScript.ClipboardActive == false && PointerScript.LockInteract == false && Input.GetMouseButtonDown(0))
        {
            PointerScript.DisableWallTrigger();

            if (DataManager.ToShove.Count < 1 && DataManager.ToDance.Count < 1)                                                  //Flicker Character Collider -> Make the Collider always "enter" the ObjectCollider on Click
            {
                DMReference.CurrentCharacter.GetComponent<Collider2D>().enabled = false;
                DMReference.CurrentCharacter.GetComponent<Collider2D>().enabled = true;
            }

            DMReference.DisplayObjectNameScript.DeactivateNameDisplay();                        //Deactivate the NamePanel
            DMReference.CursorScript.ActivateCursorSprite();                                    //Reactivate the Standard Cursor
            
            RequestInteract = true;                                                             //Remember that this Object requests an Interaction
            DataManager.Highlighted_Current.Add(ThisObject);                                    //Access List in MoveScript, Set RequestInteract false, ClearHighlight, Remove Object
            CompareNewInput();
            if(!isBackground && !IsFullTrigger)
            {
                StartCoroutine(ExpandAndContract());
            }
        }



        // Unlock by Item Drag Success
        //----------------------------------------------------------------------------------------------------------------------------------------------------
        if (Input.GetMouseButtonUp(0) && DMReference.InventoryRef.TryDragUnlock == true && DMReference.InventoryRef.DraggedItemID == Item_Key_ID)
        {

            FetchAllData();
            Lock_State = false;
            UpdateAllData();
            if (GrantReward_Script != null) { GrantReward_Script.GrantReward(); }

            DMReference.MoveScript.DisableInput();
            DMReference.MoveScript.DisableInteract();
            DMReference.MoveScript.Activate_CallEnableInput();
            DMReference.MoveScript.Activate_CallEnableInteract();

            foreach (Draggable Item in DataManager.Item_List)
            {
                if(DMReference.InventoryRef.DraggedItemID == Item.ID)
                {
                    Item.RemoveOnUse();
                }
            }
            //DataManager.Item_List[DMReference.InventoryRef.DraggedItemID - 1].RemoveOnUse(); //Error: Dragged_Item_Index does not Equal Index in Item_list, but in Draggable List!!!!!!!!!!!!!!!!!!!!!!!
            // Delete Item from Draggable List


            ItemUnlock_Trigger_EditGoal();
            if (AddCodeScript != null) { AddCodeScript.AddCode(); }                         //Add Code on succesful interaction

            if (!isBackground && TriggeronUnlock)
            {
                DMReference.MoveScript.targetPosition = DMReference.MoveScript.player.position;
                DataManager.ToInteract.Clear();
                DataManager.ToInteract.Add(this);

                //if (UnlockDialogueScript != null) { UnlockDialogueScript.ModifyDialogue(); }                //Modify the Dialogue if unique Un/LockedObject Dialogue is available

                InteractionController.SetActive(true);
                InteractionController.transform.GetChild(0).gameObject.SetActive(false);                     //Enable Dialogue Button 
                InteractionController.transform.GetChild(1).gameObject.SetActive(false);                     //Enable Interact Button 
                InteractionController.GetComponent<InteractionScript>().TriggerInteraction();
            }
        }


        // Unlock by Item Drag Fail
        //----------------------------------------------------------------------------------------------------------------------------------------------------
        if (Input.GetMouseButtonUp(0) && DMReference.InventoryRef.TryDragUnlock == true && DMReference.InventoryRef.DraggedItemID != Item_Key_ID)    //When the Item does not Unlock.
        {
            DMReference.MoveScript.DisableInput();
            DMReference.MoveScript.DisableInteract();
            DMReference.MoveScript.Activate_CallEnableInput();
            DMReference.MoveScript.Activate_CallEnableInteract();

            if (!isBackground && !IsFullTrigger)
            {
                StartCoroutine(UnlockFlashRed());
            }
            if (DataManager.RosieActive == true)
            {
                if(DMReference.RosieComment != null && ObjectComment != null)
                {
                    DMReference.RosieComment.SetActive(true);
                    DMReference.ObjectCommentRosie.text = ObjectComment.CommentText[0];                                        //Set comment for this specific Object
                    //Add Typewriter Effect / pass a Text
                    StartCoroutine(ControlCommentRosie());
                }  
            }

            if (DataManager.RosieActive == false)
            {
                if(DMReference.BebeComment != null && ObjectComment != null)
                {
                    DMReference.BebeComment.SetActive(true);
                    DMReference.ObjectCommentBebe.text = ObjectComment.CommentText[1];                                         //Set comment for this specific Object
                    //Add Typewriter Effect / pass a Text
                    StartCoroutine(ControlCommentBebe());
                }
            }

            if (ForcedInteraction)                                                                          //Force Interaction Denied response when Item doesnt work
            {
                StartCoroutine(UnlockFlashRed());

                DMReference.MoveScript.targetPosition = DMReference.MoveScript.player.position;
                DataManager.ToInteract.Add(this);

                //if (UnlockDialogueScript != null) { UnlockDialogueScript.ModifyDialogue(); }                //Modify the Dialogue if unique Un/LockedObject Dialogue is available

                InteractionController.SetActive(true);
                InteractionController.transform.GetChild(0).gameObject.SetActive(false);                     //Enable Dialogue Button 
                InteractionController.transform.GetChild(1).gameObject.SetActive(false);                     //Enable Interact Button 
                InteractionController.GetComponent<InteractionScript>().TriggerInteraction();
            }
        }

    }

    //----------------------------------------------------------------------------------------------------------------------------------------------------

    private void CompareNewInput()                                                                     //This Method ensures, that only 1 Object is highlighted at a time (Exculding +1 Hover Highlight)
    {
        if (DataManager.Highlighted_Current.Count > 1)                                                  //Check if there is an Object in Highlighted List already
        {
            if (DataManager.Highlighted_Current[0] != DataManager.Highlighted_Current[1])               //Compare the previous and the new Object
            {
                DataManager.Highlighted_Current[0].ClearHighlight();                                    //On mis match, Clear the old Object's highlight
            }
            else
            {
                DataManager.Highlighted_Current.RemoveAt(1);                                            //On a match, clear the added object, for it is a double
            }
        }
    }

    public void ClearHighlight()                                                                        //This method clears the Highlight
    {
        RequestInteract = false;                                                                        //It disables the RequestInteract, to prevent the player character from interacting with an unclicked object
        if (DataManager.Highlighted_Current.Contains(this) == true)                                     //If the object is in the Highlight list currently, it is removed (very important for collectibles, for they are deleted and would break the list) 
        {
            DataManager.Highlighted_Current.RemoveAt(0);
        }
        if (!isBackground && !IsFullTrigger)                                                            //This part of the method disables the Highlight Object, activates the standard Sprite and resets the collider to its original size.
        {
            AlreadyActive = false;

            HighlightonHover.SetActive(false);
            this.ObjectSprite.enabled = true;
            //Object_Collider.size = Original_Collider;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)                                                     //Initiate Interact on Trigger Enter
    {
        if (!isBackground && other.CompareTag("Player"))
        {
            //PlayerDetected = true;
        }

        if (!isBackground && other.CompareTag("Player") && RequestInteract == true && !ForcedInteraction)
        {
            CallInteractionButtons();
        }

        if (!isBackground && other.CompareTag("Player") && RequestInteract == true && ForcedInteraction)
        {
            DMReference.MoveScript.targetPosition = DMReference.MoveScript.player.position;

            DataManager.ToInteract.Add(this);

            /*
            if(TransformDialogueScript != null) 
            { 
                TransformDialogueScript.TransformeDialogue(); 
            }
            */

            if (LockedDialogueScript != null)
            {
                LockedDialogueScript.ModifyDialogue(); 
            }                //Modify the Dialogue if unique LockedObject Dialogue is available

            InteractionController.SetActive(true);
            InteractionController.transform.GetChild(0).gameObject.SetActive(false);                     //Enable Dialogue Button 
            InteractionController.transform.GetChild(1).gameObject.SetActive(false);                     //Enable Interact Button 
            InteractionController.GetComponent<InteractionScript>().TriggerInteraction();
        }
    }

    public void DialogueInteraction()
    {
        DataManager.ToInteract.Add(this);
        
        if(InteractionController == null)
        {
            InteractionController = GameObject.FindGameObjectWithTag("InteractionController");
        }

        InteractionController.SetActive(true);
        InteractionController.transform.GetChild(0).gameObject.SetActive(false);                     //Enable Dialogue Button 
        InteractionController.transform.GetChild(1).gameObject.SetActive(false);                     //Enable Interact Button 
        InteractionController.GetComponent<InteractionScript>().TriggerInteraction();
    }

    public void CallInteractionButtons()
    {
        FetchAllData();
        DMReference.MoveScript.targetPosition = DMReference.MoveScript.player.position;
        DataManager.ToInteract.Add(this);

        if (TransformDialogueScript != null) 
        {
            TransformDialogueScript.TransformeDialogue(); 
        }

        if (LockedDialogueScript != null) 
        { 
            LockedDialogueScript.ModifyDialogue(); 
        }                //Modify the Dialogue if unique LockedObject Dialogue is available


        InteractionController.SetActive(true);
        InteractionController.transform.GetChild(0).gameObject.SetActive(true);                     //Enable Dialogue Button 
        InteractionController.transform.GetChild(1).gameObject.SetActive(true);                     //Enable Interact Button 

        if (CannotTalk == true)
        {
            InteractionController.transform.GetChild(0).gameObject.SetActive(false);                //Disable Dialogue Button 
        }

        if (CannotInteract == true || !AlreadyTalked)
        {
            InteractionController.transform.GetChild(1).gameObject.SetActive(false);                //Disable Interact Button 
        }

        InteractionController.transform.position = this.transform.position;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && 0 < DataManager.ToInteract.Count && DataManager.ToInteract[0] == this)
        {
            //PlayerDetected = false;
            DataManager.ToInteract.RemoveAt(0);
            if (InteractionController != null)
            {
                DMReference.MoveScript.EnableInput();
                DMReference.MoveScript.EnableInteract();
                InteractionController.SetActive(false);
            }
        }
    }

    //Unlock Manager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    //Unlock this Object with a Key (Item or ShovablePosition)
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void Unlock_Object()                                                                         //Call on Object Interaction to check for Unlock
    {
        //print("UnlockMethod" + UnlockMethod);
        if (UnlockMethod == 1)                                                                          //If the Unlock Method is 1 use SwitchUnlock
        {
            SwitchStateUnlock IUReference = null;                                                       //Create an Unlock Variable, which will be used to access the CallSwitchState Method
            IUReference = (SwitchStateUnlock)UnSReference;                                              //Convert the Parent UnlockScript Type(UnSReference) into the SwitchStateUnlock Type 
            IUReference.CallSwitchStateUnlock(ObjectList_ID, ObjectIndex);                              //Call Switch Unlock Initiator in SwitchUnlock Script, pass this Object's List and Index
        }
        if (UnlockMethod == 2)                                                                          //If the Unlock Method is 2 use ShovableUnlock
        {
            ShovableUnlock IUReference = null;                                                          //Create a ItemUnlock Variable, which will be used to access the CallItemUnlock Method
            IUReference = (ShovableUnlock)UnSReference;                                                 //Convert the Parent UnlockScript Type(UnSReference) into the ItemUnlock Type 
            IUReference.CallShovableUnlock(ObjectList_ID, ObjectIndex);                                 //Call Shovable Unlock Initiator in Shovable Unlock Script, pass this Object's List and Index
        }
    }

    //SequenceUnlock
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void ObjectSequenceUnlock()
    {
        if (CanSequenceUnlock == true)
        {
            SeqUReference = this.GetComponent<SequenceUnlock>();
            SeqUReference.CallSequenceUnlock();                                                             //Call Sequence Unlock Method in Sequence Unlock Script
        }
    }


    //Talked
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void TalkedtoObject()
    {
        if (!AlreadyTalked)
        {
            FetchAllData();
            AlreadyTalked = true;


            UpdateAllData();
        }
    }


    public void SuccessfulInteract()
    {   
        if (GrantReward_Script != null) { GrantReward_Script.GrantReward(); }           //Add Code on succesful interaction
        Interact_Trigger_EditGoal();
        if (AddCodeScript != null) { AddCodeScript.AddCode(); }                         //Add Code on succesful interaction
        if(InteractTransformDialogue != null) { InteractTransformDialogue.AddTransformDialogue(); }

        if(InteractionController != null)
        {
            if(ObjectIndex == 2)
            {
                InteractionController.GetComponent<InteractionScript>().ObjectSlideRosie.start(); //Sound
            }
            InteractionController.GetComponent<InteractionScript>().CallAnimation();
        }
    }


    //Trigger Goal Edit (Add Goal or Complete Goal)
    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Dialogue_Trigger_EditGoal()
    {
        if (TriggerGoal == true)
        {
            if (ControlGoalScript.Dialogue_Triggered == true)
            {
                ControlGoalScript.EditGoal();
            }
        }
    }

    public void Interact_Trigger_EditGoal()
    {
        if (TriggerGoal)
        {
            if (ControlGoalScript.Interaction_Triggered == true)
            {
                ControlGoalScript.EditGoal();
            }
        }
    }

    public void ItemUnlock_Trigger_EditGoal()
    {
        if (TriggerGoal)
        {
            if (ControlGoalScript.ItemUnlock_Triggered == true)
            {
                ControlGoalScript.EditGoal();
            }
        }
    }



    public IEnumerator FlashRed()
    {
        if (GetComponent<LockedDialogue>() != null)
        {
            GetComponent<LockedDialogue>().CollectedItemModifyDeniedInteraction();
        }

        if (GetComponent<NPCDialogue>() != null)
        {
            GetComponent<NPCDialogue>().InLockResponse = true;
            GetComponent<NPCDialogue>().advancedDialogueManager.ObjectLockedDialogue(gameObject.GetComponent<NPCDialogue>());
        }
        float elapsedTime = 0f;
        while (elapsedTime < 1)
        {
            ObjectSprite.color = Color.Lerp(Color.red, originalColor, elapsedTime / 1);
            HighlightObjectSprite.color = Color.Lerp(Color.red, originalHighlightColor, elapsedTime / 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ObjectSprite.color = originalColor;
        HighlightObjectSprite.color = originalHighlightColor;

        PassTriggerActivate(2);                                                                             //Activate a Trigger if connected
        if (GetComponent<NPCDialogue>() != null)
        {
            GetComponent<NPCDialogue>().InLockResponse = false;
        }

        if (AlwaysDenyInteraction)
        {
            DMReference.MoveScript.EnableInput();
            DMReference.MoveScript.EnableInteract();
        }
    }


    //Flash Red for Drag Unlock Fail
    public IEnumerator UnlockFlashRed()                                                                     
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1)
        {
            ObjectSprite.color = Color.Lerp(Color.red, originalColor, elapsedTime / 1);
            HighlightObjectSprite.color = Color.Lerp(Color.red, originalHighlightColor, elapsedTime / 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ObjectSprite.color = originalColor;
        HighlightObjectSprite.color = originalHighlightColor;

        if (AlwaysDenyInteraction)
        {
            DMReference.MoveScript.EnableInput();
            DMReference.MoveScript.EnableInteract();
        }
    }


    /*
     public float ExpandFactor = 1.5f; // Factor by which the object expands
    public float ExpandDuration = 0.3f; // Duration for the expansion and contraction
    private Vector3 ObjectSize;

     */

    private IEnumerator ExpandAndContract()
    {
        
        float ExpandDuration = 0.08f; // Duration for the expansion and contraction
        float ContractDuration = 0.15f; // Duration for the expansion and contraction

        Vector3 ExpandSize = ObjectSize * 1.3f;
        Vector3 ContractSize = Original_Collider / 1.3f;
        
        float timeElapsed = 0f;
        
        while (timeElapsed < ExpandDuration)
        {
            transform.localScale = Vector3.Lerp(ObjectSize, ExpandSize, timeElapsed / ExpandDuration);
            DMReference.CurrentCharacter.GetComponent<Collider2D>().enabled = false;
            //Object_Collider.size = Vector3.Lerp(Original_Collider, -ContractSize, timeElapsed / ExpandDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = ExpandSize; // Ensure it reaches the exact target scale

        timeElapsed = 0f;
        while (timeElapsed < ContractDuration)
        {
            transform.localScale = Vector3.Lerp(ExpandSize, ObjectSize, timeElapsed / ContractDuration);

            //Object_Collider.size = Vector3.Lerp(ContractSize, -Original_Collider, timeElapsed / ExpandDuration);
            
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        DMReference.CurrentCharacter.GetComponent<Collider2D>().enabled = true;
        transform.localScale = ObjectSize; // Ensure it reaches the original scale
       // Object_Collider.size = Original_Collider;
    }



    public IEnumerator ControlCommentRosie()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        DMReference.RosieComment.SetActive(false);
    }


    public IEnumerator ControlCommentBebe()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        DMReference.BebeComment.SetActive(false);
    }


    
    public void PassTriggerActivate(int TriggerType)
    {
        if (TriggerScript != null) 
        {
            TriggerScript.CallTriggerActivation(TriggerType); 
        }
    }

    public void CallColliderToggle()
    {
        if (ColliderScript != null)
        {
            ColliderScript.ToggleCollider();
        }
    }


    private void DialogueIDUnlock()
    {
        if(DialogueUnlock)
        {
            foreach(int D_ID in DataManager.ProgressDialogueList)
            {
                if(D_ID == DialogueKey_ID)
                {
                    Lock_State = false;
                    UpdateAllData();
                    break;
                }
            }
        }
    }


    public void FetchAllData()
    {
        DialogueIDUnlock();

        switch (ObjReference.ObjectList_ID)
        {
            case 1:
                Collectable CollectableObjectRef = null;                                                                                                                                                                                    //Create an Unlock Variable, which will be used to access the CallSwitchState Method
                CollectableObjectRef = (Collectable)ObjReference;                                                                                                                                                                           //Convert the Parent UnlockScript Type(UnSReference) into the SwitchStateUnlock Type 
                CollectableObjectRef.FetchData(DataManager.Collectable_List[ObjectIndex].Stored_Lock_State, DataManager.Collectable_List[ObjectIndex].Stored_AlreadyTalked, DataManager.Collectable_List[ObjectIndex].Stored_Collected);    //Fetch new State from DataManager;                              //Call Switch Unlock Initiator in SwitchUnlock Script, pass this Object's List and Index
                ObjReference.ToggleSprites();
                break;
            case 2:
                Shovable ShovableObjectRef = null;                                                                                                                                                                                          //Create an Unlock Variable, which will be used to access the CallSwitchState Method
                ShovableObjectRef = (Shovable)ObjReference;                                                                                                                                                                                 //Convert the Parent UnlockScript Type(UnSReference) into the SwitchStateUnlock Type 
                ShovableObjectRef.FetchData(DataManager.Shovable_List[ObjectIndex].Stored_Lock_State, DataManager.Shovable_List[ObjectIndex].Stored_AlreadyTalked, DataManager.Shovable_List[ObjectIndex].Stored_Shove_Position);           //Fetch new State from DataManager
                ObjReference.ToggleSprites();
                break;
            case 3:
                Portal PortalObjectRef = null;                                                                                                                                                                                              //Create an Unlock Variable, which will be used to access the CallSwitchState Method
                PortalObjectRef = (Portal)ObjReference;                                                                                                                                                                                     //Convert the Parent UnlockScript Type(UnSReference) into the SwitchStateUnlock Type 
                PortalObjectRef.FetchData(DataManager.Portal_List[ObjectIndex].Stored_Lock_State, DataManager.Portal_List[ObjectIndex].Stored_AlreadyTalked, DataManager.Portal_List[ObjectIndex].Stored_Traversed);                        //Fetch new State from DataManager
                ObjReference.ToggleSprites();
                break;
            case 4:
                Switchable SwitchObjectRef = null;                                                                                                                                                                                          //Create an Unlock Variable, which will be used to access the CallSwitchState Method
                SwitchObjectRef = (Switchable)ObjReference;                                                                                                                                                                                 //Convert the Parent UnlockScript Type(UnSReference) into the SwitchStateUnlock Type 
                SwitchObjectRef.FetchData(DataManager.SwitchState_List[ObjectIndex].Stored_Lock_State, DataManager.SwitchState_List[ObjectIndex].Stored_AlreadyTalked, DataManager.SwitchState_List[ObjectIndex].Stored_SwitchState);       //Fetch new State from DataManager
                ObjReference.ToggleSprites();
                break;
            case 5:
                EventSource EventObjectRef = null;                                                                                                                                                                                          //Create an Unlock Variable, which will be used to access the CallSwitchState Method
                EventObjectRef = (EventSource)ObjReference;                                                                                                                                                                                 //Convert the Parent UnlockScript Type(UnSReference) into the SwitchStateUnlock Type 
                EventObjectRef.FetchData(DataManager.EventSource_List[ObjectIndex].Stored_Lock_State, DataManager.EventSource_List[ObjectIndex].Stored_AlreadyTalked, DataManager.EventSource_List[ObjectIndex].Stored_Event_Passed);       //Fetch new State from DataManager
                ObjReference.ToggleSprites();
                break;
            case 6:
                Triggerable TriggerObjectRef = null;                                                                                                                                                                                          //Create an Unlock Variable, which will be used to access the CallSwitchState Method
                TriggerObjectRef = (Triggerable)ObjReference;                                                                                                                                                                                 //Convert the Parent UnlockScript Type(UnSReference) into the SwitchStateUnlock Type 
                TriggerObjectRef.FetchData(DataManager.Triggerable_List[ObjectIndex].Stored_Lock_State, DataManager.Triggerable_List[ObjectIndex].Stored_AlreadyTalked, DataManager.Triggerable_List[ObjectIndex].Stored_Trigger_Passed);       //Fetch new State from DataManager
                ObjReference.ToggleSprites();
                break;
            case 7:
                DancePad DancePadObjectRef = null;                                                                                                                                                                                          //Create an Unlock Variable, which will be used to access the CallSwitchState Method
                DancePadObjectRef = (DancePad)ObjReference;                                                                                                                                                                                 //Convert the Parent UnlockScript Type(UnSReference) into the SwitchStateUnlock Type 
                DancePadObjectRef.FetchData(DataManager.DancePad_List[ObjectIndex].Stored_Lock_State, DataManager.DancePad_List[ObjectIndex].Stored_AlreadyTalked);       //Fetch new State from DataManager
                ObjReference.ToggleSprites();
                break;
            default:
                break;
        }
        ObjReference.CallColliderToggle();
    }

    public void UpdateAllData()
    {
        switch (ObjReference.ObjectList_ID)
        {
            case 1:
                ObjReference.ToggleSprites();
                Collectable CollectableObjectRef = null;                                                                                                                                                                                    //Create an Unlock Variable, which will be used to access the CallSwitchState Method
                CollectableObjectRef = (Collectable)ObjReference;                                                                                                                                                                           //Convert the Parent UnlockScript Type(UnSReference) into the SwitchStateUnlock Type 
                CollectableObjectRef.UpdateData();                                                                                                                                                                                          //Call Switch Unlock Initiator in SwitchUnlock Script, pass this Object's List and Index
                break;
            case 2:
                ObjReference.ToggleSprites();
                Shovable ShovableObjectRef = null;                                                                                                                                                                                          //Create an Unlock Variable, which will be used to access the CallSwitchState Method
                ShovableObjectRef = (Shovable)ObjReference;                                                                                                                                                                                 //Convert the Parent UnlockScript Type(UnSReference) into the SwitchStateUnlock Type 
                ShovableObjectRef.UpdateData();                                                                                                                                                                                             //Call Switch Unlock Initiator in SwitchUnlock Script, pass this Object's List and Index
                break;
            case 3:
                ObjReference.ToggleSprites();
                Portal PortalObjectRef = null;                                                                                                                                                                                              //Create an Unlock Variable, which will be used to access the CallSwitchState Method
                PortalObjectRef = (Portal)ObjReference;                                                                                                                                                                                     //Convert the Parent UnlockScript Type(UnSReference) into the SwitchStateUnlock Type 
                PortalObjectRef.UpdateData();                                                                                                                                                                                               //Call Switch Unlock Initiator in SwitchUnlock Script, pass this Object's List and Index
                break;
            case 4:
                ObjReference.ToggleSprites();
                Switchable SwitchObjectRef = null;                                                                                                                                                                                          //Create an Unlock Variable, which will be used to access the CallSwitchState Method
                SwitchObjectRef = (Switchable)ObjReference;                                                                                                                                                                                 //Convert the Parent UnlockScript Type(UnSReference) into the SwitchStateUnlock Type 
                SwitchObjectRef.UpdateData();                                                                                                                                                                                               //Call Switch Unlock Initiator in SwitchUnlock Script, pass this Object's List and Index
                break;
            case 5:
                ObjReference.ToggleSprites();
                EventSource EventObjectRef = null;                                                                                                                                                                                          //Create an Unlock Variable, which will be used to access the CallSwitchState Method
                EventObjectRef = (EventSource)ObjReference;                                                                                                                                                                                 //Convert the Parent UnlockScript Type(UnSReference) into the SwitchStateUnlock Type 
                EventObjectRef.UpdateData();                                                                                                                                                                                                //Call Switch Unlock Initiator in SwitchUnlock Script, pass this Object's List and Index
                break;
            case 6:
                ObjReference.ToggleSprites();
                Triggerable TriggerObjectRef = null;                                                                                                                                                                                          //Create an Unlock Variable, which will be used to access the CallSwitchState Method
                TriggerObjectRef = (Triggerable)ObjReference;                                                                                                                                                                                 //Convert the Parent UnlockScript Type(UnSReference) into the SwitchStateUnlock Type 
                TriggerObjectRef.UpdateData();                                                                                                                                                                                                //Call Switch Unlock Initiator in SwitchUnlock Script, pass this Object's List and Index
                break;
            case 7:
                ObjReference.ToggleSprites();
                DancePad DancePadObjectRef = null;                                                                                                                                                                                          //Create an Unlock Variable, which will be used to access the CallSwitchState Method
                DancePadObjectRef = (DancePad)ObjReference;                                                                                                                                                                                 //Convert the Parent UnlockScript Type(UnSReference) into the SwitchStateUnlock Type 
                DancePadObjectRef.UpdateData();                                                                                                                                                                                                //Call Switch Unlock Initiator in SwitchUnlock Script, pass this Object's List and Index
                break;
            default:
                break;
        }
    }

}

