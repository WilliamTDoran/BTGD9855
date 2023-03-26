using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject controls;
    public GameObject blood;
    public GameObject backbutton;
    public GameObject nextbutton;
    // Start is called before the first frame update
    void Start()
    {
        controls.SetActive(true);
        blood.SetActive(false);
        backbutton.SetActive(false);
        nextbutton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        
        Application.LoadLevel("MazeScene");

    }

    public void Next()
    {
        controls.SetActive(false);
        blood.SetActive(true);
        backbutton.SetActive(true);
        nextbutton.SetActive(false);
    }

    public void back()
    {
        controls.SetActive(true);
        blood.SetActive(false);
        backbutton.SetActive(false);
        nextbutton.SetActive(true);
    }
}
