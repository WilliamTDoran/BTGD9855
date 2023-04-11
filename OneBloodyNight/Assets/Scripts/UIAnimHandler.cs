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
    [SerializeField]
    private GameObject Credits;
    // Start is called before the first frame update
    void Start()
    {
        Credits.SetActive(false);
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
        Credits.SetActive(true);
        startButton.SetActive(true);
        LoreButton.SetActive(true);
        optionButton.SetActive(true);
        quitButton.SetActive(true);
    }

}
