using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class Frogger : MonoBehaviour

{
    private SpriteRenderer spriteRenderer;
    public Sprite idleSprite;
    public Sprite leapSprite;
    public Sprite deadSprite;
    public Vector3 spawnPosition;
    private float farthestRow;
    public bool noMove;
    SoundManager soundManager;

    private EventInstance FrJump; //ganz viele Sounds kommen jetzt hier her
    private EventInstance FrDeath;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnPosition = transform.position;
        soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
        noMove = false;

        FrJump = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.FrJump); //Sound
        FrDeath = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.FrDeath);
    }
    public float scrollSpeed = 3.0f;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && noMove == false) {
            transform.rotation = Quaternion.Euler(0f , 0f, 0f);
            Move(Vector3.up);
            soundManager.PlaySfx(soundManager.jump);
        }

        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) && noMove == false) {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            Move(Vector3.down);
            soundManager.PlaySfx(soundManager.jump);
        } 
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) && noMove == false) {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            Move(Vector3.left);
            soundManager.PlaySfx(soundManager.jump);
        } 
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) && noMove == false) {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            Move(Vector3.right);
            soundManager.PlaySfx(soundManager.jump);
        }
        //else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            //Move(Vector3.right);
        }
        //else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            //Move(Vector3.left);
        }
        //float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        //Vector3 newPosition = transform.position + Vector3.up * scrollInput;
        //transform.position = newPosition;
    }

    private void Move(Vector3 direction)
    {

        Vector3 destination = transform.position + direction;

        Collider2D barrier = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Barrier"));
        Collider2D platform = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Platform"));
        Collider2D obstacle = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Obstacle"));

        if (barrier != null) {
            return;
        }
        if (platform != null)
        {
            transform.SetParent(platform.transform);}
            else
            {
            transform.SetParent(null);
            }
        if (obstacle != null && platform == null)
        {
            
            transform.position = destination;
            Death();
        }
        else
        {
            if (destination.y > farthestRow)
            {
                farthestRow = destination.y;
                FindAnyObjectByType<GameManager1>().AdvanceRow();
            }

            StartCoroutine(Leap(destination));
        }
    }

    private IEnumerator Leap(Vector3 destination)
    {
        FrJump.start(); //Sound

        Vector3 StartPosition = transform.position;
        
        float elapsed = 0f;
        float duration = 0.125f;

        spriteRenderer.sprite = leapSprite;

        while (elapsed < duration)
        {
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

    public void Death()
    {
        StopAllCoroutines();    

        transform.rotation = Quaternion.identity;
        spriteRenderer.sprite = deadSprite;
        enabled = false;
        
        //Invoke(nameof(Respawn), 1f);
        
        FindAnyObjectByType<GameManager1>().Died();

        FrDeath.start(); //Sound

    }

    public void Respawn()
    {
        StopAllCoroutines();

        transform.SetParent(null);
        transform.rotation = Quaternion.identity;
        farthestRow = spawnPosition.y;
        spriteRenderer.sprite = idleSprite;
        gameObject.SetActive(true);
        transform.position = spawnPosition;
        enabled = true; 


    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (enabled && other.gameObject.layer == LayerMask.NameToLayer("Obstacle") && transform.parent == null)
        {
            Death();    
        }
    }
}
