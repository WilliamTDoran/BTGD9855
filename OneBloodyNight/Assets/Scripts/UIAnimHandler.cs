using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimHandler : MonoBehaviour
{

    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private GameObject LoreButton;
    [SerializeField]
    private GameObject optionButton;
    [SerializeField]
    private GameObject quitButton;
    // Start is called before the first frame update
    void Start()
    {
        startButton.SetActive(false);
        LoreButton.SetActive(false);
        optionButton.SetActive(false);
        quitButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimationDone()
    {
        startButton.SetActive(true);
        LoreButton.SetActive(true);
        optionButton.SetActive(true);
        quitButton.SetActive(true);
    }

}
