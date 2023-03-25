using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTotem : MonoBehaviour
{

    [SerializeField]
    GameObject upgradeHUD;
    [SerializeField]
    float range;

    void Update()
    {
        if (Input.GetButtonDown("Interact") && Vector3.Distance(Player.plr.transform.position, transform.position) < range) 
        {
            upgradeHUD.SetActive(true);
        }
    }
}
