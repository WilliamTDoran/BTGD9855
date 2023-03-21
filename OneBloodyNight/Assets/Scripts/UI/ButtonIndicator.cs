using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonIndicator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject pointer;
    [SerializeField]
    private GameObject pointer1;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        pointer.SetActive(false);
        pointer1.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData e)
    {
        pointer.SetActive(true);
        pointer1.SetActive(true);
        audioSource.Play();
    }

    public void OnPointerExit(PointerEventData e)
    {
        pointer.SetActive(false);
        pointer1.SetActive(false);
        audioSource.Stop();

    }
}
