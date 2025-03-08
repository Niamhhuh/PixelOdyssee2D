using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoveScript : MonoBehaviour
{
    Vector3 StartPosition;
    Vector3 TargetPosition;

    GameObject LeftButton;
    GameObject RightButton;

    private void Start()
    {
        LeftButton = GameObject.FindGameObjectWithTag("ShoveButtonLeft");
        RightButton = GameObject.FindGameObjectWithTag("ShoveButtonRight");
    }

    public void ControlButtons()
    {
        LeftButton.SetActive(true);
        RightButton.SetActive(true);

        //Control Left Button
        if (-DataManager.ToShove[0].Max_Shove >= DataManager.ToShove[0].Shove_Position || -DataManager.ToShove[0].Max_Shove_Left >= DataManager.ToShove[0].Shove_Position)                                                                     //if the Current Position is equal/lesser than the negative Max Shove Position
        {
            LeftButton.SetActive(false);                           //deactivate the Left Shove Button
        }

        //Control Right Button
        if (DataManager.ToShove[0].Max_Shove <= DataManager.ToShove[0].Shove_Position || DataManager.ToShove[0].Max_Shove_Right <= DataManager.ToShove[0].Shove_Position)                                                                     //if the Current Position is equal/greater than the positive Max Shove Position
        {
            RightButton.SetActive(false);                          //deactivate the Right Shove Button
        }
    }

    public void ShoveLeft ()
    {
        DataManager.ToShove[0].DMReference.MoveScript.SetOtherArrowFalse();

        if (-DataManager.ToShove[0].Max_Shove < DataManager.ToShove[0].Shove_Position)
        {
            StartPosition = DataManager.ToShove[0].ShoveBox.transform.position;                         //Set Start Position
            TargetPosition = new Vector3(StartPosition.x - 3, StartPosition.y, StartPosition.z);        //Set Target Position

            DataManager.ToShove[0].Shove_Position--;
            DataManager.ToShove[0].StartMove(StartPosition, TargetPosition);                            //Call Method in Shovable, which starts the Shove coroutine
            DataManager.ToShove[0].UpdateData();
            DataManager.ToShove.RemoveAt(0);                                                            //Remove the Shovable from the ToShove List
            GameObject.FindGameObjectWithTag("ShoveControl").SetActive(false);                          //Deactivate the Shove Arrows
        }
        else
        {
            //GameObject.FindGameObjectWithTag("ShoveControl").SetActive(false);                          //Deactivate the Shove Arrows
            //Add indicator, that Object can't be shoved further that way.            
        }
    }

    public void ShoveRight()
    {

        DataManager.ToShove[0].DMReference.MoveScript.SetOtherArrowFalse();

        if (DataManager.ToShove[0].Max_Shove > DataManager.ToShove[0].Shove_Position)
        {
            StartPosition = DataManager.ToShove[0].ShoveBox.transform.position;                         //Set Start Position
            TargetPosition = new Vector3(StartPosition.x + 3, StartPosition.y, StartPosition.z);        //Set Target Position

            DataManager.ToShove[0].Shove_Position++;
            DataManager.ToShove[0].StartMove(StartPosition, TargetPosition);                            //Call Method in Shovable, which starts the Shove coroutine
            DataManager.ToShove[0].UpdateData();
            DataManager.ToShove.RemoveAt(0);                                                            //Remove the Shovable from the ToShove List
            GameObject.FindGameObjectWithTag("ShoveControl").SetActive(false);                          //Deactivate the Shove Arrows
        }
        else
        {
            //GameObject.FindGameObjectWithTag("ShoveControl").SetActive(false);                          //Deactivate the Shove Arrows
            //Add indicator, that Object can't be shoved further that way.            
        }
    }
}
