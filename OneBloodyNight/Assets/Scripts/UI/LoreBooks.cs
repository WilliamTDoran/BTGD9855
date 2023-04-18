using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoreBooks : MonoBehaviour
{
    public GameObject Lorebook1;
    public GameObject Lorebook2;
    public GameObject Lorebook3;
    public GameObject Lorebook4;
    public GameObject Lorebook5;
    public GameObject Lorebook6;

    public GameObject LoreScreen;

    public GameObject Back1, Back2, Back3, Back4, Back5, Back6;
    public GameObject FirstButton;

    public GameObject BACK;

    // Start is called before the first frame update
    void Start()
    {
        Lorebook1.SetActive(false);
        Lorebook2.SetActive(false);
        Lorebook3.SetActive(false);
        Lorebook4.SetActive(false);
        Lorebook5.SetActive(false);
        Lorebook6.SetActive(false);

        LoreScreen.SetActive(true);
        BACK.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Back()
    {
        Lorebook1.SetActive(false);
        Lorebook2.SetActive(false);
        Lorebook3.SetActive(false);
        Lorebook4.SetActive(false);
        Lorebook5.SetActive(false);
        Lorebook6.SetActive(false);
        BACK.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(FirstButton);
    }

    public void LorebookOne()
    {
        
        Lorebook1.SetActive(true);
        BACK.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(Back1);
    }
    public void LorebookTwo()
    {

        Lorebook2.SetActive(true);
        BACK.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(Back2);
    }
    public void Lorebookthree()
    {

        Lorebook3.SetActive(true);
        BACK.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(Back3);
    }
    public void Lorebookfour()
    {

        Lorebook4.SetActive(true);
        BACK.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(Back4);
    }
    public void Lorebookfive()
    {

        Lorebook5.SetActive(true);
        BACK.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(Back5);
    }
    public void Lorebooksix()
    {

        Lorebook6.SetActive(true);
        BACK.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(Back6);
    }
}

