using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject screen2;
    //public Bloodmeter bloodmeter;
    public AudioSource audioSource;

    internal static GameOver instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
    }
    void Start()
    {

        screen2.SetActive(false);
        //StartCoroutine("Timer");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Victory();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Application.LoadLevel("MazeScene");
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Application.Quit();
        }


    }


    public void Victory()
    {
        screen2.SetActive(true);
        Time.timeScale = 0f;
        audioSource.Play();
    }

    public void NO()
    {
        Application.Quit();
    }

    public void YES()
    {
        Application.LoadLevel("Title");
    }



    /*private IEnumerator Timer()
    {
        int secs = ;
        int start = 1;
        while (start < secs)
        {
            start = start + 1;
            yield return new WaitForSeconds(1f);
        }
        Victory();
    }
    */
}
