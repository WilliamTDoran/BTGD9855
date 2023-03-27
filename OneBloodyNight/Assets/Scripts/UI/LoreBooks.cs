using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreBooks : MonoBehaviour
{
    public GameObject Lorebook1;
    public GameObject Lorebook2;
    public GameObject Lorebook3;
    public GameObject Lorebook4;
    public GameObject Lorebook5;
    public GameObject Lorebook6;

    public GameObject LoreScreen;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Back()
    {
        Lorebook1.SetActive(false);
        Lorebook2.SetActive(false);
        Lorebook3.SetActive(false);
        Lorebook4.SetActive(false);
        Lorebook5.SetActive(false);
        Lorebook6.SetActive(false);
    }

    public void LorebookOne()
    {
        
        Lorebook1.SetActive(true);
    }
}

