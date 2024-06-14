using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RosiePower : MonoBehaviour
{
    private Transform Box;
    private Transform Rosie;
    public GameObject RosieGO;
    public MoveToMouse MoveScript;

    public bool topush;

    public int BoxPushedLeft; 
    public int BoxPushedRight;
    public int MaxPush;
    public float speed = 5f;

    public Vector3 pushvector;

    // Start is called before the first frame update
    void Start()
    {
        topush = false;
        BoxPushedLeft = 0;
        BoxPushedRight = 0;
        pushvector = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }


    //target = transform.position;
    //transform.position = Vector3.MoveTowards(transform.position, target, speed* Time.deltaTime);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && topush == true)
        {
            Rosie = collision.gameObject.GetComponent<Transform>();
            MoveScript = collision.gameObject.GetComponent<MoveToMouse>();


            MoveScript.target = new Vector3(Rosie.transform.position.x, Rosie.transform.position.y, Rosie.transform.position.z);
            
            
            // Check to see where the player is
            if (transform.position.x > Rosie.transform.position.x && MaxPush > BoxPushedRight)
            {

                pushvector = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
                transform.position = pushvector;
                ++BoxPushedRight;
                --BoxPushedLeft;
            }
            else if (transform.position.x < Rosie.transform.position.x && MaxPush > BoxPushedLeft)
            {
                
                pushvector = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);
                transform.position = pushvector;
                ++BoxPushedLeft;
                --BoxPushedRight;
            }
            topush = false;

        }

    }

    private void OnMouseOver()
    {   
        if (Input.GetMouseButtonDown(1))
        {
            topush = true;
        }

    }

    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, pushvector, speed * Time.deltaTime);

    }




}
