using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorImageScript : MonoBehaviour
{
    public Texture2D CursorTexture;
    public Texture2D CursorHighlightTexture;
    public Vector2 CursorPoint;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(CursorTexture, CursorPoint, CursorMode.Auto);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))  
        {
            Cursor.SetCursor(CursorHighlightTexture, CursorPoint, CursorMode.Auto);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(CursorTexture, CursorPoint, CursorMode.Auto);
        }
    }
}
