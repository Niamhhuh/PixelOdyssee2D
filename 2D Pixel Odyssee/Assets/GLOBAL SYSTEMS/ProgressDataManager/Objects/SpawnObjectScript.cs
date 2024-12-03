using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectScript : MonoBehaviour
{
    public int ID;
    DataManager DMReference;

    private void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
    }
    private void OnTriggerEnter2D(Collider2D other)                                                     //When Passing through the Trigger, adjust the Spawn Position
    {
        if (other.CompareTag("Player"))
        {
            DataManager.SpawnID = ID;
            DataManager.LastRoom = DMReference.currentRoom;
            //print(DataManager.SpawnID);
        }
    }
}
