using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Frogger : MonoBehaviour

{
    private SpriteRenderer spriteRenderer;
    public Sprite idleSprite;
    public Sprite leapSprite;
    public Sprite deadSprite;
    public Vector3 spawnPosition;
    private float farthestRow;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnPosition = transform.position;
    }
    public float scrollSpeed = 3.0f;
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.W)) {
            transform.rotation = Quaternion.Euler(0f , 0f, 0f);
            Move(Vector3.up);
        }

        else if (Input.GetKeyDown(KeyCode.S)) {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            Move(Vector3.down);
        } 
        else if (Input.GetKeyDown(KeyCode.A)) {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            Move(Vector3.left);
        } 
        else if (Input.GetKeyDown(KeyCode.D)) {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            Move(Vector3.right);
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
        Vector3 StartPosition = transform.position;
        
        float elapsed = 0f;
        float duration = 0.125f;

        spriteRenderer.sprite = leapSprite;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(StartPosition, destination, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

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
    }

    public void Respawn()
    {
        StopAllCoroutines();

        transform.rotation = Quaternion.identity;
        transform.position = spawnPosition;
        farthestRow = spawnPosition.y;
        spriteRenderer.sprite = idleSprite;
        gameObject.SetActive(true);
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
