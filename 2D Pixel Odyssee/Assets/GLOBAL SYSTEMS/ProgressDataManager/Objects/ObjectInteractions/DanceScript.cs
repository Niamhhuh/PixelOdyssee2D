using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceScript : MonoBehaviour
{
    GameObject TopButton;
    GameObject BottomButton;
    GameObject LeftButton;
    GameObject RightButton;

    GameObject DanceDisplay;

    GameObject DisplayArrow1;
    GameObject DisplayArrow2;
    GameObject DisplayArrow3;
    GameObject DisplayArrow4;
    GameObject DisplayArrow5;

    List<int> DanceQueue = new List<int>();
    public GameObject [] DisplayArrows = new GameObject[5];

    private void Start()
    {
        TopButton = GameObject.FindGameObjectWithTag("DanceButtonTop");
        BottomButton = GameObject.FindGameObjectWithTag("DanceButtonBottom");
        LeftButton = GameObject.FindGameObjectWithTag("DanceButtonLeft");
        RightButton = GameObject.FindGameObjectWithTag("DanceButtonRight");
        
        DanceDisplay = GameObject.FindGameObjectWithTag("DanceDisplay");

        DisplayArrow1 = DanceDisplay.transform.GetChild(0).gameObject;
        DisplayArrow2 = DanceDisplay.transform.GetChild(1).gameObject;
        DisplayArrow3 = DanceDisplay.transform.GetChild(2).gameObject;
        DisplayArrow4 = DanceDisplay.transform.GetChild(3).gameObject;
        DisplayArrow5 = DanceDisplay.transform.GetChild(4).gameObject;

        DisplayArrows[0] = DisplayArrow1;
        DisplayArrows[1] = DisplayArrow2;
        DisplayArrows[2] = DisplayArrow3;
        DisplayArrows[3] = DisplayArrow4;
        DisplayArrows[4] = DisplayArrow5;
    }

    public void ControlButtons()
    {
        LeftButton.SetActive(true);
        RightButton.SetActive(true);
        TopButton.SetActive(true);
        BottomButton.SetActive(true);
        DanceDisplay.SetActive(true);

        foreach(GameObject Arrow in DisplayArrows)
        {
            Arrow.SetActive(false);
        }
    }


    public void DanceTop()
    {
        if (DanceQueue.Count < 4)
        {
            DanceQueue.Add(1);

            SetDisplayArrow(DanceQueue.Count, 1);

            if (DanceQueue.Count >= 4)
            {
                CheckCompletedInput();
            }
        }
    }

    public void DanceLeft()
    {
        if (DanceQueue.Count < 4)
        {
            DanceQueue.Add(2);

            SetDisplayArrow(DanceQueue.Count, 2);

            if (DanceQueue.Count >= 4)
            {
                CheckCompletedInput();
            }
        }
    }

    public void DanceBottom()
    {
        if (DanceQueue.Count < 4)
        {
            DanceQueue.Add(3);

            SetDisplayArrow(DanceQueue.Count, 3);

            if(DanceQueue.Count >= 4)
            {
                CheckCompletedInput();
            }
        }
    }

    public void DanceRight()
    {
        if (DanceQueue.Count < 4)
        {
            DanceQueue.Add(4);

            SetDisplayArrow(DanceQueue.Count, 4);

            if (DanceQueue.Count >= 4)
            {
                CheckCompletedInput();
            }
        }
    }

    public void SetDisplayArrow(int ArrowNumber, int Direction)
    {
        DisplayArrows[ArrowNumber - 1].SetActive(true);
        DisplayArrows[ArrowNumber - 1].transform.Rotate(0, 0, 90 * (Direction - 1));
    }
    public void CheckCompletedInput()
    {
        foreach (int Input in DataManager.ToDance[0].TargetInput)
        {
            int i = 0;
            if (Input != DanceQueue[i])
            {

            }
            i++;
        }
    }

    public void EndDance()
    {
        DataManager.ToDance.RemoveAt(0);                                                            //Remove the DancePad from the ToDance List
        GameObject.FindGameObjectWithTag("DanceControl").SetActive(false);                          //Deactivate the Dance Arrows
    }
}