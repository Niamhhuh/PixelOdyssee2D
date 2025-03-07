using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{

	public bool canBePressed;
    public bool hasBeenHit;
    public KeyCode keyToPress;
    public Animator silverAnimator;

    public SpriteRenderer spriteRenderer;
    public List<Sprite> arrowSprites;
    public List<KeyCode> arrowKeys;

    public float randomizationChance = 0.4f;

    private bool hasBeenRandomized = false; 

    public bool round2;

    private EventInstance SFArrowSwitch; //Sound


    // Start is called before the first frame update
    void Start()
    {
        GameObject Silver = GameObject.Find("Silver");
        silverAnimator = Silver.GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        canBePressed = false;
        hasBeenHit = false;

        SFArrowSwitch = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.SFArrowSwitch); //Sound
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(keyToPress)){
            if(canBePressed && !hasBeenHit){
                hasBeenHit = true;
                GameManager_Street.instance.NoteHit();
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Activator"){
    	    canBePressed = true;
        }
        else if (other.tag == "RandomizerZone" && !hasBeenRandomized && round2){
            TryRandomizeArrows();
        }
    }

    private void OnTriggerExit2D(Collider2D other){

    if(other.tag == "Activator" && !hasBeenHit){
    	canBePressed = false;

        GameManager_Street.instance.NoteMissed();
        hasBeenHit = false;
    }
}
    void TryRandomizeArrows(){
        hasBeenRandomized = true;

        float roll = Random.Range(0f, 1f);
        if (roll <= randomizationChance){
            RandomizeArrow();
        }
    }

    void RandomizeArrow(){
        silverAnimator.SetTrigger("SnappingFingers");

        SFArrowSwitch.start(); //Sound

        int randomIndex = Random.Range(0, arrowSprites.Count);
        spriteRenderer.sprite = arrowSprites[randomIndex];
        keyToPress = arrowKeys[randomIndex];
    }

}
