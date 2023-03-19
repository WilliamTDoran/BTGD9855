using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider master;

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

    public void SetSFX(float sfx_)
    {
        mixer.SetFloat("EXSFX", sfx_);
    }

    public void SetMusic(float music)
    {
        mixer.SetFloat("EXMusic", music);
    }

    public void SetMaster(float master)
    {
        mixer.SetFloat("SFXMaster", master);
    }

    public void Back()
    {
        optionScreen.SetActive(false);
    }
}
