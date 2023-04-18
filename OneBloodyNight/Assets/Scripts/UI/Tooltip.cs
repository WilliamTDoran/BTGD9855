using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string msg;
    private bool show;
    private bool thisGO;

    [SerializeField]
    private GameObject cost150;
    [SerializeField]
    private GameObject cost250;


    // Enable the script when the mouse enters the game object
    public void OnPointerEnter(PointerEventData eventData)
    {
        thisGO = true;
        ToolTipManager._instance.SetAndShowToolTip(msg);

        if (this.tag == "Cost150")
        {
            cost150.SetActive(true);
        }
        if (this.tag == "Cost250")
        {
            cost250.SetActive(true);
        }
    }

    // Disable the script when the mouse exits the game object
    public void OnPointerExit(PointerEventData eventData)
    {
        show = false;

        cost150.SetActive(false);
        cost250.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        thisGO = true;
        ToolTipManager._instance.SetAndShowToolTip(msg);

    }

    public void OnDeSelect(BaseEventData eventData)
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
