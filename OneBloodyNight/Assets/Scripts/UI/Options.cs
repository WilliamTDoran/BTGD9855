using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    public AudioMixer mixer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSFX(float sliderValue)
    {
        mixer.SetFloat("EXSFX", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusic(float sliderValue)
    {
        mixer.SetFloat("EXMusic", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMaster(float sliderValue)
    {
        mixer.SetFloat("EXMaster", Mathf.Log10(sliderValue) * 20);
    }
}
