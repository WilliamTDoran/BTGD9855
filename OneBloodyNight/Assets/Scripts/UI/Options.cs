using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider master;
    public Toggle toggle;

    [SerializeField]
    private GameObject optionScreen;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Load()
    {
        //float LoadedSfx = PlayerPrefs.GetFloat(SFX);
        //float LoadedMusic = PlayerPrefs.GetFloat(Music);
        //float LoadedMaster = PlayerPrefs.GetFloat(Master);
    }

    private void Set()
    {
        //mixer.SetFloat("EXSFX", LoadedSfx);
        //mixer.SetFloat("EXSFX", LoadedMusic);
        //mixer.SetFloat("EXSFX", LoadedMaster);
    }

    public void SetSFX(float sfx_)
    {
        mixer.SetFloat("EXSFX", sfx_);
        PlayerPrefs.SetFloat("SFX", sfx_);
    }

    public void SetMusic(float music)
    {
        mixer.SetFloat("EXMusic", music);
        PlayerPrefs.SetFloat("Music", music);
    }

    public void SetMaster(float master)
    {
        mixer.SetFloat("SFXMaster", master);
        PlayerPrefs.SetFloat("Master", master);
    }

    public void Back()
    {
        optionScreen.SetActive(false);
    }

    public void tutorial()
    {
        if (toggle.isOn == true)
        {
            toggle.isOn = false;
        }
        else
        {
            toggle.isOn = true;
        }
    }
}
