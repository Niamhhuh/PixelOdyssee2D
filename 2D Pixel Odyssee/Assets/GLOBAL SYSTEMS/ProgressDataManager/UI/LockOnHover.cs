using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class LockOnHover : MonoBehaviour
{
    UiToMouse PointerScript;

    void Start ()
    {
        PointerScript = GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>();
    }

    private void OnMouseOver()
    {
        print("Hi");
        PointerScript.DisableInput();
        PointerScript.DisableInteract();
    }
}
