using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    //Buttons
    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private GameObject LoreButton;
    [SerializeField]
    private GameObject optionButton;
    [SerializeField]
    private GameObject quitButton;

    //Screens
    [SerializeField]
    private GameObject optionScreen;
    [SerializeField]
    private GameObject loreScreen;


    // Start is called before the first frame update
    void Start()
    {
        //startButton.SetActive(false);
        //LoreButton.SetActive(false);
        //optionButton.SetActive(false);
        //quitButton.SetActive(false);

        optionScreen.SetActive(false);
        loreScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        Application.LoadLevel("TestScene");

    }

    public void Options()
    {
        optionScreen.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();

    }

    public void LoreBooks()
    {
        loreScreen.SetActive(true);
    }

    public void Continue()
    {

    }

    public void Pause()
    {

    }

    public void Back()
    {
        optionScreen.SetActive(false);
    }

    public void AnimationDone()
    {
        startButton.SetActive(true);
        LoreButton.SetActive(true);
        optionButton.SetActive(true);
        quitButton.SetActive(true);
    }
}
