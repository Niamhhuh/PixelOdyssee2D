using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pong_PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool isAI;
    [SerializeField] private GameObject ball;
    private Rigidbody2D rb;
    private Vector2 playerMove;
    [SerializeField] private float speed = 4;

    private bool isReversed = false; // Tracks whether controls are reversed

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
        float input = Input.GetAxisRaw("Vertical");

        if (isReversed)
        {
            input *= -1; // Reverse movement input
        }

        playerMove = new Vector2(0, input);
        rb.velocity = new Vector2(0, input * speed);
    }

    private void AIControl()
    {
        if (ball != null && ball.transform.position.y > transform.position.y + 1.25f)
        {
            playerMove = new Vector2(0, 1);
        }
        else if (ball != null && ball.transform.position.y < transform.position.y - 1.25f)
        {
            playerMove = new Vector2(0, -1);
        }
        else
        {
            playerMove = new Vector2(0, 0);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = playerMove * movementSpeed;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    //---------------------------------------------Punishments
    //-------------------------------------------------------------------------------------------------------------------------
    public void ToggleReverseControls()
    {
        isReversed = !isReversed; // Toggle between normal and reversed controls
        Debug.Log("Controls Reversed: " + isReversed);
    }


    public void IncreaseSpeed(float multiplier) //Increases Speed 
    {
        movementSpeed *= multiplier;
        Debug.Log("Player speed increased! New speed: " + movementSpeed);
    }
}
