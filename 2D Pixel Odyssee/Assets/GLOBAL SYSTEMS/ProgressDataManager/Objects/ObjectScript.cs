using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    [Range(0, 2)] public int UnlockMethod = 0;           //Pass Unlock Method from attached Unlock Script to Object Script
    public bool CanSequenceUnlock = false;                   //Enable Object Script to use SequenceUnlock Method, when SequenceUnlock is attached
    private SpriteRenderer ObjectSprite;
    private BoxCollider2D Object_Collider;
    private GameObject HighlightonHover = null;
    //public GameObject InteractionDetector;
    private void Start()
    {
        ObjectSprite = this.GetComponent<SpriteRenderer>();
        Object_Collider = this.GetComponent<BoxCollider2D>();
        HighlightonHover = this.transform.GetChild(0).gameObject;             //the first child must ALWAYS be the Highlight Object
        HighlightonHover.SetActive(false);
        //InteractionDetector = this.transform.GetChild(1).gameObject;       //the second child must ALWAYS be the Interaction Collider
    }

    private void OnMouseEnter()
    {
        HighlightonHover.SetActive(true);
        this.ObjectSprite.enabled = false;
        Object_Collider.size = new Vector2(Object_Collider.size.x + 4, Object_Collider.size.y + 2);
    }

    private void OnMouseExit()
    {
        HighlightonHover.SetActive(false);
        this.ObjectSprite.enabled = true;
        Object_Collider.size = new Vector2(Object_Collider.size.x - 4, Object_Collider.size.y - 2);
    }
}
