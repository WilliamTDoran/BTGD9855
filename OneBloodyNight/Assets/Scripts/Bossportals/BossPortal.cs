using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossPortal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartBoss()
    {
        yield return new WaitForSeconds(5);
        Application.LoadLevel("ImpunduluBossRoom");

    }
    public void OnTriggerEnter(Collider col)
    {
        Debug.Log("Contact");
        if (col.gameObject.tag == "Player")
        {
            UpgradeTotemHUD.instance.Saver();
            if (SceneManager.GetActiveScene().name == "MazeScene") { 
            Application.LoadLevel("ImpunduluBossRoom");
            }
            else if (SceneManager.GetActiveScene().name == "ImpunduluBossRoom")
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
