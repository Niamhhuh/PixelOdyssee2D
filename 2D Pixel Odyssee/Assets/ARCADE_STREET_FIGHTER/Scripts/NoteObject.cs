using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{

	public bool canBePressed;
    public bool hasBeenHit;
    public KeyCode keyToPress;


    // Start is called before the first frame update
    void Start()
    {
        
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
}

    private void OnTriggerExit2D(Collider2D other){

    if(other.tag == "Activator" && !hasBeenHit){
    	canBePressed = false;

        GameManager_Street.instance.NoteMissed();
        hasBeenHit = false;
    }
}
}
