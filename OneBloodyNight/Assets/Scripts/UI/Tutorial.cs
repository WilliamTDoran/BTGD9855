using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject controls;
    public GameObject blood;
    public GameObject upgrade;
    public GameObject Intro;
    public GameObject backbutton;
    public GameObject nextbutton;
    private int count = 1;
    // Start is called before the first frame update
    void Start()
    {
        Intro.SetActive(false);
        controls.SetActive(true);
        blood.SetActive(false);
        upgrade.SetActive(false);
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
        if(count == 1)//page 2
        {
            controls.SetActive(false);
            blood.SetActive(true);
            upgrade.SetActive(false);
            backbutton.SetActive(true);
            nextbutton.SetActive(true);
            Intro.SetActive(false);
            count++;
        }
        else if(count == 2)//page 3
        {
            controls.SetActive(false);
            blood.SetActive(false);
            upgrade.SetActive(true);
            backbutton.SetActive(true);
            nextbutton.SetActive(true);
            Intro.SetActive(false);
            count++;
        }
        else if(count == 3)//page 4
        {
            controls.SetActive(false);
            blood.SetActive(false);
            upgrade.SetActive(false);
            backbutton.SetActive(true);
            nextbutton.SetActive(false);
            Intro.SetActive(true);
            count++;
        }
        
    }

    public void back()
    {
        if(count == 4)//page 3
        {
            controls.SetActive(false);
            blood.SetActive(false);
            upgrade.SetActive(true);
            backbutton.SetActive(true);
            nextbutton.SetActive(true);
            Intro.SetActive(false);
            count--;
        }
        else if(count == 3)//page 2
        {
            controls.SetActive(false);
            blood.SetActive(true);
            upgrade.SetActive(false);
            backbutton.SetActive(true);
            nextbutton.SetActive(true);
            count--;
        }
        else if(count == 2)//page 1
        {
            controls.SetActive(true);
            blood.SetActive(false);
            upgrade.SetActive(false);
            backbutton.SetActive(false);
            nextbutton.SetActive(true);
            count--;
        }
        
    }
}
