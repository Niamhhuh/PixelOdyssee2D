using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 10;
    [SerializeField] private float speedIncrease = 0.25f;
    [SerializeField] private Text playerScore;
    [SerializeField] private Text AIScore;
    [SerializeField] private GameObject winPanel;

    private int aiScore;
    private int plScore;

    private Rigidbody2D rb;
    private int hitCounter;
    PSSoundManager PSSoundManager;
    //_____________________________________________________________________________________
    //-----------------------Set-up below--------------------------------------------------

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        Invoke("StartBall", 2.0f);
        aiScore = 0;
        plScore = 0;

        PSSoundManager = GameObject.FindGameObjectWithTag("SoundManagerPS").GetComponent<PSSoundManager>();
    }

    private void FixedUpdate() {        //set velocity of ball throughout the game
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, initialSpeed + (speedIncrease * hitCounter));
    }

    private void StartBall() {          //set inital speed
        rb.velocity = new Vector2(-1, 0) * (initialSpeed + speedIncrease * hitCounter);
    }

    private void ResetBall() {          //resets ball to position 0,0 and invokes StartBall()
        rb.velocity = new Vector2(0,0);
        transform.position = new Vector2(0,0);
        hitCounter = 0;
        Invoke("StartBall", 2f);
    }
    //_____________________________________________________________________________________
    //-----------------------Ball movement below-------------------------------------------

    private void PlayerBounce(Transform myObject) {
        hitCounter++;
        Vector2 ballPos = transform.position;
        Vector2 playerPos = myObject.position;
        float xDirection, yDirection;

        if(transform.position.x > 0) {
            xDirection = -1;
        }
        else {
            xDirection = 1;
        }
        yDirection = (ballPos.y - playerPos.y) / myObject.GetComponent<Collider2D>().bounds.size.y;
        if(yDirection == 0) {
            yDirection = 0.25f;
        }
        rb.velocity = new Vector2(xDirection, yDirection) * (initialSpeed + (speedIncrease*hitCounter));
    }

    //_____________________________________________________________________________________
    //-----------------------Collisions below----------------------------------------------

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Player" || collision.gameObject.name == "AI") {
            PlayerBounce(collision.transform);

            PSSoundManager.PlaySfxPS(PSSoundManager.CollisionPlayer);
        }

        //if (collision.gameObject.name == "Wall")
        //{
            //PSSoundManager.PlaySfxPS(PSSoundManager.CollisionWall);
        //}
    }

    //--------------------enter deathzone---------------------------------------------------

    void OnTriggerEnter2D(Collider2D col) {
        if (transform.position.x > 0) {
            playerScore.text = (int.Parse(playerScore.text) + 1).ToString();
            plScore++;

            PSSoundManager.PlaySfxPS(PSSoundManager.PointPlayer);
        }
        else if (transform.position.x < 0) {
            AIScore.text = (int.Parse(AIScore.text) + 1).ToString();
            aiScore++;
            PSSoundManager.PlaySfxPS(PSSoundManager.PointEnemy);
        }
        
        if (aiScore == 3 || plScore == 3) //checks whether the game is finished
        {
            winPanel.SetActive(true);
            Destroy(gameObject);

            PSSoundManager.StopMusicPS(PSSoundManager.MusicPainStation);
            PSSoundManager.PlaySfxPS(PSSoundManager.PlayerWin);
            //PSSoundManager.PlaySfxPS(PSSoundManager.PlayerLost);
        }
        else{
            ResetBall();
        }
    }
}