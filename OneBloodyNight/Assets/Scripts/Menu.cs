using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    //Buttons

    //Screens
    [SerializeField]
    private GameObject optionScreen;
    [SerializeField]
    private GameObject loreScreen;


    // Start is called before the first frame update
    void Start()
    {

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
        loreScreen.SetActive(false);
    }

}
