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
    private bool nearThis;

    // Start is called before the first frame update
    void Start()
    {
        bloodMeter = Bloodmeter.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Player.plr.transform.position, transform.position) < range && Time.timeScale > 0)
        {
            nearThis = true;
            Player.plr.enableInteractToolTip();
            if (Input.GetButtonDown("Interact"))
            {
                bloodMeter.changeBlood(healAmnt);
                nearThis = false;
                Player.plr.disableInteractToolTip();
                gameObject.SetActive(false);
            }
        } else if (nearThis)
        {
            nearThis = false;
            Player.plr.disableInteractToolTip();
        }
    }
}
