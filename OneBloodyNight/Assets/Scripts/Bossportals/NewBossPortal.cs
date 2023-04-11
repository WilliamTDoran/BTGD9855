using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBossPortal : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Victory;
    public AudioSource Audio;

    void Start()
    {
        Victory.SetActive(false);
    }

    public void OnTriggerEnter(Collider col)
    {
        Debug.Log("Contact");
        if (col.gameObject.tag == "Player")
        {
            Audio.Play();
            Victory.SetActive(true);
        }
        //StartCoroutine(StartBoss());
    }
}
