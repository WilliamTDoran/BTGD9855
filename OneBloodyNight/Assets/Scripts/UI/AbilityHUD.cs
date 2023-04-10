using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHUD : MonoBehaviour
{
    public GameObject Empty;
    public GameObject batSwarm;
    public GameObject Invis;
    public GameObject abilityCost;
    // Start is called before the first frame update
    void Start()
    {
        abilityCost.SetActive(false);
        Empty.SetActive(true);
        batSwarm.SetActive(false);
        Invis.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            
            Invis.SetActive(false);
            batSwarm.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            
            batSwarm.SetActive(false);
            Invis.SetActive(true);
        }
        if (Input.GetMouseButtonDown(1))
        {
           
            Empty.SetActive(true);
            Invis.SetActive(false);
            batSwarm.SetActive(false);
        }
    }
}
