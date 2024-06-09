using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RosiePower : MonoBehaviour
{
    private Transform Box;
    private Transform Rosie;
    public GameObject RosieGO;

    public bool topush;

    public int BoxPushedLeft; 
    public int BoxPushedRight;
    public int MaxPush;

    public Vector3 pushvector;

    // Start is called before the first frame update
    void Start()
    {
        topush = false;
        BoxPushedLeft = 0;
        BoxPushedRight = 0;
    }
    
    //target = transform.position;
    //transform.position = Vector3.MoveTowards(transform.position, target, speed* Time.deltaTime);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && topush == true)
        {
            Rosie = collision.gameObject.GetComponent<Transform>();
            Box = this.GetComponent<Transform>();

            // Check to see where the player is, and turn toward them
            if (Box.transform.position.x > Rosie.transform.position.x && MaxPush > BoxPushedRight)
            {

                pushvector = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z); 
                Box.transform.position = pushvector;
                ++BoxPushedRight;
                --BoxPushedLeft;
            }
            else if (Box.transform.position.x < Rosie.transform.position.x && MaxPush > BoxPushedLeft)
            {
                
                pushvector = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
                Box.transform.position = pushvector;
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

    


}
