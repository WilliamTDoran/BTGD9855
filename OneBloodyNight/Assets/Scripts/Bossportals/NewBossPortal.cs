using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBossPortal : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Victory;

    void Start()
    {
        Victory.SetActive(false);
    }

    public void OnTriggerEnter(Collider col)
    {
        Debug.Log("Contact");
        if (col.gameObject.tag == "Player")
        {
            Victory.SetActive(true);
        }
        //StartCoroutine(StartBoss());
    }
}
