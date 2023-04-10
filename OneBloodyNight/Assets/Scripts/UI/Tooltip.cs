using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string msg;
    private bool show;
    private bool thisGO;
    // Enable the script when the mouse enters the game object
    public void OnPointerEnter(PointerEventData eventData)
    {
        thisGO = true;
        ToolTipManager._instance.SetAndShowToolTip(msg);
    }

    // Disable the script when the mouse exits the game object
    public void OnPointerExit(PointerEventData eventData)
    {
        show = false;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() && thisGO)
        {
            show = true;
        }
        if (thisGO && !show)
        {
            ToolTipManager._instance.HideToolTip();
            thisGO = false;
        }
    }
}
