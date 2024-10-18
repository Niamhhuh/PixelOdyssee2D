using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CharacterScript : MonoBehaviour
{
    public GameObject RosieObj;
    public GameObject BeBeObj;

    //public static GameObject Character;
    
    public float Movespeed = 5f;
    public Vector3 TargetPosition;
    public bool Mousepressed;

    public float DestinationMargin = 0.1f; //Tom and ChatGBT team up to make more trash
    private bool MoveToPoint = false; //Tom and ChatGBT team up to make more trash

    //public Animation Chaanimation;  //Kimi added this for animation
    //public Animator Chaanimator; //Tom trying stuff for animation
    SoundManagerHub SoundManager;

    void Start()
    {
        //Chaanimation = GetComponent<Animation>();
        TargetPosition = transform.position;
        BeBeObj.SetActive(false);

        SoundManager = GameObject.FindGameObjectWithTag("SoundManagerHub").GetComponent<SoundManagerHub>();

        var cinemachine = FindObjectOfType<CinemachineVirtualCamera>();
        if (cinemachine != null)
        {
            cinemachine.Follow = this.transform;
        }
    }

    public void SwitchCharacters()
    {
        if(RosieObj.activeSelf == true)
        {
            print(this);
            print("I am called1");
            RosieObj.SetActive(false);
            BeBeObj.SetActive(true);
        }
        else
        {
            print("I'm called2");
            RosieObj.SetActive(true);
            BeBeObj.SetActive(false);
        }
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
}

