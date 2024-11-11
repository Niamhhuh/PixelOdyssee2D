using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialToggleButtons : MonoBehaviour
{
    public GameObject SwitchButton;
    public GameObject InventoryButton;

    private void Awake()
    {
        SwitchButton = GameObject.Find("SwitchCharacter");
        InventoryButton = GameObject.Find("Open Inventory");
    }

    public void DisableInventoryButton()
    {
        InventoryButton.SetActive(false);
    }
    public void DisableSwitchButton()
    {
        SwitchButton.SetActive(false);
    }

    public void ActivateInventoryButton ()
    {
        InventoryButton.SetActive(true);
    }
    public void ActivateSwitchButton()
    {
        SwitchButton.SetActive(true);
    }

}
