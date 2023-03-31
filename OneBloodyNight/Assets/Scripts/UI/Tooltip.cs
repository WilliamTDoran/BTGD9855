using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public string msg;
    public void OnMouseEnter()
    {
        Debug.Log("Entered the box!");
        ToolTipManager._instance.SetAndShowToolTip(msg);
    }

    public void OnMouseExit()
    {
        Debug.Log("Exited the box!");
        ToolTipManager._instance.HideToolTip();
    }
}
