using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Bloodmeter : MonoBehaviour
{
    public Slider bloodmeter;
    private int currentBlood;
    public int AbilityCost;
    public int damage;

    [SerializeField]
    private float bloodLossRate;

    public float bloodGainRate;

    internal static Bloodmeter instance;

    private void Awake()
    {
        if (instance != null & instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        bloodmeter.value = bloodmeter.maxValue;
        Debug.Log("Done");
        StartCoroutine("DMG");
    }

    // Update is called once per frame
    /*private void Update()
    {
        if (Input.GetButtonDown("AdvancedFire"))
        {
            bloodmeter.value = bloodmeter.value - AbilityCost;
        }
    }*/

    private IEnumerator DMG()
    {

        while (bloodmeter.value > bloodmeter.minValue)
        {
            bloodmeter.value = bloodmeter.value - bloodLossRate;//takes 1 blood per second
            yield return new WaitForSeconds(0.03f);//slows down the damage rate
        }
        yield return null;//Player is dead
    }

    internal void changeBlood(float difference)
    {
        float targetValue = bloodmeter.value;

        targetValue = Math.Clamp(targetValue + difference, 0, bloodmeter.maxValue);

        bloodmeter.value = targetValue;
    }

    private IEnumerator Gain()
    {

        while (bloodmeter.value > bloodmeter.minValue)
        {
            bloodmeter.value = bloodmeter.value + bloodGainRate;//takes 1 blood per second
            yield return new WaitForSeconds(0.03f);//slows down the damage rate
        }
        yield return null;//Player is dead
    }
}
