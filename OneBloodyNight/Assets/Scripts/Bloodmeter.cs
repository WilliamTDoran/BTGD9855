using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bloodmeter : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Player;
    public Slider slider;


    void Awake()
    {
        slider.value = slider.maxValue;
        StartCoroutine("Decrease");
    }

    // Update is called once per frame
    void Update()
    {
        //if player uses power
        //if player is hit
        //if player uses blood for upgrade
        //
    }

    private IEnumerable Decrease()
    {
       
            while (slider.value > slider.minValue)
        {
            slider.value = slider.value - 1;
        }
        yield return null;

    }
}
