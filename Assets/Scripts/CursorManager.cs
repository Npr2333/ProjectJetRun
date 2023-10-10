using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{


    public Texture2D cursorTex;
    void Awake() { Cursor.SetCursor(cursorTex, new Vector2(50,50), CursorMode.ForceSoftware); }

}

