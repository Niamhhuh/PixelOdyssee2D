using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectScript : MonoBehaviour
{
    public int ID;

    private void OnTriggerEnter2D(Collider2D other)                                                     //When Passing through the Trigger, adjust the Spawn Position
    {
        if (other.CompareTag("Player"))
        {
            DataManager.SpawnID = ID;
            //print(DataManager.SpawnID);
        }
    }
}
