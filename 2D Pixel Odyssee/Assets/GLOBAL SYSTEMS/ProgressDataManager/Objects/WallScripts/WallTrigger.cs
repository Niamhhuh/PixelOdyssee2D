using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    DataManager DMReference;
    // Start is called before the first frame update
    void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
    }


    //Stop Movement When Character Enters this 
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DMReference.MoveScript.targetPosition = DMReference.MoveScript.player.position;
            DMReference.MoveScript.DisableInput();
        }
    }
}
