using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    List<Collectable> Collectable_List = new List<Collectable>();       //Create a List to store all relevant Variables of Collectable Items
    List<Pushable> Pushable_List = new List<Pushable>();                //Create a List to store all relevant Variables of Pushable Objects
    List<Portal> Portal_List = new List<Portal>();                      //Create a List to store all relevant Variables of Doors and Arcade Machines
    List<Acquired> Acquired_List = new List<Acquired>();                //Create a List to store all relevant Variables of Inventory Items


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
