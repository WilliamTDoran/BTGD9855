using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject screen1;
    public GameObject screen2;
    public Bloodmeter bloodmeter;
    public AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        screen1.SetActive(false);
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

            if (bloodmeter.bloodmeter.value <= 0)
        {
            //screen1.SetActive(true);
        }
    }

    public void yes()
    {
        Application.LoadLevel("TestScene");

    }

    public void no()
    {
        Application.Quit();
    }

    private void Victory()
    {
        screen2.SetActive(true);
        Time.timeScale = 0f;
        audioSource.Play();
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
