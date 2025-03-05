using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DanceScript : MonoBehaviour
{
    GameObject TopButton;
    GameObject BottomButton;
    GameObject LeftButton;
    GameObject RightButton;

    GameObject DanceController;

    public GameObject DanceDisplay;

    GameObject DisplayArrow1;
    GameObject DisplayArrow2;
    GameObject DisplayArrow3;
    GameObject DisplayArrow4;
    GameObject DisplayArrow5;

    DataManager DMReference;

    Vector3 ArrowSize;

    public List<int> DanceQueue = new List<int>();
    public GameObject[] DisplayArrows = new GameObject[5];

    private void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();

        TopButton = GameObject.FindGameObjectWithTag("DanceButtonTop");
        BottomButton = GameObject.FindGameObjectWithTag("DanceButtonBottom");
        LeftButton = GameObject.FindGameObjectWithTag("DanceButtonLeft");
        RightButton = GameObject.FindGameObjectWithTag("DanceButtonRight");

        DanceController = GameObject.FindGameObjectWithTag("DanceControl");


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

        //DanceDisplay.transform.position = new Vector3(DanceController.transform.position.x, DanceController.transform.position.y, DanceController.transform.position.z);

        foreach (GameObject Arrow in DisplayArrows)
        {
            Arrow.GetComponent<Image>().color = Color.white;
            Arrow.SetActive(false);
            ArrowSize = Arrow.transform.localScale;
        }
    }

    public void DanceBottom()
    {
        if (DanceQueue.Count < 5)
        {
            DanceQueue.Add(1);

            SetDisplayArrow(DanceQueue.Count, 1);

            if (DanceQueue.Count >= 5)
            {
                CheckCompletedInput();
            }
        }
    }

    public void DanceLeft()
    {
        if (DanceQueue.Count < 5)
        {
            DanceQueue.Add(2);

            SetDisplayArrow(DanceQueue.Count, 2);

            if (DanceQueue.Count >= 5)
            {
                CheckCompletedInput();
            }
        }
    }

    public void DanceTop()
    {
        if (DanceQueue.Count < 5)
        {
            DanceQueue.Add(3);

            SetDisplayArrow(DanceQueue.Count, 3);

            if (DanceQueue.Count >= 5)
            {
                CheckCompletedInput();
            }
        }
    }

    public void DanceRight()
    {
        if (DanceQueue.Count < 5)
        {
            DanceQueue.Add(4);

            SetDisplayArrow(DanceQueue.Count, 4);

            if (DanceQueue.Count >= 5)
            {
                CheckCompletedInput();
            }
        }
    }


    public void SetDisplayArrow(int ArrowNumber, int Direction)
    {
        DisplayArrows[ArrowNumber - 1].SetActive(true);
        DisplayArrows[ArrowNumber - 1].transform.rotation = Quaternion.Euler(0, 0, -90 * (Direction - 2));
    }


    public void CheckCompletedInput()           //Go through ConditionList of DancePad 
    {
        DataManager.ToDance[0].DMReference.MoveScript.SetOtherArrowFalse();
        bool mismatch = false;
        int i = 0;

        foreach (int Input in DataManager.ToDance[0].TargetInput)
        {
            if (Input != DanceQueue[i])
            {
                mismatch = true;
                break;
            }
            i++;
        }

        if (!mismatch)
        {
            //CORRECT INPUT
            DataManager.ToDance[0].DanceUnlock();

            StartCoroutine(TrueInput());
            StartCoroutine(ExpandAndContract());
        }

        if (mismatch)
        {
            //WRONG INPUT

            StartCoroutine(WrongInput());
        }
    }

    public void EndDance()
    {
        foreach (GameObject Arrow in DisplayArrows)
        {
            Arrow.transform.rotation = Quaternion.Euler(0, 0, 90);
            Arrow.SetActive(false);
        }
        DanceDisplay.SetActive(false);

        DanceQueue.Clear();
        if (DataManager.ToDance.Count > 0)                                                               //If the Object is in the ToShove List
        {
            DataManager.ToDance.RemoveAt(0);                                                            //Remove it
        }

        DanceController.SetActive(false);                          //Deactivate the Dance Arrows
        DMReference.MoveScript.InCatScene = false;
        DMReference.MoveScript.Activate_CallEnableInput();
        DMReference.MoveScript.Activate_CallEnableInteract();
    }



    //Flash Red for Drag Unlock Fail
    public IEnumerator WrongInput()
    {

        float elapsedTime = 0f;
        while (elapsedTime < 1)
        {
            foreach (GameObject Arrow in DisplayArrows)
            {
                Arrow.GetComponent<Image>().color = Color.Lerp(Color.red, Color.white, elapsedTime / 1);
                //Arrow.SetActive(false);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        EndDance();
    }

    public IEnumerator TrueInput()
    {
        float elapsedTime = 0f;


        while (elapsedTime < 1)
        {
            foreach (GameObject Arrow in DisplayArrows)
            {
                Arrow.GetComponent<Image>().color = Color.Lerp(Color.blue, Color.white, elapsedTime / 1);
                //Arrow.SetActive(false);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        EndDance();
    }


    private IEnumerator ExpandAndContract()
    {

        float ExpandDuration = 0.08f; // Duration for the expansion and contraction
        float ContractDuration = 0.15f; // Duration for the expansion and contraction

        Vector3 ExpandSize = ArrowSize * 1.3f;

        float timeElapsed = 0f;

        while (timeElapsed < ExpandDuration)
        {
            foreach (GameObject Arrow in DisplayArrows)
            {
                Arrow.transform.localScale = Vector3.Lerp(ArrowSize, ExpandSize, timeElapsed / ExpandDuration);
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        foreach (GameObject Arrow in DisplayArrows)
        {
            Arrow.transform.localScale = ExpandSize; // Ensure it reaches the exact target scale
        }
        timeElapsed = 0f;
        while (timeElapsed < ContractDuration)
        {
            foreach (GameObject Arrow in DisplayArrows)
            {
                Arrow.transform.localScale = Vector3.Lerp(ExpandSize, ArrowSize, timeElapsed / ContractDuration);
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        foreach (GameObject Arrow in DisplayArrows)
        {
            Arrow.transform.localScale = ArrowSize;
        }
    }















}
