using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiToMouse: MonoBehaviour
{
    private RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        Vector3 newPosition = new Vector3(mousePosition.x, rectTransform.position.y, rectTransform.position.z);

        rectTransform.position = newPosition;
    }
}
