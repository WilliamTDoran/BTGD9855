using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTotem : MonoBehaviour
{

    [SerializeField]
    GameObject upgradeHUD;


    void Update()
    {
        if (Input.GetButtonDown("UpgradeHUD"))
        {
            upgradeHUD.SetActive(true);
        }
    }
}
