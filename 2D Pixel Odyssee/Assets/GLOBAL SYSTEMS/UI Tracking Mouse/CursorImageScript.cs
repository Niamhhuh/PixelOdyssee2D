using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorImageScript : MonoBehaviour
{
    public Texture2D CursorTexture;
    public Vector2 CursorPoint;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(CursorTexture, CursorPoint, CursorMode.Auto);
    }
}
