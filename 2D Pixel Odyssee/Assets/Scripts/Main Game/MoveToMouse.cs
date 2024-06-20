using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMouse : MonoBehaviour
{
    public static List<MoveToMouse> moveableObjects = new List<MoveToMouse>();
    public float speed = 5f;
    public Vector3 target;
    private bool selected;

    public Animation anim;  //Kimi added this for animation
    public Animator animator; //Tom trying stuff for animation
    public float distanceThreshold = 0.1f; //Tom and ChatGBT team up to make more trash
    private bool targetSet = false; //Tom and ChatGBT team up to make more trash

    SoundManagerHub SoundManagerHub;

    void Awake()
    {
        moveableObjects.Clear();
        SoundManagerHub = GameObject.FindGameObjectWithTag("SoundManagerHub").GetComponent<SoundManagerHub>();
    }

    void Start()
    {
        anim = GetComponent<Animation>();
        moveableObjects.Add(this);
        target = transform.position;
    }

    void Update()
    {
        // Change to right mouse button (index 1)
        if (Input.GetMouseButtonDown(1) && selected)
        {
            speed = 5f;//Tom and ChatGBT team up to make more trash
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;
            target.y = transform.position.y;

            targetSet = true; ////Tom and ChatGBT team up to make more trash
            animator.SetFloat("Speed", speed); //Tom effing around
        }

        if (targetSet)      //Tom and ChatGBT team up to make more trash
        {
            float step = speed * Time.deltaTime;    //Tom and ChatGBT team up to make more trash
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) <= distanceThreshold)//Tom and ChatGBT team up to make more trash
            {
                Debug.Log("Ziel erreicht!");//Tom and ChatGBT team up to make more trash
                targetSet = false;//Tom and ChatGBT team up to make more trash
                animator.SetFloat("Speed", 0); ; //Tom and ChatGBT team up to make more trash
            }
        }
    }

    // Change to right mouse button (index 1)
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            selected = true;
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;

            foreach (MoveToMouse obj in moveableObjects)
            {
                if (obj != this)
                {
                    obj.selected = false;
                    obj.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }

            var cinemachine = FindObjectOfType<CinemachineVirtualCamera>();
            if (cinemachine != null)
            {
                cinemachine.Follow = this.transform;
            }
        }
    }
}
