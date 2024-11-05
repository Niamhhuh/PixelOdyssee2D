using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    //Generic Object Variables ------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public int ID;				                                                    //ID of the Object, required to find it in the list
    public bool Lock_State;                                                         //check if this Object is Interaction_Locked/Limited
    //public(Dialogue)			                                                    //Dialogue of this object

    [HideInInspector] public bool NewObject = true;
    [HideInInspector] public bool IsTriggerableObj = false;

    public CharacterScript CurrentCharacter;

    //Interaction Variables ---------------------------------------------------------------------------------------------------------------------------------------------------
    public SpriteRenderer ObjectSprite = null;              //SpriteRenderer of Object, which is disabled on Highlight
    private BoxCollider2D Object_Collider = null;            //Collider of the Object, which is expanded when the Object is marked for interaction
    private Vector2 Original_Collider;                       //This Vector stores the initial size of the collider
    private Color originalColor;

    private ObjectScript ThisObject = null;                  //ObjectScript, which is added to the Currently_Highlighted List, check if object is selected
    public GameObject HighlightonHover = null;              //Child Object, which contains the Highlighted Sprite of the Object
    public UiToMouse PointerScript = null;


    [HideInInspector] public bool RequestInteract = false;                     //Request is set true, when the Object is clicked and reset, when another object is clicked. 
    [HideInInspector] public bool AlreadyActive = false;                       //AlreadyActive marks an Object as already highlighted, preventing it from expanding its collider multiple times
    public bool isBackground;                                //isBackground is set true on the background, disabling all functions for it


    public GameObject InteractionController = null;                       //
    public ObjectScript ObjReference = null;

    //Lock/Unlock Variables ---------------------------------------------------------------------------------------------------------------------------------------------------
    [Range(0, 2)] public int UnlockMethod = 0;               //Pass Unlock Method from attached Unlock Script to Object Script
    public bool CanSequenceUnlock = false;                   //Enable Object Script to use SequenceUnlock Method, when SequenceUnlock is attached


    [HideInInspector] public int ObjectList_ID;                                                      //ID which marks the List this Object is stored in          //used for UnlockMethods
    [HideInInspector] public int ObjectIndex;                                                        //Index of this Object in its list                          //used for UnlockMethods

    public DataManager DMReference = null;                                                 //
    [HideInInspector] public SequenceUnlock SeqUReference = null;                                     //
    [HideInInspector] public UnlockScript UnSReference = null;                                        //


    //Set Data
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {
        if(gameObject.GetComponent<Triggerable>() != null )
        {
            IsTriggerableObj = true;
        }
        ThisObject = this.GetComponent<ObjectScript>();
        CurrentCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterScript>();
        PointerScript = GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>();
        if (!isBackground && !IsTriggerableObj)
        {
            ObjectSprite = this.GetComponent<SpriteRenderer>();
            originalColor = ObjectSprite.color;
            Object_Collider = this.GetComponent<BoxCollider2D>();
            Original_Collider = Object_Collider.size;
            HighlightonHover = this.transform.GetChild(0).gameObject;                                   //the first child must ALWAYS be the Highlight Object
            HighlightonHover.SetActive(false);
            InteractionController = GameObject.FindGameObjectWithTag("InteractionController");
        }
    }

    //Interaction Manager
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void OnMouseEnter()                                                                         //When the Cursor enters an Object, Highlight it, mark it as Highlighted
    {
        if (PointerScript.InventoryActive == false && !isBackground && !AlreadyActive && !IsTriggerableObj)
        {
            AlreadyActive = true;
            HighlightonHover.SetActive(true);
            this.ObjectSprite.enabled = false;
            Object_Collider.size = new Vector2(Object_Collider.size.x + 1, Object_Collider.size.y + 2);             //ATTENTION: MAYBE COLLIDER SIZE SHOULD BE MODULAR
        }
    }

    private void OnMouseExit()                                                                          //When the Cursor Exits an Object, clear the Highlight and Mark
    {
        if (!RequestInteract)
        {
            ClearHighlight();
        }
    }

    private void OnMouseOver()                                                                          //When the Object is clicked, it remains marked and is added to the Highlighted List in DataManager
    {
        if (PointerScript.LockInteract == false && Input.GetMouseButtonDown(0))
        {
            RequestInteract = true;
            DataManager.Highlighted_Current.Add(ThisObject); //Access List in MoveScript, Set RequestInteract false, ClearHighlight, Remove Object
            CompareNewInput();
        }
    }

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
        if (!isBackground && !IsTriggerableObj)                                                                              //This part of the method disables the Highlight Object, activates the standard Sprite and resets the collider to its original size.
        {
            AlreadyActive = false;

            HighlightonHover.SetActive(false);
            this.ObjectSprite.enabled = true;
            Object_Collider.size = Original_Collider;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)                                                     //Initiate Interact on Trigger Enter
    {
        if (other.CompareTag("Player") && RequestInteract == true)
        {
            DMReference.MoveScript.targetPosition = DMReference.MoveScript.player.position;
            DataManager.ToInteract.Add(this);
            InteractionController.SetActive(true);
            //Activate the required Arrows 
            InteractionController.transform.position = this.transform.position;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && 0 < DataManager.ToInteract.Count && DataManager.ToInteract[0] == this)
        {
            DataManager.ToInteract.RemoveAt(0);
            if (InteractionController != null)
            {
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
        if (UnlockMethod == 1)                                                                          //If the Unlock Method is 1 use SwitchUnlock
        {
            //print("I'm called2");
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

    public void ObjectSequenceUnlock()
    {
        if (CanSequenceUnlock == true)
        {
            SeqUReference = this.GetComponent<SequenceUnlock>();
            SeqUReference.CallSequenceUnlock();                                                             //Call Sequence Unlock Method in Sequence Unlock Script
        }
    }


    public IEnumerator FlashRed()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1)
        {
            ObjectSprite.color = Color.Lerp(Color.red, originalColor, elapsedTime / 1);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }
        ObjectSprite.color = originalColor; 
    }
}

