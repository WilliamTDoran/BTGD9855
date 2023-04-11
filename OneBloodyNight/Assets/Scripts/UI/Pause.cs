using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    [SerializeField]
    private GameObject optionScreen;

    [SerializeField]
    private GameObject pause;

    // Start is called before the first frame update
    void Start()
    {
        optionScreen.SetActive(false);
        pause.SetActive(false);

        Time.timeScale = 1;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1f)
            {
                pause.SetActive(true);
                Time.timeScale = 0f;
                Player.plr.Stunned = true;

            }
            else if (Time.timeScale == 0f)
            {
                optionScreen.SetActive(false);
                pause.SetActive(false);
                Time.timeScale = 1f;
                Player.plr.Stunned = false;

            }
        }
    }

    public void Options()
    {

        optionScreen.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();

    }

    public void MainMenu()
    {
        Application.LoadLevel("Title");

    }
    public void Continue()
    {
        optionScreen.SetActive(false);
        pause.SetActive(false);
        Time.timeScale = 1f;
        Player.plr.Stunned = false;

    }
}
