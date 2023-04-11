using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource audioSource1;

    public Toggle toggle;

    

    //Buttons

    //Screens
    [SerializeField]
    private GameObject optionScreen;
    [SerializeField]
    private GameObject loreScreen;
    [SerializeField]
    private GameObject Background;
    [SerializeField]
    private GameObject Credit;

    public GameObject LoreFirstButton;
    public GameObject OptionFirstButton;
    public GameObject menuFirstButton;


    // Start is called before the first frame update
    void Start()
    {
        Background.SetActive(true);
        optionScreen.SetActive(false);
        loreScreen.SetActive(false);
        Credit.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        StartCoroutine("Starter");
        //Application.LoadLevel("TestScene");

    }

    public void Options()
    {
        Background.SetActive(false);
        Credit.SetActive(false);
        loreScreen.SetActive(false);
        optionScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(OptionFirstButton);
    }

    public void Quit()
    {
        Application.Quit();

    }

    public void LoreBooks()
    {
        Background.SetActive(false);
        Credit.SetActive(false);
        loreScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(LoreFirstButton);

    }

    public void Continue()
    {

    }

    public void Pause()
    {

    }

    public void Back()
    {
        Background.SetActive(true);
        optionScreen.SetActive(false);
        loreScreen.SetActive(false);
        Credit.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(menuFirstButton);
    }

    private IEnumerator Starter()
    {
        audioSource1.Stop();
        audioSource.Play();
        
        yield return new WaitForSeconds(2f);
        if(toggle.isOn == true)
        {
            Application.LoadLevel("Tutorial");
        }
        else
        {
            Application.LoadLevel("MazeScene");
        }
        
    }

    public void Credits()
    {
        Background.SetActive(true);
        optionScreen.SetActive(false);
        loreScreen.SetActive(false);
        Credit.SetActive(true);
        
    }
}
