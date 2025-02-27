using FMOD.Studio;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Frogger : MonoBehaviour

{
    private SpriteRenderer spriteRenderer;
    public Sprite idleSprite;
    public Sprite leapSprite;
    public Sprite deadSprite;
    public Vector3 spawnPosition;
    private float farthestRow;
    public bool noMove;
    public bool died;
    SoundManager soundManager;

    private EventInstance FrJump; //ganz viele Sounds kommen jetzt hier her

//___________________________________________________________________________________________________
//------------------------Standard functions---------------------------------------------------------
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnPosition = transform.position;
        noMove = false;
        died = false;
    }

    private void Start() {
        FrJump = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.FrJump); //Sound
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && noMove == false) {
            transform.rotation = Quaternion.Euler(0f , 0f, 0f);
            Move(Vector3.up);
        }

        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) && noMove == false) {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            Move(Vector3.down);
        } 
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) && noMove == false) {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            Move(Vector3.left);
        } 
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) && noMove == false) {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            Move(Vector3.right);
        }
    }

//___________________________________________________________________________________________________
//------------------------Movement-------------------------------------------------------------------
    private void Move(Vector3 direction) {
        Vector3 destination = transform.position + direction;

        Collider2D barrier = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Barrier"));
        Collider2D platform = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Platform"));
        Collider2D obstacle = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Obstacle"));

        if (barrier != null) {
            return;
        }

        if (platform != null) {
        transform.SetParent(platform.transform);
        }
        else {
            transform.SetParent(null);
        }

        if (obstacle != null && platform == null) {
            
            transform.position = destination;
            Death();
        }
        else {
            if (destination.y > farthestRow) {
                farthestRow = destination.y;
            }
            StartCoroutine(Leap(destination));
        }
    }


    private IEnumerator Leap(Vector3 destination) {
        Debug.Log(died);
        FrJump.start(); //sound

        Vector3 StartPosition = transform.position;
        
        float elapsed = 0f;
        float duration = 0.125f;

        spriteRenderer.sprite = leapSprite;

        while (elapsed < duration) {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(StartPosition, destination, t);
            elapsed += Time.deltaTime;
            noMove = true;
            yield return null;
        }

        noMove = false; 
        transform.position = destination;
        spriteRenderer.sprite = idleSprite;
    }

//___________________________________________________________________________________________________
//------------------------Death stuff----------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D other) {
        if (enabled && other.gameObject.layer == LayerMask.NameToLayer("Obstacle") && transform.parent == null) {
            Death();    
        }
    }
    
    public void Death() {
        if (FindAnyObjectByType<GameManager1>().frogger.gameObject.activeInHierarchy == true) {
            died = true;
            StopAllCoroutines();    

            transform.rotation = Quaternion.identity;
            spriteRenderer.sprite = deadSprite;
            enabled = false;
        
            FindAnyObjectByType<GameManager1>().Died();
        }
    }

    public void Respawn() {
        died = false;
        StopAllCoroutines();

        transform.SetParent(null);
        transform.rotation = Quaternion.identity;
        farthestRow = spawnPosition.y;
        spriteRenderer.sprite = idleSprite;
        gameObject.SetActive(true);
        transform.position = spawnPosition;
        enabled = true; 
    }
}