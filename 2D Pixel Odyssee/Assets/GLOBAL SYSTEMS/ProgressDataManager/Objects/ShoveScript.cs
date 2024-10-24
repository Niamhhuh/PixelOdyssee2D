using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoveScript : MonoBehaviour
{
    Vector3 StartPosition;
    Vector3 TargetPosition;
    
    public void ShoveLeft ()
    {
        StartPosition = DataManager.ToShove[0].ShoveBox.transform.position;                         //Set Start Position
        TargetPosition = new Vector3(StartPosition.x -3, StartPosition.y, StartPosition.z);        //Set Target Position

        DataManager.ToShove[0].StartMove(StartPosition, TargetPosition);                            //Call Method in Shovable, which starts the Shove coroutine

        DataManager.ToShove[0].Shove_Position --;
        DataManager.ToShove.RemoveAt(0);                                                            //Remove the Shovable from the ToShove List
        GameObject.FindGameObjectWithTag("ShoveControl").SetActive(false);                          //Deactivate the Shove Arrows
    }

    public void ShoveRight()
    {
        StartPosition = DataManager.ToShove[0].ShoveBox.transform.position;                         //Set Start Position
        TargetPosition = new Vector3(StartPosition.x + 3, StartPosition.y, StartPosition.z);        //Set Target Position

        DataManager.ToShove[0].StartMove(StartPosition, TargetPosition);                            //Call Method in Shovable, which starts the Shove coroutine
        DataManager.ToShove[0].Shove_Position ++;
        DataManager.ToShove.RemoveAt(0);                                                            //Remove the Shovable from the ToShove List
        GameObject.FindGameObjectWithTag("ShoveControl").SetActive(false);                          //Deactivate the Shove Arrows
    }
}
