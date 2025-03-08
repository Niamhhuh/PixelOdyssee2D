using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{

	private SpriteRenderer theSR;
	public Sprite defaultImage;
	public Sprite pressedImage;

    private EventInstance ArrowClick; //ganz viele Sounds kommen jetzt hier her
    private HashSet<GameObject> countedArrows = new HashSet<GameObject>(); // Track counted arrows

    // Start is called before the first frame update
    void Start()
    {
    	theSR = GetComponent<SpriteRenderer>();

        ArrowClick = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.Click); //Sound

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)){
        	theSR.sprite = pressedImage;

            ArrowClick.start(); //Sound
        }

        if(Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)){
        	theSR.sprite = defaultImage;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Arrow") && !countedArrows.Contains(other.gameObject)) {
            FindObjectOfType<ArrowSpawner>().counter++;
            countedArrows.Add(other.gameObject);
            
            if (FindObjectOfType<ArrowSpawner>().counter == 27) {
                StartCoroutine(FindObjectOfType<GameManager_Street>().endRound());
            }
        }
    }
}
