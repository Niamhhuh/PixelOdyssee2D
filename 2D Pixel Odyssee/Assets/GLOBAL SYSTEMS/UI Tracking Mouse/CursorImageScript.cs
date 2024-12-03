using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorImageScript : MonoBehaviour
{
    public Texture2D CursorTexture;
    public Texture2D CursorHighlightTexture;
    public Texture2D transparentCursor;
    
    public Vector2 CursorPoint;

    public bool Transparent = false;
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
        if (Input.GetMouseButtonUp(0) && !Transparent)
        {
            Cursor.SetCursor(CursorTexture, CursorPoint, CursorMode.Auto);
        }
        if (Input.GetMouseButtonUp(0) && Transparent)
        {
            Cursor.SetCursor(transparentCursor, CursorPoint, CursorMode.Auto);
        }
    }


    public void DeactivateCursorSprite()
    {
        Cursor.SetCursor(transparentCursor, CursorPoint, CursorMode.Auto);
        Transparent = true;
    }

    public void ActivateCursorSprite()
    {
        if (Transparent) 
        {
            Cursor.SetCursor(CursorTexture, CursorPoint, CursorMode.Auto);
            Transparent = false;
        }
        
    }


}
