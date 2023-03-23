using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Presets : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioMixer mixer;
    private bool Started = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((SceneManager.GetActiveScene().name == "Title") && Started == false)
        {
            Started = true;
            LoadStart();
        }
        else if (SceneManager.GetActiveScene().name == "TestScene")
        {

        }
        else if (SceneManager.GetActiveScene().name == "ImpunduluBossRoom")
        {
            
        }
    }

    private void LoadStart()
    {
        PlayerPrefs.SetFloat("SFX", 20);
        PlayerPrefs.SetFloat("Music", 20f);
        PlayerPrefs.SetFloat("Master", 20f);

        PlayerPrefs.SetInt("LoreBooks", 0);

        
    }
}
