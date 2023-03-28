using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossPortalYara : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnTriggerEnter(Collider col)
    {
        Debug.Log("Contact");
        if (col.gameObject.tag == "Player")
        {
            if (SceneManager.GetActiveScene().name == "MazeScene") { 
            Application.LoadLevel("YaraBossRoom");
            }
            else if (SceneManager.GetActiveScene().name == "YaraBossRoom")
            {
                Application.LoadLevel("MazeScene");
            }
        }
        //StartCoroutine(StartBoss());
    }
    void OnTriggerExit2D(Collider2D col)/////////Stops transition
    {

        //StopCoroutine(StartBoss());
    }
}
