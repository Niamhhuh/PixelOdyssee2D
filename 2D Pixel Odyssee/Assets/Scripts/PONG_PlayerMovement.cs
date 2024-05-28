using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pong_PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool isAI;
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject p_warmth;
    [SerializeField] private GameObject p_whip;
    [SerializeField] private GameObject p_electro;

    private Rigidbody2D rb;
    private Vector2 playerMove;
    [SerializeField] private float speed = 4;

//-------------------------------------------------------------------------------------------------------------------------
//---------------------------------------------During Play
//-------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isAI)
        {
            AIControl();
        }
        else
        {
            PlayerControl();
        }
    }

//-------------------------------------------------------------------------------------------------------------------------
//---------------------------------------------Functions - Player/Ai Movement
//-------------------------------------------------------------------------------------------------------------------------

    private void PlayerControl()
    {
        playerMove = new Vector2(0, Input.GetAxisRaw("Vertical"));
        rb.velocity = new Vector2(0, Input.GetAxisRaw("Vertical") * speed); //this doesn´t work
    }

    private void AIControl()
    {
        if(ball.transform.position.y > transform.position.y + 0.5f)
        {
            playerMove =new Vector2(0,1);
        }
        else if(ball.transform.position.y < transform.position.y - 0.5f)
        {
            playerMove = new Vector2(0, -1);
        }
        else
        {
            playerMove = new Vector2(0,0);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = playerMove * movementSpeed;
    }

//-------------------------------------------------------------------------------------------------------------------------
//---------------------------------------------Functions - Pain Effects
//-------------------------------------------------------------------------------------------------------------------------


}
