using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bloodmeter : MonoBehaviour
{
    public Slider bloodmeter;
    private int currentBlood;
    public int AbilityCost;
    public int damage;
    
    
    // Start is called before the first frame update
    void Start()
    {
        bloodmeter.value = bloodmeter.maxValue;
        Debug.Log("Done");
        StartCoroutine("DMG");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("AdvancedFire"))
        {
            bloodmeter.value = bloodmeter.value - AbilityCost;
        }
    }

    private IEnumerator DMG()
    {

        while (bloodmeter.value > bloodmeter.minValue)
        {
            bloodmeter.value = bloodmeter.value - 1;//takes 1 blood per second
            yield return new WaitForSeconds(0.25f);//slows down the damage rate
        }
        yield return null;//Player is dead
    }

    private void changeBlood()
    {
        
    }
}
