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
    public bool hasNext;
    public bool hasBack;

    private bool canInputNext = true;
    private bool canInputBack = true;

    // Start is called before the first frame update
    void Start()
    {
        Intro.SetActive(false);
        controls.SetActive(true);
        blood.SetActive(false);
        upgrade.SetActive(false);
        backbutton.SetActive(false);
        nextbutton.SetActive(true);

        hasBack = false;
        hasNext = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            
            StartGame();
        }
        if(Input.GetAxis("LeftTrigger") == -1 && hasBack == true && canInputBack)
        {
            canInputBack = false;
            back();
            StartCoroutine("waiter");
        }
        if( Input.GetAxis("RightTrigger") == 1 && hasNext == true && canInputNext)
        {
            canInputNext = false;
            Next();
            StartCoroutine("waiter");
            
        }
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(.5f);

        canInputBack = true;
        canInputNext = true;
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

            hasNext = true;
            hasBack = true;
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

            hasNext = true;
            hasBack = true;
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

            hasNext = false;
            hasBack = true;
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

            hasNext = true;
            hasBack = true;
        }
        else if(count == 3)//page 2
        {
            controls.SetActive(false);
            blood.SetActive(true);
            upgrade.SetActive(false);
            backbutton.SetActive(true);
            nextbutton.SetActive(true);
            count--;

            hasNext = true;
            hasBack = true;
        }
        else if(count == 2)//page 1
        {
            controls.SetActive(true);
            blood.SetActive(false);
            upgrade.SetActive(false);
            backbutton.SetActive(false);
            nextbutton.SetActive(true);
            count--;

            hasNext = true;
            hasBack = false;
        }
        
    }
}
