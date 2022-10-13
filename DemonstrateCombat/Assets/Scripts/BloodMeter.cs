using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodMeter : MonoBehaviour
{
    public Slider bloodmeter;
    private int currentBlood;
    // Start is called before the first frame update
    void Start()
    {
        bloodmeter.value = bloodmeter.maxValue;
        StartCoroutine("DMG");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DMG()
    {
        
        while (bloodmeter.value > bloodmeter.minValue)
        {
            bloodmeter.value = bloodmeter.value - 1;//takes 1 blood per second
            //yield return new WaitForSeconds(0.25f);//slows down the damage rate
        }
        yield return null;//Player is dead
    }

    void OnTriggerEnter(Collider col)//if enemy is in poison cloud they get poisoned
    {
        if (col.gameObject.tag == "Monster")
        {
            bloodmeter.value = bloodmeter.value - 5;//this is for when the player is damaged
        }
    }

 }
