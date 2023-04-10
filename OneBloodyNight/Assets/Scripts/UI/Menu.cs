using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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


    // Start is called before the first frame update
    void Start()
    {
        Background.SetActive(true);
        optionScreen.SetActive(false);
        loreScreen.SetActive(false);
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
        loreScreen.SetActive(false);
        optionScreen.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();

    }

    public void LoreBooks()
    {
        Background.SetActive(false);
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
        Background.SetActive(true);
        optionScreen.SetActive(false);
        loreScreen.SetActive(false);
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
}
