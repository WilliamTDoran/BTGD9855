using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPoolRun : MonoBehaviour
{
    public Bloodmeter bloodmeter;

    public void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Player")
        {
            Bloodmeter.instance.healing = true;
            bloodmeter.StartCoroutine("Gain");
        }

    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Bloodmeter.instance.healing = false;
            bloodmeter.StopCoroutine("Gain");
        }
    }
}
