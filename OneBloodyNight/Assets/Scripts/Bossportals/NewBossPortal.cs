using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewBossPortal : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Victory;
    public AudioSource Audio;

    //controller
    public GameObject firstPauseButton;
    ///

    void Start()
    {
        Victory.SetActive(false);
    }

    public void OnTriggerEnter(Collider col)
    {
        Debug.Log("Contact");
        if (col.gameObject.tag == "Player")
        {
            int Bossnum = PlayerPrefs.GetInt("Boss");
            if (Bossnum == 0)
            {
                PlayerPrefs.SetInt("Boss", 1);
                Application.LoadLevel("MazeScene");
            }
            else 
            {
                Player.plr.Stunned = true;
                Audio.Play();
                Victory.SetActive(true);

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(firstPauseButton);
            }

        }
        //StartCoroutine(StartBoss());
    }
}
