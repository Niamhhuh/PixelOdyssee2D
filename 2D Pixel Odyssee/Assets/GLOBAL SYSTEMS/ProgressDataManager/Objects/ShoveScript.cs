using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoveScript : MonoBehaviour
{
    Vector3 CurrentPosition;

    private void Awake()
    {

    }

    public void ShoveLeft ()
    {
        CurrentPosition = DataManager.ToShove[0].ShoveBox.transform.position;
        DataManager.ToShove[0].ShoveBox.transform.position = new Vector3(CurrentPosition.x - 2, CurrentPosition.y, CurrentPosition.z);
        DataManager.ToShove[0].Shove_Position --;
        DataManager.ToShove.RemoveAt(0);
        GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>().EnableInput();    //Something like this
        GameObject.FindGameObjectWithTag("ShoveControl").SetActive(false);
    }

    public void ShoveRight()
    {
        CurrentPosition = DataManager.ToShove[0].ShoveBox.transform.position;
        DataManager.ToShove[0].ShoveBox.transform.position = new Vector3(CurrentPosition.x + 2, CurrentPosition.y, CurrentPosition.z);
        DataManager.ToShove[0].Shove_Position ++;
        DataManager.ToShove.RemoveAt(0);
        GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>().EnableInput();    //Something like this
        GameObject.FindGameObjectWithTag("ShoveControl").SetActive(false);
    }
}
