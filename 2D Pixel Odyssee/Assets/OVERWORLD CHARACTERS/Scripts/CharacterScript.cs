using System.Collections;

using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CharacterScript : MonoBehaviour
{
    public GameObject RosieObj;
    public GameObject BeBeObj;

    [HideInInspector] public GameObject RosieComment = null;
    [HideInInspector] public GameObject BebeComment = null;
    UiToMouse uitomouse;
    private Transform newPlayer;

    AdvancedDialogueManager DialogueScript;
    PauseMenu PauseScript;

    //public bool AllowInput;

    //public static GameObject Character;

    //public float Movespeed = 5f;
    //public Vector3 TargetPosition;


    //public float DestinationMargin = 0.1f; //Tom and ChatGBT team up to make more trash
    //private bool MoveToPoint = false; //Tom and ChatGBT team up to make more trash

    //public Animation Chaanimation;  //Kimi added this for animation
    //public Animator Chaanimator; //Tom trying stuff for animation
    SoundManagerHub SoundManager;

    void Start()
    {
        RosieComment = GameObject.FindGameObjectWithTag("CommentSpriteRosie");
        BebeComment = GameObject.FindGameObjectWithTag("CommentSpriteBebe");

        uitomouse = GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>();

        DialogueScript = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<AdvancedDialogueManager>();
        PauseScript = GameObject.FindGameObjectWithTag("PauseController").GetComponent<PauseMenu>();

        if(RosieComment != null)
        {
            RosieComment.SetActive(false);
            BebeComment.SetActive(false);
        }

        if (uitomouse != null && uitomouse.playerAnimator != null)
        {
            uitomouse.playerAnimator = GameObject.FindGameObjectWithTag("Rosie").GetComponent<Animator>();
            uitomouse.playerAnimator2 = GameObject.FindGameObjectWithTag("Bebe").GetComponent<Animator>();
        }

        var cinemachine = FindObjectOfType<CinemachineVirtualCamera>();
        if (cinemachine != null)
        {
            cinemachine.Follow = this.transform;
        }

        SetCharacter();
    }

    public void SetStandardCase()
    {
        RosieComment.SetActive(false);
        BebeComment.SetActive(false);
        //Chaanimation = GetComponent<Animation>();
        //TargetPosition = transform.position;
        BeBeObj.SetActive(false);
        DataManager.RosieActive = true;
        //AllowInput = true;
    }

    public void SwitchCharacters()
    {
        if (!PauseScript.InPause && !DialogueScript.InDialogue)
        { 
            DataManager.RosieActive = !DataManager.RosieActive; 
            
            if (DataManager.RosieActive == false)
            {
                RosieObj.SetActive(false);
                BeBeObj.SetActive(true);
                uitomouse.SwitchCharacter();
            }
            else
            {
                RosieObj.SetActive(true);
                BeBeObj.SetActive(false);
                uitomouse.SwitchCharacter();
            }

        }        
    }

    private void SetCharacter()
    {
            if (DataManager.RosieActive == false)
            {
                RosieObj.SetActive(false);
                BeBeObj.SetActive(true);
                uitomouse.SwitchCharacter();
            }
            else
            {
                RosieObj.SetActive(true);
                BeBeObj.SetActive(false);
                uitomouse.SwitchCharacter();
            }
    }


    /*
   public void DisableInput ()
   {
       AllowInput = false;
   }

   public void EnableInput()
   {
       AllowInput = true;
   }

   void Update()
   {
       if (Input.GetMouseButtonDown(0) && AllowInput)
       {
           TargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
           TargetPosition.z = transform.position.z;
           TargetPosition.y = transform.position.y;

           MoveToPoint = true; ////Tom and ChatGBT team up to make more trash
           //Chaanimator.SetFloat("Speed", Movespeed); //Tom effing around
       }

       if (MoveToPoint)      //Tom and ChatGBT team up to make more trash
       {
           transform.position = Vector3.MoveTowards(transform.position, TargetPosition, Movespeed * Time.deltaTime);

           if (Vector3.Distance(transform.position, TargetPosition) <= DestinationMargin)//Tom and ChatGBT team up to make more trash
           {
               MoveToPoint = false;//Tom and ChatGBT team up to make more trash
               //Chaanimator.SetFloat("Speed", 0); ; //Tom and ChatGBT team up to make more trash
           }
       }
   }
   */
}

