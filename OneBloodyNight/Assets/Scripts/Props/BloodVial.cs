using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodVial : MonoBehaviour
{
    private Bloodmeter bloodMeter;
    [SerializeField]
    private float range;
    [SerializeField]
    private float healAmnt;

    // Start is called before the first frame update
    void Start()
    {
        bloodMeter = Bloodmeter.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && Vector3.Distance(Player.plr.transform.position, transform.position) < range)
        {
            bloodMeter.changeBlood(healAmnt);
            gameObject.SetActive(false);
        }
    }
}
