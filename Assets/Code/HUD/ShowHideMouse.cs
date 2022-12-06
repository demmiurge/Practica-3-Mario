using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideMouse : MonoBehaviour
{
    public void ShowMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void HideMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
