using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPoolRun : MonoBehaviour
{
    public Bloodmeter bloodmeter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Player")
        {
            
            bloodmeter.StartCoroutine("Gain");
        }

    }
    void OnTriggerExit(Collider col)
    {

        if (col.gameObject.tag == "Player")
        {
            
            bloodmeter.StopCoroutine("Gain");
        }
    }
}
