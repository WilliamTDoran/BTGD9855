using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonIndicator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject pointer;

    // Start is called before the first frame update
    void Start()
    {
        pointer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData e)
    {
        pointer.SetActive(true);
    }

    public void OnPointerExit(PointerEventData e)
    {
        pointer.SetActive(false);

    }
}
